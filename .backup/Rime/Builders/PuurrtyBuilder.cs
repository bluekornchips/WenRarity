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
    public class PuurrtiesBuilder : GenericBuilder, ITokenBuilder
    {
        public PuurrtiesBuilder() : base("Puurrties", "f96584c4fcd13cd1702c9be683400072dd1aac853431c99037a3ab1e")
        {
            //DeleteTableData();
            SetAttributes();
            Cleaner = Clean;
            Build();
            Rarity();
            //OutputWithWeights();
            //Listings();
        }

        public Asset Clean(JToken jToken)
        {
            Puurrties puurty = new Puurrties();
            puurty.Name = jToken["name"].ToString();
            try
            {

                puurty.Image = "https://ipfs.blockfrost.dev/ipfs/" + jToken["image"].ToString().Substring(7);
                puurty.Fur = jToken["fur"].ToString(); if (puurty.Fur != "None") ++puurty.TraitCount;
                puurty.Hat = jToken["hat"].ToString(); if (puurty.Hat != "None") ++puurty.TraitCount;
                puurty.Eyes = jToken["eyes"].ToString(); if (puurty.Eyes != "None") ++puurty.TraitCount;
                puurty.Mask = jToken["mask"].ToString(); if (puurty.Mask != "None") ++puurty.TraitCount;
                puurty.Tail = jToken["tail"].ToString(); if (puurty.Tail != "None") ++puurty.TraitCount;
                puurty.Hands = jToken["hands"].ToString(); if (puurty.Hands != "None") ++puurty.TraitCount;
                puurty.Mouth = jToken["mouth"].ToString(); if (puurty.Mouth != "None") ++puurty.TraitCount;
                puurty.Wings = jToken["wings"].ToString(); if (puurty.Wings != "None") ++puurty.TraitCount;
                puurty.Outfit = jToken["outfit"].ToString(); if (puurty.Outfit != "None") ++puurty.TraitCount;
                puurty.Background = jToken["background"].ToString(); if (puurty.Background != "None") ++puurty.TraitCount;
            }
            catch (Exception)
            {

                throw;
            }
            return puurty;
        }

        public void Rarity()
        {
            Logger.Info($"Generating Rarity for {PolicyName}.");
            using (RimeContext db = new RimeContext())
            {
                List<string>[] traitsList = new List<string>[] {
                    (from b in db.Puurrties select b.Background).ToList(),
                    (from b in db.Puurrties select b.Eyes).ToList(),
                    (from b in db.Puurrties select b.Fur).ToList(),
                    (from b in db.Puurrties select b.Hands).ToList(),
                    (from b in db.Puurrties select b.Hat).ToList(),
                    (from b in db.Puurrties select b.Mask).ToList(),
                    (from b in db.Puurrties select b.Mouth).ToList(),
                    (from b in db.Puurrties select b.Outfit).ToList(),
                    (from b in db.Puurrties select b.Tail).ToList(),
                    (from b in db.Puurrties select b.Wings).ToList()
                };
                List<int> traitsCount = (from b in db.Puurrties select b.TraitCount).ToList();
                db.Database.ExecuteSqlCommand($"DELETE FROM {PolicyName}Rarities");

                foreach (var token in Tokens)
                {
                    Puurrties foundToken = (from r in db.Puurrties.Where(l => l.Fingerprint == token.Value.Token.Fingerprint) select r).FirstOrDefault();

                    if (foundToken != null)
                    {
                        int i = 0;

                        PuurrtiesRarity rarityToken = new PuurrtiesRarity()
                        {
                            Fingerprint = foundToken.Fingerprint,
                            Background = Math.Round(1.00 / ((double)traitsList[i].Where(b => b == foundToken.Background).ToList().Count() / traitsList[i].Count()), 7),
                            Eyes = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Eyes).ToList().Count() / traitsList[i].Count()), 7),
                            Fur = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Fur).ToList().Count() / traitsList[i].Count()), 7),
                            Hands = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Hands).ToList().Count() / traitsList[i].Count()), 7),
                            Hat = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Hat).ToList().Count() / traitsList[i].Count()), 7),
                            Mask = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Mask).ToList().Count() / traitsList[i].Count()), 7),
                            Mouth = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Mouth).ToList().Count() / traitsList[i].Count()), 7),
                            Outfit = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Outfit).ToList().Count() / traitsList[i].Count()), 7),
                            Tail = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Tail).ToList().Count() / traitsList[i].Count()), 7),
                            Wings = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Wings).ToList().Count() / traitsList[i].Count()), 7),
                            TraitCount = Math.Round(1.00 / ((double)traitsCount.Where(b => b == foundToken.TraitCount).ToList().Count() / traitsCount.Count()), 7),
                        };
                        rarityToken.Weighting =
                            rarityToken.Background
                            + rarityToken.Eyes
                            + rarityToken.Fur
                            + rarityToken.Hands
                            + rarityToken.Hat
                            + rarityToken.Mask
                            + rarityToken.Mouth
                            + rarityToken.Outfit
                            + rarityToken.Tail
                            + rarityToken.Wings
                            + rarityToken.TraitCount;
                        db.PuurrtiesRarities.Add(rarityToken);
                        db.SaveChanges();
                    }
                }
            }
        }

        public void SetAttributes()
        {
            Attributes = new[] { "Background", "Eyes", "Fur", "Hands", "Hat", "Mask", "Mouth", "Outfit", "Tail", "Wings" };
        }
    }
}
