using BlockfrostQuery.Util;
using Rime.Builders;
using System.Threading;

namespace Rime
{
	public class Program
	{
		static void Main(string[] args)
		{
			Builders();
			//IListing jPGStore = new JPGStore();
			//jPGStore.GetListings("c56d4cceb8a8550534968e1bf165137ca41e908d2d780cc1402079bd");
		}

		static void Builders()
		{
			do
			{
				//IBuilder b = new LionessBuilder();
				//IBuilder ck = new ChilledKongBuilder();
				//ITokenBuilder cheeky = new CheekyUntBuilder();
				//ITokenBuilder happy = new HappyHopperBuilder();
				//ITokenBuilder winternaru = new WinterNaruBuilder();
				//ITokenBuilder brightpalz = new BrightPalzBuilder();
				//ITokenBuilder raveBuilder = new RaveBuilder();
				//ITokenBuilder GhostWatchBuilder = new GhostWatchBuilder();
				ITokenBuilder PuurrtyBuilder = new PuurrtiesBuilder();
				//ITokenBuilder PendulumBuilder = new PendulumBuilder();



				Logger.Info("Sleeping for 150 seconds...");
				Thread.Sleep(150000);
			} while (true);
		}
	}
}
