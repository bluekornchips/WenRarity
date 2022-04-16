using BlockfrostQuery.Util;
using Newtonsoft.Json.Linq;
using Rime.ADO.Classes.Listings;
using Rime.API.JPGStore;
using Rime.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rime.Store.Stores
{

    public class JPGStore : IListing
    {
        public static JPGStoreAPI api = new JPGStoreAPI();

        public void GetListings(string policy)
        {
            var listingsRequest = ListingsForPolicyById(policy);
            var parsedJtokens = JToken.Parse(listingsRequest);
            ParseRequest(parsedJtokens);
        }

        private static void UpdateDB(JPGStoreListing listing)
            => RimeDBContextController.UpdateListingsJPG(listing);

        private string ListingsForPolicyById(string policy)
            => api.ListingsForPolicyById(policy);
        
        public void MatchToRarity()
        {

        }

        private void ParseRequest(JToken jtokens)
        {
            string asset = "";
            string asset_display_name = "";
            string price_lovelace = "";
            string listed_at = "";
            RimeDBContextController.ClearListings();
            foreach (JToken token in jtokens)
            {
                try
                {
                    asset = token["asset"].ToString();
                    asset_display_name = token["asset_display_name"].ToString();
                    price_lovelace = token["price_lovelace"].ToString();
                    listed_at = token["listed_at"].ToString();
                    if(asset != null && asset != "")
                         RimeDBContextController.UpdateListingsJPG(new JPGStoreListing(asset, asset_display_name, price_lovelace , listed_at));
                    else
                    {
                        Logger.Info($"Unable to add JPGStore listing for {asset}.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("JPGStore", "ParseRequest", ex.Message);
                    throw;
                }
            }
        }
    }
}
