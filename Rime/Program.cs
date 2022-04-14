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
        }

        static void Builders()
        {
            //ITokenBuilder ElMatadorBuilder = new ElMatadorBuilder();
            ITokenBuilder PuurrtyBuilder = new PuurrtiesBuilder();
            //ITokenBuilder PendulumBuilder = new PendulumBuilder();
            //do
            //{

            //             Logger.Info("Sleeping for 150 seconds...");
            //	Thread.Sleep(150000);
            //} while (true);
        }
    }
}
