using Rime.ADO;
using Rime.Builders;
using Rime.Utils;
using Rime.ViewModels.Asset;
using Rime.ViewModels.Collection;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Rime.API
{
    public class BlockfrostAPI
    {
        private static BlockfrostAPI instance;
        public static BlockfrostAPI Instance => instance ?? (instance = new BlockfrostAPI());
        private static Ducky _ducky = Ducky.Instance;
        private static AssetBuilder _assetBuilder = AssetBuilder.Instance;

        public static readonly string _mainNet = "https://cardano-mainnet.blockfrost.io/api/v0/";
        public static readonly string _queryToken = "mainnetqW1HNm5UljEVmlVm5Rr7hdseDMaMZccB";
        private static readonly string _asset = $"{_mainNet}/assets/";

        private BlockfrostAPI() { }

        #region GET
        public void Asset_One(CollectionViewModel cvm, string assetName, out AssetViewModel asset)
        {
            asset = new AssetViewModel();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("project_id", _queryToken);
                try
                {
                    string json = client.GetStringAsync($"{_asset}{assetName}").Result;
                    _assetBuilder.Parse(cvm, json, out asset);
                }
                catch (Exception ex)
                {
                    _ducky.Error("API", "Asset_One", ex.Message);
                }
            }
        }

        public void Assets_ByPolicy(CollectionViewModel cvm, int page, out List<BlockfrostPolicyItem> assets)
        {
            assets = new List<BlockfrostPolicyItem>();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("project_id", _queryToken);
                try
                {
                    //a = client.GetStringAsync($"{_asset}/policy/{policy}?page={page}&order=desc").Result;
                    string json = client.GetStringAsync($"{_asset}/policy/{cvm.PolicyId}?page={page}").Result;
                    _assetBuilder.Parse(cvm, json, out assets);
                }
                catch (Exception ex)
                {
                    _ducky.Error("API", "Assets_ByPolicy", ex.Message);
                }
            }
        }
        #endregion
    }
}
