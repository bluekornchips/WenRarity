using Blockfrost.ADO;
using WenRarityLibrary;
using WenRarityLibrary.ADO.Blockfrost.Models;
using WenRarityLibrary.ADO.Blockfrost.Models.OnChainMetaData.Token;

namespace Stats.Controller
{
    internal class BlockfrostController
    {
        private static BlockfrostController _instance;
        public static BlockfrostController Instance => _instance ?? (_instance = new BlockfrostController());
        private BlockfrostController() { }

        private static Ducky _ducky = Ducky.Instance;

        public void CollectionByName(string name, out Collection collection)
        {
            using BlockfrostADO context = new();
            collection = context.Collection.Where(i => i.Name == name).FirstOrDefault();
        }

        //##_:
        //##_:KBot+
        public void GetKBot(out List<KBot> results)
        {
            using BlockfrostADO context = new();
            results = context.KBot.ToList();
        }
        //##_:KBot-
    }
}
