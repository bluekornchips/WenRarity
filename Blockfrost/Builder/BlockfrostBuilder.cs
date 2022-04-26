using Blockfrost.Controller;
using WenRarityLibrary.Builders;
using WenRarityLibrary;
using BlockfrostLibrary.API;
using BlockfrostLibrary.ADO.Models.OnChainMetaData;
using BlockfrostLibrary.ADO.Models.Collection;
using BlockfrostLibrary.ADO;
using BlockfrostLibrary.Builders;
using BlockfrostLibrary.ViewModels;

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
        BlockfrostFrameworkBuilder _bffb = BlockfrostFrameworkBuilder.Instance;
        OnChainMetaDataModelHandler _onChainMetaDataModelHandler = OnChainMetaDataModelHandler.Instance;

        private Dictionary<string, OnChainMetaData> _assets = new();
        private string _type = "DefaultOnChainMetaData";
        private BlockfrostCollection _collection = new BlockfrostCollection();

        /// <summary>
        /// Build the collection and neccesary files.
        /// </summary>
        /// <param name="collection"></param>
        public void Build(BlockfrostCollection collection, bool overwrite = false)
        {
            _collection = collection;
            _type = _collection.Name;

            if (!_blockfrostController.CollectionExists(_collection.PolicyId))
            {
                if (NewCollection(overwrite))
                {
                    return;
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="resetJson"></param>
        public void Retrieve(BlockfrostCollection collection, bool resetJson = false)
        {
            _collection = collection;
            _type = _collection.Name;

            ExistingRecords();

            if (resetJson) JsonReset();

            RetrieveJson();
        }

        /// <summary>
        /// Reset the json for the collection. An easy helper method for resetting local state to match when onchain changes from manual intervention.
        /// </summary>
        private void JsonReset()
        {
            List<BlockfrostItemJson> jsonItems = new();
            _blockfrostController.JsonRetrieve(_collection, out jsonItems);
            _blockfrostController.JsonClearCollectionInfo(jsonItems);
            _assets.Clear();
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
            // Clear existing data, if specified
            if (overwrite) ClearTableRecords();

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
            _bffb.CreateToken(_collection, vm, overwrite, out bool isNewCollection);

            // Add the collection to the db.
            _blockfrostController.AddCollection(_collection);

            // Complain if any errors.
            if (!_bffb.Build(_collection, vm)) throw new Exception("");
            return isNewCollection;
        }

        /// <summary>
        /// Get existing records, if any.
        /// </summary>
        private void ExistingRecords()
            => _blockfrostController.GetOnChainMetaDataAsModel(_collection.Name, out _assets);

        /// <summary>
        /// Clear all the records for the given table.
        /// </summary>
        private void ClearTableRecords()
            => _blockfrostController.DeleteTokenTable(_collection);

        /// <summary>
        /// Checks if we have a local copy of the json and returns it and a true value.
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        private bool ExistingJson(string asset, out string json)
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
        private void RetrieveJson()
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
