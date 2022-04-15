using RimeTwo.ADO.Asset;
using System;

namespace RimeTwo.ViewModels
{
    public class AssetViewModel
    {
        public string asset { get; set; }
        public string policy_id { get; set; }
        public string asset_name { get; set; }
        public string fingerprint { get; set; }
        public int quantity { get; set; }
        public string initial_mint_tx_hash { get; set; }
        public string mint_or_burn_count { get; set; }
        public string metadata { get; set; }
        public OnChainMetaDataViewModel onchain_metadata { get; set; }

        public AssetModel AsModel()
        {
            AssetModel asset = new AssetModel();
            asset.asset_name = asset_name;
            asset.policy_id = policy_id;
            asset.fingerprint = fingerprint;
            asset.quantity = quantity;
            asset.initial_mint_tx_hash = initial_mint_tx_hash;
            asset.metadata = metadata;
            asset.mint_or_burn_count = mint_or_burn_count;
            asset.onchain_metadata = onchain_metadata.AsModel();
            return asset;
        }
    }
}
