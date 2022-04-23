using System.ComponentModel.DataAnnotations.Schema;

namespace WenRarityLibrary.ADO.Blockfrost.Models.OnChainMetaData.Token
{
	[Table("DeadRabbits")]
	public partial class DeadRabbits : OnChainMetaData
	{
		public string Jaw { get; set; }
		public string Pin { get; set; }
		public string Ears { get; set; }
		public string Eyes { get; set; }
		public string Order { get; set; }
		public string Teeth { get; set; }
		public string Eyewear { get; set; }
		public string Clothing { get; set; }
		public string EarTags { get; set; }
		public string Headwear { get; set; }
		public string Background { get; set; }
		public string MouthBling { get; set; }
		public string Twitter { get; set; }
		public string Publisher { get; set; }
		[NotMapped]
		public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
		[NotMapped]
		public string[] description { get; set; }
	}
}
