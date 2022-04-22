using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

namespace WenRarityLibrary.ADO.Rime.Models.Rarity.Token
{
	[Table("PuurrtyCatsSocietyEyesRarity")]
	public partial class PuurrtyCatsSocietyEyesRarity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		 public int id{ get; set; }
		public string eyes { get; set; }
		public int Count { get; set; }
	}
}
