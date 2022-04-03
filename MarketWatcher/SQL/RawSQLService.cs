using MarketWatcher.Classes.JPGStore;
using MarketWatcher.EntityFramework.Context.MarketWatcher;
using MarketWatcher.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketWatcher.SQL
{
    public class RawSQLService
    {

        private static RawSQLService _instance;
        private static Ducky _ducky = Ducky.getInstance();

        private RawSQLService() { }

        public static RawSQLService getInstance()
        {
            if (_instance == null) _instance = new RawSQLService();
            return _instance;
        }

        public void AddRows(List<JPGStoreListing> items, string collectionName)
        {
            items.ForEach((item) => { AddRow(item, collectionName); });
        }

        public void AddRows(List<JPGStoreSale> items, string collectionName)
        {
            items.ForEach((item) => { AddRow(item, collectionName); });
        }

        public void AddRow(JPGStoreListing listing, string collectionName)
        {
            if (listing.display_name.Contains('\''))
            {
                listing.display_name = listing.display_name.Replace("'", "''");
            }
            listing.collection_name = collectionName;
            using (MarketWatcherContext context = new MarketWatcherContext())
            {
                try
                {
                    using (var conn = context.Database.Connection)
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = $"INSERT INTO {collectionName}" +
                            $" ([asset_id]," +
                            $" [display_name]," +
                            $" [tx_hash]," +
                            $" [listing_id]," +
                            $" [listed_at]," +
                            $" [price_lovelace]," +
                            $" [collection_name])" +
                            $" VALUES ('{listing.asset_id}'," +
                            $" '{listing.display_name}'," +
                            $" '{listing.tx_hash}'," +
                            $" '{listing.listing_id}'," +
                            $" '{listing.listed_at}'," +
                            $" {listing.price_lovelace}," +
                            $" '{listing.collection_name}');";
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    _ducky.Error("SQLService", "AddRow", ex.Message);
                    throw;
                }
            }
        }

        public void AddRow(JPGStoreSale listing, string collectionName)
        {
            if (listing.display_name.Contains('\''))
            {
                listing.display_name = listing.display_name.Replace("'", "''");
            }
            listing.collection_name = collectionName;
            using (MarketWatcherContext context = new MarketWatcherContext())
            {
                try
                {
                    using (var conn = context.Database.Connection)
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = $"INSERT INTO {collectionName}" +
                            $" ([asset_id]," +
                            $" [display_name]," +
                            $" [tx_hash]," +
                            $" [listing_id]," +
                            $" [confirmed_at]," +
                            $" [price_lovelace]," +
                            $" [collection_name])" +
                            $" VALUES ('{listing.asset_id}'," +
                            $" '{listing.display_name}'," +
                            $" '{listing.tx_hash}'," +
                            $" '{listing.listing_id}'," +
                            $" '{listing.confirmed_at}'," +
                            $" {listing.price_lovelace}," +
                            $" '{listing.collection_name}');";
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    _ducky.Error("SQLService", "AddRow", ex.Message);
                    throw;
                }
            }
        }

        public void DropCollection(string collectionName)
        {
            DropTable(collectionName);
            DropTable(collectionName + "_Sales");
            using (MarketWatcherContext context = new MarketWatcherContext())
            {
                try
                {
                    using (var conn = context.Database.Connection)
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = $"DELETE FROM [dbo].[JPGStoreCollections] where [collection_name_underscore] = '{collectionName}'";
                        cmd.ExecuteNonQuery();
                        _ducky.Info($"Dropped collection entry for {collectionName}");
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    _ducky.Error("SQLService", "DropTable", ex.Message);
                    throw;
                }
            }
        }
        public void CreateTable_Collection(string collectionName)
        {
            using (MarketWatcherContext context = new MarketWatcherContext())
            {
                try
                {
                    using (var conn = context.Database.Connection)
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = $"CREATE TABLE {collectionName}" +
                            $" (asset_id varchar(255)," +
                            $" display_name varchar(255)," +
                            $" tx_hash varchar(255)," +
                            $" listing_id bigint," +
                            $" listed_at varchar(255)," +
                            $" price_lovelace bigint," +
                            $" collection_name varchar(255));";
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        _ducky.Info($"Created table {collectionName}.");
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    _ducky.Error("SQLService", "CreateTable_Collection", ex.Message);
                }
            }
        }

        public void CreateTable_Sales(string collectionName)
        {
            using (MarketWatcherContext context = new MarketWatcherContext())
            {
                try
                {
                    using (var conn = context.Database.Connection)
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = $"CREATE TABLE {collectionName}" +
                            $" (asset_id varchar(255)," +
                            $" display_name varchar(255)," +
                            $" tx_hash varchar(255)," +
                            $" listing_id bigint," +
                            $" confirmed_at varchar(255)," +
                            $" price_lovelace bigint," +
                            $" collection_name varchar(255));";
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        _ducky.Info($"Created table {collectionName}.");
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    _ducky.Error("SQLService", "CreateTable_Sales", ex.Message);
                }
            }
        }

        public void Sales_Action(JPGStoreSale sale, string collectionName)
        {
            using (MarketWatcherContext context = new MarketWatcherContext())
            {
                try
                {
                    using (var conn = context.Database.Connection)
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = $"DELETE FROM [dbo].[{collectionName}] WHERE [listing_id] = {sale.listing_id};";
                        cmd.ExecuteNonQuery();
                        _ducky.Info($"Confirmed sale for {collectionName}, {sale.display_name}");
                        //discord.Sale(sale);
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    _ducky.Error("SQLService", "Sales_Action", ex.Message);
                    throw;
                }
            }
        }

        public void DropTable(string collectionName)
        {
            using (MarketWatcherContext context = new MarketWatcherContext())
            {
                try
                {
                    using (var conn = context.Database.Connection)
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = $"IF OBJECT_ID('[dbo].[{collectionName}]', 'U') IS NOT NULL" +
                            $" BEGIN" +
                            $" DROP TABLE {collectionName}" +
                            $" END";
                        cmd.ExecuteNonQuery();
                        _ducky.Info($"Dropped table {collectionName}");
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    _ducky.Error("SQLService", "DropTable", ex.Message);
                    throw;
                }
            }
        }

        private void CheckRowsExist(string policy, string collectionName)
        {
            collectionName = collectionName.Replace(" ", "_");
            using (MarketWatcherContext context = new MarketWatcherContext())
            {
                try
                {
                    bool exists = false;
                    using (var conn = context.Database.Connection)
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = $"SELECT COUNT(*) FROM {collectionName}";
                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                exists = true;
                            }
                        }
                        conn.Close();
                    }
                    if (exists) DropTable(policy);
                }
                catch (Exception ex)
                {
                    _ducky.Error("SQLService", "CheckRowsExist", ex.Message);
                    throw;
                }
            }
        }

        public void RetrieveMostRecent(string collectionName, out JPGStoreListing results)
        {
            results = new JPGStoreListing();
            using (MarketWatcherContext context = new MarketWatcherContext())
            {
                try
                {

                    using (var conn = context.Database.Connection)
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = $"SELECT TOP(1) * FROM [dbo].[{collectionName}] ORDER BY [listing_id] DESC";
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

                                results = new JPGStoreListing(rowResults);
                            }
                        }
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    _ducky.Error("SQLService", "RetrieveMostRecent(JPGStoreListing)", ex.Message);
                }
            }
        }

        public void RetrieveMostRecent(string collectionName, out JPGStoreSale results)
        {
            results = new JPGStoreSale();
            using (MarketWatcherContext context = new MarketWatcherContext())
            {
                try
                {

                    using (var conn = context.Database.Connection)
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = $"SELECT TOP(1) * FROM [dbo].[{collectionName}] ORDER BY [listing_id] DESC";
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

                                results = new JPGStoreSale(rowResults);
                            }
                        }
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    _ducky.Error("SQLService", "RetrieveMostRecent(JPGStoreSale)", ex.Message);
                }
            }
        }

        public void RetrieveFloor(string collectionName, out double floor)
        {
            floor = 0.0;
            using (MarketWatcherContext context = new MarketWatcherContext())
            {
                try
                {
                    using (var conn = context.Database.Connection)
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = $"SELECT TOP(1) [price_lovelace] FROM [dbo].[{collectionName}] ORDER BY [price_lovelace] ASC";
                        conn.Open();
                        string[] rowResults;
                        using (var reader = cmd.ExecuteReader())
                        {
                            var rowCount = reader.FieldCount;
                            int i = 0;
                            while (reader.Read())
                            {
                                rowResults = new string[rowCount];
                                rowResults[i] = reader.GetValue(i).ToString();
                                floor = Double.Parse(rowResults[i]);
                            }
                        }
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    _ducky.Error("SQLService", "RetrieveFloor(collectionName, floor)", ex.Message);
                    throw;
                }
            }
        }

        public void Setfloor(string collectionName, double floor)
        {
            using (MarketWatcherContext context = new MarketWatcherContext())
            {
                try
                {
                    using (var conn = context.Database.Connection)
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = $"UPDATE [dbo].[JPGStoreCollectionItems] SET [floor] = {floor} WHERE [collection_name] = '{collectionName}'";
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    _ducky.Error("SQLService", "Setfloor(collectionName, floor)", ex.Message);
                    throw;
                }
            }
        }
    }
}