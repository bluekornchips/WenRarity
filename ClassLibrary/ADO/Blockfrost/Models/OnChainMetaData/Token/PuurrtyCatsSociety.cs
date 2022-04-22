using System.ComponentModel.DataAnnotations.Schema;

namespace WenRarityLibrary.ADO.Blockfrost.Models.OnChainMetaData.Token
{
	[Table("PuurrtyCatsSociety")]
	public partial class PuurrtyCatsSociety : OnChainMetaData
	{
		public string fur { get; set; }
		public string hat { get; set; }
		public string eyes { get; set; }
		public string mask { get; set; }
		public string tail { get; set; }
		public string hands { get; set; }
		public string mouth { get; set; }
		public string wings { get; set; }
		public string outfit { get; set; }
		public string background { get; set; }
		public string collection { get; set; }
	}
}
