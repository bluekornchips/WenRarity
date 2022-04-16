using BlockfrostQuery.Util;
using Newtonsoft.Json.Linq;
using Rime.ADO;
using Rime.ADO.Classes;
using Rime.API.JPGStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Rime.Builders
{
    public class PendulumBuilder : GenericBuilder, ITokenBuilder
    {
        public PendulumBuilder() : base("Pendulum", "a616aab3b18eb855b4292246bd58f9e131d7c8c25d1d1d7c88b666c4")
        {
            //DeleteTableData();
            SetAttributes();
            Cleaner = Clean;
            Build();
            Rarity();
            //OutputWithWeights();
        }

        public Asset Clean(JToken jToken)
        {
            Pendulum token = new Pendulum();

            try
            {

                token.Name = jToken["name"].ToString();
                token.Image = "https://ipfs.blockfrost.dev/ipfs/" + jToken["image"].ToString().Substring(7);
                jToken = jToken["attributes"];
                if (jToken["Background"] != null) token.Background = jToken["Background"].ToString(); if (token.Background != "") ++token.TraitCount;
                if (jToken["Body"] != null) token.Body = jToken["Body"].ToString(); if (token.Body != "") ++token.TraitCount;
                if (jToken["Ear"] != null) token.Ear = jToken["Ear"].ToString(); if (token.Ear != "") ++token.TraitCount;
                if (jToken["Eye"] != null) token.Eye = jToken["Eye"].ToString(); if (token.Eye != "") ++token.TraitCount;
                if (jToken["Eyes"] != null) token.Eyes = jToken["Eyes"].ToString(); if (token.Eyes != "") ++token.TraitCount;
                if (jToken["Head"] != null) token.Head = jToken["Head"].ToString(); if (token.Head != "") ++token.TraitCount;
                if (jToken["Mouth"] != null) token.Mouth = jToken["Mouth"].ToString(); if (token.Mouth != "") ++token.TraitCount;
                if (jToken["Skin"] != null) token.Skin = jToken["Skin"].ToString(); if (token.Skin != "") ++token.TraitCount;
            }
            catch (Exception)
            {

                throw;
            }
            return token;
        }

        public void Rarity()
        {
            Logger.Info($"Generating Rarity for {PolicyName}.");
            using (RimeContext db = new RimeContext())
            {
                List<string>[] traitsList = new List<string>[] {
                    (from b in db.Pendulums select b.Background).ToList(),
                    (from b in db.Pendulums select b.Body).ToList(),
                    (from b in db.Pendulums select b.Ear).ToList(),
                    (from b in db.Pendulums select b.Eye).ToList(),
                    (from b in db.Pendulums select b.Eyes).ToList(),
                    (from b in db.Pendulums select b.Head).ToList(),
                    (from b in db.Pendulums select b.Mouth).ToList(),
                    (from b in db.Pendulums select b.Skin).ToList()
                };
                List<int> traitsCount = (from b in db.Pendulums select b.TraitCount).ToList();
                db.Database.ExecuteSqlCommand($"DELETE FROM {PolicyName}Rarities");

                foreach (var token in Tokens)
                {
                    Pendulum foundToken = (from r in db.Pendulums.Where(l => l.Fingerprint == token.Value.Token.Fingerprint) select r).FirstOrDefault();

                    if (foundToken != null)
                    {
                        int i = 0;

                        PendulumRarity rarityToken = new PendulumRarity()
                        {
                            Fingerprint = foundToken.Fingerprint,
                            Background = Math.Round(1.00 / ((double)traitsList[i].Where(b => b == foundToken.Background).ToList().Count() / traitsList[i].Count()), 7),
                            Body = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Body).ToList().Count() / traitsList[i].Count()), 7),
                            Ear = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Ear).ToList().Count() / traitsList[i].Count()), 7),
                            Eye = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Eye).ToList().Count() / traitsList[i].Count()), 7),
                            Eyes = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Eyes).ToList().Count() / traitsList[i].Count()), 7),
                            Head = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Head).ToList().Count() / traitsList[i].Count()), 7),
                            Mouth = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Mouth).ToList().Count() / traitsList[i].Count()), 7),
                            Skin = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Skin).ToList().Count() / traitsList[i].Count()), 7),
                            TraitCount = Math.Round(1.00 / ((double)traitsCount.Where(b => b == foundToken.TraitCount).ToList().Count() / traitsCount.Count()), 7),
                        };
                        rarityToken.Weighting =
                            rarityToken.Background
                            + rarityToken.Body
                            + rarityToken.Ear
                            + rarityToken.Eye
                            + rarityToken.Eyes
                            + rarityToken.Head
                            + rarityToken.Mouth
                            + rarityToken.Skin
                            + rarityToken.TraitCount;
                        db.PendulumRarities.Add(rarityToken);
                        db.SaveChanges();
                    }
                }
            }
        }


        public void SetAttributes()
        {
            Attributes = new[] { "Background", "Body", "Ear", "Eye", "Eyes", "Head", "Mouth", "Skin" };
        }
    }
}
