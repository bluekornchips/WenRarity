using System.ComponentModel.DataAnnotations;

namespace MarketWatcher.EntityFramework.JPGStore
{
    public class JPGStoreCollectionItem
    {
        [Key]
        public string policy { get; set; }
        public string collection_name { get; set; }
        public string collection_name_underscore { get; set; }
        public double floor { get; set; }
        public JPGStoreCollectionItem(string pol, string collName)
        {
            policy = pol;
            collection_name = collName;
            collection_name_underscore = collName.Replace(" ", "_");
            floor = 0.0;
        }
    }
}
