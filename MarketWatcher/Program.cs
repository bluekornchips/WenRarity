using MarketWatcher.Builders;
using MarketWatcher.Utility;
using System.Threading;

namespace MarketWatcher
{
    internal class Program
    {
		private static Ducky _ducky = Ducky.GetInstance();
		public static void Main()
		{
            //Thread.Sleep(25000);
            Workers("a616aab3b18eb855b4292246bd58f9e131d7c8c25d1d1d7c88b666c4", "Pendulum", false);
            Workers("f96584c4fcd13cd1702c9be683400072dd1aac853431c99037a3ab1e", "Puurrty Cats Society", false);
            Workers("c76e5286fce9e6f5c9b1c5a61f74bc7fa89ed0f946ff2ae5d875f2cb", "ElMatador", false);
        }

		public static void Workers(string policy, string collectionName, bool reset)
		{
			JPGStoreBuilder builder = new JPGStoreBuilder(policy, collectionName, reset);
			Thread t = new Thread(new ThreadStart(builder.Start));
			t.Start();
		}
	}
}
