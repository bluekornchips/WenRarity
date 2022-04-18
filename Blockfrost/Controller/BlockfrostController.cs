using Rime.ADO;
using WenRarityLibrary;
using WenRarityLibrary.ADO.Blockfrost;
using WenRarityLibrary.ADO.Rime.Models;
using WenRarityLibrary.ADO.Rime.Models.OnChainMetaData;

namespace Blockfrost.Controller
{
    public class BlockfrostController
    {
        private static BlockfrostController _instance;
        public static BlockfrostController Instance => _instance ?? (_instance = new BlockfrostController());
        private static Ducky _ducky = Ducky.Instance;
        private BlockfrostController() { }


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
                    _ducky.Error("BlockfrostController", "AddCollection", ex.Message);
                    trans.Rollback();
                    throw;
                }
            }
        }

        public bool CollectionExists(string policyId)
        {
            using BlockfrostADO context = new();
            return context.Collection.Where(i => i.PolicyId == policyId).Any();
        }

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
                    _ducky.Error("BlockfrostController", "JsonAdd", ex.Message);
                    trans.Rollback();
                    throw;
                }
            }
        }

        public void JsonGetAll(string policyId, out List<BlockfrostItemJson> items)
        {
            items = new();
            using BlockfrostADO context = new();
            {
                try
                {
                    items = context.BlockfrostItemJson.Where(i => i.policy_id == policyId).ToList();
                }
                catch (Exception ex)
                {
                    _ducky.Error("BlockfrostController", "JsonGetAll", ex.Message);
                }
            }
        }

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
                _ducky.Error("BlockfrostController", "Reset", ex.Message);
                dbContextTransaction.Rollback();
                throw;
            }
        }

        public void GetOnChainMetaData(string collection, out Dictionary<string, OnChainMetaData> items)
        {
            items = new Dictionary<string, OnChainMetaData>();
            using BlockfrostADO context = new();
            try
            {
                switch (collection)
                {
                    //##_:
					case "DeluxeBotOGCollection" :
						var foundDeluxeBotOGCollection = context.DeluxeBotOGCollection.ToList();
						foreach (var item in foundDeluxeBotOGCollection) items.Add(item.asset, item);
						break;
                    case "KBot":
                        var foundKBot = context.KBot.ToList();
                        foreach (var item in foundKBot) items.Add(item.asset, item);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}




