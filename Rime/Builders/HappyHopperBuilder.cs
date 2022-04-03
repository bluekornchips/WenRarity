using Newtonsoft.Json.Linq;
using Rime.ADO;
using Rime.ADO.Classes;
using Rime.ADO.Classes.Tokens;
using System.Linq;
using System.Collections.Generic;
using Rime.ADO.Classes.Tokens.Rarity;
using System;
using BlockfrostQuery.Util;

namespace Rime.Builders
{
    public class HappyHopperBuilder : GenericBuilder,ITokenBuilder
    {
        public HappyHopperBuilder() : base("HappyHoppers", "11ff0e0d9ad037d18e3ed575cd35a0513b8473f83008124db89f1d8f")
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
            HappyHopper hh = new HappyHopper();
            hh.Name = jToken["name"].ToString();
            if (jToken["image"] != null) hh.Image = "https://ipfs.blockfrost.dev/ipfs/" + jToken["image"].ToString().Substring(7);

            if (jToken["Background"] != null) { hh.Background = jToken["Background"].ToString(); ++hh.TraitCount; }
            if (jToken["Eyes"] != null) { hh.Eyes = jToken["Eyes"].ToString(); ++hh.TraitCount; }
            if (jToken["Hands"] != null) { hh.Hands = jToken["Hands"].ToString(); ++hh.TraitCount; }
            if (jToken["Hat"] != null) { hh.Hat = jToken["Hat"].ToString(); ++hh.TraitCount; }
            if (jToken["Mouth"] != null) { hh.Mouth = jToken["Mouth"].ToString(); ++hh.TraitCount; }
            if (jToken["Skin"] != null) { hh.Skin = jToken["Skin"].ToString(); ++hh.TraitCount; }
            if (jToken["Wings"] != null) { hh.Wings = jToken["Wings"].ToString(); ++hh.TraitCount; }

            return hh;
        }

        public void Rarity()
        {
            Logger.Info($"Generating Rarity for {PolicyName}.");
            using (RimeContext db = new RimeContext())
            {
                List<string>[] traitsList = new List<string>[]
                {
                    (from b in db.HappyHoppers select b.Background).ToList(),
                    (from b in db.HappyHoppers select b.Eyes).ToList(),
                    (from b in db.HappyHoppers select b.Hands).ToList(),
                    (from b in db.HappyHoppers select b.Hat).ToList(),
                    (from b in db.HappyHoppers select b.Mouth).ToList(),
                    (from b in db.HappyHoppers select b.Skin).ToList(),
                    (from b in db.HappyHoppers select b.Wings).ToList()
                };
                List<int> traitsCount = (from b in db.HappyHoppers select b.TraitCount).ToList();

                db.Database.ExecuteSqlCommand($"DELETE FROM {PolicyName}Rarities");
                
                foreach (var token in Tokens)
                {
                    HappyHopper hh = (from r in db.HappyHoppers.Where(l => l.Fingerprint == token.Value.Token.Fingerprint) select r).FirstOrDefault();
                    if (hh != null)
                    {
                        int i = 0;

                        HappyHoppersRarity hhr = new HappyHoppersRarity()
                        {
                            Fingerprint = hh.Fingerprint,
                            Background = Math.Round(1.00 / ((double)traitsList[i].Where(b => b == hh.Background).ToList().Count() / traitsList[i].Count()), 7),
                            Eyes = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == hh.Eyes).ToList().Count() / traitsList[i].Count()), 7),
                            Hands = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == hh.Hands).ToList().Count() / traitsList[i].Count()), 7),
                            Hat = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == hh.Hat).ToList().Count() / traitsList[i].Count()), 7),
                            Mouth = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == hh.Mouth).ToList().Count() / traitsList[i].Count()), 7),
                            Skin = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == hh.Skin).ToList().Count() / traitsList[i].Count()), 7),
                            Wings = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == hh.Wings).ToList().Count() / traitsList[i].Count()), 7),
                            TraitCount = Math.Round(1.00 / ((double)traitsCount.Where(b => b == hh.TraitCount).ToList().Count() / traitsList[i].Count()), 7),
                        };
                        hhr.Weighting = hhr.Background
                            + hhr.Eyes
                            + hhr.Hands
                            + hhr.Hat
                            + hhr.Mouth
                            + hhr.Skin
                            + hhr.Wings
                            + hhr.TraitCount
                            ;

                        db.HappyHopperRaritys.Add(hhr);
                        db.SaveChanges();
                    }
                }
            }
        }

        public void SetAttributes()
        {
            Attributes = new[] { "Background", "Eyes", "Hands", "Hat", "Mouth", "Skin", "Wings" };
        }
    }
}
