using BlockfrostQuery.Util;
using Newtonsoft.Json.Linq;
using Rime.ADO;
using Rime.ADO.Classes;
using Rime.ADO.Classes.Tokens;
using Rime.ADO.Classes.Tokens.Rarity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rime.Builders
{
    public class WinterNaruBuilder : GenericBuilder, ITokenBuilder
    {
        public WinterNaruBuilder() : base("WinterNaru", "595e1be95d466dcf8b56691671cb903db5259d4560b0012af1f21efd")
        {
            //DeleteTableData();
            SetAttributes();
            Cleaner = Clean;
            Build();
            Rarity();
            OutputWithWeights();
        }

        public Asset Clean(JToken jToken)
        {
            WinterNaru hh = new WinterNaru();
            hh.Name = jToken["name"].ToString();
            if (jToken["image"] != null) hh.Image = "https://ipfs.blockfrost.dev/ipfs/" + jToken["image"].ToString().Substring(7);
            
            jToken = jToken["attributes"];

            if (jToken["Background"] != null) { hh.Background = jToken["Background"].ToString(); ++hh.TraitCount; }
            if (jToken["Body"] != null) { hh.Body = jToken["Body"].ToString(); ++hh.TraitCount; }
            if (jToken["Face"] != null) { hh.Face = jToken["Face"].ToString(); ++hh.TraitCount; }
            if (jToken["Headwear"] != null) { hh.Headwear = jToken["Headwear"].ToString(); ++hh.TraitCount; }

            return hh;
        }

        public void Rarity()
        {
            Logger.Info($"Generating Rarity for {PolicyName}.");
            using (RimeContext db = new RimeContext())
            {
                List<string>[] traitsList = new List<string>[] {
                    (from b in db.WinterNarus select b.Background).ToList(),
                    (from b in db.WinterNarus select b.Body).ToList(),
                    (from b in db.WinterNarus select b.Face).ToList(),
                    (from b in db.WinterNarus select b.Headwear).ToList()
                };
                List<int> traitsCount = (from b in db.WinterNarus select b.TraitCount).ToList();

                db.Database.ExecuteSqlCommand($"DELETE FROM {PolicyName}Rarities");

                foreach (var token in Tokens)
                {
                    WinterNaru foundToken = (from r in db.WinterNarus.Where(l => l.Fingerprint == token.Value.Token.Fingerprint) select r).FirstOrDefault();
                    
                    if(foundToken != null)
                    {
                        int i = 0;

                        WinterNaruRarity rarityToken = new WinterNaruRarity()
                        {
                            Fingerprint = foundToken.Fingerprint,
                            Background = Math.Round(1.00 / ((double)traitsList[i].Where(b => b == foundToken.Background).ToList().Count() / traitsList[i].Count()), 7),
                            Body = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Body).ToList().Count() / traitsList[i].Count()), 7),
                            Face = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Face).ToList().Count() / traitsList[i].Count()), 7),
                            Headwear = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Headwear).ToList().Count() / traitsList[i].Count()), 7),
                            TraitCount = Math.Round(1.00 / ((double)traitsCount.Where(b => b == foundToken.TraitCount).ToList().Count() / traitsList[i].Count()), 7),
                        };

                        rarityToken.Weighting = rarityToken.Background
                            + rarityToken.Body
                            + rarityToken.Face
                            + rarityToken.Headwear
                            + rarityToken.TraitCount
                            ;
                        db.WinterNaruRaritys.Add(rarityToken);
                        db.SaveChanges();
                    }
                }
            }
        }

        public void SetAttributes()
        {
            Attributes = new[] { "Background", "Body", "Face", "Headwear" };
        }
    }
}
