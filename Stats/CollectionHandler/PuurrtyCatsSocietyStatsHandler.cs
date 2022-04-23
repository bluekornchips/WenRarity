using WenRarityLibrary.ADO.Blockfrost;
using WenRarityLibrary.ADO.Rime;
using WenRarityLibrary.ADO.Rime.Models.Rarity.Token;

namespace Stats.Builders
{
	public class PuurrtyCatsSocietyStatsHandler : BaseStatsHandler
	{
		public override void Handle()
		{
			using BlockfrostADO bfContext = new();
			var tokens = bfContext.PuurrtyCatsSociety.ToList();

			_ducky.Info($"Found {tokens.Count()} tokens for PuurrtyCatsSociety");

			if (tokens != null)
			{

				// fur
				var fur = tokens.GroupBy(t => t.fur);
				var furItems = new List<PuurrtyCatsSocietyFurRarity>();
				foreach (var item in fur) furItems.Add(new PuurrtyCatsSocietyFurRarity()
				{
					 fur = item.Key,
					 Count = item.Count()
				});

				// hat
				var hat = tokens.GroupBy(t => t.hat);
				var hatItems = new List<PuurrtyCatsSocietyHatRarity>();
				foreach (var item in hat) hatItems.Add(new PuurrtyCatsSocietyHatRarity()
				{
					 hat = item.Key,
					 Count = item.Count()
				});

				// eyes
				var eyes = tokens.GroupBy(t => t.eyes);
				var eyesItems = new List<PuurrtyCatsSocietyEyesRarity>();
				foreach (var item in eyes) eyesItems.Add(new PuurrtyCatsSocietyEyesRarity()
				{
					 eyes = item.Key,
					 Count = item.Count()
				});

				// mask
				var mask = tokens.GroupBy(t => t.mask);
				var maskItems = new List<PuurrtyCatsSocietyMaskRarity>();
				foreach (var item in mask) maskItems.Add(new PuurrtyCatsSocietyMaskRarity()
				{
					 mask = item.Key,
					 Count = item.Count()
				});

				// tail
				var tail = tokens.GroupBy(t => t.tail);
				var tailItems = new List<PuurrtyCatsSocietyTailRarity>();
				foreach (var item in tail) tailItems.Add(new PuurrtyCatsSocietyTailRarity()
				{
					 tail = item.Key,
					 Count = item.Count()
				});

				// hands
				var hands = tokens.GroupBy(t => t.hands);
				var handsItems = new List<PuurrtyCatsSocietyHandsRarity>();
				foreach (var item in hands) handsItems.Add(new PuurrtyCatsSocietyHandsRarity()
				{
					 hands = item.Key,
					 Count = item.Count()
				});

				// mouth
				var mouth = tokens.GroupBy(t => t.mouth);
				var mouthItems = new List<PuurrtyCatsSocietyMouthRarity>();
				foreach (var item in mouth) mouthItems.Add(new PuurrtyCatsSocietyMouthRarity()
				{
					 mouth = item.Key,
					 Count = item.Count()
				});

				// wings
				var wings = tokens.GroupBy(t => t.wings);
				var wingsItems = new List<PuurrtyCatsSocietyWingsRarity>();
				foreach (var item in wings) wingsItems.Add(new PuurrtyCatsSocietyWingsRarity()
				{
					 wings = item.Key,
					 Count = item.Count()
				});

				// outfit
				var outfit = tokens.GroupBy(t => t.outfit);
				var outfitItems = new List<PuurrtyCatsSocietyOutfitRarity>();
				foreach (var item in outfit) outfitItems.Add(new PuurrtyCatsSocietyOutfitRarity()
				{
					 outfit = item.Key,
					 Count = item.Count()
				});

				// background
				var background = tokens.GroupBy(t => t.background);
				var backgroundItems = new List<PuurrtyCatsSocietyBackgroundRarity>();
				foreach (var item in background) backgroundItems.Add(new PuurrtyCatsSocietyBackgroundRarity()
				{
					 background = item.Key,
					 Count = item.Count()
				});

				// traitCount
				var traitCount = tokens.GroupBy(t => t.traitCount);
				var traitCountItems = new List<PuurrtyCatsSocietyTraitCountRarity>();
				foreach (var item in traitCount) traitCountItems.Add(new PuurrtyCatsSocietyTraitCountRarity()
				{
					 traitCount = item.Key,
					 Count = item.Count()
				});

				using RimeADO context = new();
				var trans = context.Database.BeginTransaction();
				try
				{
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[PuurrtyCatsSocietyFurRarity]");
					context.PuurrtyCatsSocietyFurRarity.AddRange(furItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[PuurrtyCatsSocietyHatRarity]");
					context.PuurrtyCatsSocietyHatRarity.AddRange(hatItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[PuurrtyCatsSocietyEyesRarity]");
					context.PuurrtyCatsSocietyEyesRarity.AddRange(eyesItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[PuurrtyCatsSocietyMaskRarity]");
					context.PuurrtyCatsSocietyMaskRarity.AddRange(maskItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[PuurrtyCatsSocietyTailRarity]");
					context.PuurrtyCatsSocietyTailRarity.AddRange(tailItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[PuurrtyCatsSocietyHandsRarity]");
					context.PuurrtyCatsSocietyHandsRarity.AddRange(handsItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[PuurrtyCatsSocietyMouthRarity]");
					context.PuurrtyCatsSocietyMouthRarity.AddRange(mouthItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[PuurrtyCatsSocietyWingsRarity]");
					context.PuurrtyCatsSocietyWingsRarity.AddRange(wingsItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[PuurrtyCatsSocietyOutfitRarity]");
					context.PuurrtyCatsSocietyOutfitRarity.AddRange(outfitItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[PuurrtyCatsSocietyBackgroundRarity]");
					context.PuurrtyCatsSocietyBackgroundRarity.AddRange(backgroundItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[PuurrtyCatsSocietyTraitCountRarity]");
					context.PuurrtyCatsSocietyTraitCountRarity.AddRange(traitCountItems);
					context.SaveChanges();
					trans.Commit();
					_ducky.Info($"Updated PuurrtyCatsSocietyRarity.");
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
				var blockfrostItems = bfContext.PuurrtyCatsSociety.ToList();
				var size = blockfrostItems.Count;
				var traitCountRarity = rimeContext.PuurrtyCatsSocietyTraitCountRarity.ToList();
				var furRarity = rimeContext.PuurrtyCatsSocietyFurRarity.ToList();
				var hatRarity = rimeContext.PuurrtyCatsSocietyHatRarity.ToList();
				var eyesRarity = rimeContext.PuurrtyCatsSocietyEyesRarity.ToList();
				var maskRarity = rimeContext.PuurrtyCatsSocietyMaskRarity.ToList();
				var tailRarity = rimeContext.PuurrtyCatsSocietyTailRarity.ToList();
				var handsRarity = rimeContext.PuurrtyCatsSocietyHandsRarity.ToList();
				var mouthRarity = rimeContext.PuurrtyCatsSocietyMouthRarity.ToList();
				var wingsRarity = rimeContext.PuurrtyCatsSocietyWingsRarity.ToList();
				var outfitRarity = rimeContext.PuurrtyCatsSocietyOutfitRarity.ToList();
				var backgroundRarity = rimeContext.PuurrtyCatsSocietyBackgroundRarity.ToList();

				var rimeItems = new List<PuurrtyCatsSocietyRarity>();

				foreach (var item in blockfrostItems)
				{
					var rarity = new PuurrtyCatsSocietyRarity()
					{
						asset = item.asset,
						name = item.name,
						fingerprint = item.fingerprint
					};

					rarity.weighting = 0;
					rarity.traitCount = MH(traitCountRarity.Where(i => i.traitCount == item.traitCount).FirstOrDefault().Count, size);

					rarity.weighting += rarity.traitCount;
					rarity.fur = MH(furRarity.Where(i => i.fur == item.fur).FirstOrDefault().Count, size);

					rarity.weighting += rarity.fur;
					rarity.hat = MH(hatRarity.Where(i => i.hat == item.hat).FirstOrDefault().Count, size);

					rarity.weighting += rarity.hat;
					rarity.eyes = MH(eyesRarity.Where(i => i.eyes == item.eyes).FirstOrDefault().Count, size);

					rarity.weighting += rarity.eyes;
					rarity.mask = MH(maskRarity.Where(i => i.mask == item.mask).FirstOrDefault().Count, size);

					rarity.weighting += rarity.mask;
					rarity.tail = MH(tailRarity.Where(i => i.tail == item.tail).FirstOrDefault().Count, size);

					rarity.weighting += rarity.tail;
					rarity.hands = MH(handsRarity.Where(i => i.hands == item.hands).FirstOrDefault().Count, size);

					rarity.weighting += rarity.hands;
					rarity.mouth = MH(mouthRarity.Where(i => i.mouth == item.mouth).FirstOrDefault().Count, size);

					rarity.weighting += rarity.mouth;
					rarity.wings = MH(wingsRarity.Where(i => i.wings == item.wings).FirstOrDefault().Count, size);

					rarity.weighting += rarity.wings;
					rarity.outfit = MH(outfitRarity.Where(i => i.outfit == item.outfit).FirstOrDefault().Count, size);

					rarity.weighting += rarity.outfit;
					rarity.background = MH(backgroundRarity.Where(i => i.background == item.background).FirstOrDefault().Count, size);

					rarity.weighting += rarity.background;

					rimeItems.Add(rarity);
				}
				rimeContext.Database.ExecuteSqlCommand($"DELETE FROM PuurrtyCatsSocietyRarity; ");
				rimeContext.Database.ExecuteSqlCommand($"DBCC CHECKIDENT('PuurrtyCatsSocietyRarity', RESEED, 0); ");

				rimeContext.PuurrtyCatsSocietyRarity.AddRange(rimeItems);
				trans.Commit();
				rimeContext.SaveChanges();

				_ducky.Info($"Created rarity table for PuurrtyCatsSociety.");
			}
			catch (Exception ex)
			{
				trans.Rollback();
				_ducky.Error("PuurrtyCatsSocietyStatsHandler", "GenerateCollectionRarity_SQL", ex.Message);
				throw;
			}
		}
	}
}
