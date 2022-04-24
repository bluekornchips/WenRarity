using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

namespace WenRarityLibrary.ADO.Rime.Models.Rarity.Token
{
	[Table("TavernSquadTraitCount")]
	public partial class TavernSquadTraitCount
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id{ get; set; }
		public int traitCount { get; set; }
		public int Count { get; set; }
	}
}
