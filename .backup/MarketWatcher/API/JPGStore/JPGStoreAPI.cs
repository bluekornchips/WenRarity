using System;
using System.Net.Http;

namespace MarketWatcher.API.JPGStore
{
    public class JPGStoreAPI : MarketplaceAPI
    {
		private static readonly string _baseUrl = @"https://server.jpgstoreapis.com/policy/";
		private static JPGStoreAPI _instance = GetInstance();

		private JPGStoreAPI() { }

		/// <summary>
		/// Singleton
		/// </summary>
		/// <returns></returns>
		public static JPGStoreAPI GetInstance()
		{
			if (_instance == null) _instance = new JPGStoreAPI();
			return _instance;
		}

		#region GET
		/// <summary>
		/// Get Listings
		/// </summary>
		/// <param name="policy"></param>
		/// <param name="page"></param>
		/// <returns></returns>
		public string Listings(string policy, int page)
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

		/// <summary>
		/// Get Sales
		/// </summary>
		/// <param name="policy"></param>
		/// <param name="page"></param>
		/// <returns></returns>
		public string Sales(string policy, int page)
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
