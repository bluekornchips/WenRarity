using Rime.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Rime.ADO.Classes
{
    public class Token
    {
        [Key]
        public int Id { get; set; }
        public string _Asset { get; set; }
        public string PolicyID { get; set; }
        public string PolicyName { get; set; }
        public string AssetName { get; set; }
        public string Fingerprint { get; set; }
        public int Quantity { get; set; }
        public string InitialMintTxHash { get; set; }
        public int MintOrBurnCount { get; set; }

        public Token(TokenViewModel tokenViewModel)
        {
            _Asset = tokenViewModel.Token._Asset;
            PolicyID = tokenViewModel.Token.PolicyID;
            PolicyName = tokenViewModel.Token.PolicyName;
            AssetName = tokenViewModel.Token.AssetName;
            Fingerprint = tokenViewModel.Token.Fingerprint;
            Quantity = tokenViewModel.Token.Quantity;
            InitialMintTxHash= tokenViewModel.Token.InitialMintTxHash;
            MintOrBurnCount = tokenViewModel.Token.MintOrBurnCount;

        }

        public Token()
        {

        }
    }
}