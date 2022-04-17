using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rime.ADO
{
	public partial class KBot : OnChainMetaData
	{
		public string Pet { get; set; }
		public string str_Id { get; set; }
		public string website { get; set; }
		public string copyright { get; set; }
		public string royalties { get; set; }
		public string collection { get; set; }
	}

	public class KBotPet
    {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id { get; set; }
		public string asset { get; set; }
		public string Pet { get; set; }
	}
}
