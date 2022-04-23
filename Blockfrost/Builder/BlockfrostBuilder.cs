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
    /// <summary>
    /// Blockfrost Project
    /// Calls Blockfrost API, stores JSON, builds Asset and OnChainMetaData tokens.
    /// </summary>
    public class BlockfrostBuilder
    {
        private static BlockfrostBuilder instance;
        public static BlockfrostBuilder Instance => instance ?? (instance = new BlockfrostBuilder());
        private BlockfrostBuilder() { }


        Ducky _ducky = Ducky.Instance;
        BlockfrostController _blockfrostController = BlockfrostController.Instance;
        JsonBuilder _jsonBuilder = JsonBuilder.Instance;
        BlockfrostAPI _blockfrostAPI = BlockfrostAPI.Instance;
        OnChainMetaDataModelHandler _onChainMetaDataModelHandler = OnChainMetaDataModelHandler.Instance;
        FrameworkDirector frameworkDirector = FrameworkDirector.Instance;

        private Dictionary<string, OnChainMetaData> _assets = new();
        private string _type = "DefaultOnChainMetaData";
        private Collection _collection = new Collection();

        public void Build(Collection collection)
        {
            _collection = collection;
            _type = _collection.Name;

            if (!_blockfrostController.CollectionExists(_collection.PolicyId))
            {
                bool overwrite = false;

                if (NewCollection(overwrite))
                {
                    return;
                };
            }

            if (_blockfrostController.CollectionExists(_collection.PolicyId))
            {
                _blockfrostController.DeleteTokenTable(collection);
            }

            ExistingRecords();
            RetrieveJson();
        }


        /// <summary>
        /// Create a new Collection
        /// - Updates Collection Table
        /// - Creates the NFT Asset class.
        /// 
        /// Always needs to have the Database updated after running
        ///     update-databaase
        /// </summary>
        /// 
        /// <param name="overwrite"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private bool NewCollection(bool overwrite = false)
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
            frameworkDirector.bf.CreateToken(_collection, vm, overwrite, out bool isNewCollection);

            // Add the collection to the db.
            _blockfrostController.AddCollection(_collection);

            // Complain if any errors.
            if (!frameworkDirector.bf.Build(_collection, vm)) throw new Exception("");
            return isNewCollection;
        }

        /// <summary>
        /// Get existing records, if any.
        /// </summary>
        public void ExistingRecords()
            => _blockfrostController.GetOnChainMetaDataAsModel(_collection.Name, out _assets);

        /// <summary>
        /// Checks if we have a local copy of the json and returns it and a true value.
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="json"></param>
        /// <returns></returns>
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

            _ducky.Info($"Building collection for {_collection.Name}.");

            do
            {
                _blockfrostAPI.Assets_ByPolicy(_collection.PolicyId, ++page, out string policyJson);

                if (policyJson == "[]")
                {
                    _ducky.Info("End of collection data from Blockfrost API.");
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
                            addedFromDb = false;
                            _blockfrostAPI.Asset(policyItem.Asset, out assetJson);
                            _blockfrostController.JsonAdd(new BlockfrostItemJson()
                            {
                                policy_id = _collection.PolicyId,
                                policy_name = _collection.Name,
                                asset = policyItem.Asset,
                                json = assetJson
                            });
                        }
                        
                        _jsonBuilder.AsCIPStandardModel(assetJson, _type, out OnChainMetaDataViewModel vm);
                        
                        // Update model attributes with related VM info.
                        vm.model.asset = policyItem.Asset;
                        vm.model.policy_id = _collection.PolicyId;
                        vm.model.fingerprint = vm.fingerprint;

                        // Add to the database with the appropriate class information and table data.
                        _onChainMetaDataModelHandler.Add(vm.AsModel(_type));

                        // Add to the in memory dictionary.
                        _assets.Add(policyItem.Asset,vm.model);

                        // If the item is not found locally, we have a new item.
                        if(!addedFromDb) _ducky.Info($"Added {vm.model.name}.");
                    }

                    // All items are accounted for, safe to exit.
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
