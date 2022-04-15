using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RimeTwo.ViewModels;

namespace RimeTwo.API
{
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
        public void Parse(string json, out AssetViewModel builtAsset)
        {
            builtAsset = JsonConvert.DeserializeObject<AssetViewModel>(json);
            JObject deserialized = (JObject)JsonConvert.DeserializeObject(json);

            builtAsset = JsonConvert.DeserializeObject<AssetViewModel>(json);
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
                    string lower = property.Name.ToLower();
                    if (lower.Contains("attribute"))
                    {
                        AttributesHelper(child);
                    }
                    else
                    {
                        AttributeHelper(property, out bool valid);
                        if (valid) builtAsset.onchain_metadata.attributes.Add(property.Name, property.Value.ToString());
                    }
                }
            }
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

        private void AttributesHelper(JToken item)
        {

        }
    }
}
