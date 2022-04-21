using WenRarityLibrary;
using WenRarityLibrary.ADO.Rime;
using WenRarityLibrary.ADO.Rime.Models.Rarity.Token;

namespace Stats.Controller
{
    internal class RimeController
    {
        private static RimeController _instance;
        public static RimeController Instance => _instance ?? (_instance = new RimeController());
        private RimeController() { }

        private static Ducky _ducky = Ducky.Instance;

        //##_:update+
		
		
        public void UpdateKBotRarity_Pet(List<KBotPetRarity> values)
        {
            using RimeADO context = new();
            var trans = context.Database.BeginTransaction();
            try
            {
                context.Database.ExecuteSqlCommand($"DELETE FROM [dbo].[KBotPetRarity]");
                context.KBotPetRarity.AddRange(values);
                context.SaveChanges();
                trans.Commit();
                _ducky.Info($"Updated KBotPetRarity.");
            }
            catch (Exception ex)
            {
                _ducky.Error("RimeController", "UpdateKBotRarity_Pet", ex.Message);
                throw;
            }
        }
    }
}


















