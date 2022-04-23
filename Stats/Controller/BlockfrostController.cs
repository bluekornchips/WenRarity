using WenRarityLibrary.ADO.Blockfrost;
using WenRarityLibrary.ADO.Blockfrost.Models;

namespace Stats.Controller
{
    internal class BlockfrostController
    {
        private static BlockfrostController _instance;
        public static BlockfrostController Instance => _instance ?? (_instance = new BlockfrostController());
        private BlockfrostController() { }


        public void CollectionByName(string name, out Collection collection)
        {
            using BlockfrostADO context = new();
            collection = context.Collection.Where(i => i.Name == name).FirstOrDefault();
        }
    }
}
