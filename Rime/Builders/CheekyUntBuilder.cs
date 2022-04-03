using BlockfrostQuery.Util;
using Newtonsoft.Json.Linq;
using Rime.ADO.Classes;
using Rime.ADO.Classes.Tokens;
using System;

namespace Rime.Builders
{
    public class CheekyUntBuilder : GenericBuilder, ITokenBuilder
    {
        public CheekyUntBuilder() : base("CheekyUnts", "0db1c36331b3b789f31d596274e210e0649ae0d4e86bb9a7c92a7482")
        {
            //DeleteTableData();
            Cleaner = Clean;
            Build();
        }

        public Asset Clean(JToken jToken)
        {
            CheekyUnt cu = new CheekyUnt();
            cu.Name = jToken["name"].ToString();
            if (jToken["image"] != null) cu.Image = "https://ipfs.blockfrost.dev/ipfs/" + jToken["image"].ToString().Substring(7);

            //var jToken = jToken["attributes"];
            try
            {
                if (jToken["unt"] != null) { cu.Unt = jToken["unt"].ToString(); ++cu.TraitCount; }
                if (jToken["eyes"] != null) { cu.Eyes = jToken["eyes"].ToString(); ++cu.TraitCount; }
                if (jToken["face"] != null) { cu.Face = jToken["face"].ToString(); ++cu.TraitCount; }
                if (jToken["lefty"] != null) { cu.Lefty = jToken["lefty"].ToString(); ++cu.TraitCount; }
                if (jToken["mouth"] != null) { cu.Mouth = jToken["mouth"].ToString(); ++cu.TraitCount; }
                if (jToken["righty"] != null) { cu.Righty = jToken["righty"].ToString(); ++cu.TraitCount; }
                if (jToken["accessory"] != null) { cu.Accessory = jToken["accessory"].ToString(); ++cu.TraitCount; }
                if (jToken["hat"] != null) { cu.Hat = jToken["hat"].ToString(); ++cu.TraitCount; }
                if (jToken["location"] != null) { cu.Location = jToken["location"].ToString(); ++cu.TraitCount; }
                if (jToken["special"] != null) { cu.Special = jToken["special"].ToString(); ++cu.TraitCount; }
                if (jToken["season"] != null) { cu.Season = jToken["season"].ToString(); }
            }
            catch (Exception ex)
            {
                Logger.Error("CheekyUntBuilder", "Clean", ex.Message);
                throw;
            }
            return cu;
        }


        public static bool CleanOLD(JToken jToken, out CheekyUnt cu)
        {
            cu = new CheekyUnt();
            cu.Name = jToken["name"].ToString();
            if (jToken["image"] != null) cu.Image = "https://ipfs.blockfrost.dev/ipfs/" + jToken["image"].ToString().Substring(7);

            //var jToken = jToken["attributes"];
            try
            {
                if (jToken["unt"] != null) { cu.Unt = jToken["unt"].ToString(); ++cu.TraitCount; }
                if (jToken["eyes"] != null) { cu.Eyes = jToken["eyes"].ToString(); ++cu.TraitCount; }
                if (jToken["face"] != null) { cu.Face = jToken["face"].ToString(); ++cu.TraitCount; }
                if (jToken["lefty"] != null) { cu.Lefty = jToken["lefty"].ToString(); ++cu.TraitCount; }
                if (jToken["mouth"] != null) { cu.Mouth = jToken["mouth"].ToString(); ++cu.TraitCount; }
                if (jToken["righty"] != null) { cu.Righty = jToken["righty"].ToString(); ++cu.TraitCount; }
                if (jToken["accessory"] != null) { cu.Accessory = jToken["accessory"].ToString(); ++cu.TraitCount; }
                if (jToken["hat"] != null) { cu.Hat = jToken["hat"].ToString(); ++cu.TraitCount; }
                if (jToken["location"] != null) { cu.Location = jToken["location"].ToString(); ++cu.TraitCount; }
                if (jToken["special"] != null) { cu.Special = jToken["special"].ToString(); ++cu.TraitCount; }
                if (jToken["season"] != null) { cu.Season = jToken["season"].ToString(); }
            }
            catch (Exception ex)
            {
                Logger.Error("CheekyUntBuilder", "Clean", ex.Message);
                throw;
            }
            return true;
        }

        public void SetAttributes()
        {
            Attributes = new[] { "Unt ", "Eyes ", "Face", "Lefty ", "Mouth ", "Righty", "Accessory", "Hat", "Location", "Special", "Season" };
        }

        public void Rarity()
        {
            throw new NotImplementedException();
        }
    }
}