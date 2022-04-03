using MarketWatcher.Classes;
using MarketWatcher.Classes.JPGStore;
using MarketWatcher.Discord.Webhooks;
using MarketWatcher.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MarketWatcher.Discord
{
    public class DiscordBot
    {
        // token OTU3ODMxNjU0OTgwODU3ODk2.YkEgPg.ZNNCKvqVYMhoYvQ-RTIvIiZVidE
        // "{\"username\": \"Username Placeholder\",\"embeds\":[    {\"description\":\"WebHook Description\", \"title\":\"WebHook Title\", \"color\":1018364}]  }"
        private static DiscordBot _instance = getInstance();
        private static WebHookBuilder _webhookBuilder = WebHookBuilder.GetInstance();
        private Ducky _ducky = Ducky.getInstance();

        private static Dictionary<string, string> _urls = new Dictionary<string, string>()
        {
            { "test", "https://discord.com/api/webhooks/957835242763333652/V_FkSC6FGd_uf8K9SipGPi9z_ZrrM1j-Ev6wjYRtUEyQ-Ltxavr1MGiYo_krFA1hAcpb" },
            { "listing", "https://discord.com/api/webhooks/960209935541600377/12WCD1Sq8TpI4oj20zb1ffzR0QFTiw8uhjZ5c06Yf-488-Q9C_3atXoJKFFAi1M_F2A9" },
            { "sales", "https://discord.com/api/webhooks/960210051358937098/Id_J-v15sczAw_zt-rfJ9Dg50azG6u6GHNyayfIb6ZrS7wqVY54yNMqCSD62uG3abl2F" },
        };
        //private static string _url = "https://discord.com/api/WebHooks/959131053119930423/YtFKTBRJgiPS9gVd_GdAflOtrl1srzeVb1nZQF9fOXlMiWc_WKF_8e50tHIUHiNO5LOA"; //Public

        private Queue<WebHook> _saleQueue = new Queue<WebHook>();
        private Queue<WebHook> _listingQueue = new Queue<WebHook>();

        private bool _listingOutputActive = false;
        private bool _salesOutputActive = false;

        private bool _outputActive = true;


        private DiscordBot() { }

        public static DiscordBot getInstance()
        {
            if (_instance == null) _instance = new DiscordBot();
            return _instance;
        }

        public void Listing(CollectionItemData item)
        {
            var WebHook = _webhookBuilder.Build(item);
            _listingQueue.Enqueue(WebHook);
            _ducky.Debug("DiscordBot", "Listing", $"Added {item.jPGStoreItem.display_name}");
            ListingHandler();
        }

        public void Sale(CollectionItemData item)
        {
            var WebHook = _webhookBuilder.Build(item);
            _saleQueue.Enqueue(WebHook);
            _ducky.Debug("DiscordBot", "Sale", $"Added {item.jPGStoreItem.display_name}");
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

        private void WriteSale()
        {
            var item = _saleQueue.Dequeue();
            if (!_outputActive) return;
            WriteItem(item, "test");
        }

        /// <summary>
        /// Ref: https://birdie0.github.io/discord-WebHooks-guide/tools/postman.html
        /// If status shows 204 No Content means request succeed!
        /// </summary>
        private void WriteListing()
        {
            var item = _listingQueue.Dequeue();
            if (!_outputActive) return;
            WriteItem(item, "test");

        }

        private async void WriteItem(WebHook item, string url)
        {
            using (HttpClient client = new())
            {
                try
                {
                    HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, _urls[url]);
                    var jsonContent = JsonConvert.SerializeObject(item);
                    var data = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    httpRequestMessage.Content = data;
                    //Thread.Sleep(5000);
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
