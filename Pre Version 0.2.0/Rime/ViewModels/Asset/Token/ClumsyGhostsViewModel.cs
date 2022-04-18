using Rime.ADO;
namespace Rime.ViewModels.Asset.Token
{
	public class ClumsyGhostsViewModel : OnChainMetaDataViewModel
	{
		public override OnChainMetaData Model()
		{
			ClumsyGhosts m = new();

			m.name = name;
			m.image = image;
			m.mediaType = mediaType;
			m.policy_id = policy_id;
			m.asset = asset;
			m.str_id = attributes.GetValueOrDefault("str_id");
			m.hat = attributes.GetValueOrDefault("hat");
			m.body = attributes.GetValueOrDefault("body");
			m.eyes = attributes.GetValueOrDefault("eyes");
			m.hands = attributes.GetValueOrDefault("hands");
			m.outfit = attributes.GetValueOrDefault("outfit");
			m.project = attributes.GetValueOrDefault("project");
			m.website = attributes.GetValueOrDefault("website");
			m.backdrop = attributes.GetValueOrDefault("backdrop");

			return m;
		}
		public override void Add()
		{
			OnChainMetaData m = Model();

			using RimeDb context = new();

			var trans = context.Database.BeginTransaction();
			try
			{
				context.ClumsyGhosts.Add((ClumsyGhosts)m);
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

			var all = context.ClumsyGhosts.ToList();
			try
			{
				foreach (var item in all)
				assets.Add(item.asset, item);
			}
			catch (Exception ex)
			{
				_ducky.Error("ClumsyGhostsViewModel", "Get()", ex.Message);
			}
		}

        public override void AttributeHandler()
        {
            throw new NotImplementedException();
        }
    }
}
