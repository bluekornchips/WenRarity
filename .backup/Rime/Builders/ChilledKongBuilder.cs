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
    public class ChilledKongBuilder : GenericBuilder, ITokenBuilder
    {
        public ChilledKongBuilder() : base("ChilledKong", "c56d4cceb8a8550534968e1bf165137ca41e908d2d780cc1402079bd")
        {
            //DeleteTableData();
            Rarity();
            OutputRarityToCSV();
        }

        public Asset Clean(JToken jToken)
        {
            ChilledKong ck = new ChilledKong();
            ck.Name = jToken["name"].ToString();
            if (jToken["image"] != null) ck.Image = "https://ipfs.blockfrost.dev/ipfs/" + jToken["image"].ToString().Substring(7);

            var attributes = jToken["attributes"];
            try
            {
                ck.Background = jToken["background"].ToString();
                if (jToken["body"] != null) { ck.Body = jToken["body"].ToString(); ++ck.TraitCount; }
                if (jToken["clothes"] != null) { ck.Clothes = jToken["clothes"].ToString(); ++ck.TraitCount; }
                if (jToken["earrings"] != null) { ck.Earrings = jToken["earrings"].ToString(); ++ck.TraitCount; }
                if (jToken["eyes"] != null) { ck.Eyes = jToken["eyes"].ToString(); ++ck.TraitCount; }
                if (jToken["hat"] != null) { ck.Hat = jToken["hat"].ToString(); ++ck.TraitCount; }
                if (jToken["image"] != null) { ck.Image = jToken["image"].ToString(); }
                if (jToken["mouth"] != null) { ck.Mouth = jToken["mouth"].ToString(); ++ck.TraitCount; }
                if (jToken["special"] != null) { ck.Special = jToken["special"].ToString(); ++ck.TraitCount; }
            }
            catch (Exception ex)
            {
                Logger.Error("ChilledKongBuilder", "Clean", ex.Message);
                throw;
            }
            return ck;
        }
        public void Rarity()
        {
            using (RimeContext db = new RimeContext())
            {
                List<string> backgroundsList = (from b in db.ChilledKong select b.Background).ToList();
                double background = 0.0;
                List<string> bodyList = (from b in db.ChilledKong select b.Body).ToList();
                double body = 0.0;
                List<string> clothesList = (from b in db.ChilledKong select b.Clothes).ToList();
                double clothes = 0.0;
                List<string> earringsList = (from b in db.ChilledKong select b.Earrings).ToList();
                double earrings = 0.0;
                List<string> eyesList = (from b in db.ChilledKong select b.Eyes).ToList();
                double eyes = 0.0;
                List<string> hatList = (from b in db.ChilledKong select b.Hat).ToList();
                double hat = 0.0;
                List<string> mouthList = (from b in db.ChilledKong select b.Mouth).ToList();
                double mouth = 0.0;
                List<string> specialList = (from b in db.ChilledKong select b.Special).ToList();
                double special = 0.0;
                List<int> traitCounts = (from b in db.ChilledKong select b.TraitCount).ToList();
                double traitCount = 0.0;

                double weighting = 0.0;

                db.Database.ExecuteSqlCommand($"DELETE FROM ChilledKongRarities");

                foreach (var token in Tokens)
                {
                    ChilledKong ck = (from r in db.ChilledKong.Where(l => l.Fingerprint == token.Value.Token.Fingerprint) select r as ChilledKong).FirstOrDefault();
                    if (ck != null)
                    {
                        background = Math.Round(1.00 / ((double)backgroundsList.Where(b => b == ck.Background).ToList().Count() / (double)backgroundsList.Count()), 7);
                        body = Math.Round(1.00 / ((double)bodyList.Where(b => b == ck.Body).ToList().Count() / (double)bodyList.Count()), 7);
                        clothes = Math.Round(1.00 / ((double)clothesList.Where(b => b == ck.Clothes).ToList().Count() / (double)clothesList.Count()), 7);
                        earrings = Math.Round(1.00 / ((double)earringsList.Where(b => b == ck.Earrings).ToList().Count() / (double)earringsList.Count()), 7);
                        eyes = Math.Round(1.00 / ((double)eyesList.Where(b => b == ck.Eyes).ToList().Count() / (double)eyesList.Count()), 7);
                        hat = Math.Round(1.00 / ((double)hatList.Where(b => b == ck.Hat).ToList().Count() / (double)hatList.Count()), 7);
                        mouth = Math.Round(1.00 / ((double)mouthList.Where(b => b == ck.Mouth).ToList().Count() / (double)mouthList.Count()), 7);
                        special = Math.Round(1.00 / ((double)specialList.Where(b => b == ck.Special).ToList().Count() / (double)specialList.Count()), 7);
                        traitCount = Math.Round(1.00 / ((double)traitCounts.Where(b => b == ck.TraitCount).ToList().Count() / (double)traitCounts.Count()), 7);
                        weighting = Math.Round(background + clothes + earrings + eyes + hat + mouth + special + traitCount, 7);

                        db.ChilledKongRarity.Add(new ChilledKongRarity()
                        {
                            Fingerprint = ck.Fingerprint,
                            Background = background,
                            Clothes = clothes,
                            Earrings = earrings,
                            Eyes = eyes,
                            Hat = hat,
                            Mouth = mouth,
                            Special = special,
                            TraitCount = traitCount,
                            Weighting = weighting
                        });
                    }
                }
                db.SaveChanges();
            }
        }

        public void SetAttributes()
        {
            Attributes = new[]{ "Background", "Body", "Clothes", "Earrings", "Eyes", "Hat", "Mouth", "Special" };
        }
    }
}
