using System.ComponentModel.DataAnnotations.Schema;

namespace WenRarityLibrary.ADO.Blockfrost.Models.OnChainMetaData.Token
{
	[Table("FalseIdols")]
	public partial class FalseIdols : OnChainMetaData
	{
		public string Back { get; set; }
		public string Face { get; set; }
		public string Head { get; set; }
		public string Outfit { get; set; }
		public string Character { get; set; }
		public string Background { get; set; }
		public string str_id { get; set; }
		public string website { get; set; }
		public string collection { get; set; }
	}
}
