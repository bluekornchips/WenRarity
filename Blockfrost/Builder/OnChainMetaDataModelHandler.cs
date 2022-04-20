using Blockfrost.ADO;
using WenRarityLibrary;
using WenRarityLibrary.ADO.Blockfrost.Models.OnChainMetaData.Token;

namespace Blockfrost.Builder
{
    public class OnChainMetaDataModelHandler
    {
        private static OnChainMetaDataModelHandler instance;
        public static OnChainMetaDataModelHandler Instance => instance ?? (instance = new OnChainMetaDataModelHandler());
        private OnChainMetaDataModelHandler() { }

        private static Ducky _ducky = Ducky.Instance;

        //##_:
		//##_:KBot+
		public void Add(KBot item)
		{
			using BlockfrostADO context = new();
			var trans = context.Database.BeginTransaction();
			try
			{
				context.KBot.Add(item);
				trans.Commit();
				context.SaveChanges();
			}
			catch (Exception ex)
			{
				trans.Rollback();
				_ducky.Error("OnChainMetaDataModelHandler", "Add(KBot)", ex.Message);
			}
		}
		//##_:KBot-
    }
}




















