using Stats.Controller;
using WenRarityLibrary.ADO.Blockfrost.Models.OnChainMetaData.Token;
using WenRarityLibrary.ADO.Rime.Models.Rarity.Token;
using WenRarityLibrary.Generic;

namespace Stats.Builders
{
    public class KBotStatsHandler : StatsHandler
    {
        private static KBotStatsHandler _instance;
        public static KBotStatsHandler Instance => _instance ?? (_instance = new KBotStatsHandler());

        private KBotStatsHandler() {}

        private static BlockfrostController _bfController = BlockfrostController.Instance;
        private static RimeController _rimeController = RimeController.Instance;

        public override void Handle()
        {
            // Get all the values for the attribute
            _bfController.GetKBot(out List<KBot> tokens);

            if (tokens != null)
            {
                var pets = tokens.GroupBy(t => t.Pet);

                // Pet
                var values = new List<KBotPetRarity>();
                foreach (var pet in pets) values.Add(new KBotPetRarity()
                {
                    Pet = pet.Key,
                    Count = pet.Count()
                });
                _rimeController.UpdateKBotRarity_Pet(values);
            }
        }
    }

    public abstract class StatsHandler
    {
        private static StatsHandler _instance;
        public static StatsHandler Instance => _instance ?? (_instance = new BaseStatsHandler());

        public StatsHandler() { }

        public abstract void Handle();
    }

    public class BaseStatsHandler : StatsHandler
    {
        public override void Handle()
        {
            throw new NotImplementedException();
        }
    }
}
