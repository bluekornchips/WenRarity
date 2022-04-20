using System.Data.Entity;
using WenRarityLibrary.ADO.Rime.Models.Rarity.Token;

namespace WenRarityLibrary.ADO.Rime
{
    public class RimeADO : DbContext
    {
        public RimeADO() : base("Rime") { }

        //##_:
		public virtual DbSet<KBotRarity> KBotRarity { get; set; }
		public virtual DbSet<KBotPetRarity> KBotPetRarity { get; set; }
    }
}
