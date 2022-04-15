using Newtonsoft.Json;
using RimeTwo.Util;
using RimeTwo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace RimeTwo.API
{
    public class BlockfrostAPI
    {
        private static BlockfrostAPI instance;
        public static BlockfrostAPI Instance => instance ?? (instance = new BlockfrostAPI());
        private static Ducky _ducky = Ducky.Instance;
        private static AssetHandler _assetHandler = AssetHandler.Instance;

        public static readonly string _mainNet = "https://cardano-mainnet.blockfrost.io/api/v0/";
        public static readonly string _queryToken = "mainnetqW1HNm5UljEVmlVm5Rr7hdseDMaMZccB";
        private static readonly string _asset = $"{_mainNet}/assets/";

        private BlockfrostAPI() { }

        #region GET
        public void Asset_One(string assetName, out AssetViewModel asset)
        {
            asset = new();
            using (HttpClient client = new())
            {
                client.DefaultRequestHeaders.Add("project_id", _queryToken);
                try
                {
                    string json = client.GetStringAsync($"{_asset}{assetName}").Result;
                    _assetHandler.Parse(json, out asset);
                }
                catch (Exception ex)
                {
                    _ducky.Error("API", "Asset_One", ex.Message);
                }
            }
        }

        public void Assets_ByPolicy(string policy, int page, out List<BlockfrostPolicyItem> items)
        {
            items = new List<BlockfrostPolicyItem>();
            using (HttpClient client = new())
            {
                client.DefaultRequestHeaders.Add("project_id", _queryToken);
                try
                {
                    //a = client.GetStringAsync($"{_asset}/policy/{policy}?page={page}&order=desc").Result;
                    string json = client.GetStringAsync($"{_asset}/policy/{policy}?page={page}").Result;
                    items = JsonConvert.DeserializeObject<List<BlockfrostPolicyItem>>(json);
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
