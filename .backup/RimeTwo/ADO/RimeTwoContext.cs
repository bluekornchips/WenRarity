using RimeTwo.ADO.Asset.Token;
using RimeTwo.ADO.Tables;
using System.Data.Entity;

namespace RimeTwo.ADO
{
    internal class RimeTwoContext : DbContext
    {
        public RimeTwoContext() : base("RimeTwo") { }

        // Main Tables
        public virtual DbSet<CollectionModel> Collections { get; set; }

        // Assets
        public virtual DbSet<KBotModel> KBots { get; set; }

        // SPs
    }
}
