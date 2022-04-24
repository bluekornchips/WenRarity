using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

namespace WenRarityLibrary.ADO.Rime.Models.Rarity.Token
{
	[Table("TavernSquadBackground")]
	public partial class TavernSquadBackground
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id{ get; set; }
		public string Background { get; set; }
		public int Count { get; set; }
	}
}
