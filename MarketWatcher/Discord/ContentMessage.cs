using MarketWatcher.Classes.JPGStore;
using MarketWatcher.Discord.Webhooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketWatcher.Discord
{
    public class ContentMessage : WebHookContainer, IWebhookContainer
    {
        private string _quackType { get; set; }
        private string _className { get; set; }
        private string _methodName { get; set; }
        private string _duckyMessage { get; set; }
        protected WebHook _webHook { get; set; }

        public ContentMessage(string duckyType, string className, string methodName, string duckyMessage)
        {
            _quackType = duckyType;
            _className = className;
            _methodName = methodName;
            _duckyMessage = duckyMessage;
        }

        public void Defaults()
        {
            _webHook = new WebHook();
            _webHook.username = "WenRarity";
            _webHook.content = "@bkc";
        }

        public void Embeds()
        {

            WebHookEmbed embed = new WebHookEmbed();
            embed.title = _quackType;

            embed.fields = new List<EmbedField>();
            Fields("Class", $"{_className}", true, out EmbedField classStr);
            Fields("Method", $"{_methodName}", true, out EmbedField methodStr);
            Fields("Message", $"{_duckyMessage}", false, out EmbedField messageStr);

            embed.fields.Add(methodStr);
            embed.fields.Add(methodStr);
            embed.fields.Add(messageStr);

            _webHook.embeds.Add(embed);
        }

        public string GetTitle()
            => _duckyMessage;

        public WebHook AsWebHook()
        {
            Defaults();
            Embeds();
            return _webHook;
        }
    }
}
