namespace RimeTwo
{
    public class Asset
    {
        // CIP721 Standard
        // https://github.com/cardano-foundation/CIPs/blob/8b1f2f0900d81d6233e9805442c2b42aa1779d2d/CIP-NFTMetadataStandard.md
        public string asset { get; set; }
        public string policy_id { get; set; }
        public string asset_name { get; set; }
        public string fingerprint { get; set; }
        public int quantity { get; set; }
        public string initial_mint_tx_hash { get; set; }
        public string mint_or_burn_count { get; set; }
        public string metadata { get; set; }
        public OnChainMetaData onchain_metadata { get; set; }
    }
}