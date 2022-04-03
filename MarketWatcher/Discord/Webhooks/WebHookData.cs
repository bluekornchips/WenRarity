namespace MarketWatcher.Discord.Webhooks
{
    public class WebHookData
    {
        public int RarityRank { get; set; }
        public int CollectionSize { get; set; }
        public double BuyMeter { get; set; }
        public string Image { get; set; } = "";
    }
}
