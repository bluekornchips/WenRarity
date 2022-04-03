using MarketWatcher.Builders;
using MarketWatcher.Utility;
using System.Threading;

namespace MarketWatcher
{
    internal class Program
    {
		private static Ducky _ducky = Ducky.getInstance();
		public static void Main()
		{
			//builder.Start("b000e9f3994de3226577b4d61280994e53c07948c8839d628f4a425a", "Clumsy Ghosts");
			//Thread.Sleep(25000);
			Workers("a616aab3b18eb855b4292246bd58f9e131d7c8c25d1d1d7c88b666c4", "Pendulum");
			Workers("f96584c4fcd13cd1702c9be683400072dd1aac853431c99037a3ab1e", "Puurrty Cats Society");
		}

		public static void Workers(string policy, string collectionName)
		{
			JPGStoreBuilder builder = new JPGStoreBuilder(policy, collectionName);
			Thread t = new Thread(new ThreadStart(builder.Start));
			t.Start();
		}
	}
}
