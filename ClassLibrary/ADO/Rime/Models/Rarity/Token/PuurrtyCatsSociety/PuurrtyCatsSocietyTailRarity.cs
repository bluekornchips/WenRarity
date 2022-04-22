using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

namespace WenRarityLibrary.ADO.Rime.Models.Rarity.Token
{
	[Table("PuurrtyCatsSocietyTailRarity")]
	public partial class PuurrtyCatsSocietyTailRarity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		 public int id{ get; set; }
		public string tail { get; set; }
		public int Count { get; set; }
	}
}
