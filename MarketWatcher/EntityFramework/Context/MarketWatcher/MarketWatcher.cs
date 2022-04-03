using MarketWatcher.EntityFramework.JPGStore;
using System.Data.Entity;

namespace MarketWatcher.EntityFramework.Context.MarketWatcher
{
    public class MarketWatcherContext : DbContext
    {
        public MarketWatcherContext() : base("MarketWatcher") { }
        public virtual DbSet<JPGStoreCollectionItem> JPGStoreCollectionItems { get; set; }
        public virtual DbSet<JPGStoreListing> JPGStoreListings { get; set; }
        public virtual DbSet<JPGStoreSale> JPGStoreSales { get; set; }
    }
}