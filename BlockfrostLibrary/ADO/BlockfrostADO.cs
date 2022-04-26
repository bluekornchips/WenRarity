using BlockfrostLibrary.ADO.Models.Collection;
using BlockfrostLibrary.ADO.Models.OnChainMetaData.Token;
using System.Data.Entity;

namespace BlockfrostLibrary.ADO
{
    public class BlockfrostADO : DbContext
    {
        public BlockfrostADO() : base("Blockfrost") { }
        public virtual DbSet<BlockfrostItemJson> BlockfrostItemJson { get; set; }
        public virtual DbSet<BlockfrostCollection> Collection { get; set; }
		//##_:tokens+
		//##_:TavernSquad+
		public virtual DbSet<TavernSquad> TavernSquad { get; set; }
		//##_:TavernSquad-
        //##_:tokens-
    }
}
