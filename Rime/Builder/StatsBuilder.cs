using Blockfrost.Controller;
using BlockfrostLibrary.ADO.Models.Collection;
using RimeLibrary.Builders;
using RimeLibrary.Classes;
using WenRarityLibrary;

namespace Rime.Builders
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

            _bfController.CollectionByName(name, out BlockfrostCollection collection);

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
                    break;
                case "TavernSquad":
                    _stats.traitsIncluded = new()
                    {
                        "Back",
                        "Eyes",
                        "Face",
                        "Head",
                        "Race",
                        "Armor",
                        "Mouth",
                        "Racial",
                        "Familiar",
                        "SkinTone",
                        "Background",
                    };
                    break;
                default:
                    break;
            }
        }
    }
}



































