using Rime.ADO;
namespace Rime.ViewModels.Asset.Token
{
	public class GrandmasterAdventurerViewModel : OnChainMetaDataViewModel
	{
		public override OnChainMetaData Model()
		{
			GrandmasterAdventurer m = new();

			m.name = name;
			m.image = image;
			m.mediaType = mediaType;
			m.policy_id = policy_id;
			m.asset = asset;
			m.race = attributes.GetValueOrDefault("race");
			m.classAttr = attributes.GetValueOrDefault("className");
			m.genre = attributes.GetValueOrDefault("genre");
			m.level = attributes.GetValueOrDefault("level");
			m.weapon = attributes.GetValueOrDefault("weapon");
			m.subrace = attributes.GetValueOrDefault("subrace");

			return m;
		}
		public override void Add()
		{
			OnChainMetaData m = Model();

			using RimeDb context = new();

			var trans = context.Database.BeginTransaction();
			try
			{
				context.GrandmasterAdventurers.Add((GrandmasterAdventurer)m);
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

			var all = context.GrandmasterAdventurers.ToList();
			try
			{
				foreach (var item in all)
				assets.Add(item.asset, item);
			}
			catch (Exception ex)
			{
				_ducky.Error("GrandmasterAdventurerViewModel", "Get()", ex.Message);
			}
		}
	}
}
