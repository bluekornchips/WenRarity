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
        private static RimeFrameworkBuilder _rimeFB = RimeFrameworkBuilder.Instance;
        private static StatsFrameworkBuilder _statsFB = StatsFrameworkBuilder.Instance;
        private static StatsHandler _statsHandler = StatsHandler.Instance;

        private static StatsContainer _stats = new StatsContainer();

        /// <summary>
        /// Main Build operation.
        /// </summary>
        /// <param name="name"></param>
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
                collection = collection,
                includeTraitCount = true
            };

            AttributesForRarity();

            if (!_statsFB.CollectionExists(name))
            {
                _rimeFB.CreateToken(_stats);
                _statsFB.Build(collection, _stats);
                _ducky.Info("\n\nPlease add the Token to the AttributesForRarity() method.\n\n");
                return;
            }

            //// Uncomment to force an update.
            //_rimeFB.CreateToken(_stats);
            //_statsFB.Build(collection, _stats);

            PopulateAttributeTables();
        }

        private void PopulateAttributeTables()
        {
            switch (_stats.collection.Name)
            {
                //##_:populate+
				//##_:FalseIdols+
				case "FalseIdols":
					_statsHandler.handler = new FalseIdolsStatsHandler();
					_statsHandler.handler.Handle();
					_statsHandler.handler.GenerateCollectionRarity_SQL();
					break;
				//##_:FalseIdols-
				
				
				
				
				
				
                //##_:DeadRabbits+
                //case "DeadRabbits":
                //    _statsHandler.statsHandler = new DeadRabbitsStatsHandler();
                //    _statsHandler.statsHandler.Handle();
                //    _statsHandler.statsHandler.GenerateCollectionRarity_SQL();
                //    break;
                //##_:DeadRabbits-
                
                //##_:PuurrtyCatsSociety+
                case "PuurrtyCatsSociety":
                    _statsHandler.handler = new PuurrtyCatsSocietyStatsHandler();
                    _statsHandler.handler.Handle();
                    _statsHandler.handler.GenerateCollectionRarity_SQL();
                    break;
                //##_:PuurrtyCatsSociety-
                //##_:KBot+
                case "KBot":
                    _statsHandler.handler = new KBotStatsHandler();
                    _statsHandler.handler.Handle();
                    _statsHandler.handler.GenerateCollectionRarity_SQL();
                    break;
                //##_:KBot-
                default:
                    break;
            }
        }

        /// <summary>
        /// Easy method to handle which traits we want included in the rarity generation
        /// </summary>
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
                case "FalseIdols":
                    _stats.traitsIncluded = new List<string>()
                    {
                        "Back",
                        "Face",
                        "Head",
                        "Outfit",
                        "Character",
                        "Background"
                    };
                    _stats.includeTraitCount = true;
                    break;
                case "DeadRabbits":
                    _stats.traitsIncluded = new()
                    {
                        "Jaw",
                        "Pin",
                        "Ears",
                        "Eyes",
                        "Order",
                        "Teeth",
                        "Eyewear",
                        "Clothing",
                        "EarTags",
                        "Headwear",
                        "Background",
                        "MouthBling"
                    };
                    _stats.includeTraitCount = true;
                    break;
                default:
                    break;
            }
        }
    }
}
















