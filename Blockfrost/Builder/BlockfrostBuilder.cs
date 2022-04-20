using Blockfrost.Controller;
using WenRarityLibrary.ADO.Blockfrost;
using WenRarityLibrary.ADO.Blockfrost.Models;
using WenRarityLibrary.Builders;
using WenRarityLibrary.ADO.Blockfrost.Models.OnChainMetaData;
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
        private static FrameworkDirector frameworkDirector = FrameworkDirector.Instance;

        private Dictionary<string, OnChainMetaData> _assets = new();
        private string _type = "DefaultOnChainMetaData";
        private Collection _collection = new Collection();

        public void Build(Collection collection)
        {
            _collection = collection;
            _type = _collection.Name;


            bool reset = true;

            // Helper for reseting data.
            if (reset)
            {
                _blockfrostController.Delete(_collection);
                //_blockfrostController.AddCollection(_collection);

                FrameworkBuilder fb = new();
                fb.RemoveAllCollectionInfoFromFiles(_collection);
            }


            if (!_blockfrostController.CollectionExists(_collection.PolicyId))
            {
                NewCollection();
                return;
            }


            ExisitingRecords();
            RetrieveJson();
        }

        private void NewCollection()
        {


            // Get a sample of the data from the first page.
            _blockfrostAPI.Assets_ByPolicy(_collection.PolicyId, 1, out string policyJson);
            
            // Use the sample data to generate policy items.
            _jsonBuilder.AsBlockfrostPolicyItems(policyJson, out List<BlockfrostPolicyItem> policyItems);
            
            // Remove the 0 quantity items (burned).
            policyItems = policyItems.Where(i => i.Quantity > 0).ToList();
            
            // Select the first entry.
            BlockfrostPolicyItem item = policyItems.FirstOrDefault();

            // Retrive the items json from blockfrost api.
            _blockfrostAPI.Asset(item.Asset, out string assetJson);

            // Create the sample model - will contain nulls!
            _jsonBuilder.AsCIPStandardModel(assetJson, _type, out OnChainMetaDataViewModel vm);

            // Create the framework token.
            frameworkDirector.bf.CreateToken(_collection, vm);

            // Add the collection to the db.
            _blockfrostController.AddCollection(_collection);

            // Complain if any errors.
            if (!frameworkDirector.bf.Build(_collection, vm, out string status)) throw new Exception(status);
        }

        /// <summary>
        /// Get existing records, if any.
        /// </summary>
        public void ExisitingRecords()
        {
            _blockfrostController.GetOnChainMetaDataAsModel(_collection.Name, out _assets);
        }

        // If there is existing json, return the item and true.
        public bool ExistingJson(string asset, out string json)
        {
            json = "";
            _blockfrostController.JsonGetOne(asset, out BlockfrostItemJson item);
            if (item == null) return false;
            json = item.json;
            return true;
        }

        /// <summary>
        /// Retrieve the assets json from either the local database copy or from blockfrostapi.
        /// </summary>
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

                // Trim 0 Quantities - Usually the first item in the list.
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

                        //_onChainMetaDataModelHandler.Add(vm.AsModel(_type));
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
