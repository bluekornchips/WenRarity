using System.ComponentModel.DataAnnotations.Schema;

namespace WenRarityLibrary.ADO.Blockfrost.Models.OnChainMetaData.Token
{
	[Table("KBot")]
	public partial class KBot : OnChainMetaData
	{
		public string Pet { get; set; }
		public string str_Id { get; set; }
		public string website { get; set; }
		public string copyright { get; set; }
		public string royalties { get; set; }
		public string collection { get; set; }
	}
}
