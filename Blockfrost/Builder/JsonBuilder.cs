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

            if (input == "") return; // Empty input - Should never happen.

            JObject deserialized = JsonConvert.DeserializeObject(input) as JObject;
            BlockfrostAsset bfAsset = JsonConvert.DeserializeObject<BlockfrostAsset>(input);

            JToken onchain_metadata = deserialized.GetValue("onchain_metadata");
            var attributes = onchain_metadata["attributes"];

            // Secondary check for capital letter Attributes.
            if(attributes == null) attributes = onchain_metadata["Attributes"];

            if (onchain_metadata == null) return;

            built.model = GenerateOnChainMetaData(type, onchain_metadata.ToString());
            built.model.traitCount = 0;
            built.fingerprint = deserialized["fingerprint"].ToString();

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


            // Safety Checks
            // Required because projects do not follow standards and forget to add data. Angery.

            built.model.mediaType = "NONE"; 

            // Safety Checks

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
                    if (valid) built.attributes.Add(safeName, value);
                }
            }

            // Trait Counter
            foreach (var item in built.model.attributes)
            {
                string value = AttributeCleaner(item.Value.ToString());
                string safeValue = value.ToUpper();

                if (safeValue != "EMPTY" && safeValue != "NONE" && value != "" && !string.IsNullOrEmpty(safeValue))
                {
                    if(!CollectionSpecificOverride(safeValue)) built.model.traitCount++;
                }
            }
        }

        /// <summary>
        /// Returns the json string as a collection of json items - 0 to 100 expected.
        /// Reference: https://docs.blockfrost.io/#tag/Cardano-Assets/paths/~1assets~1policy~1{policy_id}/get
        /// </summary>
        /// <param name="json"></param>
        /// <param name="items"></param>
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

        /// <summary>
        /// Helper method for ignoring fields that do not follow CIP25
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="valid"></param>
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

        /// <summary>
        /// Helper mehod for returning SQL Table and C# Class safe column and attribute names.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string AttributeCleaner(string value)
        {
            value.Replace(" ", "");
            switch (value)
            {
                case "class": return "classAttr";
                default: return value;
            }
        }

        /// <summary>
        /// Switch handle or different _existing_ OnChainMetaData types.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        private OnChainMetaData GenerateOnChainMetaData(string type, string json)
        {
            switch (type)
            {//##_:switch+
				//##_:TavernSquad+
				case "TavernSquad":return HandleTavernSquad(json);
				//##_:TavernSquad-
				
				//##_:DeadRabbits+
				case "DeadRabbits":return HandleDeadRabbits(json);
				//##_:DeadRabbits-//##_:FalseIdols+
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

        #region Handler//##_:handle+
		//##_:TavernSquad+
		private TavernSquad HandleTavernSquad(string json)
		{
			TavernSquad model = JsonConvert.DeserializeObject<TavernSquad>(json);
			model.Back = model.attributes.GetValueOrDefault("Back");
			model.Eyes = model.attributes.GetValueOrDefault("Eyes");
			model.Face = model.attributes.GetValueOrDefault("Face");
			model.Head = model.attributes.GetValueOrDefault("Head");
			model.Race = model.attributes.GetValueOrDefault("Race");
			model.Armor = model.attributes.GetValueOrDefault("Armor");
			model.Mouth = model.attributes.GetValueOrDefault("Mouth");
			model.Racial = model.attributes.GetValueOrDefault("Racial");
			model.Familiar = model.attributes.GetValueOrDefault("Familiar");
			model.SkinTone = model.attributes.GetValueOrDefault("SkinTone");
			model.Background = model.attributes.GetValueOrDefault("Background");
			return model;
		}

		//##_:TavernSquad-
		//##_:DeadRabbits+
		private DeadRabbits HandleDeadRabbits(string json)
		{
			DeadRabbits model = JsonConvert.DeserializeObject<DeadRabbits>(json);
			return model;
		}

		//##_:DeadRabbits-//##_:FalseIdols+
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

        #endregion Hand;er

        private bool CollectionSpecificOverride(string value)
        {
            return false;
        }
    }
}





