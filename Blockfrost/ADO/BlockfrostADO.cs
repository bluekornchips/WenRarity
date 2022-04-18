using System.Data.Entity;
using WenRarityLibrary.ADO.Blockfrost;
using WenRarityLibrary.ADO.Rime.Models;
using WenRarityLibrary.ADO.Rime.Models.OnChainMetaData.Token;

namespace Rime.ADO
{
    public class BlockfrostADO : DbContext
    {
        public BlockfrostADO() : base("Blockfrost") { }
        public virtual DbSet<BlockfrostItemJson> BlockfrostItemJson { get; set; }
        public virtual DbSet<Collection> Collection { get; set; }
        //##_:tokens+
		public virtual DbSet<DeluxeBotOGCollection> DeluxeBotOGCollection { get; set; }
		public virtual DbSet<KBot> KBot { get; set; }
        //##_:tokens-

        //public virtual DbSet<BlockfrostJson> BlockfrostJson { get; set; }
        //public virtual DbSet<CollectionData> CollectionData { get; set; }
        //public virtual DbSet<KBotPet> KBotPets { get; set; }
        //public virtual DbSet<Pendulum> Pendulums { get; set; }
        //public virtual DbSet<ClumsyGhosts> ClumsyGhosts { get; set; }
        //public virtual DbSet<GrandmasterAdventurer> GrandmasterAdventurers { get; set; }
        // ##_:
    }
}






