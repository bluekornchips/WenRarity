using System;

namespace MarketWatcher.Classes.JPGStore
{
    public class JPGStoreListing : JPGStoreItem
    {
        public DateTime listed_at { get; set; }

        public JPGStoreListing()
        {
        }

        public JPGStoreListing(string[] values)
        {
            int index = 0;
            asset_id = values[index++];
            display_name = values[index++];
            tx_hash = values[index++];
            listing_id = int.Parse(values[index++]);
            listed_at = DateTime.Parse(values[index++]);
            price_lovelace = ulong.Parse(values[index++]);
            collection_name = values[index++];
        }
    }
}