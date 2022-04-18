using System.ComponentModel.DataAnnotations.Schema;

namespace WenRarityLibrary.ADO.Rime.Models.OnChainMetaData.Token
{
	[Table("DeluxeBotOGCollection")]
	public partial class DeluxeBotOGCollection : OnChainMetaData
	{
		public string Hat { get; set; }
		public string Face { get; set; }
		public string Pose { get; set; }
		public string BackDrop { get; set; }
		public string BodyPaint { get; set; }
		public string FacePlate { get; set; }
		public string str_Id { get; set; }
		public string website { get; set; }
		public string copyright { get; set; }
		public string royalties { get; set; }
		public string collection { get; set; }
	}
}
