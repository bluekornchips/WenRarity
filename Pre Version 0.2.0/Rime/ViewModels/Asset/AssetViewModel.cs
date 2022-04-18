using Rime.ADO;
using WenRarityLibrary;

namespace Rime.ViewModels.Asset
{
    public class AssetViewModel
    {
        private Ducky _ducky = Ducky.Instance;
        public string policy_id { get; set; }
        public string asset { get; set; }
        public string fingerprint { get; set; }
        public int quantity { get; set; }
        public string initial_mint_tx_hash { get; set; }
        public int mint_or_burn_count { get; set; }
        public string metadata { get; set; }
        public OnChainMetaDataViewModel onchain_metadata { get; set; } = new BaseOnChainMetaDataViewModel();

        public void Add()
        {
            BlockfrostAsset bfa = new BlockfrostAsset();
            bfa.asset = asset;
            bfa.policy_id = policy_id;
            bfa.fingerprint = fingerprint;
            bfa.quantity = quantity;
            bfa.initial_mint_tx_hash = initial_mint_tx_hash;
            bfa.metadata = metadata;
            bfa.mint_or_burn_count = mint_or_burn_count;

            using RimeDb context = new();
            var trans = context.Database.BeginTransaction();
            try
            {
                context.BlockfrostAssets.Add(bfa);
                context.SaveChanges();
                trans.Commit();
            }
            catch (Exception ex)
            {
                _ducky.Error("AssetViewModel", "Add()", ex.Message);
                trans.Rollback();
            }
        }

        public class BaseOnChainMetaDataViewModel : OnChainMetaDataViewModel
        {
            public string Id { get; set; }

            public override void Add()
            {
                throw new NotImplementedException();
            }

            public override void AttributeHandler()
            {
                throw new NotImplementedException();
            }

            public override void Get(out Dictionary<string, OnChainMetaData> metadata)
            {
                throw new NotImplementedException();
            }

            public override OnChainMetaData Model()
            {
                throw new NotImplementedException();
            }
        }
    }
}
