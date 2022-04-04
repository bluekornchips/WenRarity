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
        public void Defaults();
        public void Embeds();
    }

    public class WebHookContainer
    {
        protected void Fields(string name, string value, bool inline, out EmbedField embedField)
            => embedField = new EmbedField() { name = name, value = value, inline = inline };
    }
}
