using WenRarityLibrary;

namespace Rime.Builders
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
        protected static string _csvDir = "";
        public abstract void GenerateCollectionRarity_SQL();
        public abstract void Handle();
        public abstract void RarityChart();

        public BaseStatsHandler()
        {
           DirectorySafetyChecks();
        }

        private void DirectorySafetyChecks()
        {
            string cwd = "";
            string[] splitCwd = Environment.CurrentDirectory.Split('\\');
            bool complete = false;

            foreach (string item in splitCwd)
            {
                if (!complete) cwd += $"\\{item}";
                if (item.Equals("WenRarity")) complete = true;
            }

            cwd = cwd.Substring(1, cwd.Length - 1);
            cwd += "\\";

            _csvDir = cwd + "Data\\CSV";
        }

        public double MH(double frequency, double collectionSize) => Math.Round(1.00 / ((frequency / collectionSize)), 7);
    }
}
