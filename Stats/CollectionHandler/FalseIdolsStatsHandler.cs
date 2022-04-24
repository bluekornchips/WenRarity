
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

		public override void Handle()
		{
			using BlockfrostADO bfContext = new();
			var tokens = bfContext.FalseIdols.ToList();

			_ducky.Info($"Found {tokens.Count()} tokens for FalseIdols");

			if (tokens != null)
			{

				// Back
				var back = tokens.GroupBy(t => t.Back);
				var backItems = new List<FalseIdolsBack>();
				foreach (var item in back) backItems.Add(new FalseIdolsBack()
				{
					 Back = item.Key,
					 Count = item.Count()
				});

				// Face
				var face = tokens.GroupBy(t => t.Face);
				var faceItems = new List<FalseIdolsFace>();
				foreach (var item in face) faceItems.Add(new FalseIdolsFace()
				{
					 Face = item.Key,
					 Count = item.Count()
				});

				// Head
				var head = tokens.GroupBy(t => t.Head);
				var headItems = new List<FalseIdolsHead>();
				foreach (var item in head) headItems.Add(new FalseIdolsHead()
				{
					 Head = item.Key,
					 Count = item.Count()
				});

				// Outfit
				var outfit = tokens.GroupBy(t => t.Outfit);
				var outfitItems = new List<FalseIdolsOutfit>();
				foreach (var item in outfit) outfitItems.Add(new FalseIdolsOutfit()
				{
					 Outfit = item.Key,
					 Count = item.Count()
				});

				// Character
				var character = tokens.GroupBy(t => t.Character);
				var characterItems = new List<FalseIdolsCharacter>();
				foreach (var item in character) characterItems.Add(new FalseIdolsCharacter()
				{
					 Character = item.Key,
					 Count = item.Count()
				});

				// Background
				var background = tokens.GroupBy(t => t.Background);
				var backgroundItems = new List<FalseIdolsBackground>();
				foreach (var item in background) backgroundItems.Add(new FalseIdolsBackground()
				{
					 Background = item.Key,
					 Count = item.Count()
				});

				// traitCount
				var traitCount = tokens.GroupBy(t => t.traitCount);
				var traitCountItems = new List<FalseIdolsTraitCount>();
				foreach (var item in traitCount) traitCountItems.Add(new FalseIdolsTraitCount()
				{
					 traitCount = item.Key,
					 Count = item.Count()
				});

				using RimeADO context = new();
				var trans = context.Database.BeginTransaction();
				try
				{
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[FalseIdolsBack]");
					context.FalseIdolsBack.AddRange(backItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[FalseIdolsFace]");
					context.FalseIdolsFace.AddRange(faceItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[FalseIdolsHead]");
					context.FalseIdolsHead.AddRange(headItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[FalseIdolsOutfit]");
					context.FalseIdolsOutfit.AddRange(outfitItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[FalseIdolsCharacter]");
					context.FalseIdolsCharacter.AddRange(characterItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[FalseIdolsBackground]");
					context.FalseIdolsBackground.AddRange(backgroundItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[FalseIdolsTraitCount]");
					context.FalseIdolsTraitCount.AddRange(traitCountItems);
					context.SaveChanges();
					trans.Commit();
					_ducky.Info($"Cleared FalseIdolsRarity.");
				}
				catch (Exception ex)
				{
					_ducky.Error("FalseIdolsStatsHandler", "FalseIdols_Pet", ex.Message);
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
				var traitCountRarity = rimeContext.FalseIdolsTraitCount.ToList();
				var back = rimeContext.FalseIdolsBack.ToList();
				var face = rimeContext.FalseIdolsFace.ToList();
				var head = rimeContext.FalseIdolsHead.ToList();
				var outfit = rimeContext.FalseIdolsOutfit.ToList();
				var character = rimeContext.FalseIdolsCharacter.ToList();
				var background = rimeContext.FalseIdolsBackground.ToList();

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

					rarity.Back = MH(back.Where(i => i.Back == item.Back).FirstOrDefault().Count, size);
					rarity.weighting += rarity.Back;

					rarity.Face = MH(face.Where(i => i.Face == item.Face).FirstOrDefault().Count, size);
					rarity.weighting += rarity.Face;

					rarity.Head = MH(head.Where(i => i.Head == item.Head).FirstOrDefault().Count, size);
					rarity.weighting += rarity.Head;

					rarity.Outfit = MH(outfit.Where(i => i.Outfit == item.Outfit).FirstOrDefault().Count, size);
					rarity.weighting += rarity.Outfit;

					rarity.Character = MH(character.Where(i => i.Character == item.Character).FirstOrDefault().Count, size);
					rarity.weighting += rarity.Character;

					rarity.Background = MH(background.Where(i => i.Background == item.Background).FirstOrDefault().Count, size);
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

        public override void RarityChart()
        {
            throw new NotImplementedException();
        }
    }
}
