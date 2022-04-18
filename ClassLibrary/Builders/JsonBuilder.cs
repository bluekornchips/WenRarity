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
        public void AsCIPStandardModel(string input, string type, out OnChainMetaDataViewModel built)
        {
            built = new OnChainMetaDataViewModel();
            if (input == "") return;

            JObject deserialized = JsonConvert.DeserializeObject(input) as JObject;
            BlockfrostAsset bfAsset = JsonConvert.DeserializeObject<BlockfrostAsset>(input);
            JToken onchain_metadata = deserialized.GetValue("onchain_metadata");
            var attributes = onchain_metadata["attributes"];
            built.model = GenerateOnChainMetaData(type, onchain_metadata.ToString());

            if(built.model.GetType().Name == "DefaultOnChainMetaData")
            {
                built.model.attributes = attributes.ToObject<Dictionary<string, string>>();
            }

            if (onchain_metadata == null) return;
            foreach (JToken child in onchain_metadata.Children())
            {
                JProperty property = (JProperty)child;
                string lower = property.Name.ToLower();

                // Id
                if (lower.Equals("id")) built.attributes.Add("str_" + property.Name.Replace(" ", ""), property.Value.ToString());
                else
                {
                    AttributeHelper(property, out bool valid);
                    string value = AttributeCleaner(property.Value.ToString());
                    if (valid) built.attributes.Add(property.Name.Replace(" ", ""), value);
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
            value.Replace(" ", "");
            switch (value)
            {
                case "class": return "classAttr";
                default: return value;
            }
        }

        public OnChainMetaData GenerateOnChainMetaData(string type, string json)
        {
            switch (type)
            {
                //##_:switch+
				case "DeluxeBotOGCollection":return HandleDeluxeBotOGCollection(json);
				case "KBot":return HandleKBot(json);
                default: return new DefaultOnChainMetaData();
            }
        }

        //##_:handle+
		private DeluxeBotOGCollection HandleDeluxeBotOGCollection(string json)
		{
			DeluxeBotOGCollection model = JsonConvert.DeserializeObject<DeluxeBotOGCollection>(json);
			model.Hat = model.attributes.GetValueOrDefault("Hat");
			model.Face = model.attributes.GetValueOrDefault("Face");
			model.Pose = model.attributes.GetValueOrDefault("Pose");
			model.BackDrop = model.attributes.GetValueOrDefault("Back Drop");
			model.BodyPaint = model.attributes.GetValueOrDefault("Body Paint");
			model.FacePlate = model.attributes.GetValueOrDefault("Face Plate");
			return model;
		}

		private KBot HandleKBot(string json)
		{
			KBot model = JsonConvert.DeserializeObject<KBot>(json);
			model.Pet = model.attributes.GetValueOrDefault("Pet");
			return model;
		}
    }
}











