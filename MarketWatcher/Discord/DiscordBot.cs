using MarketWatcher.Discord.Webhooks;
using MarketWatcher.EntityFramework.JPGStore;
using MarketWatcher.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MarketWatcher.Discord
{
	public class DiscordBot
	{
		// token OTU3ODMxNjU0OTgwODU3ODk2.YkEgPg.ZNNCKvqVYMhoYvQ-RTIvIiZVidE
		// "{\"username\": \"Username Placeholder\",\"embeds\":[    {\"description\":\"WebHook Description\", \"title\":\"WebHook Title\", \"color\":1018364}]  }"
		private static DiscordBot _instance = getInstance();
		//private static string _url = "https://discord.com/api/WebHooks/957835242763333652/V_FkSC6FGd_uf8K9SipGPi9z_ZrrM1j-Ev6wjYRtUEyQ-Ltxavr1MGiYo_krFA1hAcpb"; //Testing
		private static string _url = "https://discord.com/api/WebHooks/959131053119930423/YtFKTBRJgiPS9gVd_GdAflOtrl1srzeVb1nZQF9fOXlMiWc_WKF_8e50tHIUHiNO5LOA"; //Public

		private Queue<WebHook> _saleQueue = new Queue<WebHook>();
		private Queue<WebHook> _listingQueue = new Queue<WebHook>();

		private bool _listingOutputActive = false;
		private bool _salesOutputActive = false;

		private bool _outputActive = true;

		private Ducky _ducky = Ducky.getInstance();

		private DiscordBot() { }

		public static DiscordBot getInstance()
		{
			if (_instance == null) _instance = new DiscordBot();
			return _instance;
		}

		public void Listing(JPGStoreListing item)
		{
			var WebHook = new WebHook(item);
			_listingQueue.Enqueue(WebHook);
			_ducky.Debug("DiscordBot", "Listing", $"Added {item.display_name}");
			ListingHandler();
		}

		public void Sale(JPGStoreSale item)
		{
			var WebHook = new WebHook(item);
			_saleQueue.Enqueue(WebHook);
			_ducky.Debug("DiscordBot", "Sale", $"Added {item.display_name}");
			SaleHandler();
		}


		public void SaleHandler()
		{
			if (!_salesOutputActive)
			{
				_salesOutputActive = true;
				while (_saleQueue.Count > 0)
				{
					WriteSale();
					if (_saleQueue.Count < 1) _salesOutputActive = false;
				}
			}
		}

		public void ListingHandler()
		{
			if (!_listingOutputActive)
			{
				_listingOutputActive = true;
				while (_listingQueue.Count > 0)
				{
					WriteListing();
					if (_listingQueue.Count < 1) _listingOutputActive = false;
				}
			}
		}

		private async void WriteSale()
		{
			var sale = _saleQueue.Dequeue();
			if (!_outputActive) return;
			using (HttpClient client = new())
			{
				try
				{
					HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, _url);
					var text = sale.Jank();
					var data = new StringContent(text, Encoding.UTF8, "application/json");
					httpRequestMessage.Content = data;
					Thread.Sleep(5000);
					var res = await client.SendAsync(httpRequestMessage);
				}
				catch (Exception ex)
				{
					_ducky.Error("DiscordBot", "WriteSale()", ex.Message);
					throw;
				}
			}
		}
		/// <summary>
		/// Ref: https://birdie0.github.io/discord-WebHooks-guide/tools/postman.html
		/// If status shows 204 No Content means request succeed!
		/// </summary>
		private async void WriteListing()
		{
			var listing = _listingQueue.Dequeue();
			if (!_outputActive) return;
			using (HttpClient client = new())
			{
				try
				{
					HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, _url);
					var text = listing.Jank();
					var data = new StringContent(text, Encoding.UTF8, "application/json");
					httpRequestMessage.Content = data;
					Thread.Sleep(5000);
					var res = await client.SendAsync(httpRequestMessage);
				}
				catch (Exception ex)
				{
					_ducky.Error("DiscordBot", "WriteListing()", ex.Message);
					throw;
				}
			}
		}
	}
}
