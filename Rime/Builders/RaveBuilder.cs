using BlockfrostQuery.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rime.ADO;
using Rime.ADO.Classes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rime.Builders
{
    public class RaveBuilder : GenericBuilder, ITokenBuilder
    {
        public RaveBuilder() : base("Rave", "be646c13d6999bec03aa680e87cf18cb0bc5c1e3965a1d325088fd6c")
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
            Rave rave = new Rave();
            rave.Name = jToken["name"].ToString();
            if (jToken["image"] != null) rave.Image = "https://ipfs.blockfrost.dev/ipfs/" + jToken["image"].ToString().Substring(7);

            var attributes = jToken["attributes"].Children();
            try
            {
                //var a = JsonConvert.DeserializeObject(attributes[0].ToString());
                //rave.Background = attributes[0].ToString();
                //rave.Wing = attributes[1].ToString();
                //rave.Body = attributes[2].ToString();
                //rave.Beak = attributes[3].ToString();
                //rave.Eye = attributes[4].ToString();
                if (attributes["Background"] != null) { rave.Background = attributes["Background"].First().ToString(); ++rave.TraitCount; }
                if (attributes["Wing"] != null) { rave.Wing = attributes["Wing"].First().ToString(); ++rave.TraitCount; }
                if (attributes["Body"] != null) { rave.Body = attributes["Body"].First().ToString(); ++rave.TraitCount; }
                if (attributes["Beak"] != null) { rave.Beak = attributes["Beak"].First().ToString(); ++rave.TraitCount; }
                if (attributes["Eye"] != null) { rave.Eye = attributes["Eye"].First().ToString(); ++rave.TraitCount; }
            }
            catch (Exception ex)
            {
                Logger.Error("RaveBuilder", "Clean", ex.Message);
                throw;
            }
            return rave;
        }

        public void Rarity()
        {
            Logger.Info($"Generating Rarity for {PolicyName}.");
            using (RimeContext db = new RimeContext())
            {
                List<string>[] traitsList = new List<string>[] {
                    (from b in db.Raves select b.Background).ToList(),
                    (from b in db.Raves select b.Wing).ToList(),
                    (from b in db.Raves select b.Body).ToList(),
                    (from b in db.Raves select b.Beak).ToList(),
                    (from b in db.Raves select b.Eye).ToList()
                };
                List<int> traitsCount = (from b in db.Raves select b.TraitCount).ToList();

                db.Database.ExecuteSqlCommand($"DELETE FROM {PolicyName}Rarities");

                foreach (var token in Tokens)
                {
                    Rave foundToken = (from r in db.Raves.Where(l => l.Fingerprint == token.Value.Token.Fingerprint) select r).FirstOrDefault();

                    if (foundToken != null)
                    {
                        int i = 0;

                        RaveRarity rarityToken = new RaveRarity()
                        {
                            Fingerprint = foundToken.Fingerprint,
                            Background = Math.Round(1.00 / ((double)traitsList[i].Where(b => b == foundToken.Background).ToList().Count() / traitsList[i].Count()), 7),
                            Wing = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Wing).ToList().Count() / traitsList[i].Count()), 7),
                            Body = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Body).ToList().Count() / traitsList[i].Count()), 7),
                            Beak = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Beak).ToList().Count() / traitsList[i].Count()), 7),
                            Eye = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Eye).ToList().Count() / traitsList[i].Count()), 7),
                        };

                        rarityToken.Weighting =
                            rarityToken.Background
                            + rarityToken.Wing
                            + rarityToken.Body
                            + rarityToken.Beak
                            + rarityToken.Eye
                            + rarityToken.TraitCount
                            ;
                        db.RaveRaritys.Add(rarityToken);
                        db.SaveChanges();
                    }
                }
            }
        }

        public void SetAttributes()
        {
            Attributes = new[] { "Background", "Wing", "Body", "Beak", "Eye" };
        }
    }
}
