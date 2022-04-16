using Rime.ADO;
namespace Rime.ViewModels.Asset.Token
{
	public class KBotViewModel : OnChainMetaDataViewModel
	{
		public override OnChainMetaData Model()
		{
			KBot m = new();

			m.name = name;
			m.image = image;
			m.mediaType = mediaType;
			m.policy_id = policy_id;
			m.asset = asset;
			m.Pet = attributes.GetValueOrDefault("Pet");
			m.str_Id = attributes.GetValueOrDefault("str_Id");
			m.website = attributes.GetValueOrDefault("website");
			m.copyright = attributes.GetValueOrDefault("copyright");
			m.royalties = attributes.GetValueOrDefault("royalties");
			m.collection = attributes.GetValueOrDefault("collection");

			return m;
		}
		public override void Add()
		{
			OnChainMetaData m = Model();

			using RimeDb context = new();

			var trans = context.Database.BeginTransaction();
			try
			{
				context.KBots.Add((KBot)m);
				trans.Commit();
				context.SaveChanges();
			}
			catch (Exception ex)
			{
				trans.Rollback();
				_ducky.Error("KBotViewModel", "Add()", ex.Message);
			}
		}
		public override void Get(out Dictionary<string, OnChainMetaData> assets)
		{
			assets = new();

			using RimeDb context = new();

			var all = context.KBots.ToList();
			try
			{
				foreach (var item in all)
				assets.Add(item.asset, item);
			}
			catch (Exception ex)
			{
				_ducky.Error("KBotViewModel", "Get()", ex.Message);
			}
		}
	}
}
