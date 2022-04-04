using MarketWatcher.Classes.JPGStore;
using MarketWatcher.Discord.Webhooks;
using Rime.ADO.Classes;
using Rime.ADO.Classes.Tokens.Rarity;
using System;
using System.Collections.Generic;

namespace MarketWatcher.Classes
{
    public class CollectionItemData : WebHookContainer, IWebhookContainer
    {
        public string Fingerprint { get; set; } = "";
        public string ItemName { get; set; } = "";
        public int RarityRank { get; set; }
        public int CollectionSize { get; set; }
        public double BuyMeter { get; set; }
        public Asset asset { get; set; } = new Asset();
        public Rarity rarity { get; set; }
        public JPGStoreItem jPGStoreItem { get; set; }

        protected WebHook _webHook { get; set; }

        public void Defaults()
        {
            ItemName = jPGStoreItem.display_name;
            _webHook = new WebHook();
            _webHook.username = "WenRarity";
        }

        public void Embeds()
        {
            _webHook.uri = new Uri($"{jPGStoreItem._url}{jPGStoreItem.asset_id}");

            WebHookEmbed embed = new WebHookEmbed();
            embed.image = new EmbedMedia() { url = $"{asset.Image}", width = 80, height = 80 };
            embed.title = jPGStoreItem.display_name;
            embed.url = $"{jPGStoreItem._url}{jPGStoreItem.asset_id}";

            embed.fields = new List<EmbedField>();
            Fields("Price", $"{jPGStoreItem.price_lovelace / 1000000} ₳", true, out EmbedField price);

            var marketDate = new DateTime();
            string marketType = "Sold At";
            if (jPGStoreItem.GetType() == typeof(JPGStoreListing))
            {
                marketDate = (jPGStoreItem as JPGStoreListing).listed_at;
                marketType = "Listed At";
            }
            else if (jPGStoreItem.GetType() == typeof(JPGStoreSale))
            {
                marketDate = (jPGStoreItem as JPGStoreSale).confirmed_at;
            }

            Fields(marketType, $"{marketDate} EST", false, out EmbedField date);
            Fields("Rank", $"{RarityRank}/{CollectionSize}", true, out EmbedField rank);
            Fields("Weighting", $"{Math.Round(rarity.Weighting, 2)}", true, out EmbedField weighting);
            Fields("URL", $"{_webHook.uri}", false, out EmbedField link);

            embed.fields.Add(price);
            embed.fields.Add(rank);
            embed.fields.Add(weighting);
            embed.fields.Add(date);
            embed.fields.Add(link);

            _webHook.embeds.Add(embed);
        }

        public string GetTitle()
            => ItemName;
        public WebHook AsWebHook()
        {
            Defaults();
            Embeds();
            return _webHook;
        }
    }
}
