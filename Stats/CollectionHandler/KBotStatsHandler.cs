using WenRarityLibrary.ADO.Blockfrost;
using WenRarityLibrary.ADO.Rime;
using WenRarityLibrary.ADO.Rime.Models.Rarity.Token;

namespace Stats.Builders
{
	public class KBotStatsHandler : BaseStatsHandler
	{
		public override void Handle()
		{
			using BlockfrostADO bfContext = new();
			var tokens = bfContext.KBot.ToList();

			_ducky.Info($"Found {tokens.Count()} tokens for KBot");

			if (tokens != null)
			{
				// Pet
				var pet = tokens.GroupBy(t => t.Pet);
				var petItems = new List<KBotPetRarity>();
				foreach (var item in pet) petItems.Add(new KBotPetRarity()
				{
					 Pet = item.Key,
					 Count = item.Count()
				});

				using RimeADO context = new();
				var trans = context.Database.BeginTransaction();
				try
				{
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[KBotPetRarity]");
					context.KBotPetRarity.AddRange(petItems);
					context.SaveChanges();
					trans.Commit();
					_ducky.Info($"Updated KBotRarity.");
				}
				catch (Exception ex)
				{
					_ducky.Error("RimeController", "UpdateKBotRarity_Pet", ex.Message);
					throw;
				}
			}
		}

		public override void GenerateCollectionRarity_SQL()
		{
			using RimeADO rimeContext = new();
			using BlockfrostADO bfContext = new();
			var trans = rimeContext.Database.BeginTransaction();
			try
			{
				var blockfrostItems = bfContext.KBot.ToList();
				var size = blockfrostItems.Count;

				var petRarity = rimeContext.KBotPetRarity.ToList();

				var rimeItems = new List<KBotRarity>();

				foreach (var item in blockfrostItems)
				{
					var rarity = new KBotRarity()
					{
						asset = item.asset,
						name = item.name,
						fingerprint = item.fingerprint
					};

					rarity.Pet = MH(petRarity.Where(i => i.Pet == item.Pet).FirstOrDefault().Count, size);

					rimeItems.Add(rarity);
				}
				rimeContext.Database.ExecuteSqlCommand($"DELETE FROM KBotRarity; ");
				rimeContext.Database.ExecuteSqlCommand($"DBCC CHECKIDENT('KBotRarity', RESEED, 0); ");

				rimeContext.KBotRarity.AddRange(rimeItems);
				trans.Commit();
				rimeContext.SaveChanges();

				_ducky.Info($"Created rarity table for KBot.");
			}
			catch (Exception ex)
			{
				trans.Rollback();
				_ducky.Error("KBotStatsHandler", "GenerateCollectionRarity_SQL", ex.Message);
				throw;
			}
		}

        public override void RarityChart()
        {
            throw new NotImplementedException();
        }
    }
}
