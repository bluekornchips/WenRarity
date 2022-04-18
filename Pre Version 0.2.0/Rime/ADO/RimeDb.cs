using System.Data.Entity;

namespace Rime.ADO
{
    public class RimeDb : DbContext
    {
        public RimeDb() : base("Rime") { }
        public virtual DbSet<BlockfrostAsset> BlockfrostAssets { get; set; }
        public virtual DbSet<BlockfrostJson> BlockfrostJson { get; set; }
        public virtual DbSet<CollectionData> CollectionData { get; set; }
        public virtual DbSet<Collection> Collections { get; set; }
        public virtual DbSet<KBot> KBots { get; set; }
        public virtual DbSet<KBotPet> KBotPets { get; set; }
        public virtual DbSet<Pendulum> Pendulums { get; set; }
        public virtual DbSet<ClumsyGhosts> ClumsyGhosts { get; set; }
        public virtual DbSet<GrandmasterAdventurer> GrandmasterAdventurers { get; set; }
		// ##_:
    }
}
