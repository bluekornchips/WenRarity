using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

namespace WenRarityLibrary.ADO.Rime.Models.Rarity.Token
{
	[Table("TavernSquadMouth")]
	public partial class TavernSquadMouth
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id{ get; set; }
		public string Mouth { get; set; }
		public int Count { get; set; }
	}
}
