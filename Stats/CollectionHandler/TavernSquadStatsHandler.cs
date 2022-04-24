
using Stats.Controller;
using System.ComponentModel.DataAnnotations;
using WenRarityLibrary.ADO.Blockfrost;
using WenRarityLibrary.ADO.Blockfrost.Models.OnChainMetaData.Token;
using WenRarityLibrary.ADO.Rime;
using WenRarityLibrary.ADO.Rime.Models.Rarity.Token;
using WenRarityLibrary.Utils;


namespace Stats.Builders
{
	public class TavernSquadStatsHandler : BaseStatsHandler
	{
	static WenRarityFileIO _fileIO = WenRarityFileIO.Instance;

		public override void Handle()
		{
			using BlockfrostADO bfContext = new();
			var tokens = bfContext.TavernSquad.ToList();

			_ducky.Info($"Found {tokens.Count()} tokens for TavernSquad");

			if (tokens != null)
			{

				// Back
				var back = tokens.GroupBy(t => t.Back);
				var backItems = new List<TavernSquadBack>();
				foreach (var item in back) backItems.Add(new TavernSquadBack()
				{
					 Back = item.Key,
					 Count = item.Count()
				});

				// Eyes
				var eyes = tokens.GroupBy(t => t.Eyes);
				var eyesItems = new List<TavernSquadEyes>();
				foreach (var item in eyes) eyesItems.Add(new TavernSquadEyes()
				{
					 Eyes = item.Key,
					 Count = item.Count()
				});

				// Face
				var face = tokens.GroupBy(t => t.Face);
				var faceItems = new List<TavernSquadFace>();
				foreach (var item in face) faceItems.Add(new TavernSquadFace()
				{
					 Face = item.Key,
					 Count = item.Count()
				});

				// Head
				var head = tokens.GroupBy(t => t.Head);
				var headItems = new List<TavernSquadHead>();
				foreach (var item in head) headItems.Add(new TavernSquadHead()
				{
					 Head = item.Key,
					 Count = item.Count()
				});

				// Race
				var race = tokens.GroupBy(t => t.Race);
				var raceItems = new List<TavernSquadRace>();
				foreach (var item in race) raceItems.Add(new TavernSquadRace()
				{
					 Race = item.Key,
					 Count = item.Count()
				});

				// Armor
				var armor = tokens.GroupBy(t => t.Armor);
				var armorItems = new List<TavernSquadArmor>();
				foreach (var item in armor) armorItems.Add(new TavernSquadArmor()
				{
					 Armor = item.Key,
					 Count = item.Count()
				});

				// Mouth
				var mouth = tokens.GroupBy(t => t.Mouth);
				var mouthItems = new List<TavernSquadMouth>();
				foreach (var item in mouth) mouthItems.Add(new TavernSquadMouth()
				{
					 Mouth = item.Key,
					 Count = item.Count()
				});

				// Racial
				var racial = tokens.GroupBy(t => t.Racial);
				var racialItems = new List<TavernSquadRacial>();
				foreach (var item in racial) racialItems.Add(new TavernSquadRacial()
				{
					 Racial = item.Key,
					 Count = item.Count()
				});

				// Familiar
				var familiar = tokens.GroupBy(t => t.Familiar);
				var familiarItems = new List<TavernSquadFamiliar>();
				foreach (var item in familiar) familiarItems.Add(new TavernSquadFamiliar()
				{
					 Familiar = item.Key,
					 Count = item.Count()
				});

				// SkinTone
				var skintone = tokens.GroupBy(t => t.SkinTone);
				var skintoneItems = new List<TavernSquadSkinTone>();
				foreach (var item in skintone) skintoneItems.Add(new TavernSquadSkinTone()
				{
					 SkinTone = item.Key,
					 Count = item.Count()
				});

				// Background
				var background = tokens.GroupBy(t => t.Background);
				var backgroundItems = new List<TavernSquadBackground>();
				foreach (var item in background) backgroundItems.Add(new TavernSquadBackground()
				{
					 Background = item.Key,
					 Count = item.Count()
				});

				// traitCount
				var traitCount = tokens.GroupBy(t => t.traitCount);
				var traitCountItems = new List<TavernSquadTraitCount>();
				foreach (var item in traitCount) traitCountItems.Add(new TavernSquadTraitCount()
				{
					 traitCount = item.Key,
					 Count = item.Count()
				});

				using RimeADO context = new();
				var trans = context.Database.BeginTransaction();
				try
				{
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[TavernSquadBack]");
					context.TavernSquadBack.AddRange(backItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[TavernSquadEyes]");
					context.TavernSquadEyes.AddRange(eyesItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[TavernSquadFace]");
					context.TavernSquadFace.AddRange(faceItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[TavernSquadHead]");
					context.TavernSquadHead.AddRange(headItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[TavernSquadRace]");
					context.TavernSquadRace.AddRange(raceItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[TavernSquadArmor]");
					context.TavernSquadArmor.AddRange(armorItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[TavernSquadMouth]");
					context.TavernSquadMouth.AddRange(mouthItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[TavernSquadRacial]");
					context.TavernSquadRacial.AddRange(racialItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[TavernSquadFamiliar]");
					context.TavernSquadFamiliar.AddRange(familiarItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[TavernSquadSkinTone]");
					context.TavernSquadSkinTone.AddRange(skintoneItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[TavernSquadBackground]");
					context.TavernSquadBackground.AddRange(backgroundItems);
					context.Database.ExecuteSqlCommand($"DELETE FROM[dbo].[TavernSquadTraitCount]");
					context.TavernSquadTraitCount.AddRange(traitCountItems);
					context.SaveChanges();
					trans.Commit();
					_ducky.Info($"Cleared TavernSquadRarity.");
				}
				catch (Exception ex)
				{
					_ducky.Error("TavernSquadStatsHandler", "TavernSquad_Pet", ex.Message);
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
			var blockfrostItems = bfContext.TavernSquad.ToList();
			var size = blockfrostItems.Count;
			var traitCountRarity = rimeContext.TavernSquadTraitCount.ToList();
			var back = rimeContext.TavernSquadBack.ToList();
			var eyes = rimeContext.TavernSquadEyes.ToList();
			var face = rimeContext.TavernSquadFace.ToList();
			var head = rimeContext.TavernSquadHead.ToList();
			var race = rimeContext.TavernSquadRace.ToList();
			var armor = rimeContext.TavernSquadArmor.ToList();
			var mouth = rimeContext.TavernSquadMouth.ToList();
			var racial = rimeContext.TavernSquadRacial.ToList();
			var familiar = rimeContext.TavernSquadFamiliar.ToList();
			var skintone = rimeContext.TavernSquadSkinTone.ToList();
			var background = rimeContext.TavernSquadBackground.ToList();

			var rimeItems = new List<TavernSquadRarity>();

			foreach (var item in blockfrostItems)
			{
				var rarity = new TavernSquadRarity()
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

				rarity.Eyes = MH(eyes.Where(i => i.Eyes == item.Eyes).FirstOrDefault().Count, size);
				rarity.weighting += rarity.Eyes;

				rarity.Face = MH(face.Where(i => i.Face == item.Face).FirstOrDefault().Count, size);
				rarity.weighting += rarity.Face;

				rarity.Head = MH(head.Where(i => i.Head == item.Head).FirstOrDefault().Count, size);
				rarity.weighting += rarity.Head;

				rarity.Race = MH(race.Where(i => i.Race == item.Race).FirstOrDefault().Count, size);
				rarity.weighting += rarity.Race;

				rarity.Armor = MH(armor.Where(i => i.Armor == item.Armor).FirstOrDefault().Count, size);
				rarity.weighting += rarity.Armor;

				rarity.Mouth = MH(mouth.Where(i => i.Mouth == item.Mouth).FirstOrDefault().Count, size);
				rarity.weighting += rarity.Mouth;

				rarity.Racial = MH(racial.Where(i => i.Racial == item.Racial).FirstOrDefault().Count, size);
				rarity.weighting += rarity.Racial;

				rarity.Familiar = MH(familiar.Where(i => i.Familiar == item.Familiar).FirstOrDefault().Count, size);
				rarity.weighting += rarity.Familiar;

				rarity.SkinTone = MH(skintone.Where(i => i.SkinTone == item.SkinTone).FirstOrDefault().Count, size);
				rarity.weighting += rarity.SkinTone;

				rarity.Background = MH(background.Where(i => i.Background == item.Background).FirstOrDefault().Count, size);
				rarity.weighting += rarity.Background;

				rimeItems.Add(rarity);
			}
			rimeContext.Database.ExecuteSqlCommand($"DELETE FROM TavernSquadRarity; ");
			rimeContext.Database.ExecuteSqlCommand($"DBCC CHECKIDENT('TavernSquadRarity', RESEED, 0); ");

			rimeContext.TavernSquadRarity.AddRange(rimeItems);
			trans.Commit();
			rimeContext.SaveChanges();

			_ducky.Info($"Created rarity table for TavernSquad.");
		}
		catch (Exception ex)
		{
			trans.Rollback();
			_ducky.Error("TavernSquadStatsHandler", "GenerateCollectionRarity_SQL", ex.Message);
			throw;
		}
	}

	public override void RarityChart()
	{
		using RimeADO rimeContext = new();
		using BlockfrostADO bfContext = new();
		try
		{
			var rimeItems = rimeContext.TavernSquadRarity.ToList();
			var bfItems = bfContext.TavernSquad.ToList();
			var joined = rimeItems.Join(bfItems,
				rimeItem => rimeItem.asset,
				bfItem => bfItem.asset,
				(rimeItem, bfItem) => new
				{
					Rank = 0,
					Weighting = rimeItem.weighting,
					Name = bfItem.name,
					Back = bfItem.Back,
					BackRarity = rimeItem.Back,
					Eyes = bfItem.Eyes,
					EyesRarity = rimeItem.Eyes,
					Face = bfItem.Face,
					FaceRarity = rimeItem.Face,
					Head = bfItem.Head,
					HeadRarity = rimeItem.Head,
					Race = bfItem.Race,
					RaceRarity = rimeItem.Race,
					Armor = bfItem.Armor,
					ArmorRarity = rimeItem.Armor,
					Mouth = bfItem.Mouth,
					MouthRarity = rimeItem.Mouth,
					Racial = bfItem.Racial,
					RacialRarity = rimeItem.Racial,
					Familiar = bfItem.Familiar,
					FamiliarRarity = rimeItem.Familiar,
					SkinTone = bfItem.SkinTone,
					SkinToneRarity = rimeItem.SkinTone,
					Background = bfItem.Background,
					BackgroundRarity = rimeItem.Background,
					
					TraitCount = bfItem.traitCount,
					TraitCountRarity = rimeItem.traitCount,
					Asset = bfItem.asset
				});

				var ordered = joined.OrderByDescending(joined => joined.Weighting);
				_fileIO.Write_CSV(ordered, _csvDir + "TavernSquad.csv");
			}
			catch (Exception ex)
			{
				_ducky.Error("TavernSquadStatsHandler", "RarityChart", ex.Message);
				throw;
			}
		}
	}
}
