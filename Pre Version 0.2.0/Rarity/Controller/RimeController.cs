using Rime.ADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WenRarityLibrary;

namespace Rarity.Controller
{
    internal class RimeController
    {
        private static RimeController _instance;
        public static RimeController Instance => _instance ?? (_instance = new RimeController());
        private static Ducky _ducky = Ducky.Instance;
        private RimeController() { }


        public void Get_CollectionById(string policyId, out Collection collection)
        {
            using RimeDb context = new RimeDb();
            try
            {
                collection = context.Collections.Where(c => c.PolicyId == policyId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _ducky.Error("RimeController", "", ex.Message);
                throw;
            }
        }
        public void Get_TokensFromCollection(Collection collection, out List<OnChainMetaData> ocmd)
        {
            ocmd = new();
            using RimeDb context = new RimeDb();
            try
            {
                dynamic entries;
                switch (collection.Name)
                {
                    case "GrandmasterAdventurer":
                        entries = context.GrandmasterAdventurers.ToList();
                        foreach (var item in entries) ocmd.Add(item);
                        break;
                    case "KBot":
                        entries = context.KBots.ToList();
                        foreach (var item in entries) ocmd.Add(item);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                _ducky.Error("RimeController", "Get_TokensFromCollection", ex.Message);
                throw;
            }
        }
        private void template()
        {
            using RimeDb context = new RimeDb();
            var dbContextTransaction = context.Database.BeginTransaction();
            {
                try
                {
                    context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    _ducky.Error("RimeController", "", ex.Message);
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
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
