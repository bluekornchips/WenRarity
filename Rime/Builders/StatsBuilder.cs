using Rime.ADO;
using Rime.Controller;
using Rime.ViewModels.Collection;

namespace Rime.Builders
{
    public class StatsBuilder
    {
        private static StatsBuilder instance;
        public static StatsBuilder Instance => instance ?? (instance = new StatsBuilder());
        private static RimeController _rimeController = RimeController.Instance;
        private StatsBuilder() { }

        public void Build(CollectionViewModel collection)
        {

        }
    }
}

