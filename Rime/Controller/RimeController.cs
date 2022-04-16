using Rime.ADO;
using WenRarityLibrary;

namespace Rime.Controller
{
    internal class RimeController
    {
        private static RimeController _instance;
        public static RimeController Instance => _instance ?? (_instance = new RimeController());
        private static Ducky _ducky = Ducky.Instance;
        //private static readonly string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\DB\Rime\Rime.mdf;Integrated Security=True;Connect Timeout=30";

        private RimeController() { }
        public void AddCollection(Collection collection)
        {
            using RimeDb context = new RimeDb();
            var dbContextTransaction = context.Database.BeginTransaction();
            {
                try
                {
                    context.Collections.Add(collection);
                    context.SaveChanges();
                    dbContextTransaction.Commit();
                    _ducky.Info($"Added {collection.Name} to database.");
                }
                catch (Exception ex)
                {
                    _ducky.Error("RimeController", "AddCollection", ex.Message);
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public void AddJson(BlockfrostJson bfj)
        {
            using RimeDb context = new RimeDb();
            var dbContextTransaction = context.Database.BeginTransaction();
            {
                try
                {
                    context.BlockfrostJson.Add(bfj);
                    context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    _ducky.Error("RimeController", "AddCollection", ex.Message);
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public void Reset(Collection collection)
        {
            using RimeDb context = new RimeDb();
            var dbContextTransaction = context.Database.BeginTransaction();
            try
            {
                // Collection
                //context.Database.ExecuteSqlCommand($"DELETE FROM [dbo].[Collections] WHERE [PolicyId] = '{collection.PolicyId}'");

                //// RawJson
                //context.Database.ExecuteSqlCommand($"DELETE FROM BlockfrostJsons WHERE [policy_id] = '{collection.PolicyId}';");

                // Blockfrost Assets
                context.Database.ExecuteSqlCommand($"DELETE FROM BlockFrostAssets WHERE [policy_id] = '{collection.PolicyId}';");

                // OnChainMetaData
                context.Database.ExecuteSqlCommand($"DELETE FROM {collection.NamePlural};");
                context.Database.ExecuteSqlCommand($"DBCC CHECKIDENT ('{collection.NamePlural}', RESEED, 0);");

                context.SaveChanges();
                dbContextTransaction.Commit();

                _ducky.Info($"Removed Table: {collection.Name}");
                _ducky.Info($"Removed Asset records.");
            }
            catch (Exception ex)
            {
                _ducky.Error("RimeController", "Reset", ex.Message);
                dbContextTransaction.Rollback();
                throw;
            }
        }

        public bool CollectionExists(Collection collection)
        {
            bool exists = false;
            using (RimeDb context = new RimeDb())
            {
                try
                {
                    // First check if the collection already exists. If it does, do nothing.
                    var existing = context.Collections.Where(item => item.PolicyId == collection.PolicyId).FirstOrDefault();
                    if (existing != null)
                    {
                        exists = true;
                        _ducky.Info($"Existing {collection.Name} collection found.");
                    }
                }
                catch (Exception ex)
                {
                    _ducky.Error("RimeController", "CollectionExists", ex.Message);
                    throw;
                }
            }
            return exists;
        }

        public void Get_BlockfrostJson(string assetName, out string json)
        {
            json = "";
            using RimeDb context = new RimeDb();
            var match = context.BlockfrostJson.Where(b => b.asset == assetName).FirstOrDefault();
            if (match != null) json = match.json;
        }
    }
}
