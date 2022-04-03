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
            collectionItemData.rarity = rimeContext.PendulumRarities.Where(t => t.Fingerprint == fp).FirstOrDefault();
        }
    }
}
