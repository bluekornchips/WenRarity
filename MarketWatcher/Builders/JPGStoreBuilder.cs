using MarketWatcher.API.JPGStore;
using MarketWatcher.Classes;
using MarketWatcher.Classes.JPGStore;
using MarketWatcher.Discord;
using MarketWatcher.EntityFramework.Context.MarketWatcher;
using MarketWatcher.SQL;
using MarketWatcher.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rime.ADO.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MarketWatcher.Builders
{
    internal class JPGStoreBuilder
    {
        private readonly JPGStoreAPI _jpgStoreAPI = JPGStoreAPI.getInstance();
        private Ducky _ducky = Ducky.getInstance();
        private DiscordBot _discord = DiscordBot.getInstance();
        private RawSQLService _sqlService = RawSQLService.getInstance();
        private CollectionDataBuilder _collectionDataBuilder = CollectionDataBuilder.Instance;

        private string _activeCollectionName = "";
        private string _activeCollectionNameUnderscore = "";
        private string _policy = "";
        private bool _validCollection = false;

        public JPGStoreBuilder(string policy, string collectionName)

        {
            _activeCollectionName = collectionName;
            _activeCollectionNameUnderscore = _activeCollectionName.Replace(" ", "_");
            _policy = policy;
            CreateCollection(policy, out bool valid);
            _validCollection = valid;
        }

        public void Start()
        {
            int sleepTime = 60;
            //_sqlService.DropCollection(_activeCollectionNameUnderscore); return;
            if (_validCollection)
            {
                do
                {
                    UpdateFloor();

                    Listings(_policy);
                    Sales(_policy);
                    _ducky.Info($"Sleeping for {sleepTime} seconds...");
                    Thread.Sleep(sleepTime * 1000);

                } while (true);
            }
        }

        private void CreateCollection(string policy, out bool validConnection)
        {
            validConnection = false;
            using (MarketWatcherContext context = new())
            {
                //First check the database for the cached records we have.

               var collection = (from coll in context.JPGStoreCollectionItems.Where(c => c.policy == policy) select coll);
                if (collection.Count() == 0) // Empty Collection
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
                            _ducky.Debug("JPGStoreBuilder", "Listings", "Entered an empty collect name for a new collection.");
                        }
                }
                else
                {
                    validConnection = true;
                }
            }
        }

        private void Listings(string policy)
        {
            using (MarketWatcherContext context = new())
            {
                try
                {
                    // Get the most recent database entry for the collection to find the timestamp.
                    // Retrieve the new JPGStore listings between the current time and the most recent database entry.
                    bool updated = false;
                    int page = 0;
                    int newRecords = 0;

                    _sqlService.RetrieveMostRecent(_activeCollectionNameUnderscore, out JPGStoreListing latest);

                    ListingData(latest);

                    while (!updated)
                    {
                        newRecords = 0;
                        JToken deserialized = JsonConvert.DeserializeObject(_jpgStoreAPI.GET_Listings(policy, page++)) as JToken;
                        List<JPGStoreListing> listings = deserialized.ToObject<JPGStoreListing[]>().ToList();
                        AddCollectionInformation(listings);
                        listings.OrderBy(l => l.listed_at);
                        foreach (var item in listings)
                        {
                            if (item.listing_id > latest.listing_id)
                            {
                                ++newRecords;
                                ListingData(item);
                                _sqlService.AddRow(item, _activeCollectionNameUnderscore);
                                UpdateFloor();
                            }
                        }
                        if (newRecords < 25) updated = true;
                    }
                    _ducky.Info($"{_activeCollectionName} up to date.");
                }
                catch (Exception ex)
                {
                    _ducky.Error("JPGStoreBuilder", "Listings", ex.Message);
                }
            }

        }

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
                        newRecords = 0;
                        JToken deserialized = (JToken)JsonConvert.DeserializeObject(_jpgStoreAPI.GET_Sales(policy, page++));
                        List<JPGStoreSale> listings = deserialized.ToObject<JPGStoreSale[]>().ToList();
                        AddCollectionInformation(listings);
                        listings.OrderBy(l => l.listing_id);
                        foreach (var item in listings)
                        {
                            //if(item.listed_at > latest.listed_at)
                            if (item.listing_id > latest.listing_id)
                            {
                                ++newRecords;
                                SaleData(item);
                                _sqlService.AddRow(item, _activeCollectionNameUnderscore + "_Sales");
                                _sqlService.Sales_Action(item, _activeCollectionNameUnderscore);
                                UpdateFloor();
                            }
                        }
                        if (newRecords < 25) updated = true;
                    }
                    _ducky.Info($"{_activeCollectionName}_Sales up to date.");
                }
                catch (Exception ex)
                {
                    _ducky.Error("JPGStoreBuilder", "Sales", ex.Message);
                }
            }
        }

        private void UpdateFloor()
        {
            _sqlService.RetrieveFloor(_activeCollectionNameUnderscore, out double floor);
            _sqlService.Setfloor(_activeCollectionNameUnderscore, floor);
        }

        private void InitialLoad(string policy)
        {
            _ducky.Info($"Initial Load for {_activeCollectionName}.");
            //If the table already exists for whatever reason, drop it.
           _sqlService.CreateTable_Collection(_activeCollectionNameUnderscore);
           _sqlService.CreateTable_Sales(_activeCollectionNameUnderscore + "_Sales");

            string results = "";
            int page = 0;
            _ducky.Info($"Loading {_activeCollectionName} from JPGStore API...");
            while (results != "[]")
            {
                results = _jpgStoreAPI.GET_Listings(policy, page++);
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
                results = _jpgStoreAPI.GET_Sales(policy, page++);
                JToken deserialized = (JToken)JsonConvert.DeserializeObject(results);
                if (deserialized != null)
                {
                    List<JPGStoreSale> listings = deserialized.ToObject<JPGStoreSale[]>().ToList();
                    AddCollectionInformation(listings);
                    _sqlService.AddRows(listings, _activeCollectionNameUnderscore + "_Sales");
                }
            }
            _ducky.Info($"Completed data load for {_activeCollectionName}!");
        }

        private void AddCollectionInformation(List<JPGStoreListing> items)
        {
            items.ForEach(item =>
            {
                item.collection_name = _activeCollectionName;
                item.collection_name_underscore = _activeCollectionNameUnderscore;
            });
        }


        private void AddCollectionInformation(List<JPGStoreSale> items)
        {
            items.ForEach(item =>
            {
                item.collection_name = _activeCollectionName;
                item.collection_name_underscore = _activeCollectionNameUnderscore;
            });
        }

        private void ListingData(JPGStoreListing item)
        {
            _collectionDataBuilder.SetAsset(item.display_name, _activeCollectionName, out CollectionItemData collectionItemData);
            collectionItemData.jPGStoreItem = item;
            _discord.Listing(collectionItemData);
        }

        private void SaleData(JPGStoreSale item)
        {
            _collectionDataBuilder.SetAsset(item.display_name, _activeCollectionName, out CollectionItemData collectionItemData);
            collectionItemData.jPGStoreItem = item;
            _discord.Sale(collectionItemData);
        }
    }
}
