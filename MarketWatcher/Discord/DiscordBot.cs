using MarketWatcher.Classes;
using MarketWatcher.Classes.JPGStore;
using MarketWatcher.Discord.Webhooks;
using MarketWatcher.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace MarketWatcher.Discord
{
    public class DiscordBot
    {
        private static DiscordBot _instance = null;
        private static Ducky _ducky = Ducky.GetInstance();

        private static Dictionary<string, string> _urls = new Dictionary<string, string>()
        {
            { "test", "https://discord.com/api/webhooks/957835242763333652/V_FkSC6FGd_uf8K9SipGPi9z_ZrrM1j-Ev6wjYRtUEyQ-Ltxavr1MGiYo_krFA1hAcpb" },
            { "content", "https://discord.com/api/webhooks/960231610790342726/C2xQjDmVcvNzvq7MHbmkfEXfY_0ybTeN5QtG7kvoriMcPbmUMz2yC8RjxRit2PhIPaGC" },
            { "listings", "https://discord.com/api/webhooks/960209935541600377/12WCD1Sq8TpI4oj20zb1ffzR0QFTiw8uhjZ5c06Yf-488-Q9C_3atXoJKFFAi1M_F2A9" },
            { "sales", "https://discord.com/api/webhooks/960210051358937098/Id_J-v15sczAw_zt-rfJ9Dg50azG6u6GHNyayfIb6ZrS7wqVY54yNMqCSD62uG3abl2F" },
        };

        private Queue<IWebhookContainer> _saleQueue = new();
        private Queue<IWebhookContainer> _listingQueue = new();

        private bool _listingOutputActive = false;
        private bool _salesOutputActive = false;
        private bool _outputActive = true;

        private DiscordBot() { }

        /// <summary>
        /// Singleton
        /// </summary>
        public static DiscordBot Instance
        {
            get
            {
                if (_instance == null) _instance = new DiscordBot();
                return _instance;
            }
        }

        public void Listing(CollectionItemData item)
        {
            _listingQueue.Enqueue(item);
            ListingHandler(); // Enters, but only outputs if able.
        }

        public void Sale(CollectionItemData item)
        {
            _saleQueue.Enqueue(item);
            SaleHandler(); // Enters, but only outputs if able.
        }

        public void DuckyOutput(IWebhookContainer content)
            => Output(content, "content");

        /// <summary>
        /// A safety method to prevent multiple calls.
        /// </summary>
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

        /// <summary>
        /// A safety method to prevent multiple calls.
        /// </summary>
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

        /// <summary>
        /// Decerement the sale queue and output it, if enabled.
        /// </summary>
        private void WriteSale()
        {
            var item = _saleQueue.Dequeue();
            if (!_outputActive) return;
            Output(item, "sales");
        }

        /// <summary>
        /// Decerement the listing queue and output it, if enabled.
        /// </summary>
        private void WriteListing()
        {
            var item = _listingQueue.Dequeue();
            if (!_outputActive) return;
            Output(item, "listings");
        }

        /// <summary>
        /// Ref: https://birdie0.github.io/discord-WebHooks-guide/tools/postman.html
        /// If status shows 204 No Content means request succeed!
        /// Output the object to the given Discord Webhook.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="url"></param>
        private async void Output(IWebhookContainer item, string url)
        {
            using HttpClient client = new();
            try
            {
                HttpRequestMessage httpRequestMessage = new(HttpMethod.Post, _urls[url]);

                var jsonContent = JsonConvert.SerializeObject(item.AsWebHook());
                var data = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                httpRequestMessage.Content = data;

                Thread.Sleep(5000); // Sleep for 5 seconds to not be rate limited by Discord

                var res = await client.SendAsync(httpRequestMessage);

                if (res.IsSuccessStatusCode) _ducky.Info($"Successfully output {item.GetTitle()}");
                else _ducky.Error("DiscordBot", "Output(IWebhookContainer item, string url)", $"Unable to output {item.GetTitle()}");
            }
            catch (Exception ex)
            {
                _ducky.Error("DiscordBot", "Output(IWebhookContainer item, string url)", ex.Message);
            }
        }
    }
}
