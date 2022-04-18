using Rime.Builders;
using WenRarityLibrary;
using Rime.ViewModels.Collection;
using Rime.Builders;

namespace RimeTwo
{
    internal class Program
    {
        private static Ducky _ducky;
        static void Main(string[] args)
        {
            Setup();

            FrameworkBuilder builder = FrameworkBuilder.Instance;

            if(builder.Build(out CollectionViewModel collection))
            {
                _ducky.Info("Successfully wrote new Framework Items.");
                _ducky.Info("\n\nUPDATE DATABASE\n\n");
                return;
            }

            CollectionBuilder collectionBuilder = CollectionBuilder.Instance;
            collectionBuilder.Build(collection);

            StatsBuilder sb = StatsBuilder.Instance;
            sb.Build(collection);
        }

        static void Setup()
        {
            _ducky = Ducky.Instance;
        }
    }
}
