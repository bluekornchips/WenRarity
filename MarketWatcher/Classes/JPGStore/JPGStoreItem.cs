using System;
using System.ComponentModel.DataAnnotations;

namespace MarketWatcher.Classes.JPGStore
{
    public abstract class JPGStoreItem
    {


        [Key]
        public string asset_id { get; set; }
        public string display_name { get; set; }
        public string tx_hash { get; set; }
        public int listing_id { get; set; }
        public ulong price_lovelace { get; set; }
        public string collection_name { get; set; }
        public string collection_name_underscore { get; set; }
        public readonly string _url = "https://www.jpg.store/asset/";

        public JPGStoreItem() { }
    }
}