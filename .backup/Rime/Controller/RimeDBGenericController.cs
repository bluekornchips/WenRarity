using BlockfrostQuery.Util;
using Rime.ADO.Classes;
using Rime.ViewModels;
using System;
using System.Data.SqlClient;

namespace Rime.Controller
{
    public class RimeDBGenericController
    {

        private static readonly string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\DB\Rime\Rime.mdf;Integrated Security=True;Connect Timeout=30";

        public static bool SelectAll(string table)
        {
            bool result = false;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string queryString = $"SELECT * FROM {table}";
                conn.Close();
                result = true;
            }

            return result;
        }

        public static bool AddToken(TokenViewModel token)
        {
            bool status = false;
            status = AddToToken(token);
            status = AddToPolicyTable(token);
            return status;
        }

        private static bool AddToToken(TokenViewModel token)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string insert = "";

                    // Insert into Token
                    insert = $"INSERT INTO Token (" +
                    "[_Asset]," +
                    "[PolicyID]," +
                    "[PolicyName]," +
                    "[AssetName]," +
                    "[Fingerprint]," +
                    "[Quantity]," +
                    "[InitialMintTxHash]," +
                    "[MintOrBurnCount])" +
                    " VALUES (" +
                    $"'{token._Asset}'," +
                    $"'{token.PolicyID}'," +
                    $"'{token.PolicyName}'," +
                    $"'{token.AssetName}'," +
                    $"'{token.Fingerprint}'," +
                    $"{token.Quantity}," +
                    $"'{token.InitialMintTxHash}'," +
                    $"{token.MintOrBurnCount}" +
                    $");";

                    SqlCommand cmd = new SqlCommand(insert, connection);
                    cmd.ExecuteNonQuery();

                    connection.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("RimeDBGenericController", "AddToToken", ex.Message);
                return false;
            }
        }

        private static bool AddToPolicyTable(TokenViewModel token)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string insert = "";
                    if (token.PolicyName == "Lioness")
                    {
                        Lioness lioness = (Lioness)token.Asset;
                        // Insert into Token
                        insert = $"INSERT INTO {token.PolicyName} (" +
                        "[Fingerprint]," +
                        "[Background]," +
                        "[Clothes]," +
                        "[Eyewear]," +
                        "[Fur]," +
                        "[Headwear]," +
                        "[Mouth]," +
                        "[TraitCount])" +
                        " VALUES (" +
                        $"'{token.Fingerprint}'," +
                        $"'{lioness.Background}'," +
                        $"'{lioness.Clothes}'," +
                        $"'{lioness.Eyewear}'," +
                        $"'{lioness.Fur}'," +
                        $"'{lioness.Headwear}'," +
                        $"'{lioness.Mouth}'," +
                        $"{lioness.TraitCount}" +
                        $");";

                        SqlCommand cmd = new SqlCommand(insert, connection);
                        cmd.ExecuteNonQuery();
                    }
                    connection.Close();
                    Logger.Info($"New asset for {token.PolicyName}: {token.AssetName}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("RimeDBGenericController", "AddToMatchingTable", ex.Message);
                return false;
            }
        }
    }
}
