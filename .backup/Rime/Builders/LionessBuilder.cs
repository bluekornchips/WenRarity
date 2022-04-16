using BlockfrostQuery.Util;
using Newtonsoft.Json.Linq;
using Rime.ADO;
using Rime.ADO.Classes;
using Rime.Controller;
using Rime.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rime.Builders
{
    public class LionessBuilder: GenericBuilder
    {
        public LionessBuilder() : base("Lioness", "9d4c40c114d3d69d4f8209e205686db296683073c0ca5c63a8d2456e")
        {
            Build();
            Rarity();
        }

        public static bool Clean(JToken jToken, out Lioness lioness)
        {
            lioness = new Lioness();
            lioness.Name = jToken["name"].ToString();
            if (jToken["image"] != null) lioness.Image = "https://ipfs.blockfrost.dev/ipfs/" + jToken["image"].ToString().Substring(7);

            var attributes = jToken["attributes"];
            try
            {
                if (attributes["Background"] != null) lioness.Background = attributes["Background"].ToString();
                if (attributes["Clothes"] != null) lioness.Clothes = attributes["Clothes"].ToString();
                if (attributes["Expression"] != null) lioness.Expression = attributes["Expression"].ToString();
                if (attributes["Eyewear"] != null) lioness.Eyewear = attributes["Eyewear"].ToString();
                if (attributes["Fur"] != null) lioness.Fur = attributes["Fur"].ToString();
                if (attributes["Headwear"] != null) lioness.Headwear = attributes["Headwear"].ToString();
                if (attributes["Mouth"] != null) lioness.Mouth = attributes["Mouth"].ToString();
                lioness.TraitCount = attributes.Children().Count();
            }
            catch (Exception ex)
            {
                Logger.Error("LionessBuilder", "Clean", ex.Message);
                return false;
            }
            return true;
        }

        public static void Rarity()
        {
            using (RimeContext db = new RimeContext())
            {
                List<string> backgroundss = (from b in db.Lioness select b.Background).ToList();
                double background = 0.0;
                List<string> clothess = (from b in db.Lioness select b.Clothes).ToList();
                double clothes = 0.0;
                List<string> expressions = (from b in db.Lioness select b.Expression).ToList();
                double expression = 0.0;
                List<string> eyewears = (from b in db.Lioness select b.Eyewear).ToList();
                double eyewear = 0.0;
                List<string> furs = (from b in db.Lioness select b.Fur).ToList();
                double fur = 0.0;
                List<string> headwears = (from b in db.Lioness select b.Headwear).ToList();
                double headwear = 0.0;
                List<string> mouths = (from b in db.Lioness select b.Mouth).ToList();
                double mouth = 0.0;
                List<int> traitCounts = (from b in db.Lioness select b.TraitCount).ToList();
                double traitCount = 0.0;

                double weighting = 0.0;

                db.Database.ExecuteSqlCommand("DELETE FROM LionessRarities");

                foreach(var token in Tokens)
                {
                    Lioness lioness = (from r in db.Lioness.Where(l => l.Fingerprint == token.Value.Token.Fingerprint) select r as Lioness).FirstOrDefault();
                    if(lioness != null)
                    {
                        background = Math.Round(1.00 / ((double)backgroundss.Where(b => b == lioness.Background).ToList().Count() / (double) backgroundss.Count()), 7);
                        clothes = Math.Round(1.00 / ((double)clothess.Where(b => b == lioness.Clothes).ToList().Count() / (double) clothess.Count()), 7);
                        expression = Math.Round(1.00 / ((double)expressions.Where(b => b == lioness.Expression).ToList().Count() / (double) expressions.Count()), 7);
                        eyewear = Math.Round(1.00 / ((double)eyewears.Where(b => b == lioness.Eyewear).ToList().Count() / (double) eyewears.Count()), 7);
                        fur = Math.Round(1.00 / ((double)furs.Where(b => b == lioness.Fur).ToList().Count() / (double) furs.Count()), 7);
                        headwear = Math.Round(1.00 / ((double)headwears.Where(b => b == lioness.Headwear).ToList().Count() / (double) headwears.Count()), 7);
                        mouth = Math.Round(1.00 / ((double)mouths.Where(b => b == lioness.Mouth).ToList().Count() / (double) mouths.Count()), 7);
                        traitCount = Math.Round(1.00 / ((double)traitCounts.Where(b => b == lioness.TraitCount).ToList().Count() / (double) traitCounts.Count()), 7);
                        weighting = Math.Round(background + clothes + expression + eyewear + fur + headwear + mouth + traitCount, 7);

                        db.LionessRarity.Add(new LionessRarity()
                        {
                            Fingerprint = lioness.Fingerprint,
                            Background = background,
                            Clothes = clothes,
                            Expression = expression,
                            Eyewear = eyewear,
                            Fur = fur,
                            Headwear = headwear,
                            Mouth = mouth,
                            TraitCount = traitCount,
                            Weighting = weighting
                        });
                    }
                }
                db.SaveChanges();
            }
        }
    }
}
