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
        private static StatsFrameworkBuilder _statsfb = StatsFrameworkBuilder.Instance;
        private static StatsHandler _statsb = StatsHandler.Instance;

        private static StatsContainer _stats = new StatsContainer();

        public void Build(string name)
        {
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


            if (!_statsfb.CollectionExists(name))
            {
                _rimefb.CreateToken(_stats);
                _statsfb.Build(collection, _stats);
                return;
            }

            //if (!_rimeController.CheckTokenExists(collection))
            //{
            //    _ducky.Critical("StatsBuilder", "Build", $"Token does not exist for {collection.Name}.");
            //    return;
            //}

            //_statsfb.Build(collection);

            //ClearAttributeTables();
            PopulateAttributeTables();
        }

        private void PopulateAttributeTables()
        {
            switch (_stats.collection.Name)
            {
                //##_:populate+
				//##_:PuurrtyCatsSociety+
				case "PuurrtyCatsSociety":
					_statsb.statsHandler = new PuurrtyCatsSocietyStatsHandler();
					_statsb.statsHandler.Handle();
					_statsb.statsHandler.GenerateCollectionRarity_SQL();
					break;
				//##_:PuurrtyCatsSociety-
				
				
				
				
				
				
				
				
				
				
				
				
				//##_:KBot+
				case "KBot":
					_statsb.statsHandler = new KBotStatsHandler();
					_statsb.statsHandler.Handle();
					_statsb.statsHandler.GenerateCollectionRarity_SQL();
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
                case "PuurrtyCatsSociety":
                    _stats.traitsIncluded = new List<string>()
                    {
                        "fur",
                        "hat",
                        "eyes",
                        "mask",
                        "tail",
                        "hands",
                        "mouth",
                        "wings",
                        "outfit",
                        "background"
                    };
                    _stats.includeTraitCount = true;
                    break;
                default:
                    break;
            }
        }
    }
}




















































































