using BlockfrostQuery.Util;
using Rime.ADO;
using Rime.ADO.Classes;
using Rime.ADO.Classes.Listings;
using Rime.ADO.Classes.Tokens;
using Rime.Util;
using Rime.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Rime.Controller
{
    public static class RimeDBContextController
    {
        public static bool Add(TokenViewModel tokenViewModel)
        {
            bool status = AddToToken(tokenViewModel.ToToken());
            if (status) status = AddToPolicyTable(tokenViewModel);
            return status;
        }

        private static bool AddToToken(Token token)
        {
            bool status = false;
            using(RimeContext db = new RimeContext())
            {
                try
                {
                    db.Tokens.Add(token);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.Error("RimeDBContextController", "AddToToken", ex.Message);
                    return status;
                }
            }
        }

        private static bool AddToPolicyTable(TokenViewModel tokenViewModel)
        {
            bool status = false;
            using (RimeContext db = new RimeContext())
            {
                try
                {
                    if(tokenViewModel.Token.PolicyName == "Lionesss") db.Lioness.Add(((Lioness)tokenViewModel.Asset));
                    else if (tokenViewModel.Token.PolicyName == "ChilledKong") db.ChilledKong.Add(((ChilledKong)tokenViewModel.Asset));
                    else if (tokenViewModel.Token.PolicyName == "CheekyUnts") db.CheekyUnts.Add(((CheekyUnt)tokenViewModel.Asset));
                    else if (tokenViewModel.Token.PolicyName == "HappyHoppers") db.HappyHoppers.Add(((HappyHopper)tokenViewModel.Asset));
                    else if (tokenViewModel.Token.PolicyName == "WinterNaru") db.WinterNarus.Add(((WinterNaru)tokenViewModel.Asset));
                    else if (tokenViewModel.Token.PolicyName == "BrightPal") db.BrightPals.Add(((BrightPal)tokenViewModel.Asset));
                    else if (tokenViewModel.Token.PolicyName == "Rave") db.Raves.Add(((Rave)tokenViewModel.Asset));
                    else if (tokenViewModel.Token.PolicyName == "GhostWatches") db.GhostWatches.Add(((GhostWatch)tokenViewModel.Asset));
                    else if (tokenViewModel.Token.PolicyName == "Puurrties") db.Puurrties.Add(((Puurrties)tokenViewModel.Asset));
                    else if (tokenViewModel.Token.PolicyName == "Pendulum") db.Pendulums.Add(((Pendulum)tokenViewModel.Asset));
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.Error("RimeDBContextController", "AddToToken", ex.Message);
                    return status;
                }
            }
        }

        //public static bool AddToListingTable()
        //{

        //}

        public static void ClearTokens(string table, string policyId)
        {
            using (RimeContext db = new RimeContext())
            {
                try
                {
                    if (table[table.Length - 1] != 's') table += "s";
                    db.Database.ExecuteSqlCommand($"DBCC CHECKIDENT ('{table}', RESEED, 0);");
                    db.Database.ExecuteSqlCommand($"DELETE FROM [Tokens] WHERE [PolicyId] = '{policyId}'");
                    db.Database.ExecuteSqlCommand($"DELETE FROM [{table}]");
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Logger.Error("RimeDBContextController", "ClearData", ex.Message);
                    throw;
                }
            }
        }

        public static void ClearListings()
        {
            using (var db = new RimeContext())
            {
                try
                {
                    db.Database.ExecuteSqlCommand($"DELETE FROM [JPGStoreListings];");
                    //db.Database.ExecuteSqlCommand($"DBCC CHECKIDENT ('[JPGStoreListings]', RESEED, 0);");
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Logger.Error("RimeDBContextController", "ClearListings", ex.Message);
                    throw;
                }
            }
        }

        public static void UpdateListingsJPG(JPGStoreListing listing)
        {
            using (var db = new RimeContext())
            {
                try
                {
                    db.JPGStoreListings.Add(listing);
                    db.SaveChanges();
                    Logger.Info($"Added {listing.asset_display_name} JPGStore listings.");
                }
                catch (Exception ex)
                {
                    Logger.Error("RimeDBContextController", "UpdateListingsJPG", ex.Message);
                    throw;
                }
            }
        }

        public static void SelectWithRarity(string query, out List<string[]> results)
        {
            results = new List<string[]>();
            using (var db = new RimeContext())
            {
                try
                {
                    using (var conn = db.Database.Connection)
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        conn.Open();
                        string[] rowResults;
                        using (var reader = cmd.ExecuteReader())
                        {
                            var rowCount = reader.FieldCount;
                            int i = 0;
                            while (reader.Read())
                            {
                                rowResults = new string[rowCount];
                                while(i < rowCount)
                                    rowResults[i] = reader.GetValue(i++).ToString();
                                i = 0;
                                results.Add(rowResults);
                            }
                        }
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("RimeDBContextController", "SelectWithRarity", ex.Message);
                    throw;
                }
            }
        }

        public static void SelectGeneric(string query, out List<string[]> results)
        {
            results = new List<string[]>();
            using (var db = new RimeContext())
            {
                try
                {
                    using (var conn = db.Database.Connection)
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        conn.Open();
                        string[] rowResults;
                        using (var reader = cmd.ExecuteReader())
                        {
                            var rowCount = reader.FieldCount;
                            int i = 0;
                            while (reader.Read())
                            {
                                rowResults = new string[rowCount];
                                while (i < rowCount)
                                    rowResults[i] = reader.GetValue(i++).ToString();
                                i = 0;
                                results.Add(rowResults);
                            }
                        }
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("RimeDBContextController", "SelectGeneric", ex.Message);
                    throw;
                }
            }
        }
    }
}
