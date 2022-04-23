using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

namespace WenRarityLibrary.ADO.Rime.Models.Rarity.Token
{
	[Table("FalseIdolsRarity")]
	public partial class FalseIdolsRarity : OnChainRarity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id{ get; set; }
		public double Back { get; set; }
		public double Face { get; set; }
		public double Head { get; set; }
		public double Outfit { get; set; }
		public double Character { get; set; }
		public double Background { get; set; }
		public double traitCount { get; set; }
	}
}
