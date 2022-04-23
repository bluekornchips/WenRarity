using WenRarityLibrary;

namespace Stats.Builders
{
    public class StatsHandler
    {
        private static StatsHandler _instance;
        public static StatsHandler Instance => _instance ?? (_instance = new StatsHandler());

        public StatsHandler() { }

        public BaseStatsHandler handler { get; set; }
    }

    public abstract class BaseStatsHandler
    {
        protected static Ducky _ducky = Ducky.Instance;
        public abstract void GenerateCollectionRarity_SQL();
        public abstract void Handle();

        public double MH(double frequency, double collectionSize) => Math.Round(1.00 / ((frequency / collectionSize)), 7);
    }
}
