using BlockfrostLibrary.ADO;
using BlockfrostLibrary.ADO.Models.Collection;
using BlockfrostLibrary.ADO.Models.OnChainMetaData;
using WenRarityLibrary;

namespace Blockfrost.Controller
{
    public class BlockfrostController
    {
        private static BlockfrostController _instance;
        public static BlockfrostController Instance => _instance ?? (_instance = new BlockfrostController());
        private BlockfrostController() { }

        private static Ducky _ducky = Ducky.Instance;

        /// <summary>
        /// Add the Collection item to the Collection Table
        /// </summary>
        /// <param name="item"></param>
        public void AddCollection(BlockfrostCollection item)
        {
            using BlockfrostADO context = new();
            var trans = context.Database.BeginTransaction();
            {
                try
                {
                    context.Collection.Add(item);
                    context.SaveChanges();
                    trans.Commit();
                    _ducky.Info($"Added {item.Name} to database.");
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _ducky.Critical("BlockfrostController", "AddCollection", ex.Message);
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns whether the collection exists in the table or not.
        /// Used as a soft indictator of whether we have a local state for the blockfrost class or not.
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        public bool CollectionExists(string policyId)
        {
            using BlockfrostADO context = new();
            return context.Collection.Where(i => i.PolicyId == policyId).Any();
        }


        public void CollectionByName(string name, out BlockfrostCollection collection)
        {
            using BlockfrostADO context = new();
            collection = context.Collection.Where(i => i.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// Add the full json data to the database.
        /// Allows prevention of future Blockfrost calls.
        /// </summary>
        /// <param name="item"></param>
        public void JsonAdd(BlockfrostItemJson item)
        {
            using BlockfrostADO context = new();
            var trans = context.Database.BeginTransaction();
            {
                try
                {
                    context.BlockfrostItemJson.Add(item);
                    context.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _ducky.Critical("BlockfrostController", "JsonAdd", ex.Message);
                    throw;
                }
            }
        }

        public void JsonRetrieve(BlockfrostCollection collection, out List<BlockfrostItemJson> items)
        {
            using BlockfrostADO context = new();
            var trans = context.Database.BeginTransaction();
            {
                try
                {
                    items = context.BlockfrostItemJson.Where(item => item.policy_id == collection.PolicyId).ToList();
                    context.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _ducky.Critical("BlockfrostController", "JsonRetrieve", ex.Message);
                    throw;
                }
            }
        }

        public void JsonClearCollectionInfo(List<BlockfrostItemJson> items)
        {
            //using BlockfrostADO context = new();
            //var trans = context.Database.BeginTransaction();
            //{
            //    try
            //    {
            //        context.BlockfrostItemJson.RemoveRange(items);
            //        trans.Commit();
            //        context.SaveChanges();
            //    }
            //    catch (Exception ex)
            //    {
            //        trans.Rollback();
            //        _ducky.Critical("BlockfrostController", "JsonClearCollectionInfo", ex.Message);
            //        throw;
            //    }
            //}
            using BlockfrostADO context = new();
            var dbContextTransaction = context.Database.BeginTransaction();
            try
            {
                context.Database.ExecuteSqlCommand($"DELETE FROM [Blockfrost].[dbo].[BlockfrostItemJson];");
                context.Database.ExecuteSqlCommand($"DBCC CHECKIDENT ('BlockfrostItemJson', RESEED, 0);");

                context.SaveChanges();
                dbContextTransaction.Commit();

                _ducky.Info($"Removed Asset records.");
            }
            catch (Exception ex)
            {
                _ducky.Critical("BlockfrostController", "JsonClearCollectionInfo", ex.Message);
                dbContextTransaction.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Returns the in database json for a single item.
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="item"></param>
        public void JsonGetOne(string asset, out BlockfrostItemJson item)
        {
            item = new();
            using BlockfrostADO context = new();
            try
            {
                item = context.BlockfrostItemJson.Where(i => i.asset == asset).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _ducky.Error("BlockfrostController", "JsonGetOne", ex.Message);
            }
        }

        /// <summary>
        /// Helper method for deleting a collection.
        /// </summary>
        /// <param name="collection"></param>
        public void DeleteTokenTable(BlockfrostCollection collection)
        {
            using BlockfrostADO context = new();
            var dbContextTransaction = context.Database.BeginTransaction();
            try
            {
                context.Database.ExecuteSqlCommand($"DELETE FROM {collection.DatabaseName};");
                context.Database.ExecuteSqlCommand($"DBCC CHECKIDENT ('{collection.DatabaseName}', RESEED, 0);");

                context.SaveChanges();
                dbContextTransaction.Commit();

                _ducky.Info($"Removed Table: {collection.Name}");
                _ducky.Info($"Removed Asset records.");
            }
            catch (Exception ex)
            {
                _ducky.Critical("BlockfrostController", "DeleteTokenTable", ex.Message);
                dbContextTransaction.Rollback();
                throw;
            }
        }

        ///// <summary>
        ///// Delete the collection from the table
        ///// </summary>
        ///// <param name="collection"></param>
        //public void DeleteFromCollectionTable(Collection collection)
        //{
        //    using BlockfrostADO context = new();
        //    var dbContextTransaction = context.Database.BeginTransaction();
        //    try
        //    {
        //        // Collection
        //        context.Database.ExecuteSqlCommand($"DELETE FROM [dbo].[Collections] WHERE [PolicyId] = '{collection.PolicyId}'");
                
        //        context.SaveChanges();
        //        dbContextTransaction.Commit();

        //        _ducky.Info($"Removed from Collection Table: {collection.Name}");
        //    }
        //    catch (Exception ex)
        //    {
        //        _ducky.Critical("BlockfrostController", "Reset", ex.Message);
        //        dbContextTransaction.Rollback();
        //        throw;
        //    }
        //}

        /// <summary>
        /// Return the given collection as a dictionary of OnChainMetaData.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="items"></param>
        public void GetOnChainMetaDataAsModel(string collection, out Dictionary<string, OnChainMetaData> items)
        {
            items = new Dictionary<string, OnChainMetaData>();
            using BlockfrostADO context = new();
            try
            {
                switch (collection)
                {
                    //##_:
					//##_:TavernSquad+
					case "TavernSquad" :
						var foundTavernSquad = context.TavernSquad.ToList();
						foreach (var item in foundTavernSquad) items.Add(item.asset, item);
						break;
					//##_:TavernSquad-
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                _ducky.Critical("BlockfrostController", "GetOnChainMetaDataAsModel", ex.Message);
                throw;
            }
        }
    }
}