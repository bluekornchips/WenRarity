using System.Data.Entity;
using WenRarityLibrary.ADO.Blockfrost;
using WenRarityLibrary.ADO.Blockfrost.Models;
using WenRarityLibrary.ADO.Blockfrost.Models.OnChainMetaData.Token;

namespace Blockfrost.ADO
{
    public class BlockfrostADO : DbContext
    {
        public BlockfrostADO() : base("Blockfrost") { }
        public virtual DbSet<BlockfrostItemJson> BlockfrostItemJson { get; set; }
        public virtual DbSet<Collection> Collection { get; set; }
        //##_:tokens+
		//##_:KBot+
		public virtual DbSet<KBot> KBot { get; set; }
		//##_:KBot-
		
		
        //##_:tokens-
    }
}






















