using System;
using System.Collections.Generic;

namespace MarketWatcher.Classes.JPGStore
{
    public class WebHook
    {
        public Uri uri { get; set; }
        public string username { get; set; } 
        public string content { get; set; }
        public List<WebHookEmbed> embeds { get; set; }
        public WebHook()
        {
            embeds = new List<WebHookEmbed>();
        }
    }
}