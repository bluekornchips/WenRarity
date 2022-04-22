using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

namespace WenRarityLibrary.ADO.Rime.Models.Rarity.Token
{
	[Table("PuurrtyCatsSocietyRarity")]
	public partial class PuurrtyCatsSocietyRarity : OnChainRarity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		 public int id{ get; set; }
		public double fur { get; set; }
		public double hat { get; set; }
		public double eyes { get; set; }
		public double mask { get; set; }
		public double tail { get; set; }
		public double hands { get; set; }
		public double mouth { get; set; }
		public double wings { get; set; }
		public double outfit { get; set; }
		public double background { get; set; }
		public double traitCount { get; set; }
	}
}
