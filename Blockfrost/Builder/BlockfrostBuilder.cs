using Blockfrost.Controller;
using WenRarityLibrary.ADO.Blockfrost;
using WenRarityLibrary.ADO.Rime.Models;
using WenRarityLibrary.Builders;
using WenRarityLibrary.ADO.Rime.Models.OnChainMetaData;
using WenRarityLibrary.API;
using WenRarityLibrary.ViewModels;
using WenRarityLibrary.ADO.Rime.Models.OnChainMetaData.Token;
using Rime.ADO;
using WenRarityLibrary;

namespace Blockfrost.Builder
{
    public class OnChainMetaDataModelHandler
    {
        private static OnChainMetaDataModelHandler instance;
        public static OnChainMetaDataModelHandler Instance => instance ?? (instance = new OnChainMetaDataModelHandler());
        private OnChainMetaDataModelHandler() { }
        private static Ducky _ducky = Ducky.Instance;

        public void Add(KBot item)
        {
            using BlockfrostADO context = new();
            var trans = context.Database.BeginTransaction();
            try
            {
                context.KBots.Add(item);
                trans.Commit();
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                _ducky.Error("OnChainMetaDataModelHandler", "Add(KBot)", ex.Message);
            }
        }
    }

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
        private Type _type = typeof(DefaultOnChainMetaData);
        private Collection _collection = new Collection();

        public void Build()
        {
            _collection = new Collection()
            {
                PolicyId = "3f00d83452b4ead45cf5e0ca811fe8da561dfc45a5e414c88c4d8759",
                Name = "KBot",
                DatabaseName = "KBots",
                RealName = "KBot"
            };
            _type = typeof(KBot);

            //if (!_blockfrostController.CollectionExists(_collection.PolicyId)) NewCollection();
            NewCollection();

            ExisitingRecords();
            RetrieveJson();
        }

        private void NewCollection()
        {
            _blockfrostAPI.Assets_ByPolicy(_collection.PolicyId, 1, out string policyJson);
            _jsonBuilder.AsBlockfrostPolicyItems(policyJson, out List<BlockfrostPolicyItem> policyItems);
            policyItems = policyItems.Where(i => i.Quantity > 0).ToList();
            BlockfrostPolicyItem item = policyItems.FirstOrDefault();
            _blockfrostAPI.Asset(item.Asset, out string assetJson);
            _jsonBuilder.AsCIPStandardModel(assetJson, _type, out OnChainMetaDataViewModel vm);
            frameworkBuilder.Build(_collection, vm);
        }

        public void ExisitingRecords()
        {
            _blockfrostController.GetOnChainMetaData(out _assets);
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

            //_blockfrostController.Reset(_collection);
            //_blockfrostController.AddCollection(_collection);

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
