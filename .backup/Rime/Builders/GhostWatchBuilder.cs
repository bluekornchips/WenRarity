using BlockfrostQuery.Util;
using Newtonsoft.Json.Linq;
using Rime.ADO;
using Rime.ADO.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Rime.Builders
{
    public class GhostWatchBuilder : GenericBuilder, ITokenBuilder
    {
        string filePath = "";
        public GhostWatchBuilder() : base("GhostWatches", "b000e43ed65c89e305bdb5920001558d9f642f3488154b2552a3ad63")
        {
            //DeleteTableData();
            SetAttributes();
            Cleaner = Clean;
            Build();
            //ScanImage();
            Rarity();
            OutputWithWeights();
        }

        public Asset Clean(JToken jToken)
        {
            GhostWatch gw = new();
            gw.Name = jToken["name"].ToString();
            if (jToken["image"] != null) gw.Image = "https://ipfs.blockfrost.dev/ipfs/" + jToken["image"].ToString().Substring(7);
            try
            {
                if (jToken["background"] != null) { gw.Background = jToken["background"].ToString(); ++gw.TraitCount; }
                if (jToken["tier"] != null) { gw.Tier = jToken["tier"].ToString(); ++gw.TraitCount; }
                if (jToken["frame"] != null) { gw.Frame = jToken["frame"].ToString(); ++gw.TraitCount; }
                if (jToken["tickers"] != null)
                {
                    var tickers = jToken["tickers"].ToString();
                    var splitTickers = tickers.Split('\u002C');
                    ++gw.TraitCount;

                    if (tickers.Contains("Ethereum")) { gw.ETH = "ETH"; }
                    if (tickers.Contains("Solan")) { gw.SOL = "SOL"; }
                    if (tickers.Contains("Polkadot")) { gw.DOT = "DOT"; }
                    if (tickers.Contains("Avalanche")) { gw.AVAX = "AVAX"; }
                    if (tickers.Contains("Binance Coin")) { gw.BNB = "BNB"; }
                    if (tickers.Contains("Bitcoin")) { gw.BTC = "BTC"; }
                    if (tickers.Contains("Polygon")) { gw.MATIC = "MATIC"; }
                    if (tickers.Contains("Shiba Inu")) { gw.SHIB = "SHIB"; }
                    if (tickers.Contains("Dogecoin")) { gw.DOGE = "DOGE"; }
                    if (tickers.Contains("Cardano")) { gw.ADA = "ADA"; }

                    //tickers = tickers.Replace("{", "");
                    //tickers = tickers.Replace("}", "");
                    //tickers = tickers.Replace(" ", "");
                    //tickers = tickers.Replace(",", ";");
                    //tickers += ";";
                    //foreach (var item in splitTickers)
                    //{
                    //}
                    //gw.Tickers = tickers;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("GhostWatchBuilder", "Clean", ex.Message);
                throw;
            }
            return gw;
        }

        public void ImageActions(string image, string name)
        {
            filePath = $"E:\\Other\\Projects\\Rime\\Data\\GhostWatch\\{name}.html";
            using WebClient web = new();
            try
            {
                if (!File.Exists(filePath))
                {
                    File.WriteAllText(filePath, web.DownloadString(image));
                }
                var htmlFile = File.ReadAllText(filePath);
                if (htmlFile != null)
                {
                    if (htmlFile.Contains("==="))
                    {
                        Console.WriteLine($"EQUALITY CHECK for {name}.");
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        public void ScanImage()
        {
            //Logger.Info($"ScanningImages for {PolicyName}.");

            //using (RimeContext db = new RimeContext())
            //{
            //    List<string>[] traitsList = new List<string>[] {
            //        (from b in db.GhostWatches select b.Background).ToList(),
            //        (from b in db.GhostWatches select b.Frame).ToList(),
            //        (from b in db.GhostWatches select b.Tier).ToList(),
            //        (from b in db.GhostWatches select b.Tickers).ToList(),
            //    };
            //    List<int> traitsCount = (from b in db.GhostWatches select b.TraitCount).ToList();

            //    db.Database.ExecuteSqlCommand($"DELETE FROM {PolicyName}Rarities");

            //    foreach (var token in Tokens)
            //    {
            //        GhostWatch foundToken = (from r in db.GhostWatches.Where(l => l.Fingerprint == token.Value.Token.Fingerprint) select r).FirstOrDefault();
            //        ImageActions(foundToken.Image, foundToken.Name);
            //    }
            //}

        }
        public void Rarity()
        {
            Logger.Info($"Generating Rarity for {PolicyName}.");
            using RimeContext db = new();
            List<string>[] traitsList = new List<string>[] {
                    (from b in db.GhostWatches select b.Background).ToList(),
                    (from b in db.GhostWatches select b.Frame).ToList(),
                    (from b in db.GhostWatches select b.Tier).ToList(),
                    (from b in db.GhostWatches select b.ETH).ToList(),
                    (from b in db.GhostWatches select b.SOL).ToList(),
                    (from b in db.GhostWatches select b.DOT).ToList(),
                    (from b in db.GhostWatches select b.AVAX).ToList(),
                    (from b in db.GhostWatches select b.BNB).ToList(),
                    (from b in db.GhostWatches select b.BTC).ToList(),
                    (from b in db.GhostWatches select b.MATIC).ToList(),
                    (from b in db.GhostWatches select b.SHIB).ToList(),
                    (from b in db.GhostWatches select b.DOGE).ToList(),
                    (from b in db.GhostWatches select b.ADA).ToList(),
                };
            List<int> traitsCount = (from b in db.GhostWatches select b.TraitCount).ToList();

            db.Database.ExecuteSqlCommand($"DELETE FROM {PolicyName}Rarities");

            foreach (var token in Tokens)
            {
                GhostWatch foundToken = (from r in db.GhostWatches.Where(l => l.Fingerprint == token.Value.Token.Fingerprint) select r).FirstOrDefault();

                if (foundToken != null)
                {
                    int i = 0;

                    GhostWatchesRarity rarityToken = new()
                    {
                        Fingerprint = foundToken.Fingerprint,
                        Background = Math.Round(1.00 / ((double)traitsList[i].Where(b => b == foundToken.Background).ToList().Count / traitsList[i].Count), 7),
                        Frame = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Frame).ToList().Count / traitsList[i].Count), 7),
                        Tier = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.Tier).ToList().Count / traitsList[i].Count), 7),
                        ETH = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.ETH).ToList().Count / traitsList[i].Count), 7),
                        SOL = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.SOL).ToList().Count / traitsList[i].Count), 7),
                        DOT = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.DOT).ToList().Count / traitsList[i].Count), 7),
                        AVAX = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.AVAX).ToList().Count / traitsList[i].Count), 7),
                        BNB = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.BNB).ToList().Count / traitsList[i].Count), 7),
                        BTC = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.BTC).ToList().Count / traitsList[i].Count), 7),
                        MATIC = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.MATIC).ToList().Count / traitsList[i].Count), 7),
                        SHIB = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.SHIB).ToList().Count / traitsList[i].Count), 7),
                        DOGE = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.DOGE).ToList().Count / traitsList[i].Count), 7),
                        ADA = Math.Round(1.00 / ((double)traitsList[++i].Where(b => b == foundToken.ADA).ToList().Count / traitsList[i].Count), 7),
                    };

                    rarityToken.Weighting =
                        rarityToken.Background
                        + rarityToken.Frame
                        + rarityToken.Tier
                        + rarityToken.ETH
                        + rarityToken.SOL
                        + rarityToken.DOT
                        + rarityToken.AVAX
                        + rarityToken.BNB
                        + rarityToken.BTC
                        + rarityToken.MATIC
                        + rarityToken.SHIB
                        + rarityToken.DOGE
                        + rarityToken.ADA
                        + rarityToken.TraitCount
                        ;
                    db.GhostWatchesRarity.Add(rarityToken);
                    db.SaveChanges();
                }
            }
        }

        public void SetAttributes()
        {
            Attributes = new[] { "Background", "Frame", "Tier", "ETH", "SOL", "DOT", "AVAX", "BNB", "BTC", "MATIC", "SHIB", "DOGE", "ADA" };
        }
    }
}
