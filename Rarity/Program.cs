using WenRarityLibrary;

namespace Rarity
{
    class Program
    {
        private static Ducky _ducky = Ducky.Instance;
        static void Main(string[] args)
        {
            Start();
        }

        static void Start()
        {
            _ducky.Start();
        }
    }
}