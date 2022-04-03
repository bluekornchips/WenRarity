using MarketWatcher.Classes.JPGStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketWatcher.Discord.Webhooks
{
    public interface IWebhookContainer
    {
        public WebHook AsWebHook();
        public string GetTitle();
    }
}
