using MarketWatcher.EntityFramework.Context.Rime;
using MarketWatcher.Utility;
using Rime.ADO.Classes;
using System;
using System.Linq;

namespace MarketWatcher.Classes
{
    public class CollectionDataBuilder
    {
        private static CollectionDataBuilder _instance;
        private static Ducky _ducky = Ducky.GetInstance();
        public static CollectionDataBuilder GetInstance()
        {
            if (_instance == null) _instance = new CollectionDataBuilder();
            return _instance;
        }

        private CollectionDataBuilder() { }

        /// <summary>
        /// Set asset 
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="activeCollectionName"></param>
        /// <param name="collectionItemData"></param>
        public void SetAsset(string itemName, string activeCollectionName, out CollectionItemData collectionItemData)
        {
            collectionItemData = new();
            try
            {
                switch (activeCollectionName)
                {
                    case "Pendulum":
                        Handle_Pendulum(itemName, out collectionItemData);
                        break;
                    case "Puurrty Cats Society":
                        Handle_Puurrty_Cats_Society(itemName, out collectionItemData);
                        break;
                    case "ElMatador":
                        Handle_ElMatador(itemName, out collectionItemData);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                _ducky.Critical("CollectionDataBuilder", "SetAsset", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Connect to the related data.
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="collectionItemData"></param>
        private void Handle_Pendulum(string itemName, out CollectionItemData collectionItemData)
        {
            collectionItemData = new();
            using RimeContext rimeContext = new RimeContext();

            collectionItemData.asset = rimeContext.Pendulums.Where(t => t.Name == itemName).FirstOrDefault();
            collectionItemData.Fingerprint = collectionItemData.asset.Fingerprint;
            var fp = collectionItemData.Fingerprint;

            var rarity = rimeContext.PendulumRarities.Where(t => t.Fingerprint == fp).FirstOrDefault();
            collectionItemData.rarity = rarity;
            var rank = rimeContext.PendulumRarities.Where(t => t.Weighting >= rarity.Weighting).Count();
            var collection = rimeContext.PendulumRarities.ToList();
            collectionItemData.RarityRank = rank;
            collectionItemData.CollectionSize = collection.Count;
        }


        /// <summary>
        /// Connect the related table data.
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="collectionItemData"></param>
        private void Handle_Puurrty_Cats_Society(string itemName, out CollectionItemData collectionItemData)
        {
            collectionItemData = new();
            using RimeContext rimeContext = new RimeContext();

            collectionItemData.asset = rimeContext.Puurrties.Where(t => t.Name == itemName).FirstOrDefault();
            collectionItemData.Fingerprint = collectionItemData.asset.Fingerprint;
            var fp = collectionItemData.Fingerprint;

            var rarity = rimeContext.PuurrtiesRarities.Where(t => t.Fingerprint == fp).FirstOrDefault();
            collectionItemData.rarity = rarity;
            var rank = rimeContext.PuurrtiesRarities.Where(t => t.Weighting >= rarity.Weighting).Count();
            var collection = rimeContext.PuurrtiesRarities.ToList();
            collectionItemData.RarityRank = rank;
            collectionItemData.CollectionSize = collection.Count;
        }

        /// <summary>
        /// Connect the related table data.
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="collectionItemData"></param>
        private void Handle_ElMatador(string itemName, out CollectionItemData collectionItemData)
        {
            collectionItemData = new();
            using RimeContext rimeContext = new RimeContext();

            collectionItemData.asset = rimeContext.ElMatadors.Where(t => t.Name == itemName).FirstOrDefault();
            collectionItemData.Fingerprint = collectionItemData.asset.Fingerprint;
            var fp = collectionItemData.Fingerprint;

            var rarity = rimeContext.ElMatadorRarities.Where(t => t.Fingerprint == fp).FirstOrDefault();
            collectionItemData.rarity = rarity;
            var rank = rimeContext.ElMatadorRarities.Where(t => t.Weighting >= rarity.Weighting).Count();
            var collection = rimeContext.ElMatadorRarities.ToList();
            collectionItemData.RarityRank = rank;
            collectionItemData.CollectionSize = collection.Count;
        }
    }
}
