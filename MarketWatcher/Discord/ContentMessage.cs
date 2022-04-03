using MarketWatcher.Classes.JPGStore;
using MarketWatcher.Discord.Webhooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketWatcher.Discord
{
    public class ContentMessage : IWebhookContainer
    {
        private string _quackType { get; set; }
        private string _className { get; set; }
        private string _methodName { get; set; }
        private DateTime _dateTime { get; set; }
        private string _duckyMessage { get; set; }
        protected WebHook _webHook { get; set; }

        public ContentMessage(string duckyType, DateTime dateTime, string className, string methodName, string duckyMessage)
        {
            _quackType = duckyType;
            _className = className;
            _methodName = methodName;
            _duckyMessage = duckyMessage;
            _dateTime = dateTime;
        }

        protected void WebHookDefaults()
        {
            _webHook = new WebHook();
            _webHook.username = "WenRarity Bot";
            _webHook.content = "@bkc";
        }
        public void WebHook_ContentMessage()
        {
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
        }

        private void Fields(string name, string value, bool inline, out EmbedField embedField)
            => embedField = new EmbedField() { name = name, value = value, inline = inline };

        public string GetTitle()
            => _duckyMessage;

        public WebHook AsWebHook()
        {
            WebHookDefaults();
            WebHook_ContentMessage();
            return _webHook;
        }
    }
}
