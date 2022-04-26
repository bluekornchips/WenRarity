using BlockfrostLibrary.ADO;
using BlockfrostLibrary.ADO.Models.OnChainMetaData;
using BlockfrostLibrary.ADO.Models.OnChainMetaData.Token;
using WenRarityLibrary;

namespace Blockfrost.Builder
{
    public class OnChainMetaDataModelHandler
    {
        private static OnChainMetaDataModelHandler instance;
        public static OnChainMetaDataModelHandler Instance => instance ?? (instance = new OnChainMetaDataModelHandler());
        private OnChainMetaDataModelHandler() { }

        private static Ducky _ducky = Ducky.Instance;

		public void Add(OnChainMetaData item) { }

		//##_:
		//##_:TavernSquad+
		public void Add(TavernSquad item)
		{
			using BlockfrostADO context = new();
			var trans = context.Database.BeginTransaction();
			try
			{
				context.TavernSquad.Add(item);
				trans.Commit();
				context.SaveChanges();
			}
			catch (Exception ex)
			{
				trans.Rollback();
				_ducky.Error("OnChainMetaDataModelHandler", "Add(TavernSquad)", ex.Message);
			}
		}
		//##_:TavernSquad-
	}
}
