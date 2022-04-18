using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WenRarityLibrary.ADO.Blockfrost;
using WenRarityLibrary.ADO.Rime.Models.OnChainMetaData;
using WenRarityLibrary.ADO.Rime.Models.OnChainMetaData.Token;
using WenRarityLibrary.ViewModels;

namespace WenRarityLibrary.Builders
{
    public class JsonBuilder
    {
        private static JsonBuilder instance;
        public static JsonBuilder Instance => instance ?? (instance = new JsonBuilder());
        private JsonBuilder() { }
        private static Ducky _ducky = Ducky.Instance;

        /// <summary>
        /// CIP 25 Standard - https://cips.cardano.org/cips/cip25/
        /// </summary>
        /// <param name="input"></param>
        public void AsCIPStandardModel(string input, Type type, out OnChainMetaDataViewModel built)
        {
            built = new OnChainMetaDataViewModel();
            if (input == "") return;

            JObject deserialized = JsonConvert.DeserializeObject(input) as JObject;
            BlockfrostAsset bfAsset = JsonConvert.DeserializeObject<BlockfrostAsset>(input);
            JToken onchain_metadata = deserialized.GetValue("onchain_metadata");
            built.model = GenerateOnChainMetaData(type, onchain_metadata.ToString());
            if (onchain_metadata == null) return;
            foreach (JToken child in onchain_metadata.Children())
            {
                JProperty property = (JProperty)child;
                string lower = property.Name.ToLower();

                // Id
                if (lower.Equals("id")) built.attributes.Add("str_" + property.Name, property.Value.ToString());
                else
                {
                    AttributeHelper(property, out bool valid);
                    string value = AttributeCleaner(property.Value.ToString());
                    if (valid) built.attributes.Add(property.Name, value);
                }
            }
        }

        public void AsBlockfrostPolicyItems(string json, out List<BlockfrostPolicyItem> items)
        {
            items = new();
            try
            {
                items = JsonConvert.DeserializeObject<List<BlockfrostPolicyItem>>(json);
            }
            catch (Exception ex)
            {
                _ducky.Error("JsonBuilder", "AsBlockfrostPolicyItem", ex.Message);
                throw;
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

        private string AttributeCleaner(string value)
        {
            switch (value)
            {
                case "class": return "classAttr";
                default: return value;
            }
        }

        public OnChainMetaData GenerateOnChainMetaData(Type type, string json)
        {
            switch (type.Name)
            {
                // ##_:switch+
                case "KBot":return HandleKBot(json);
                // ##_:switch-
                default: return new DefaultOnChainMetaData();
            }
        }

        // ##_:handle+
        private KBot HandleKBot(string json)
        {
            KBot kbot = JsonConvert.DeserializeObject<KBot>(json);
            kbot.Pet = kbot.attributes.GetValueOrDefault("Pet");
            return kbot;
        }
        //##_:handle-
    }
}
