using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

namespace WenRarityLibrary.ADO.Rime.Models.Rarity.Token
{
	[Table("PuurrtyCatsSocietyHandsRarity")]
	public partial class PuurrtyCatsSocietyHandsRarity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		 public int id{ get; set; }
		public string hands { get; set; }
		public int Count { get; set; }
	}
}
