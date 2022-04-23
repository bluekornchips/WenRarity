using System.Data.Entity;
using WenRarityLibrary.ADO.Rime.Models.Rarity.Token;

namespace WenRarityLibrary.ADO.Rime
{
    public class RimeADO : DbContext
    {
        public RimeADO() : base("Rime") { }
        //##_:
		//##_:FalseIdols+
		public virtual DbSet<FalseIdolsRarity> FalseIdolsRarity { get; set; }
		public virtual DbSet<FalseIdolsTraitCount> FalseIdolsTraitCount{ get; set; }
		public virtual DbSet<FalseIdolsBack> FalseIdolsBack { get; set; }
		public virtual DbSet<FalseIdolsFace> FalseIdolsFace { get; set; }
		public virtual DbSet<FalseIdolsHead> FalseIdolsHead { get; set; }
		public virtual DbSet<FalseIdolsOutfit> FalseIdolsOutfit { get; set; }
		public virtual DbSet<FalseIdolsCharacter> FalseIdolsCharacter { get; set; }
		public virtual DbSet<FalseIdolsBackground> FalseIdolsBackground { get; set; }
		//##_:FalseIdols-
		
		
		
		
        

        //##_:PuurrtyCatsSociety+
        public virtual DbSet<PuurrtyCatsSocietyRarity> PuurrtyCatsSocietyRarity { get; set; }
        public virtual DbSet<PuurrtyCatsSocietyTraitCountRarity> PuurrtyCatsSocietyTraitCountRarity { get; set; }
        public virtual DbSet<PuurrtyCatsSocietyFurRarity> PuurrtyCatsSocietyFurRarity { get; set; }
        public virtual DbSet<PuurrtyCatsSocietyHatRarity> PuurrtyCatsSocietyHatRarity { get; set; }
        public virtual DbSet<PuurrtyCatsSocietyEyesRarity> PuurrtyCatsSocietyEyesRarity { get; set; }
        public virtual DbSet<PuurrtyCatsSocietyMaskRarity> PuurrtyCatsSocietyMaskRarity { get; set; }
        public virtual DbSet<PuurrtyCatsSocietyTailRarity> PuurrtyCatsSocietyTailRarity { get; set; }
        public virtual DbSet<PuurrtyCatsSocietyHandsRarity> PuurrtyCatsSocietyHandsRarity { get; set; }
        public virtual DbSet<PuurrtyCatsSocietyMouthRarity> PuurrtyCatsSocietyMouthRarity { get; set; }
        public virtual DbSet<PuurrtyCatsSocietyWingsRarity> PuurrtyCatsSocietyWingsRarity { get; set; }
        public virtual DbSet<PuurrtyCatsSocietyOutfitRarity> PuurrtyCatsSocietyOutfitRarity { get; set; }
        public virtual DbSet<PuurrtyCatsSocietyBackgroundRarity> PuurrtyCatsSocietyBackgroundRarity { get; set; }
        //##_:PuurrtyCatsSociety-

        //##_:KBot+
        public virtual DbSet<KBotRarity> KBotRarity { get; set; }
        public virtual DbSet<KBotPetRarity> KBotPetRarity { get; set; }
        //##_:KBot-
    }
}
