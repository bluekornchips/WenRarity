using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

namespace WenRarityLibrary.ADO.Rime.Models.Rarity.Token
{
	[Table("PuurrtyCatsSocietyMaskRarity")]
	public partial class PuurrtyCatsSocietyMaskRarity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		 public int id{ get; set; }
		public string mask { get; set; }
		public int Count { get; set; }
	}
}
