using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rime.ADO;
using Rime.API;
using Rime.Controller;
using Rime.ViewModels.Asset;
using Rime.ViewModels.Asset.Token;
using Rime.ViewModels.Collection;
using static Rime.ViewModels.Asset.AssetViewModel;

namespace Rime.Builders
{
    public class AssetBuilder
    {
        private static AssetBuilder instance;
        public static AssetBuilder Instance => instance ?? (instance = new AssetBuilder());
        private static RimeController _rimeController = RimeController.Instance;
        private AssetBuilder() { }


        public void Parse(CollectionViewModel cvm, string json, out List<BlockfrostPolicyItem> assets)
        {
            assets = new List<BlockfrostPolicyItem>();
            //JObject deserialized = (JObject)JsonConvert.DeserializeObject(json);
            assets = JsonConvert.DeserializeObject<List<BlockfrostPolicyItem>>(json);
        }

        /// <summary>
        /// CIP 25 Standard - https://cips.cardano.org/cips/cip25/
        /// </summary>
        /// <param name="json"></param>
        /// <param name="builtAsset"></param>
        public void Parse(CollectionViewModel cvm, string json, out AssetViewModel builtAsset)
        {
            JObject deserialized = (JObject)JsonConvert.DeserializeObject(json);

            builtAsset = JsonConvert.DeserializeObject<AssetViewModel>(json);

            var onchain_metadata = deserialized.GetValue("onchain_metadata");
            if (onchain_metadata != null)
            {
                builtAsset.onchain_metadata = OnChainMetaDataViewModelHelper(cvm, JsonConvert.SerializeObject(onchain_metadata));
                foreach (JToken child in onchain_metadata.Children())
                {
                    var property = (JProperty)child;
                    string lower = property.Name.ToLower();
                    if (lower.Equals("id"))
                    {
                        builtAsset.onchain_metadata.attributes.Add("str_" + property.Name, property.Value.ToString());

                    }
                    else if (lower.Contains("attribute"))
                    {
                        //AttributesHelper(child);
                    }
                    else
                    {
                        AttributeHelper(property, out bool valid);
                        string value = AttributeCleaner(property.Value.ToString());
                        if (valid) builtAsset.onchain_metadata.attributes.Add(property.Name, value);
                    }
                }
            }
            AddRawJsonToDb(builtAsset.asset, cvm, json);
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

        private string AttributeCleaner(string value)
        {
            switch (value)
            {
                case "class": return "classAttr";
                default: return value;
            }
        }

        //private void AttributesHelper(JToken item)
        //{

        //}

        private void AddRawJsonToDb(string asset, CollectionViewModel cvm, string json)
        {
            BlockfrostJson blockfrostJson = new BlockfrostJson()
            {
                asset = asset,
                json = json,
                policy_id = cvm.PolicyId,
                policy_name = cvm.Name
            };
            _rimeController.AddJson(blockfrostJson);
        }

        private OnChainMetaDataViewModel OnChainMetaDataViewModelHelper(CollectionViewModel cvm, string json)
        {
            switch (cvm.Name)
            {
                case "KBot": return JsonConvert.DeserializeObject<KBotViewModel>(json);
                case "Pendulum": return JsonConvert.DeserializeObject<PendulumViewModel>(json);
                case "ClumsyGhosts": return JsonConvert.DeserializeObject<ClumsyGhostsViewModel>(json);
                case "GrandmasterAdventurer": return JsonConvert.DeserializeObject<GrandmasterAdventurerViewModel>(json);
				// ##_: 
                default: return JsonConvert.DeserializeObject<BaseOnChainMetaDataViewModel>(json);
            }
        }
    }
}

