using BlockfrostQuery.Util;
using Newtonsoft.Json.Linq;
using Rime.ADO;
using Rime.ADO.Classes;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Rime.Builders
{
    public class ElMatadorBuilder : GenericBuilder, ITokenBuilder
    {
        public ElMatadorBuilder() : base("ElMatador", "c76e5286fce9e6f5c9b1c5a61f74bc7fa89ed0f946ff2ae5d875f2cb")
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
            ElMatador token = new();

            try
            {

                token.Name = jToken["name"].ToString();
                token.Image = "https://ipfs.blockfrost.dev/ipfs/" + jToken["image"].ToString().Substring(7);
                if (jToken["Canvas"] != null) token.Canvas = jToken["Canvas"].ToString(); if (token.Canvas != "None") ++token.TraitCount;
                if (jToken["Chaqueta"] != null) token.Chaqueta = jToken["Chaqueta"].ToString(); if (token.Chaqueta != "None") ++token.TraitCount;
                if (jToken["Craneo"] != null) token.Craneo = jToken["Craneo"].ToString(); if (token.Craneo != "None") ++token.TraitCount;
                if (jToken["Polvo"] != null) token.Polvo = jToken["Polvo"].ToString(); if (token.Polvo != "None") ++token.TraitCount;
                if (jToken["Rociada"] != null) token.Rociada = jToken["Rociada"].ToString(); if (token.Rociada != "None") ++token.TraitCount;
            }
            catch (Exception)
            {

            }
            return token;
        }

        public void Rarity()
        {
            Logger.Info($"Generating Rarity for {PolicyName}.");
            using (RimeContext db = new RimeContext())
            {
                List<string>[] traitsList = new List<string>[] {
                    (from b in db.ElMatadors select b.Canvas).ToList(),
                    (from b in db.ElMatadors select b.Chaqueta).ToList(),
                    (from b in db.ElMatadors select b.Craneo).ToList(),
                    (from b in db.ElMatadors select b.Polvo).ToList(),
                    (from b in db.ElMatadors select b.Rociada).ToList()
                };

                List<int> traitsCount = (from b in db.ElMatadors select b.TraitCount).ToList();

                db.Database.ExecuteSqlCommand($"DELETE FROM {PolicyName}Rarities");

                foreach (var token in Tokens)
                {
                    ElMatador foundToken = (from r in db.ElMatadors.Where(l => l.Fingerprint == token.Value.Token.Fingerprint) select r).FirstOrDefault();

                    if (foundToken != null)
                    {
                        int i = 0;

                        ElMatadorRarity rarityToken = new()
                        {
                            Fingerprint = foundToken.Fingerprint,
                            Canvas = Math.Round(1.00 / ((double)traitsList[i].Where(b => b == foundToken.Canvas).ToList().Count() / traitsList[i].Count()), 7),
                            Chaqueta = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Chaqueta).ToList().Count() / traitsList[i].Count()), 7),
                            Craneo = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Craneo).ToList().Count() / traitsList[i].Count()), 7),
                            Polvo = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Polvo).ToList().Count() / traitsList[i].Count()), 7),
                            Rociada = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Rociada).ToList().Count() / traitsList[i].Count()), 7),
                            TraitCount = Math.Round(1.00 / ((double)traitsCount.Where(b => b == foundToken.TraitCount).ToList().Count() / traitsCount.Count()), 7)
                        };
                        rarityToken.Weighting =
                            rarityToken.Canvas
                            + rarityToken.Chaqueta
                            + rarityToken.Craneo
                            + rarityToken.Polvo
                            + rarityToken.Rociada
                            + rarityToken.TraitCount;
                        db.ElMatadorRarities.Add(rarityToken);
                        db.SaveChanges();
                    }
                }
            }
        }

        public void SetAttributes()
        {
            Attributes = new[] { "Canvas", "Chaqueta", "Craneo", "Polvo", "Rociada" };
        }

    }
}
