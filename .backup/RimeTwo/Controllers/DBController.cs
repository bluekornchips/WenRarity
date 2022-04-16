using RimeTwo.ADO;
using RimeTwo.ADO.Asset.Token;
using RimeTwo.ADO.Tables;
using RimeTwo.Util;
using System;
using System.Linq;

namespace RimeTwo.Controllers
{
    public class DBController
    {
        private static DBController _instance;
        public static DBController Instance => _instance ?? (_instance = new DBController());
        private static Ducky _ducky = Ducky.Instance;

        private DBController() { }
        public void AddCollection(CollectionModel collection)
        {
            using RimeTwoContext context = new();
            try
            {
                context.Collections.Add(collection);
                context.SaveChanges();
                _ducky.Info($"Added {collection.Name} to database.");
            }
            catch (Exception ex)
            {
                _ducky.Error("DBController", "AddCollection", ex.Message);
                throw;
            }
        }
        public void RetrieveAssetByPolicy(Type entityType)
        {
            using RimeTwoContext context = new();
            try
            {
                var a = context.Database.SqlQuery<KBotModel>("SELECT * FROM [dbo].[KBotModels]").ToList();
            }
            catch (Exception ex)        
            {
                _ducky.Error("DBController", "RetrieveAssetByPolicy", ex.Message);
                throw;
            }
        }

        public bool CollectionExists(CollectionModel collection)
        {
            bool exists = false;
            using RimeTwoContext context = new();
            try
            {
                // First check if the collection already exists. If it does, do nothing.
                var existing = context.Collections.Where(item => item.PolicyId == collection.PolicyId).FirstOrDefault();
                if (existing != null) exists = true;
                _ducky.Info($"Existing {collection.Name} collection found.");
            }
            catch (Exception ex)
            {
                _ducky.Error("DBController", "CollectionExists", ex.Message);
                throw;
            }
            return exists;
        }
    }
}
