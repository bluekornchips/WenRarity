using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RimeTwo.Util;
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
        public void Asset_One(string assetName, out Asset asset)
        {
            asset = new();
            using (HttpClient client = new())
            {
                client.DefaultRequestHeaders.Add("project_id", _queryToken);
                try
                {
                    string json = client.GetStringAsync($"{_asset}{assetName}").Result;
                    _assetHandler.Handle(json, out asset);
                    //asset = JsonConvert.DeserializeObject<Asset>(json);
                    //var sample = JsonConvert.DeserializeObject(json);
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
    public class BlockfrostPolicyItem
    {
        public string Asset { get; set; }
        public int Quantity { get; set; }
    }

    public class AssetHandler
    {
        private static AssetHandler instance;
        public static AssetHandler Instance => instance ?? (instance = new AssetHandler());
        private AssetHandler() { }

        /// <summary>
        /// CIP 25 Standard - https://cips.cardano.org/cips/cip25/
        /// </summary>
        /// <param name="json"></param>
        /// <param name="builtAsset"></param>
        public void Handle(string json, out Asset builtAsset)
        {
            builtAsset = JsonConvert.DeserializeObject<Asset>(json);
            JObject deserialized = (JObject)JsonConvert.DeserializeObject(json);

            builtAsset = JsonConvert.DeserializeObject<Asset>(json);
            //var asset = deserialized.GetValue("asset");
            //var policy_id = deserialized.GetValue("policy_id");
            //var asset_name = deserialized.GetValue("asset_name");
            //var fingerprint = deserialized.GetValue("fingerprint");

            var onchain_metadata = deserialized.GetValue("onchain_metadata");

            if (onchain_metadata != null)
            {
                foreach (JToken child in onchain_metadata.Children())
                {
                    var property = (JProperty)child;
                    AttributeHelper(property, out bool valid);
                    if (valid) builtAsset.onchain_metadata.attributes.Add(property.Name, property.Value.ToString());
                }
            }
            Console.WriteLine();




            //var onchain_metadata = deserialized.Properties().Where(v => v.Name == "onchain_metadata").First().Children();

            //var children = onchain_metadata.First().Children();

            //Dictionary<string, object> attributes = new();


            //foreach (var item in children)
            //{
            //    try
            //    {
            //        JProperty property = (JProperty)item;

            //        // Check if the attribute contains children.
            //        var propertyChildren = property.Children();

            //        var newItem = item;
            //        var itemChildren = newItem.First();
            //        while(itemChildren.Children().Count() > 0)
            //        {
            //            itemChildren = itemChildren.First();
            //        }

            //        //while(newItem.Children().Count() <= 1)
            //        //{
            //        //    if(newItem.Children().Count() == 1)
            //        //    {
            //        //        newItem = newItem.First;
            //        //    }
            //        //    Console.WriteLine();
            //        //}
            //        //if(item.Children().Count() > 1)
            //        //{
            //            //var first = item.First().First().Children();
            //        //}
            //    }
            //    catch (Exception)
            //    {

            //        throw;
            //    }
            //}
            Console.WriteLine();
            //items = JsonConvert.DeserializeObject<List<BlockfrostPolicyItem>>(json);
        }

        private void AttributeHelper(JProperty prop, out bool valid)
        {
            valid = false;
            string lower = prop.Name.ToLower();
            if (lower != "name"
                && lower != "mediatype"
                && lower != "image"
                && lower != "description"
                && lower != "files")
            {
                valid = true;
            }
        }
    }
}
