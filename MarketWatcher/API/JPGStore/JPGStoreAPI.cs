using System;
using System.Net.Http;

namespace MarketWatcher.API.JPGStore
{
    public class JPGStoreAPI : MarketplaceAPI
    {
		private static readonly string _baseUrl = @"https://server.jpgstoreapis.com/policy/";
		private static JPGStoreAPI _instance = getInstance();

		private JPGStoreAPI()
		{

		}

		public static JPGStoreAPI getInstance()
		{
			if (_instance == null) _instance = new JPGStoreAPI();
			return _instance;
		}

		#region GET
		public string GET_Listings(string policy, int page)
		{
			using (HttpClient httpClient = new HttpClient())
			{
				try
				{
					return httpClient.GetStringAsync($"{_baseUrl}{policy}/listings?page={page}").Result;
				}
				catch (Exception ex)
				{
					_ducky.Error("JPGStore", "Get_Listings", ex.Message);
					return "";
				}
			}
		}

		public string GET_Sales(string policy, int page)
		{
			using (HttpClient httpClient = new HttpClient())
			{
				try
				{
					return httpClient.GetStringAsync($"{_baseUrl}{policy}/sales?page={page}").Result;
				}
				catch (Exception ex)
				{
					_ducky.Error("JPGStore", "GET_Sales", ex.Message);
					return "";
				}
			}
		}
		#endregion
	}
}
