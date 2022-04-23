using WenRarityLibrary.ADO.Blockfrost;
using WenRarityLibrary;
using WenRarityLibrary.ADO.Blockfrost.Models.OnChainMetaData.Token;

namespace Blockfrost.Builder
{
    public class OnChainMetaDataModelHandler
    {
        private static OnChainMetaDataModelHandler instance;
        public static OnChainMetaDataModelHandler Instance => instance ?? (instance = new OnChainMetaDataModelHandler());
        private OnChainMetaDataModelHandler() { }

        private static Ducky _ducky = Ducky.Instance;//##_://##_:DeadRabbits+
		public void Add(DeadRabbits item)
		{
			using BlockfrostADO context = new();
			var trans = context.Database.BeginTransaction();
			try
			{
				context.DeadRabbits.Add(item);
				trans.Commit();
				context.SaveChanges();
			}
			catch (Exception ex)
			{
				trans.Rollback();
				_ducky.Error("OnChainMetaDataModelHandler", "Add(DeadRabbits)", ex.Message);
			}
		}
		//##_:DeadRabbits-//##_:FalseIdols+
		public void Add(FalseIdols item)
		{
			using BlockfrostADO context = new();
			var trans = context.Database.BeginTransaction();
			try
			{
				context.FalseIdols.Add(item);
				trans.Commit();
				context.SaveChanges();
			}
			catch (Exception ex)
			{
				trans.Rollback();
				_ducky.Error("OnChainMetaDataModelHandler", "Add(FalseIdols)", ex.Message);
			}
		}
		//##_:FalseIdols-
		
		//##_:PuurrtyCatsSociety+
		public void Add(PuurrtyCatsSociety item)
		{
			using BlockfrostADO context = new();
			var trans = context.Database.BeginTransaction();
			try
			{
				context.PuurrtyCatsSociety.Add(item);
				trans.Commit();
				context.SaveChanges();
			}
			catch (Exception ex)
			{
				trans.Rollback();
				_ducky.Error("OnChainMetaDataModelHandler", "Add(PuurrtyCatsSociety)", ex.Message);
			}
		}
		//##_:PuurrtyCatsSociety-
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
