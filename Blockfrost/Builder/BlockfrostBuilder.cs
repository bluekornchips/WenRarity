using Blockfrost.Controller;
using WenRarityLibrary.ADO.Blockfrost;
using WenRarityLibrary.ADO.Rime.Models;
using WenRarityLibrary.Builders;
using WenRarityLibrary.ADO.Rime.Models.OnChainMetaData;
using WenRarityLibrary.API;
using WenRarityLibrary.ViewModels;
using WenRarityLibrary;

namespace Blockfrost.Builder
{

    public class BlockfrostBuilder
    {
        private static BlockfrostBuilder instance;
        public static BlockfrostBuilder Instance => instance ?? (instance = new BlockfrostBuilder());
        private BlockfrostBuilder() { }
        private static Ducky _ducky = Ducky.Instance;
        BlockfrostController _blockfrostController = BlockfrostController.Instance;
        JsonBuilder _jsonBuilder = JsonBuilder.Instance;
        BlockfrostAPI _blockfrostAPI = BlockfrostAPI.Instance;
        OnChainMetaDataModelHandler _onChainMetaDataModelHandler = OnChainMetaDataModelHandler.Instance;
        private static WenRarityFrameworkBuilder frameworkBuilder = WenRarityFrameworkBuilder.Instance;

        private Dictionary<string, OnChainMetaData> _assets = new();
        private string _type = "DefaultOnChainMetaData";
        private Collection _collection = new Collection();

        public void Build()
        {
            //_collection = new Collection()
            //{
            //    PolicyId = "3f00d83452b4ead45cf5e0ca811fe8da561dfc45a5e414c88c4d8759",
            //    Name = "KBot",
            //    DatabaseName = "KBot",
            //    RealName = "KBot"
            //};

            _collection = new Collection()
            {
                PolicyId = "b65ce524203b7a7d48b55ff037c847c5ec8c185cd3bdb7abad0a02d4",
                Name = "DeluxeBotOGCollection",
                DatabaseName = "DeluxeBotOGCollection",
                RealName = "DeluxeBot OG Collection"
            };

            _type = _collection.Name;

            if (!_blockfrostController.CollectionExists(_collection.PolicyId))
            {
                NewCollection();
                return;
            }

            bool reset = false;
            if (reset)
            {
                _blockfrostController.Delete(_collection);
                _blockfrostController.AddCollection(_collection);
            }

            ExisitingRecords();
            RetrieveJson();
        }

        private void NewCollection()
        {
            _blockfrostController.AddCollection(_collection);
            _blockfrostAPI.Assets_ByPolicy(_collection.PolicyId, 1, out string policyJson);
            
            _jsonBuilder.AsBlockfrostPolicyItems(policyJson, out List<BlockfrostPolicyItem> policyItems);
            
            policyItems = policyItems.Where(i => i.Quantity > 0).ToList();
            
            BlockfrostPolicyItem item = policyItems.FirstOrDefault();
            _blockfrostAPI.Asset(item.Asset, out string assetJson);
            _jsonBuilder.AsCIPStandardModel(assetJson, _type, out OnChainMetaDataViewModel vm);

            frameworkBuilder.CreateToken(_collection, vm);
            if (!frameworkBuilder.Build(_collection, vm, out string status)) throw new Exception(status);
        }

        public void ExisitingRecords()
        {
            _blockfrostController.GetOnChainMetaData(_collection.Name, out _assets);
        }

        public bool ExistingJson(string asset, out string json)
        {
            json = "";
            _blockfrostController.JsonGetOne(asset, out BlockfrostItemJson item);
            if (item == null) return false;
            json = item.json;
            return true;
        }

        public void RetrieveJson()
        {
            bool retrieved = false;

            int page = 0;
            int matchCount = 0;

            do
            {
                _blockfrostAPI.Assets_ByPolicy(_collection.PolicyId, ++page, out string policyJson);
                if (policyJson == "[]")
                {
                    retrieved = true;
                    return;
                }
                _jsonBuilder.AsBlockfrostPolicyItems(policyJson, out List<BlockfrostPolicyItem> policyItems);

                // Trim 0 Quantities
                policyItems = policyItems.Where(i => i.Quantity > 0).ToList();
                foreach (BlockfrostPolicyItem policyItem in policyItems)
                {
                    if (_assets.ContainsKey(policyItem.Asset)) ++matchCount;
                    else
                    {
                        // Check if we already have a copy of the JSON data
                        bool addedFromDb = true;
                        if (!ExistingJson(policyItem.Asset, out string assetJson))
                        {
                            _blockfrostAPI.Asset(policyItem.Asset, out assetJson);
                            addedFromDb = false;
                            _blockfrostController.JsonAdd(new BlockfrostItemJson()
                            {
                                policy_id = _collection.PolicyId,
                                policy_name = _collection.Name,
                                asset = policyItem.Asset,
                                json = assetJson
                            });
                        }
                        
                        _jsonBuilder.AsCIPStandardModel(assetJson, _type, out OnChainMetaDataViewModel vm);
                        

                        vm.model.asset = policyItem.Asset;
                        vm.model.policy_id = _collection.PolicyId;

                        _onChainMetaDataModelHandler.Add(vm.AsModel(_type));
                        _assets.Add(policyItem.Asset,vm.model);
                        if(!addedFromDb) _ducky.Info($"Added {vm.model.name}.");
                    }

                    if (matchCount == policyItems.Count())
                    {
                        retrieved = true;
                        return;
                    }
                }

            } while (!retrieved);
        }
    }
}
