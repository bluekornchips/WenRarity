using MarketWatcher.API.JPGStore;
using MarketWatcher.Classes;
using MarketWatcher.Classes.JPGStore;
using MarketWatcher.Discord;
using MarketWatcher.EntityFramework.Context.MarketWatcher;
using MarketWatcher.SQL;
using MarketWatcher.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MarketWatcher.Builders
{
    internal class JPGStoreBuilder
    {
        private readonly JPGStoreAPI _jpgStoreAPI = JPGStoreAPI.GetInstance();
        private Ducky _ducky = Ducky.GetInstance();
        private DiscordBot _discord = DiscordBot.Instance;
        private RawSQLService _sqlService = RawSQLService.GetInstance();
        private CollectionDataBuilder _collectionDataBuilder = CollectionDataBuilder.GetInstance();

        private string _activeCollectionName = "";
        private string _activeCollectionNameUnderscore = "";
        private string _policy = "";
        private bool _validCollection = false;

        public JPGStoreBuilder(string policy, string collectionName, bool reset)

        {
            _activeCollectionName = collectionName;
            _activeCollectionNameUnderscore = _activeCollectionName.Replace(" ", "_");
            _policy = policy;

            if (reset) _sqlService.DropCollection(_activeCollectionNameUnderscore);

            CreateCollection(policy, out bool valid);
            _validCollection = valid;
        }

        public void Start()
        {
            int sleepTime = 60;
            if (_validCollection)
            {
                do
                {
                    UpdateFloor();
                    Listings(_policy);
                    Sales(_policy);

                    _ducky.Info($"{_activeCollectionName} sleeping for {sleepTime} seconds...");
                    Thread.Sleep(sleepTime * 1000);

                } while (true);
            }
        }

        /// <summary>
        /// Create a new collection for the policy.
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="validConnection"></param>
        private void CreateCollection(string policy, out bool validConnection)
        {
            validConnection = false;
            using (MarketWatcherContext context = new())
            {
                //First check the database for the cached records we have.
                var collection = (from coll in context.JPGStoreCollectionItems.Where(c => c.policy == policy) select coll);
                if (!collection.Any()) // Empty Collection
                {
                    //Add the policy to the JPGStoreCollectionItem table
                    //Safety check for empty strings -no cheating
                    if (_activeCollectionName != "")
                    {
                        context.JPGStoreCollectionItems.Add(new JPGStoreCollectionItem(policy, _activeCollectionName));
                        context.SaveChanges();

                        _ducky.Info($"Created new Collection: {_activeCollectionName}.");

                        InitialLoad(policy);
                        validConnection = true;
                    }
                    else
                    {
                        _ducky.Debug("JPGStoreBuilder", "CreateCollection", "Entered an empty collect name for a new collection.");
                    }
                }
                else
                {
                    validConnection = true;
                }
            }
        }

        /// <summary>
        /// Scan the policy for new listings.
        /// </summary>
        /// <param name="policy"></param>
        private void Listings(string policy)
        {
            using MarketWatcherContext context = new();
            try
            {
                // Get the most recent database entry for the collection to find the timestamp.
                // Retrieve the new JPGStore listings between the current time and the most recent database entry.

                bool updated = false;
                int page = 0;
                int newRecords = 0;

                _sqlService.RetrieveMostRecent(_activeCollectionNameUnderscore, out JPGStoreListing latest);

                while (!updated)
                {
                    newRecords = 0;
                    JToken deserialized = JsonConvert.DeserializeObject(_jpgStoreAPI.Listings(policy, page++)) as JToken;

                    if (deserialized == null) return; // Safety check for nulls.

                    List<JPGStoreListing> listings = deserialized.ToObject<JPGStoreListing[]>().ToList();

                    AddCollectionInformation(listings);

                    listings.OrderBy(l => l.listed_at);

                    foreach (var item in listings)
                    {
                        if (item.listing_id > latest.listing_id)
                        {
                            ++newRecords;
                            ListingData(item);
                            _ducky.Info($"New listing for {item.display_name}");
                            _sqlService.AddRow(item, _activeCollectionNameUnderscore);
                            UpdateFloor();
                        }
                    }
                    if (newRecords < 25) updated = true;
                }
                //_ducky.Info($"{_activeCollectionName} up to date.");
            }
            catch (Exception ex)
            {
                _ducky.Error("JPGStoreBuilder", "Listings", ex.Message);
            }

        }

        /// <summary>
        /// Scan the policy for new sales.
        /// </summary>
        /// <param name="policy"></param>
        private void Sales(string policy)
        {
            using (MarketWatcherContext context = new MarketWatcherContext())
            {
                try
                {
                    bool updated = false;
                    int page = 0;
                    int newRecords = 0;

                    _sqlService.RetrieveMostRecent(_activeCollectionNameUnderscore + "_Sales", out JPGStoreSale latest);

                    while (!updated)
                    {
                        JToken deserialized = (JToken)JsonConvert.DeserializeObject(_jpgStoreAPI.Sales(policy, page++));

                        if (deserialized == null) return;

                        List<JPGStoreSale> listings = deserialized.ToObject<JPGStoreSale[]>().ToList();

                        AddCollectionInformation(listings);

                        listings.OrderBy(l => l.listing_id);

                        foreach (var item in listings)
                        {
                            if (item.listing_id > latest.listing_id)
                            {
                                ++newRecords;
                                SaleData(item);
                                _ducky.Info($"New sale for {item.display_name}");
                                _sqlService.AddRow(item, _activeCollectionNameUnderscore + "_Sales");
                                _sqlService.Sales_Action(item, _activeCollectionNameUnderscore);
                                UpdateFloor();
                            }
                        }
                        if (newRecords < 25) updated = true;
                    }
                    //_ducky.Info($"{_activeCollectionName}_Sales up to date.");
                }
                catch (Exception ex)
                {
                    _ducky.Error("JPGStoreBuilder", "Sales", ex.Message);
                }
            }
        }

        /// <summary>
        /// Update the floor
        /// </summary>
        private void UpdateFloor()
        {
            _sqlService.RetrieveFloor(_activeCollectionNameUnderscore, out double floor);
            _sqlService.Setfloor(_activeCollectionNameUnderscore, floor);
        }

        /// <summary>
        /// Load the collection for the first time.
        /// </summary>
        /// <param name="policy"></param>
        private void InitialLoad(string policy)
        {
            //_ducky.Info($"Initial Load for {_activeCollectionName}...");

            _sqlService.CreateTable_Collection(_activeCollectionNameUnderscore);
            _sqlService.CreateTable_Sales(_activeCollectionNameUnderscore + "_Sales");

            string results = "";
            int page = 0;
            _ducky.Info($"Loading {_activeCollectionName} from JPGStore API...");

            while (results != "[]")
            {
                results = _jpgStoreAPI.Listings(policy, page++);
                JToken deserialized = (JToken)JsonConvert.DeserializeObject(results);
                if (deserialized != null)
                {
                    List<JPGStoreListing> listings = deserialized.ToObject<JPGStoreListing[]>().ToList();
                    AddCollectionInformation(listings);
                    _sqlService.AddRows(listings, _activeCollectionNameUnderscore);
                }
            }

            _ducky.Info($"Loading {_activeCollectionName} Sales from JPGStore API...");
            results = "";
            page = 0;

            while (results != "[]")
            {
                results = _jpgStoreAPI.Sales(policy, page++);
                JToken deserialized = (JToken)JsonConvert.DeserializeObject(results);
                if (deserialized != null)
                {
                    List<JPGStoreSale> listings = deserialized.ToObject<JPGStoreSale[]>().ToList();
                    AddCollectionInformation(listings);
                    _sqlService.AddRows(listings, _activeCollectionNameUnderscore + "_Sales");
                }
            }
            _ducky.Info($"Completed data load for {_activeCollectionName}.");
        }


        /// <summary>
        /// Output new listing.
        /// </summary>
        /// <param name="item"></param>
        private void ListingData(JPGStoreListing item)
        {
            _collectionDataBuilder.SetAsset(item.display_name, _activeCollectionName, out CollectionItemData collectionItemData);
            collectionItemData.jPGStoreItem = item;
            _discord.Listing(collectionItemData);
        }

        /// <summary>
        /// Output new sale.
        /// </summary>
        /// <param name="item"></param>
        private void SaleData(JPGStoreSale item)
        {
            _collectionDataBuilder.SetAsset(item.display_name, _activeCollectionName, out CollectionItemData collectionItemData);
            collectionItemData.jPGStoreItem = item;
            _discord.Sale(collectionItemData);
        }

        #region Helpers
        /// <summary>
        /// Helper method for the project name with spaces.
        /// </summary>
        /// <param name="items"></param>
        private void AddCollectionInformation(List<JPGStoreListing> items)
            => items.ForEach(item =>
            {
                item.collection_name = _activeCollectionName;
                item.collection_name_underscore = _activeCollectionNameUnderscore;
            });


        /// <summary>
        /// Helper method for the project name with spaces.
        /// </summary>
        /// <param name="items"></param>
        private void AddCollectionInformation(List<JPGStoreSale> items)
            => items.ForEach(item =>
            {
                item.collection_name = _activeCollectionName;
                item.collection_name_underscore = _activeCollectionNameUnderscore;
            });
        #endregion
    }
}
