using Stats.Controller;
using WenRarityLibrary;
using WenRarityLibrary.ADO.Blockfrost.Models;
using WenRarityLibrary.Builders;
using WenRarityLibrary.Generic;

namespace Stats.Builders
{
    internal class StatsBuilder
    {
        private static StatsBuilder _instance;
        public static StatsBuilder Instance => _instance ?? (_instance = new StatsBuilder());
        private StatsBuilder() { }

        private static Ducky _ducky = Ducky.Instance;
        private static BlockfrostController _bfController = BlockfrostController.Instance;
        private static RimeController _rimeController = RimeController.Instance;
        private static RimeFrameworkBuilder _rimefb = RimeFrameworkBuilder.Instance;
        private static StatsHandler _statsb = StatsHandler.Instance;

        private static StatsContainer _stats = new StatsContainer();

        public void Build()
        {
            string name = "KBot";

            _bfController.CollectionByName(name, out Collection collection);
            if (collection == null)
            {
                _ducky.Error("StatsBuilder", "Build", $"No collection found for {name}.");
                return;
            }

            _stats = new StatsContainer()
            {
                collection = collection
            };

            AttributesForRarity();

            //_rimefb.CreateToken(_stats);

            //if (!_rimeController.CheckTokenExists(collection))
            //{
            //    _ducky.Critical("StatsBuilder", "Build", $"Token does not exist for {collection.Name}.");
            //    return;
            //}

            //ClearAttributeTables();
            PopulateAttributeTables();
        }

        private void PopulateAttributeTables()
        {
            switch (_stats.collection.Name)
            {
                //##_:populate+
                //##_:KBot+
                case "KBot":
                    _statsb.Handle(_stats);
                    break;
                //##_:KBot-
                default:
                    break;
            }
        }

        public void AttributesForRarity()
        {
            switch (_stats.collection.Name)
            {
                //##_:attributes+
                case "KBot":
                    _stats.traitsIncluded = new List<string>()
                    {
                        "Pet"
                    };
                    break;
                default:
                    break;
            }
        }
    }
}
