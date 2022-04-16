using Rime.API;
using Rime.Controller;
using WenRarityLibrary;
using Rime.ViewModels.Asset;
using Rime.ViewModels.Collection;

namespace Rime.Builders.CollectionBuilder
{
    internal class FrameworkBuilder
    {
        private static FrameworkBuilder _instance;
        public static FrameworkBuilder Instance => _instance ?? (_instance = new FrameworkBuilder());
        private FrameworkWriter _frameworkWriter = FrameworkWriter.Instance;

        private static BlockfrostAPI blockfrostAPI = BlockfrostAPI.Instance;
        private static RimeController _rimeController = RimeController.Instance;
        private static Ducky _ducky = Ducky.Instance;
        private CollectionViewModel _collection = new CollectionViewModel();
        private FrameworkBuilder() { }
        public bool Build(out CollectionViewModel cvm)
        {
            _collection = new CollectionViewModel();
            //Console.WriteLine("Enter Policy ID: ");
            //string policyId = Console.ReadLine();

            //Console.WriteLine("Enter Collection Name: ");
            //string collectionName = Console.ReadLine();

            //_collection.PolicyId = "3f00d83452b4ead45cf5e0ca811fe8da561dfc45a5e414c88c4d8759";
            //_collection.Name = "KBot";
            //_collection.NamePlural = "KBots";

            //_collection.PolicyId = "a616aab3b18eb855b4292246bd58f9e131d7c8c25d1d1d7c88b666c4";
            //_collection.Name = "Pendulum";
            //_collection.NamePlural = "Pendulums";

            //_collection.PolicyId = "b000e9f3994de3226577b4d61280994e53c07948c8839d628f4a425a";
            //_collection.Name = "ClumsyGhosts";
            //_collection.NamePlural = "ClumsyGhosts";

            _collection.PolicyId = "95d9a98c2f7999a3d5e0f4d795cb1333837c09eb0f24835cd2ce954c";
            _collection.Name = "GrandmasterAdventurer";
            _collection.NamePlural = "GrandmasterAdventurers";

            CreateNew(out bool createdNew);
            cvm = _collection;
            return createdNew;
        }

        private void CreateNew(out bool createdNew)
        {
            createdNew = false;
            if (!_rimeController.CollectionExists(_collection.AsCollection()))
            {
                _ducky.Info($"Creating collection for {_collection.Name}");
                WriteNew(out bool wrote);
                if (!wrote)
                {
                    _ducky.Info("Unable to write new Collection.");
                }
                _rimeController.AddCollection(_collection.AsCollection());
                createdNew = true;
            }
        }

        private void WriteNew(out bool wrote)
        {
            int page = 1;
            wrote = false;
            // Check the policy
            blockfrostAPI.Assets_ByPolicy(_collection, page, out List<BlockfrostPolicyItem> items);

            try
            {
                if (items.Any())
                {
                    // Use the first item in sequence that has a quantity greater than 0.
                    int index = 0;
                    while (items[index].Quantity == 0) ++index;
                    BlockfrostPolicyItem item = items[index];
                    blockfrostAPI.Asset_One(_collection, item.Asset, out AssetViewModel asset);
                    _frameworkWriter.Execute(_collection.Name, asset);
                    wrote = true;
                }
            }
            catch (Exception ex)
            {
                _ducky.Error("FrameworkBuilder", "WriteNew", ex.Message);
                _ducky.Info($"Unable to write collection {_collection.Name}.");
            }
        }
    }
}
