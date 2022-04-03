using System;

namespace MarketWatcher.Classes.JPGStore
{
    public class JPGStoreSale : JPGStoreItem
    {
        public DateTime confirmed_at { get; set; }
        public JPGStoreSale() { }
        public JPGStoreSale(string[] values)
        {
            int index = 0;
            asset_id = values[index++];
            display_name = values[index++];
            tx_hash = values[index++];
            listing_id = int.Parse(values[index++]);
            confirmed_at = DateTime.Parse(values[index++]);
            price_lovelace = ulong.Parse(values[index++]);
            collection_name = values[index++];
        }
    }
}