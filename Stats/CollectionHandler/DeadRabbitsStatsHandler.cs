
//using Stats.Controller;
//using System.ComponentModel.DataAnnotations;
//using WenRarityLibrary.ADO.Blockfrost;
//using WenRarityLibrary.ADO.Blockfrost.Models.OnChainMetaData.Token;
//using WenRarityLibrary.ADO.Rime;
//using WenRarityLibrary.ADO.Rime.Models.Rarity.Token;


//namespace Stats.Builders
//{
//	public class DeadRabbitsStatsHandler : BaseStatsHandler
//	{
//		public override void Handle()
//		{
//			using BlockfrostADO bfContext = new();
//			var tokens = bfContext.DeadRabbits.ToList();

//			_ducky.Info($"Found {tokens.Count()} tokens for DeadRabbits");

//			if (tokens != null)
//			{

//				// Jaw
//				var jaw = tokens.GroupBy(t => t.Jaw);
//				var jawItems = new List<DeadRabbitsJawRarity>();
//				foreach (var item in jaw) jawItems.Add(new DeadRabbitsJawRarity()
//				{
//					 Jaw = item.Key,
//					 Count = item.Count()
//				});

//				// Pin
//				var pin = tokens.GroupBy(t => t.Pin);
//				var pinItems = new List<DeadRabbitsPinRarity>();
//				foreach (var item in pin) pinItems.Add(new DeadRabbitsPinRarity()
//				{
//					 Pin = item.Key,
//					 Count = item.Count()
//				});

//				// Ears
//				var ears = tokens.GroupBy(t => t.Ears);
//				var earsItems = new List<DeadRabbitsEarsRarity>();
//				foreach (var item in ears) earsItems.Add(new DeadRabbitsEarsRarity()
//				{
//					 Ears = item.Key,
//					 Count = item.Count()
//				});

//				// Eyes
//				var eyes = tokens.GroupBy(t => t.Eyes);
//				var eyesItems = new List<DeadRabbitsEyesRarity>();
//				foreach (var item in eyes) eyesItems.Add(new DeadRabbitsEyesRarity()
//				{
//					 Eyes = item.Key,
//					 Count = item.Count()
//				});

//				// Order
//				var order = tokens.GroupBy(t => t.Order);
//				var orderItems = new List<DeadRabbitsOrderRarity>();
//				foreach (var item in order) orderItems.Add(new DeadRabbitsOrderRarity()
//				{
//					 Order = item.Key,
//					 Count = item.Count()
//				});

//				// Teeth
//				var teeth = tokens.GroupBy(t => t.Teeth);
//				var teethItems = new List<DeadRabbitsTeethRarity>();
//				foreach (var item in teeth) teethItems.Add(new DeadRabbitsTeethRarity()
//				{
//					 Teeth = item.Key,
//					 Count = item.Count()
//				});

//				// Eyewear
//				var eyewear = tokens.GroupBy(t => t.Eyewear);
//				var eyewearItems = new List<DeadRabbitsEyewearRarity>();
//				foreach (var item in eyewear) eyewearItems.Add(new DeadRabbitsEyewearRarity()
//				{
//					 Eyewear = item.Key,
//					 Count = item.Count()
//				});

//				// Clothing
//				var clothing = tokens.GroupBy(t => t.Clothing);
//				var clothingItems = new List<DeadRabbitsClothingRarity>();
//				foreach (var item in clothing) clothingItems.Add(new DeadRabbitsClothingRarity()
//				{
//					 Clothing = item.Key,
//					 Count = item.Count()
//				});

//				// EarTags
//				var eartags = tokens.GroupBy(t => t.EarTags);
//				var eartagsItems = new List<DeadRabbitsEarTagsRarity>();
//				foreach (var item in eartags) eartagsItems.Add(new DeadRabbitsEarTagsRarity()
//				{
//					 EarTags = item.Key,
//					 Count = item.Count()
//				});

//				// Headwear
//				var headwear = tokens.GroupBy(t => t.Headwear);
//				var headwearItems = new List<DeadRabbitsHeadwearRarity>();
//				foreach (var item in headwear) headwearItems.Add(new DeadRabbitsHeadwearRarity()
//				{
//					 Headwear = item.Key,
//					 Count = item.Count()
//				});

//				// Background
//				var background = tokens.GroupBy(t => t.Background);
//				var backgroundItems = new List<DeadRabbitsBackgroundRarity>();
//				foreach (var item in background) backgroundItems.Add(new DeadRabbitsBackgroundRarity()
//				{
//					 Background = item.Key,
//					 Count = item.Count()
//				});

//				// MouthBling
//				var mouthbling = tokens.GroupBy(t => t.MouthBling);
//				var mouthblingItems = new List<DeadRabbitsMouthBlingRarity>();
//				foreach (var item in mouthbling) mouthblingItems.Add(new DeadRabbitsMouthBlingRarity()
//				{
//					 MouthBling = item.Key,
//					 Count = item.Count()
//				});

//				// traitCount
//				var traitCount = tokens.GroupBy(t => t.traitCount);
//				var traitCountItems = new List<DeadRabbitsTraitCountRarity>();
//				foreach (var item in traitCount) traitCountItems.Add(new DeadRabbitsTraitCountRarity()
//				{
//					 traitCount = item.Key,
//					 Count = item.Count()
//				});

//				using RimeADO context = new();
//				var trans = context.Database.BeginTransaction();
//				try
//				{
//					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[DeadRabbitsJawRarity]");
//					context.DeadRabbitsJawRarity.AddRange(jawItems);
//					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[DeadRabbitsPinRarity]");
//					context.DeadRabbitsPinRarity.AddRange(pinItems);
//					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[DeadRabbitsEarsRarity]");
//					context.DeadRabbitsEarsRarity.AddRange(earsItems);
//					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[DeadRabbitsEyesRarity]");
//					context.DeadRabbitsEyesRarity.AddRange(eyesItems);
//					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[DeadRabbitsOrderRarity]");
//					context.DeadRabbitsOrderRarity.AddRange(orderItems);
//					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[DeadRabbitsTeethRarity]");
//					context.DeadRabbitsTeethRarity.AddRange(teethItems);
//					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[DeadRabbitsEyewearRarity]");
//					context.DeadRabbitsEyewearRarity.AddRange(eyewearItems);
//					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[DeadRabbitsClothingRarity]");
//					context.DeadRabbitsClothingRarity.AddRange(clothingItems);
//					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[DeadRabbitsEarTagsRarity]");
//					context.DeadRabbitsEarTagsRarity.AddRange(eartagsItems);
//					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[DeadRabbitsHeadwearRarity]");
//					context.DeadRabbitsHeadwearRarity.AddRange(headwearItems);
//					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[DeadRabbitsBackgroundRarity]");
//					context.DeadRabbitsBackgroundRarity.AddRange(backgroundItems);
//					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[DeadRabbitsMouthBlingRarity]");
//					context.DeadRabbitsMouthBlingRarity.AddRange(mouthblingItems);
//					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[DeadRabbitsTraitCountRarity]");
//					context.DeadRabbitsTraitCountRarity.AddRange(traitCountItems);
//					context.SaveChanges();
//					trans.Commit();
//					_ducky.Info($"Updated DeadRabbitsRarity.");
//				}
//				catch (Exception ex)
//				{
//					_ducky.Error("DeadRabbitsStatsHandler", "DeadRabbitsRarity_Pet", ex.Message);
//					throw;
//				}
//			}
//		}

//		public override void GenerateCollectionRarity_SQL()
//		{
//			using RimeADO rimeContext = new();
//			using BlockfrostADO bfContext = new();
//			var trans = rimeContext.Database.BeginTransaction();
//			try
//			{
//				var blockfrostItems = bfContext.DeadRabbits.ToList();
//				var size = blockfrostItems.Count;
//				var traitCountRarity = rimeContext.DeadRabbitsTraitCountRarity.ToList();
//				var jawRarity = rimeContext.DeadRabbitsJawRarity.ToList();
//				var pinRarity = rimeContext.DeadRabbitsPinRarity.ToList();
//				var earsRarity = rimeContext.DeadRabbitsEarsRarity.ToList();
//				var eyesRarity = rimeContext.DeadRabbitsEyesRarity.ToList();
//				var orderRarity = rimeContext.DeadRabbitsOrderRarity.ToList();
//				var teethRarity = rimeContext.DeadRabbitsTeethRarity.ToList();
//				var eyewearRarity = rimeContext.DeadRabbitsEyewearRarity.ToList();
//				var clothingRarity = rimeContext.DeadRabbitsClothingRarity.ToList();
//				var eartagsRarity = rimeContext.DeadRabbitsEarTagsRarity.ToList();
//				var headwearRarity = rimeContext.DeadRabbitsHeadwearRarity.ToList();
//				var backgroundRarity = rimeContext.DeadRabbitsBackgroundRarity.ToList();
//				var mouthblingRarity = rimeContext.DeadRabbitsMouthBlingRarity.ToList();

//				var rimeItems = new List<DeadRabbitsRarity>();

//				foreach (var item in blockfrostItems)
//				{
//					var rarity = new DeadRabbitsRarity()
//					{
//						asset = item.asset,
//						name = item.name,
//						fingerprint = item.fingerprint
//					};

//					rarity.weighting = 0;

//					rarity.traitCount = MH(traitCountRarity.Where(i => i.traitCount == item.traitCount).FirstOrDefault().Count, size);
//					rarity.weighting += rarity.traitCount;

//					rarity.Jaw = MH(jawRarity.Where(i => i.Jaw == item.Jaw).FirstOrDefault().Count, size);
//					rarity.weighting += rarity.Jaw;

//					rarity.Pin = MH(pinRarity.Where(i => i.Pin == item.Pin).FirstOrDefault().Count, size);
//					rarity.weighting += rarity.Pin;

//					rarity.Ears = MH(earsRarity.Where(i => i.Ears == item.Ears).FirstOrDefault().Count, size);
//					rarity.weighting += rarity.Ears;

//					rarity.Eyes = MH(eyesRarity.Where(i => i.Eyes == item.Eyes).FirstOrDefault().Count, size);
//					rarity.weighting += rarity.Eyes;

//					rarity.Order = MH(orderRarity.Where(i => i.Order == item.Order).FirstOrDefault().Count, size);
//					rarity.weighting += rarity.Order;

//					rarity.Teeth = MH(teethRarity.Where(i => i.Teeth == item.Teeth).FirstOrDefault().Count, size);
//					rarity.weighting += rarity.Teeth;

//					rarity.Eyewear = MH(eyewearRarity.Where(i => i.Eyewear == item.Eyewear).FirstOrDefault().Count, size);
//					rarity.weighting += rarity.Eyewear;

//					rarity.Clothing = MH(clothingRarity.Where(i => i.Clothing == item.Clothing).FirstOrDefault().Count, size);
//					rarity.weighting += rarity.Clothing;

//					rarity.EarTags = MH(eartagsRarity.Where(i => i.EarTags == item.EarTags).FirstOrDefault().Count, size);
//					rarity.weighting += rarity.EarTags;

//					rarity.Headwear = MH(headwearRarity.Where(i => i.Headwear == item.Headwear).FirstOrDefault().Count, size);
//					rarity.weighting += rarity.Headwear;

//					rarity.Background = MH(backgroundRarity.Where(i => i.Background == item.Background).FirstOrDefault().Count, size);
//					rarity.weighting += rarity.Background;

//					rarity.MouthBling = MH(mouthblingRarity.Where(i => i.MouthBling == item.MouthBling).FirstOrDefault().Count, size);
//					rarity.weighting += rarity.MouthBling;

//					rimeItems.Add(rarity);
//				}
//				rimeContext.Database.ExecuteSqlCommand($"DELETE FROM DeadRabbitsRarity; ");
//				rimeContext.Database.ExecuteSqlCommand($"DBCC CHECKIDENT('DeadRabbitsRarity', RESEED, 0); ");

//				rimeContext.DeadRabbitsRarity.AddRange(rimeItems);
//				trans.Commit();
//				rimeContext.SaveChanges();

//				_ducky.Info($"Created rarity table for DeadRabbits.");
//			}
//			catch (Exception ex)
//			{
//				trans.Rollback();
//				_ducky.Error("DeadRabbitsStatsHandler", "GenerateCollectionRarity_SQL", ex.Message);
//				throw;
//			}
//		}
//	}
//}
