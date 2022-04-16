using MarketWatcher.Classes;
using MarketWatcher.Discord.Webhooks;
using Rime.ADO;
using Rime.ADO.Classes;
using Rime.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketWatcher.SQL.Rime
{
	public class RimeService
	{
		private static RimeService instance = new RimeService();

		private RimeService() { }

		public static RimeService Instance { get { return instance; } }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="collectionName">With underscores.</param>
		/// <param name="displayName"></param>
		/// <param name="collectionItemData"></param>
		//public void Get_Data(string displayName, out CollectionItemData collectionItemData)
  //      {
		//	collectionItemData = new CollectionItemData();
		//	using RimeContext rimeContext = new RimeContext();
		//	rimeContext.Pendulums.Where(t => t.Name == displayName).FirstOrDefault();

  //          //RimeDBContextController.GetToken(collectionName, displayName, out Asset asset);
  //      }

		//public void RetrieveRarity(string collectionName, string display_name, out CollectionData results)
		//{
		//	results = new CollectionData();
		//	switch (collectionName)
		//	{
		//		case "Pendulum":
		//			Get_Pendulum(display_name, out results);
		//			break;
		//		case "Puurrty Cats Society":
		//			Get_Puurrty(display_name, out results);
		//			break;
		//		default:
		//			break;
		//	}

		//}

		//private void Get_Pendulum(string display_name, out CollectionData results)
		//{
		//	results = new CollectionData();
		//	using (RimeContext context = new RimeContext())
		//	{
		//		var match = context.Pendulums.Where(a => a.Name == display_name).FirstOrDefault();
		//		if (match != null)
		//		{
		//			var rarityMatch = context.PendulumRarities.Where(a => a.Fingerprint == match.Fingerprint).FirstOrDefault();
		//			if (rarityMatch != null)
		//			{
		//				var rank = context.PendulumRarities.Where(a => a.Weighting >= rarityMatch.Weighting).ToList().Count();
		//				results.Image = match.Image;
		//				results.RarityRank = rank;
		//				results.CollectionSize = context.PendulumRarities.Count();
		//				// Get Floor
		//			}
		//		}
		//	}
		//}

		//private void Get_Puurrty(string display_name, out CollectionData results)
		//{
		//	results = new CollectionData();
		//	using (RimeContext context = new RimeContext())
		//	{
		//		var match = context.Puurrties.Where(a => a.Name == display_name).FirstOrDefault();
		//		if (match != null)
		//		{
		//			var rarityMatch = context.PuurrtiesRarities.Where(a => a.Fingerprint == match.Fingerprint).FirstOrDefault();
		//			if (rarityMatch != null)
		//			{
		//				var rank = context.PuurrtiesRarities.Where(a => a.Weighting >= rarityMatch.Weighting).ToList().Count();
		//				results.Image = match.Image;
		//				results.RarityRank = rank;
		//				results.CollectionSize = context.PuurrtiesRarities.Count();
		//				// Get Floor
		//			}
		//		}
		//	}
		//}
	}
}
