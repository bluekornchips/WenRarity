using BlockfrostQuery.Util;
using Newtonsoft.Json.Linq;
using Rime.ADO;
using Rime.ADO.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rime.Builders
{
    public class BrightPalzBuilder : GenericBuilder, ITokenBuilder
    {
        public BrightPalzBuilder() : base("BrightPal", "5a46271a9b32a4517ced20be4ba1f184c2f91b1a5dd480ab639eee57")
        {
            // DeleteTableData();
            SetAttributes();
            Cleaner = Clean;
            Build();
            Rarity();
            OutputWithWeights();
        }
        public Asset Clean(JToken jToken)
        {
            BrightPal bp = new BrightPal();
            bp.Name = jToken["name"].ToString();
            if (jToken["image"] != null) bp.Image = "https://ipfs.blockfrost.dev/ipfs/" + jToken["image"].ToString().Substring(7);

            var attributes = jToken["Attributes"];
            try
            {
                bp.Background = attributes["Background"].ToString(); ++bp.TraitCount;
                if (attributes["Body"] != null) { bp.Body = attributes["Body"].ToString(); ++bp.TraitCount; }
                if (attributes["Ears"] != null) { bp.Ears = attributes["Ears"].ToString(); ++bp.TraitCount; }
                if (attributes["Face"] != null) { bp.Face = attributes["Face"].ToString(); ++bp.TraitCount; }
                if (attributes["Hair"] != null) { bp.Hair = attributes["Hair"].ToString(); ++bp.TraitCount; }
                if (attributes["Head"] != null) { bp.Head = attributes["Head"].ToString(); ++bp.TraitCount; }
            }
            catch (Exception ex)
            {
                Logger.Error("BrightPalBuilder", "Clean", ex.Message);
                throw;
            }
            return bp;
        }

        public void Rarity()
        {
            Logger.Info($"Generating Rarity for {PolicyName}.");
            using (RimeContext db = new RimeContext())
            {
                List<string>[] traitsList = new List<string>[] {
                    (from b in db.BrightPals select b.Background).ToList(),
                    (from b in db.BrightPals select b.Body).ToList(),
                    (from b in db.BrightPals select b.Ears).ToList(),
                    (from b in db.BrightPals select b.Face).ToList(),
                    (from b in db.BrightPals select b.Hair).ToList(),
                    (from b in db.BrightPals select b.Head).ToList()
                };
                List<int> traitsCount = (from b in db.WinterNarus select b.TraitCount).ToList();

                db.Database.ExecuteSqlCommand($"DELETE FROM {PolicyName}Rarities");

                foreach (var token in Tokens)
                {
                    BrightPal foundToken = (from r in db.BrightPals.Where(l => l.Fingerprint == token.Value.Token.Fingerprint) select r).FirstOrDefault();

                    if (foundToken != null)
                    {
                        int i = 0;

                        BrightPalRarity rarityToken = new BrightPalRarity()
                        {
                            Fingerprint = foundToken.Fingerprint,
                            Background = Math.Round(1.00 / ((double)traitsList[i].Where(b => b == foundToken.Background).ToList().Count() / traitsList[i].Count()), 7),
                            Body = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Body).ToList().Count() / traitsList[i].Count()), 7),
                            Ears = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Ears).ToList().Count() / traitsList[i].Count()), 7),
                            Face = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Face).ToList().Count() / traitsList[i].Count()), 7),
                            Hair = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Hair).ToList().Count() / traitsList[i].Count()), 7),
                            Head = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Head).ToList().Count() / traitsList[i].Count()), 7),
                        };

                        rarityToken.Weighting =
                            rarityToken.Background
                            + rarityToken.Body
                            + rarityToken.Ears
                            + rarityToken.Face
                            + rarityToken.Hair
                            + rarityToken.Head
                            + rarityToken.TraitCount
                            ;
                        db.BrightPalRarities.Add(rarityToken);
                        db.SaveChanges();
                    }
                }
            }
        }

        public void SetAttributes()
        {
            Attributes = new[] { "Background", "Body", "Ears", "Face", "Hair", "Head" };
        }
    }
}
