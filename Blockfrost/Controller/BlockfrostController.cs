using WenRarityLibrary;
using WenRarityLibrary.ADO.Blockfrost;
using WenRarityLibrary.ADO.Blockfrost.Models;
using WenRarityLibrary.ADO.Blockfrost.Models.OnChainMetaData;

namespace Blockfrost.Controller
{
    public class BlockfrostController
    {
        private static BlockfrostController _instance;
        public static BlockfrostController Instance => _instance ?? (_instance = new BlockfrostController());
        private BlockfrostController() { }

        private static Ducky _ducky = Ducky.Instance;

        public void AddCollection(Collection item)
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

        public bool CollectionExists(string policyId)
        {
            using BlockfrostADO context = new();
            return context.Collection.Where(i => i.PolicyId == policyId).Any();
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
                _ducky.Error("BlockfrostController", "JsonGetAll", ex.Message);
            }
        }

        /// <summary>
        /// Helper method for deleting a collection.
        /// </summary>
        /// <param name="collection"></param>
        public void Delete(Collection collection)
        {
            using BlockfrostADO context = new();
            var dbContextTransaction = context.Database.BeginTransaction();
            try
            {
                // Collection
                context.Database.ExecuteSqlCommand($"DELETE FROM [dbo].[Collections] WHERE [PolicyId] = '{collection.PolicyId}'");

                //// RawJson
                //context.Database.ExecuteSqlCommand($"DELETE FROM BlockfrostJsons WHERE [policy_id] = '{collection.PolicyId}';");

                //// Blockfrost Assets
                //context.Database.ExecuteSqlCommand($"DELETE FROM [BlockfrostItemJson] WHERE [policy_id] = '{collection.PolicyId}';");
                //context.Database.ExecuteSqlCommand($"DBCC CHECKIDENT ('BlockfrostItemJson', RESEED, 0);");

                // OnChainMetaData
                context.Database.ExecuteSqlCommand($"DELETE FROM {collection.DatabaseName};");
                context.Database.ExecuteSqlCommand($"DBCC CHECKIDENT ('{collection.DatabaseName}', RESEED, 0);");

                context.SaveChanges();
                dbContextTransaction.Commit();

                _ducky.Info($"Removed Table: {collection.Name}");
                _ducky.Info($"Removed Asset records.");
            }
            catch (Exception ex)
            {
                _ducky.Critical("BlockfrostController", "Reset", ex.Message);
                dbContextTransaction.Rollback();
                throw;
            }
        }

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
					//##_:PuurrtyCatsSociety+
					case "PuurrtyCatsSociety" :
						var foundPuurrtyCatsSociety = context.PuurrtyCatsSociety.ToList();
						foreach (var item in foundPuurrtyCatsSociety) items.Add(item.asset, item);
						break;
					//##_:PuurrtyCatsSociety-
					//##_:KBot+
					case "KBot" :
						var foundKBot = context.KBot.ToList();
						foreach (var item in foundKBot) items.Add(item.asset, item);
						break;
					//##_:KBot-
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









