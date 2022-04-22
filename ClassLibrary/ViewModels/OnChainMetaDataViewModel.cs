using WenRarityLibrary.ADO.Blockfrost.Models.OnChainMetaData;
using WenRarityLibrary.ADO.Blockfrost.Models.OnChainMetaData.Token;

namespace WenRarityLibrary.ViewModels
{
    public class OnChainMetaDataViewModel
    {
        #region Model Attributes
        public string name { get; set; }
        public string image { get; set; }
        public string mediaType { get; set; }
        public string policy_id { get; set; }
        public string asset { get; set; }
        public string fingerprint { get; set; }
        #endregion
        public Dictionary<string, string> attributes { get; set; } = new Dictionary<string, string>();
        public List<OnChainFilesViewModel> files { get; set; }
        
        public OnChainMetaData model { get; set; }

        public dynamic AsModel(string type)
        {
            switch (type)
            {
                //##_:
				//##_:PuurrtyCatsSociety+
				case "PuurrtyCatsSociety": return (PuurrtyCatsSociety)model;
				//##_:PuurrtyCatsSociety-
				//##_:KBot+
				case "KBot": return (KBot)model;
				//##_:KBot-
                default: return model;
            }
        }
    }
}













































