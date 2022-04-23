using System.Data.Entity;
using WenRarityLibrary.ADO.Blockfrost;
using WenRarityLibrary.ADO.Blockfrost.Models;
using WenRarityLibrary.ADO.Blockfrost.Models.OnChainMetaData.Token;

namespace WenRarityLibrary.ADO.Blockfrost
{
    public class BlockfrostADO : DbContext
    {
        public BlockfrostADO() : base("Blockfrost") { }
        public virtual DbSet<BlockfrostItemJson> BlockfrostItemJson { get; set; }
        public virtual DbSet<Collection> Collection { get; set; }//##_:tokens+//##_:DeadRabbits+
		public virtual DbSet<DeadRabbits> DeadRabbits { get; set; }
		//##_:DeadRabbits-//##_:FalseIdols+
		public virtual DbSet<FalseIdols> FalseIdols { get; set; }
		//##_:FalseIdols-
		
		
		
		
		
		
		
		
		
		
		
		//##_:PuurrtyCatsSociety+
		public virtual DbSet<PuurrtyCatsSociety> PuurrtyCatsSociety { get; set; }
		//##_:PuurrtyCatsSociety-
		
		//##_:KBot+
		public virtual DbSet<KBot> KBot { get; set; }
		//##_:KBot-
		
		
		
		
        //##_:tokens-
    }
}
