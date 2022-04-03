using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketWatcher.EntityFramework.JPGStore
{
    public class JPGStoreItem
    {
        public string display_name { get; set; }
        public string tx_hash { get; set; }
        public int listing_id { get; set; }
        public UInt64 price_lovelace { get; set; }
        public string collection_name { get; set; }
        public string collection_name_underscore { get; set; }
    }
}
