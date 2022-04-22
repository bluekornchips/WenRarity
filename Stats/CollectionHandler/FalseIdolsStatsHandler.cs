
using Stats.Controller;
using System.ComponentModel.DataAnnotations;
using WenRarityLibrary.ADO.Blockfrost;
using WenRarityLibrary.ADO.Blockfrost.Models.OnChainMetaData.Token;
using WenRarityLibrary.ADO.Rime;
using WenRarityLibrary.ADO.Rime.Models.Rarity.Token;


namespace Stats.Builders
{
	public class FalseIdolsStatsHandler : BaseStatsHandler
	{
		private static RimeController _rimeController = RimeController.Instance;

		public override void Handle()
		{
			using BlockfrostADO bfContext = new();
			var tokens = bfContext.FalseIdols.ToList();

			_ducky.Info($"Found {tokens.Count()} tokens for FalseIdols");

			if (tokens != null)
			{

				// Back
				var back = tokens.GroupBy(t => t.Back);
				var backItems = new List<FalseIdolsBackRarity>();
				foreach (var item in back) backItems.Add(new FalseIdolsBackRarity()
				{
					 Back = item.Key,
					 Count = item.Count()
				});

				// Face
				var face = tokens.GroupBy(t => t.Face);
				var faceItems = new List<FalseIdolsFaceRarity>();
				foreach (var item in face) faceItems.Add(new FalseIdolsFaceRarity()
				{
					 Face = item.Key,
					 Count = item.Count()
				});

				// Head
				var head = tokens.GroupBy(t => t.Head);
				var headItems = new List<FalseIdolsHeadRarity>();
				foreach (var item in head) headItems.Add(new FalseIdolsHeadRarity()
				{
					 Head = item.Key,
					 Count = item.Count()
				});

				// Outfit
				var outfit = tokens.GroupBy(t => t.Outfit);
				var outfitItems = new List<FalseIdolsOutfitRarity>();
				foreach (var item in outfit) outfitItems.Add(new FalseIdolsOutfitRarity()
				{
					 Outfit = item.Key,
					 Count = item.Count()
				});

				// Character
				var character = tokens.GroupBy(t => t.Character);
				var characterItems = new List<FalseIdolsCharacterRarity>();
				foreach (var item in character) characterItems.Add(new FalseIdolsCharacterRarity()
				{
					 Character = item.Key,
					 Count = item.Count()
				});

				// Background
				var background = tokens.GroupBy(t => t.Background);
				var backgroundItems = new List<FalseIdolsBackgroundRarity>();
				foreach (var item in background) backgroundItems.Add(new FalseIdolsBackgroundRarity()
				{
					 Background = item.Key,
					 Count = item.Count()
				});

				// traitCount
				var traitCount = tokens.GroupBy(t => t.traitCount);
				var traitCountItems = new List<FalseIdolsTraitCountRarity>();
				foreach (var item in traitCount) traitCountItems.Add(new FalseIdolsTraitCountRarity()
				{
					 traitCount = item.Key,
					 Count = item.Count()
				});

				using RimeADO context = new();
				var trans = context.Database.BeginTransaction();
				try
				{
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[FalseIdolsBackRarity]");
					context.FalseIdolsBackRarity.AddRange(backItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[FalseIdolsFaceRarity]");
					context.FalseIdolsFaceRarity.AddRange(faceItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[FalseIdolsHeadRarity]");
					context.FalseIdolsHeadRarity.AddRange(headItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[FalseIdolsOutfitRarity]");
					context.FalseIdolsOutfitRarity.AddRange(outfitItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[FalseIdolsCharacterRarity]");
					context.FalseIdolsCharacterRarity.AddRange(characterItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[FalseIdolsBackgroundRarity]");
					context.FalseIdolsBackgroundRarity.AddRange(backgroundItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[FalseIdolsTraitCountRarity]");
					context.FalseIdolsTraitCountRarity.AddRange(traitCountItems);
					context.SaveChanges();
					trans.Commit();
					_ducky.Info($"Updated FalseIdolsRarity.");
				}
				catch (Exception ex)
				{
					_ducky.Error("FalseIdolsStatsHandler", "FalseIdolsRarity_Pet", ex.Message);
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
				var blockfrostItems = bfContext.FalseIdols.ToList();
				var size = blockfrostItems.Count;
				var traitCountRarity = rimeContext.FalseIdolsTraitCountRarity.ToList();
				var backRarity = rimeContext.FalseIdolsBackRarity.ToList();
				var faceRarity = rimeContext.FalseIdolsFaceRarity.ToList();
				var headRarity = rimeContext.FalseIdolsHeadRarity.ToList();
				var outfitRarity = rimeContext.FalseIdolsOutfitRarity.ToList();
				var characterRarity = rimeContext.FalseIdolsCharacterRarity.ToList();
				var backgroundRarity = rimeContext.FalseIdolsBackgroundRarity.ToList();

				var rimeItems = new List<FalseIdolsRarity>();

				foreach (var item in blockfrostItems)
				{
					var rarity = new FalseIdolsRarity()
					{
						asset = item.asset,
						name = item.name,
						fingerprint = item.fingerprint
					};

					rarity.weighting = 0;
					rarity.traitCount = MH(traitCountRarity.Where(i => i.traitCount == item.traitCount).FirstOrDefault().Count, size);

					rarity.weighting += rarity.traitCount;
					rarity.Back = MH(backRarity.Where(i => i.Back == item.Back).FirstOrDefault().Count, size);

					rarity.weighting += rarity.Back;
					rarity.Face = MH(faceRarity.Where(i => i.Face == item.Face).FirstOrDefault().Count, size);

					rarity.weighting += rarity.Face;
					rarity.Head = MH(headRarity.Where(i => i.Head == item.Head).FirstOrDefault().Count, size);

					rarity.weighting += rarity.Head;
					rarity.Outfit = MH(outfitRarity.Where(i => i.Outfit == item.Outfit).FirstOrDefault().Count, size);

					rarity.weighting += rarity.Outfit;
					rarity.Character = MH(characterRarity.Where(i => i.Character == item.Character).FirstOrDefault().Count, size);

					rarity.weighting += rarity.Character;
					rarity.Background = MH(backgroundRarity.Where(i => i.Background == item.Background).FirstOrDefault().Count, size);

					rarity.weighting += rarity.Background;

					rimeItems.Add(rarity);
				}
				rimeContext.Database.ExecuteSqlCommand($"DELETE FROM FalseIdolsRarity; ");
				rimeContext.Database.ExecuteSqlCommand($"DBCC CHECKIDENT('FalseIdolsRarity', RESEED, 0); ");

				rimeContext.FalseIdolsRarity.AddRange(rimeItems);
				trans.Commit();
				rimeContext.SaveChanges();

				_ducky.Info($"Created rarity table for FalseIdols.");
			}
			catch (Exception ex)
			{
				trans.Rollback();
				_ducky.Error("FalseIdolsStatsHandler", "GenerateCollectionRarity_SQL", ex.Message);
				throw;
			}
		}
	}
}
