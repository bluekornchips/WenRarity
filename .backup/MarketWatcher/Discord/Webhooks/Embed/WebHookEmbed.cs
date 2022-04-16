using System.Collections.Generic;

namespace MarketWatcher.Classes.JPGStore
{
    public class WebHookEmbed
    {
        public WebHookEmbed()
        {
            fields = new List<EmbedField>();
        }
        //public Color? Color { get => int.Parse(value, System.Globalization.NumberStyles.HexNumber).ToString(); set; }
        // private string Color(string color)
        //=> int.Parse(Colors.colors[color], System.Globalization.NumberStyles.HexNumber).ToString();

        public string title { get; set; }
        public string url { get; set; }
        public EmbedMedia image { get; set; }

        public List<EmbedField> fields { get; set; }
    }
}