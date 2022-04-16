using RimeTwo.ADO.Tables;
using RimeTwo.Util;
using RimeTwo.ViewModels.Collection;

namespace RimeTwo
{
    internal class Program
    {
        private static Ducky _ducky;
        static void Main(string[] args)
        {
            Setup();

            FrameworkBuilder builder = FrameworkBuilder.Instance;

            CollectionViewModel collection = builder.Build();
            CollectionBuilder collectionBuilder = CollectionBuilder.Instance;
            collectionBuilder.Build(collection);
        }

        static void Setup()
        {
            _ducky = Ducky.Instance;
        }
    }
}
