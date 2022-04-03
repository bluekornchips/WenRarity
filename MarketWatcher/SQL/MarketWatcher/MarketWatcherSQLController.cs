//using MarketWatcher.API.JPGStore;
//using MarketWatcher.Classes.JPGStore;
//using MarketWatcher.EntityFramework.Context.MarketWatcher;
//using MarketWatcher.Utility;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace MarketWatcher.SQL.MarketWatcher
//{
//    internal class MarketWatcherSQLController
//    {

//        private Ducky _ducky = Ducky.getInstance();
//        private RawSQLService _sqlService = RawSQLService.getInstance();
//        private readonly JPGStoreAPI _jpgStoreAPI = JPGStoreAPI.getInstance();
//        private BuilderData _builderData = new BuilderData();
//        public MarketWatcherSQLController(BuilderData data)
//        {
//            _builderData = data;
//        }

//        public void CreateCollection(out bool validConnection)
//        {
//            validConnection = false;
//            using (MarketWatcherContext context = new())
//            {
//                //First check the database for the cached records we have.

//                var collection = (from coll in context.JPGStoreCollectionItems.Where(c => c.policy == builder._policy) select coll);
//                if (collection.Count() == 0) // Empty Collection
//                {
//                    //Add the policy to the JPGStoreCollectionItem table
//                    //Safety check for empty strings -no cheating
//                    if (builder._activeCollectionName != "")
//                    {
//                        context.JPGStoreCollectionItems.Add(new JPGStoreCollectionItem(builder._policy, builder._activeCollectionName));
//                        context.SaveChanges();
//                        _ducky.Info($"Created new Collection: {builder._activeCollectionName}.");
//                        InitialLoad(policy);
//                        validConnection = true;
//                    }
//                    else
//                    {
//                        _ducky.Debug("MarketWatcherSQLController", "CreateCollection", "Entered an empty collect name for a new collection.");
//                    }
//                }
//                else
//                {
//                    validConnection = true;
//                }
//            }
//        }

//        public void InitialLoad()
//        {
//            _ducky.Info($"Initial Load for {builder._activeCollectionName}.");

//            //If the table already exists for whatever reason, drop it.
//            _sqlService.CreateTable_Collection(builder._activeCollectionNameUnderscore);
//            _sqlService.CreateTable_Sales(builder._activeCollectionNameUnderscore + "_Sales");

//            string results = "";
//            int page = 0;
//            _ducky.Info($"Loading {builder._activeCollectionName} from JPGStore API...");
//            while (results != "[]")
//            {
//                results = _jpgStoreAPI.GET_Listings(builder._policy, page++);
//                JToken deserialized = (JToken)JsonConvert.DeserializeObject(results);
//                if (deserialized != null)
//                {
//                    List<JPGStoreListing> listings = deserialized.ToObject<JPGStoreListing[]>().ToList();
//                    AddCollectionInformation(listings);
//                    _sqlService.AddRows(listings, builder._activeCollectionNameUnderscore);
//                }
//            }
//            _ducky.Info($"Loading {builder._activeCollectionName} Sales from JPGStore API...");
//            results = "";
//            page = 0;
//            while (results != "[]")
//            {
//                results = _jpgStoreAPI.GET_Sales(builder.policy, page++);
//                JToken deserialized = (JToken)JsonConvert.DeserializeObject(results);
//                if (deserialized != null)
//                {
//                    List<JPGStoreSale> listings = deserialized.ToObject<JPGStoreSale[]>().ToList();
//                    AddCollectionInformation(listings);
//                    _sqlService.AddRows(listings, builder._activeCollectionNameUnderscore + "_Sales");
//                }
//            }
//            _ducky.Info($"Completed data load for {builder._activeCollectionName}!");
//        }

//        public void Listings(Builde)
//        {
//            using (MarketWatcherContext context = new())
//            {
//                try
//                {
//                    // Get the most recent database entry for the collection to find the timestamp.
//                    // Retrieve the new JPGStore listings between the current time and the most recent database entry.
//                    bool updated = false;
//                    int page = 0;
//                    int newRecords = 0;

//                    _sqlService.RetrieveMostRecent(builder._activeCollectionNameUnderscore, out JPGStoreListing latest);

//                    ItemData(latest);

//                    while (!updated)
//                    {
//                        newRecords = 0;
//                        JToken deserialized = JsonConvert.DeserializeObject(_jpgStoreAPI.GET_Listings(builder._policy, page++)) as JToken;
//                        List<JPGStoreListing> listings = deserialized.ToObject<JPGStoreListing[]>().ToList();
//                        AddCollectionInformation(listings);
//                        listings.OrderBy(l => l.listed_at);
//                        foreach (var item in listings)
//                        {
//                            if (item.listing_id > latest.listing_id)
//                            {
//                                ++newRecords;
//                                _discord.Listing(item);
//                                _sqlService.AddRow(item, builder._activeCollectionNameUnderscore);
//                                UpdateFloor();
//                            }
//                        }
//                        if (newRecords < 25) updated = true;
//                    }
//                    _ducky.Info($"{builder._activeCollectionName} up to date.");
//                }
//                catch (Exception ex)
//                {
//                    _ducky.Error("MarketWatcherSQLController", "Listings", ex.Message);
//                }
//            }

//        }

//        public void Sales(BuilderData data)
//        {
//            using (MarketWatcherContext context = new MarketWatcherContext())
//            {
//                try
//                {
//                    bool updated = false;
//                    int page = 0;
//                    int newRecords = 0;

//                    _sqlService.RetrieveMostRecent(_activeCollectionNameUnderscore + "_Sales", out JPGStoreSale latest);

//                    while (!updated)
//                    {
//                        newRecords = 0;
//                        JToken deserialized = (JToken)JsonConvert.DeserializeObject(_jpgStoreAPI.GET_Sales(policy, page++));
//                        List<JPGStoreSale> listings = deserialized.ToObject<JPGStoreSale[]>().ToList();
//                        AddCollectionInformation(listings);
//                        listings.OrderBy(l => l.listing_id);
//                        foreach (var item in listings)
//                        {
//                            //if(item.listed_at > latest.listed_at)
//                            if (item.listing_id > latest.listing_id)
//                            {
//                                ++newRecords;
//                                _discord.Sale(item);
//                                _sqlService.AddRow(item, _activeCollectionNameUnderscore + "_Sales");
//                                _sqlService.Sales_Action(item, _activeCollectionNameUnderscore);
//                                UpdateFloor();
//                            }
//                        }
//                        if (newRecords < 25) updated = true;
//                    }
//                    _ducky.Info($"{_activeCollectionName}_Sales up to date.");
//                }
//                catch (Exception ex)
//                {
//                    _ducky.Error("JPGStoreBuilder", "Sales", ex.Message);
//                }
//            }
//        }

//        private void UpdateFloor(BuilderData data)
//        {
//            _sqlService.RetrieveFloor(_activeCollectionNameUnderscore, out double floor);
//            _sqlService.Setfloor(_activeCollectionNameUnderscore, floor);
//        }


//        private void AddCollectionInformation(List<JPGStoreListing> items)
//        {
//            items.ForEach(item =>
//            {
//                item.collection_name = _activeCollectionName;
//                item.collection_name_underscore = _activeCollectionNameUnderscore;
//            });
//        }


//        private void AddCollectionInformation(List<JPGStoreSale> items)
//        {
//            items.ForEach(item =>
//            {
//                item.collection_name = _activeCollectionName;
//                item.collection_name_underscore = _activeCollectionNameUnderscore;
//            });
//        }

//        private void ItemData(JPGStoreListing item)
//        {
//            _discord.Listing(item);
//            _collectionDataBuilder.SetAsset(item.display_name, out Asset asset);
//        }
//    }
//}
