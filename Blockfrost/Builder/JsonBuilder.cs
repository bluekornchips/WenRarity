using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WenRarityLibrary.ADO.Blockfrost;
using WenRarityLibrary.ADO.Blockfrost.Models.OnChainMetaData;
using WenRarityLibrary.ADO.Blockfrost.Models.OnChainMetaData.Token;
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

            // Secondary check for capital letter Attributes.
            if(attributes == null) attributes = onchain_metadata["Attributes"];

            if (onchain_metadata == null) return;

            built.model = GenerateOnChainMetaData(type, onchain_metadata.ToString());

            // New framework token handling.
            if(built.model.GetType().Name == "DefaultOnChainMetaData")
            {
                if (attributes != null)
                {
                    var unsafeAttributes = attributes.ToObject<Dictionary<string, string>>();
                    var safeAttributes = new Dictionary<string, string>();
                    foreach (var item in unsafeAttributes)
                    {
                        safeAttributes.Add(item.Key.Replace(" ", ""), item.Value);   
                    }
                    built.model.attributes = safeAttributes;
                }
            }

            built.fingerprint = deserialized["fingerprint"].ToString();
            built.model.traitCount = 0;

            built.model.mediaType = "NONE";

            foreach (JToken child in onchain_metadata.Children())
            {
                JProperty property = (JProperty)child;
                string lower = property.Name.ToLower();

                // Id - safety check.
                if (lower.Equals("id")) built.attributes.Add("str_" + property.Name.Replace(" ", ""), property.Value.ToString());
                else
                {
                    AttributeHelper(property, out bool valid);
                    string value = AttributeCleaner(property.Value.ToString());
                    string safeName = property.Name.Replace(" ", ""); // Replace spaces
                    string safeValue = value.ToUpper();
                    if(safeValue != "EMPTY" && safeValue != "NONE" && value != "" && !string.IsNullOrEmpty(safeValue))
                    {
                        built.model.traitCount++;
                    }
                    if (valid) built.attributes.Add(safeName, value);
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
				//##_:FalseIdols+
				case "FalseIdols":return HandleFalseIdols(json);
				//##_:FalseIdols-
				
				
				
				
				//##_:PuurrtyCatsSociety+
				case "PuurrtyCatsSociety":return HandlePuurrtyCatsSociety(json);
				//##_:PuurrtyCatsSociety-
				//##_:KBot+
				case "KBot":return HandleKBot(json);
				//##_:KBot-
                default: return new DefaultOnChainMetaData();
            }
        }

        //##_:handle+
		//##_:FalseIdols+
		private FalseIdols HandleFalseIdols(string json)
		{
			FalseIdols model = JsonConvert.DeserializeObject<FalseIdols>(json);
			model.Back = model.attributes.GetValueOrDefault("Back");
			model.Face = model.attributes.GetValueOrDefault("Face");
			model.Head = model.attributes.GetValueOrDefault("Head");
			model.Outfit = model.attributes.GetValueOrDefault("Outfit");
			model.Character = model.attributes.GetValueOrDefault("Character");
			model.Background = model.attributes.GetValueOrDefault("Background");
			return model;
		}

		//##_:FalseIdols-
		
		
		
		
		//##_:PuurrtyCatsSociety+
		private PuurrtyCatsSociety HandlePuurrtyCatsSociety(string json)
		{
			PuurrtyCatsSociety model = JsonConvert.DeserializeObject<PuurrtyCatsSociety>(json);
			return model;
		}

		//##_:PuurrtyCatsSociety-
		//##_:KBot+
		private KBot HandleKBot(string json)
		{
			KBot model = JsonConvert.DeserializeObject<KBot>(json);
			model.Pet = model.attributes.GetValueOrDefault("Pet");
			return model;
		}
		//##_:KBot-

    }
}

































































































































