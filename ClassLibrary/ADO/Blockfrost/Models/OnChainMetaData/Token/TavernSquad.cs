using System.ComponentModel.DataAnnotations.Schema;

namespace WenRarityLibrary.ADO.Blockfrost.Models.OnChainMetaData.Token
{
	[Table("TavernSquad")]
	public partial class TavernSquad : OnChainMetaData
	{
		public string Back { get; set; }
		public string Eyes { get; set; }
		public string Face { get; set; }
		public string Head { get; set; }
		public string Race { get; set; }
		public string Armor { get; set; }
		public string Mouth { get; set; }
		public string Racial { get; set; }
		public string Familiar { get; set; }
		public string SkinTone { get; set; }
		public string Background { get; set; }
		public string str_id { get; set; }
		public string url { get; set; }
		public string type { get; set; }
	}
}
