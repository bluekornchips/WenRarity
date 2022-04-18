using Rime.ADO;
using Rime.API;
using Rime.Controller;
using WenRarityLibrary;
using Rime.ViewModels.Asset;
using Rime.ViewModels.Collection;

namespace Rime.Builders
{
    internal class CollectionBuilder
    {
        private static CollectionBuilder _instance;
        public static CollectionBuilder Instance => _instance ?? (_instance = new CollectionBuilder());
        private static BlockfrostAPI blockfrostAPI = BlockfrostAPI.Instance;
        private static RimeController _rimeController = RimeController.Instance;
        private static Ducky _ducky = Ducky.Instance;

        private CollectionViewModel _collection = new CollectionViewModel();
        private Dictionary<string, OnChainMetaData> _onChainMetaData = new();
        private CollectionBuilder() { }

        public bool Build(CollectionViewModel collection)
        {
            _collection = collection;

            _rimeController.Reset(_collection.AsCollection());

            bool built = false;
            bool hasAssets = true;
            bool gotRecords = false;
            int page = 1;
            int newAssets = 0;
            _ducky.Info("Loading Collection data...");
            do
            {
                blockfrostAPI.Assets_ByPolicy(_collection, page++, out List<BlockfrostPolicyItem> fullItems);
                var items = fullItems.Where(item => item.Quantity != 0); // Safety for 0 Quantity items
                if (!items.Any()) hasAssets = false;
                else
                {
                    if (!gotRecords) { // Gross way to call this, but I did it so its generic and can have the viewmodel type dynamically.
                        var item = items.ElementAt(2);
                        blockfrostAPI.Asset_One(_collection, item.Asset, out AssetViewModel avm);
                        Get_OnChainMetaData(item, avm);
                        gotRecords = true;
                    }

                    int matchCount = 0;
                    foreach (var item in items)
                    {
                        if (!_onChainMetaData.ContainsKey(item.Asset))
                        {
                            AssetViewModel avm = new();
                            // Check if we have a copy of the json locally before querying blockfrost.
                            if (!LocalJson(_collection, item.Asset, out avm))
                            {
                                blockfrostAPI.Asset_One(_collection, item.Asset, out avm);
                                _ducky.Info($"New {_collection.Name}: {avm.onchain_metadata.name}");
                            }

                            avm.onchain_metadata.policy_id = _collection.PolicyId;
                            avm.onchain_metadata.asset = item.Asset;

                            Add_OnChainMetaData(item, avm);

                            ++newAssets;
                        }
                        else
                        {
                            ++matchCount;
                        }
                    }
                    if (matchCount == fullItems.Count) hasAssets = false;
                }
            } while (hasAssets);

            if(newAssets > 0) _ducky.Info($"\nFound {newAssets} for {collection.Name}.");
            _ducky.Info($"{collection.Name} contains {_onChainMetaData.Count()} tokens.");
            return built;
        }

        private void Get_OnChainMetaData(BlockfrostPolicyItem item, AssetViewModel avm)
        {
            _onChainMetaData.Clear();
            avm.onchain_metadata.Get(out _onChainMetaData);
        }

        private void Add_OnChainMetaData(BlockfrostPolicyItem item, AssetViewModel avm)
        {
            _onChainMetaData.Add(item.Asset, avm.onchain_metadata.Model());
            avm.Add();
            avm.onchain_metadata.Add();
            avm.onchain_metadata.AttributeHandler();
        }

        private bool LocalJson(CollectionViewModel cvm, string asset, out AssetViewModel avm)
        {
            avm = new();
            _rimeController.Get_BlockfrostJson(asset, out string json);
            if (json != "")
            {
                AssetBuilder ab = AssetBuilder.Instance;
                ab.Parse(cvm, json, out avm);
                //_ducky.Info($"Using local Json for {avm.onchain_metadata.name}.");
                return true;
            }
            return false;
        }
    }
}
