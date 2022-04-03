using MarketWatcher.Discord.Webhooks;
using Rime.ADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketWatcher.SQL.Rime
{
	public class RimeService
	{

		public void RetrieveRarity(string collectionName, string display_name, out WebHookData results)
		{
			results = new WebHookData();
			switch (collectionName)
			{
				case "Pendulum":
					Get_Pendulum(display_name, out results);
					break;
				case "Puurrty Cats Society":
					Get_Puurrty(display_name, out results);
					break;
				default:
					break;
			}

		}

		private void Get_Pendulum(string display_name, out WebHookData results)
		{
			results = new WebHookData();
			using (RimeContext context = new RimeContext())
			{
				var match = context.Pendulums.Where(a => a.Name == display_name).FirstOrDefault();
				if (match != null)
				{
					var rarityMatch = context.PendulumRarities.Where(a => a.Fingerprint == match.Fingerprint).FirstOrDefault();
					if (rarityMatch != null)
					{
						var rank = context.PendulumRarities.Where(a => a.Weighting >= rarityMatch.Weighting).ToList().Count();
						results.Image = match.Image;
						results.RarityRank = rank;
						results.CollectionSize = context.PendulumRarities.Count();
						// Get Floor
					}
				}
			}
		}

		private void Get_Puurrty(string display_name, out WebHookData results)
		{
			results = new WebHookData();
			using (RimeContext context = new RimeContext())
			{
				var match = context.Puurrties.Where(a => a.Name == display_name).FirstOrDefault();
				if (match != null)
				{
					var rarityMatch = context.PuurrtiesRarities.Where(a => a.Fingerprint == match.Fingerprint).FirstOrDefault();
					if (rarityMatch != null)
					{
						var rank = context.PuurrtiesRarities.Where(a => a.Weighting >= rarityMatch.Weighting).ToList().Count();
						results.Image = match.Image;
						results.RarityRank = rank;
						results.CollectionSize = context.PuurrtiesRarities.Count();
						// Get Floor
					}
				}
			}
		}
	}
}
