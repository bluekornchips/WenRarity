using Rime.ADO;
using Rime.Utils;

namespace Rime.ViewModels.Asset
{
    public abstract class OnChainMetaDataViewModel
    {
        protected Ducky _ducky = Ducky.Instance;
        #region Model Attributes
        public string name { get; set; }
        public string image { get; set; }
        public string mediaType { get; set; }
        public string policy_id { get; set; }
        public string asset { get; set; }
        #endregion
        public Dictionary<string, string> attributes { get; set; } = new Dictionary<string, string>();
        public List<OnChainFilesViewModel> files { get; set; }
        public abstract void Add();
        public abstract OnChainMetaData Model();
        public abstract void Get(out Dictionary<string, OnChainMetaData> metadata);
    }
}
