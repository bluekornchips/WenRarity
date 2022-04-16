using Rime.ADO;
namespace Rime.ViewModels.Asset.Token
{
	public class PendulumViewModel : OnChainMetaDataViewModel
	{
		public override OnChainMetaData Model()
		{
			Pendulum m = new();

			m.name = name;
			m.image = image;
			m.mediaType = mediaType;
			m.policy_id = policy_id;
			m.asset = asset;
			m.Ear = attributes.GetValueOrDefault("Ear");
			m.Eye = attributes.GetValueOrDefault("Eye");
			m.Body = attributes.GetValueOrDefault("Body");
			m.Eyes = attributes.GetValueOrDefault("Eyes");
			m.Head = attributes.GetValueOrDefault("Head");
			m.Skin = attributes.GetValueOrDefault("Skin");
			m.Mouth = attributes.GetValueOrDefault("Mouth");
			m.Background = attributes.GetValueOrDefault("Background");
			m.url = attributes.GetValueOrDefault("url");
			m.type = attributes.GetValueOrDefault("type");
			m.series = attributes.GetValueOrDefault("series");

			return m;
		}
		public override void Add()
		{
			OnChainMetaData m = Model();

			using RimeDb context = new();

			var trans = context.Database.BeginTransaction();
			try
			{
				context.Pendulums.Add((Pendulum)m);
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

			var all = context.Pendulums.ToList();
			try
			{
				foreach (var item in all)
				assets.Add(item.asset, item);
			}
			catch (Exception ex)
			{
				_ducky.Error("PendulumViewModel", "Get()", ex.Message);
			}
		}
	}
}
