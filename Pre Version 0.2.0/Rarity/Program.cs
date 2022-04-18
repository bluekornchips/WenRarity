using Rarity.Builders;
using WenRarityLibrary;

namespace Rarity
{
    class Program
    {
        private static Ducky _ducky = Ducky.Instance;
        static void Main(string[] args)
        {
            RarityBuilder rb = RarityBuilder.Instance;
            rb.Build();
        }
    }
}