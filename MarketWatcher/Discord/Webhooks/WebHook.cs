using MarketWatcher.EntityFramework.JPGStore;
using MarketWatcher.SQL.Rime;
using System.Text;

namespace MarketWatcher.Discord.Webhooks
{
    public class WebHook
    {
        private readonly string _username = "WenRarity";
        private string _embeds = "";
        private string _color = "";

        public WebHook(JPGStoreListing item)
        {
            Build(item);
        }

        public WebHook(JPGStoreSale item)
        {
            Build(item);
        }




        private void Build(JPGStoreListing item)
        {
            RimeService service = new RimeService();
            service.RetrieveRarity(item.collection_name, item.display_name, out WebHookData postData);
            Embeds(item, postData);
            _color = "Blue";
        }

        private void Build(JPGStoreSale item)
        {
            RimeService service = new RimeService();
            service.RetrieveRarity(item.collection_name, item.display_name, out WebHookData postData);
            Embeds(item, postData);
            _color = "Green";
        }

        private string Color(string color)
            => int.Parse(Colors.colors[color], System.Globalization.NumberStyles.HexNumber).ToString();

        private void Embeds(JPGStoreListing item, WebHookData postData)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[{");
            sb.Append(embedBuilder_Single("title", item.display_name));
            sb.Append(",");
            sb.Append(embedBuilder_Single("url", $"https://www.jpg.store/asset/{item.asset_id}"));
            sb.Append(",");

            sb.Append("\"fields\" : [");
            sb.Append(Fields("**Rank**", $"{postData.RarityRank} / {postData.CollectionSize}", true));
            sb.Append(",");
            sb.Append(Fields("**Price**", $"{(item.price_lovelace / 1000000).ToString()} ₳", true));
            sb.Append(",");
            sb.Append(Fields("**Listed At**", $"{ item.listed_at.ToString()} EST", false));
            sb.Append("]");
            sb.Append(",");

            sb.Append("\"image\" : {");
            sb.Append(embedBuilder_Single("url", postData.Image));
            sb.Append("}");
            sb.Append(",");

            sb.Append("\"footer\" : {");
            sb.Append(embedBuilder_Single("text", $"{_username} Buy Meter: TBD"));
            sb.Append("}");


            sb.Append("}]");
            _embeds = sb.ToString();
        }

        private void Embeds(JPGStoreSale item, WebHookData postData)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[{");
            sb.Append(embedBuilder_Single("title", item.display_name));
            sb.Append(",");
            sb.Append(embedBuilder_Single("url", $"https://www.jpg.store/asset/{item.asset_id}"));
            sb.Append(",");

            sb.Append("\"fields\" : [");
            sb.Append(Fields("**Rank**", $"{postData.RarityRank} / {postData.CollectionSize}", true));
            sb.Append(",");
            sb.Append(Fields("**Price**", $"{(item.price_lovelace / 1000000).ToString()} ₳", true));
            sb.Append(",");
            sb.Append(Fields("**Listed At**", $"{ item.confirmed_at.ToString()} EST", false));
            sb.Append("]");
            sb.Append(",");

            sb.Append("\"image\" : {");
            sb.Append(embedBuilder_Single("url", postData.Image));
            sb.Append("}");
            sb.Append(",");

            sb.Append("\"footer\" : {");
            sb.Append(embedBuilder_Single("text", $"{_username} Buy Meter: TBD"));
            sb.Append("}");


            sb.Append("}]");
            _embeds = sb.ToString();
        }


        private string embedBuilder_Single(string field, string fieldData)
            => $"\"{field}\" : \"{fieldData}\"";

        private string Fields(string name, string value, bool isInline)
        {
            string inline = "false";
            if (isInline) inline = "true";
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append(embedBuilder_Single("name", name));
            sb.Append(",");
            sb.Append(embedBuilder_Single("value", value));
            sb.Append(",");
            sb.Append(embedBuilder_Single("inline", inline));
            sb.Append("}");
            return sb.ToString();

        }
        public string Jank()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append(embedBuilder_Single("username", _username));
            sb.Append(",");
            //sb.Append(embedBuilder_Single("content", ":blue_circle:"));
            //sb.Append(",");
            sb.Append(embedBuilder_Single("color", Color("blue")));
            sb.Append(",");
            sb.Append($"\"embeds\" : {_embeds}");
            sb.Append("}");
            return sb.ToString();
        }
    }
}
