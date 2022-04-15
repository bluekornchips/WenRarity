using RimeTwo.ADO.Asset;
using RimeTwo.ADO.Asset.Token;
using RimeTwo.ADO.Tables;
using RimeTwo.API;
using RimeTwo.Controllers;
using RimeTwo.Util;
using RimeTwo.ViewModels;
using RimeTwo.ViewModels.Collection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace RimeTwo
{
    internal class CollectionBuilder
    {
        private static CollectionBuilder _instance;
        public static CollectionBuilder Instance => _instance ?? (_instance = new CollectionBuilder());
        private static BlockfrostAPI blockfrostAPI = BlockfrostAPI.Instance;
        private static DBController _dbController = DBController.Instance;
        private static Ducky _ducky = Ducky.Instance;

        private CollectionViewModel _collection = new CollectionViewModel();
        private Dictionary<string, AssetModel> assets = new();
        private CollectionBuilder() { }

        public bool Build(CollectionViewModel collection)
        {
            _collection = collection;

            bool built = false;
            bool hasAssets = true;
            int page = 1;
            GetAssets();
            do
            {
                blockfrostAPI.Assets_ByPolicy(_collection.PolicyId, page++, out List<BlockfrostPolicyItem> items);
                if (!items.Any()) hasAssets = false;
            } while (hasAssets);

            return built;
        }

        private void GetAssets()
        {
            assets.Clear();
            _dbController.RetrieveAssetByPolicy(typeof(KBotModel));
        }
    }

    internal class FrameworkBuilder
    {
        private static FrameworkBuilder _instance;
        public static FrameworkBuilder Instance => _instance ?? (_instance = new FrameworkBuilder());
        private RimeWriter _rimeWriter = RimeWriter.Instance;

        private static BlockfrostAPI blockfrostAPI = BlockfrostAPI.Instance;
        private static DBController _dbController = DBController.Instance;
        private static Ducky _ducky = Ducky.Instance;
        private CollectionViewModel _collection = new CollectionViewModel();
        private FrameworkBuilder() { }
        public CollectionViewModel Build()
        {
            _collection = new CollectionViewModel();
            //Console.WriteLine("Enter Policy ID: ");
            //string policyId = Console.ReadLine();

            //Console.WriteLine("Enter Collection Name: ");
            //string collectionName = Console.ReadLine();
            _collection.PolicyId = "3f00d83452b4ead45cf5e0ca811fe8da561dfc45a5e414c88c4d8759";
            string collectionName = "KBot";
            _collection.Name = collectionName;
            CreateNew();
            return _collection;
        }
        private void CreateNew()
        {
            if (!_dbController.CollectionExists(_collection.AsModel()))
            {
                _ducky.Info($"Creating collection for {_collection.Name}");
                WriteNew(out bool wrote);
                if (!wrote) return;
                _dbController.AddCollection(_collection.AsModel());
            }
        }

        private void WriteNew(out bool wrote)
        {
            int page = 1;
            wrote = false;
            // Check the policy
            blockfrostAPI.Assets_ByPolicy(_collection.PolicyId, page, out List<BlockfrostPolicyItem> items);

            try
            {
                if (items.Any())
                {
                    // Use the first item in sequence that has a quantity greater than 0.
                    int index = 0;
                    while (items[index].Quantity == 0) ++index;
                    BlockfrostPolicyItem item = items[index];
                    blockfrostAPI.Asset_One(item.Asset, out AssetViewModel asset);
                    _rimeWriter.BuildViewModel(_collection.Name, asset);
                    _rimeWriter.BuildModel(_collection.Name, asset);
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