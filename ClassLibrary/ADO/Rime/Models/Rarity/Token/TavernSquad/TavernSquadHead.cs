using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

namespace WenRarityLibrary.ADO.Rime.Models.Rarity.Token
{
	[Table("TavernSquadHead")]
	public partial class TavernSquadHead
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id{ get; set; }
		public string Head { get; set; }
		public int Count { get; set; }
	}
}
