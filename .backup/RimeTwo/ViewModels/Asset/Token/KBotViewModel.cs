using RimeTwo.ADO.Asset.Token;
namespace RimeTwo.ViewModels.Asset.Token
{
	public class KBotViewModel : OnChainMetaDataViewModel
	{
		public string Pet { get; set; }
		public string Id { get; set; }
		public string website { get; set; }
		public string copyright { get; set; }
		public string royalties { get; set; }
		public string collection { get; set; }
		public KBotViewModel(){ }
	}
}
