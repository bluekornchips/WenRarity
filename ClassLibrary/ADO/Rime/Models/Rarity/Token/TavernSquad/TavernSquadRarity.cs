using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

namespace WenRarityLibrary.ADO.Rime.Models.Rarity.Token
{
	[Table("TavernSquadRarity")]
	public partial class TavernSquadRarity : OnChainRarity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id{ get; set; }
		public double Back { get; set; }
		public double Eyes { get; set; }
		public double Face { get; set; }
		public double Head { get; set; }
		public double Race { get; set; }
		public double Armor { get; set; }
		public double Mouth { get; set; }
		public double Racial { get; set; }
		public double Familiar { get; set; }
		public double SkinTone { get; set; }
		public double Background { get; set; }
		public double traitCount { get; set; }
	}
}
