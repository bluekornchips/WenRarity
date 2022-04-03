using System;
using System.ComponentModel.DataAnnotations;

namespace MarketWatcher.EntityFramework.JPGStore
{
    public class JPGStoreSale : JPGStoreItem
    {
        [Key]
        public string asset_id { get; set; }
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
            price_lovelace = UInt64.Parse(values[index++]);
            collection_name = values[index++];
        }
    }
}
