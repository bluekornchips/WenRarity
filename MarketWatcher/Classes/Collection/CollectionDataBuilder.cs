using MarketWatcher.EntityFramework.Context.Rime;
using Rime.ADO.Classes;
using System;
using System.Linq;

namespace MarketWatcher.Classes
{
    public class CollectionDataBuilder
    {
        private static CollectionDataBuilder collectionDataBuilder  = new CollectionDataBuilder();
        public static CollectionDataBuilder Instance { get { return collectionDataBuilder; } }  

        private CollectionDataBuilder() { }

        public void SetAsset(string itemName, string activeCollectionName, out CollectionItemData collectionItemData)
        {
            collectionItemData = new();
            switch (activeCollectionName)
            {
                case "Pendulum":
                    Handle_Pendulum(itemName, activeCollectionName, out collectionItemData);
                    break;
                case "Puurrty Cats Society":
                    Handle_Puurrty_Cats_Society(itemName, activeCollectionName, out collectionItemData);
                    break;
                case "ElMatador":
                    Handle_ElMatador(itemName, activeCollectionName, out collectionItemData);
                    break;
                default:
                    break;
            }
        }

        private void Handle_Pendulum(string itemName, string activeCollectionName, out CollectionItemData collectionItemData)
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


        private void Handle_Puurrty_Cats_Society(string itemName, string activeCollectionName, out CollectionItemData collectionItemData)
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

        private void Handle_ElMatador(string itemName, string activeCollectionName, out CollectionItemData collectionItemData)
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
