using System.Text;
using WenRarityLibrary.ADO.Blockfrost.Models;
using WenRarityLibrary.Generic;

namespace WenRarityLibrary.Builders
{
    //sb.Append($"{OH(1, tabs)}");
    public class StatsFrameworkBuilder : FrameworkBuilder
    {
        private static StatsFrameworkBuilder instance;
        public static StatsFrameworkBuilder Instance => instance ?? (instance = new StatsFrameworkBuilder());
        private StatsFrameworkBuilder()
        {
            DirectorySafetyChecks();
        }

        public bool Build(Collection collection, StatsContainer stats)
        {
            Remove_StatsFiles(collection);
            Update_StatsBuilder_PopulateAttributeTables(collection);
            //Update_RimeController(collection);
            StatsHandlerBlueprint(collection, stats);
            return false;
        }

        private void Update_RimeController(Collection collection)
        {
            StringBuilder sb = new();

            string fileLoc = statsDir + "\\Controller\\RimeController.cs";
            string startToken = _marker + "update+";

            _fileIO.Read(fileLoc, out string readText);

            int startLoc = readText.IndexOf(startToken);
            string fileStartToMarker = readText.Substring(0, startLoc);
            string filerMarkerToEnd = readText.Substring(startLoc + startToken.Length);

            StringBuilder newSb = new();
            int tabs = 2;

            newSb.Append($"{OH(1, tabs)}{_marker}{collection.Name}+");// START MARKER

            sb.Append($"{OH(1, tabs)}");
            sb.Append($"{OH(1, tabs)}");
            sb.Append($"{OH(1, tabs)}");
            sb.Append($"{OH(1, tabs)}");
            sb.Append($"{OH(1, tabs)}");
            sb.Append($"{OH(1, tabs)}");
            sb.Append($"{OH(1, tabs)}");
            sb.Append($"{OH(1, tabs)}");
            sb.Append($"{OH(1, tabs)}");
            sb.Append($"{OH(1, tabs)}");
            sb.Append($"{OH(1, tabs)}");
            sb.Append($"{OH(1, tabs)}");

            newSb.Append($"{OH(1, tabs)}{_marker}{collection.Name}-");// END MARKER

            string newText = newSb.ToString();

            sb.Append(fileStartToMarker);
            sb.Append(startToken);
            sb.Append(newText);
            sb.Append(filerMarkerToEnd);

            _fileIO.Write(sb.ToString(), fileLoc);
        }

        private void StatsHandlerBlueprint(Collection collection, StatsContainer stats)
        {
            StringBuilder sb = new();
            int tabs = 0;

            string className = $"{collection.Name}StatsHandler";
            string fileName = $"{className}.cs";
            string fileLoc = statsDir + "\\CollectionHandler\\" + fileName;

            // Header
            sb.Append($"{OH(1, tabs)}using Stats.Controller;");
            sb.Append($"{OH(1, tabs)}using System.ComponentModel.DataAnnotations;");
            sb.Append($"{OH(1, tabs)}using WenRarityLibrary.ADO.Blockfrost;");
            sb.Append($"{OH(1, tabs)}using WenRarityLibrary.ADO.Blockfrost.Models.OnChainMetaData.Token;");
            sb.Append($"{OH(1, tabs)}using WenRarityLibrary.ADO.Rime;");
            sb.Append($"{OH(1, tabs)}using WenRarityLibrary.ADO.Rime.Models.Rarity.Token;");
            sb.Append($"{OH(1, tabs)}");
            sb.Append($"{OH(2, tabs)}namespace Stats.Builders");
            sb.Append($"{OH(1, tabs)}" + "{");
            sb.Append($"{OH(1, ++tabs)}public class {collection.Name}StatsHandler : BaseStatsHandler");
            sb.Append($"{OH(1, tabs)}" + "{");
            sb.Append($"{OH(1, ++tabs)}private static RimeController _rimeController = RimeController.Instance;");

            // Handle
            sb.Append($"{OH(2, tabs)}public override void Handle()");
            sb.Append($"{OH(1, tabs)}" + "{");
            sb.Append($"{OH(1, ++tabs)}using BlockfrostADO bfContext = new();");
            sb.Append($"{OH(1, tabs)}var tokens = bfContext.{collection.Name}.ToList();");
            sb.Append($"{OH(2, tabs)}_ducky.Info($\"Found " + "{tokens.Count()} tokens for " + $"{collection.Name}\");");
            sb.Append($"{OH(2, tabs)}if (tokens != null)");
            sb.Append($"{OH(1, tabs++)}" + "{");

            foreach (var item in stats.traitsIncluded)
            {
                var itemName = $"{collection.Name}{item.Substring(0, 1).ToUpper() + item.Substring(1)}Rarity";

                sb.Append($"{OH(2, tabs)}// {item}");
                sb.Append($"{OH(1, tabs)}var {item.ToLower()} = tokens.GroupBy(t => t.{item});");
                sb.Append($"{OH(1, tabs)}var {item.ToLower()}Items = new List<{itemName}>();");
                sb.Append($"{OH(1, tabs)}foreach (var item in {item.ToLower()}) {item.ToLower()}Items.Add(new {itemName}()");
                sb.Append($"{OH(1, tabs)}" + "{");
                sb.Append($"{OH(1, ++tabs)} {item} = item.Key,");
                sb.Append($"{OH(1, tabs)} Count = item.Count()");
                sb.Append($"{OH(1, --tabs)}" + "});");
            }

            // Trait Count
            if (stats.includeTraitCount)
            {
                var itemName = $"{collection.Name}TraitCountRarity";

                sb.Append($"{OH(2, tabs)}// traitCount");
                sb.Append($"{OH(1, tabs)}var traitCount = tokens.GroupBy(t => t.traitCount);");
                sb.Append($"{OH(1, tabs)}var traitCountItems = new List<{itemName}>();");
                sb.Append($"{OH(1, tabs)}foreach (var item in traitCount) traitCountItems.Add(new {itemName}()");
                sb.Append($"{OH(1, tabs)}" + "{");
                sb.Append($"{OH(1, ++tabs)} traitCount = item.Key,");
                sb.Append($"{OH(1, tabs)} Count = item.Count()");
                sb.Append($"{OH(1, --tabs)}" + "});");
            }

            // Database Updates
            sb.Append($"{OH(2, tabs)}using RimeADO context = new();");
            sb.Append($"{OH(1, tabs)}var trans = context.Database.BeginTransaction();");

            sb.Append($"{OH(1, tabs)}try");
            sb.Append($"{OH(1, tabs++)}" + "{");

            foreach (var item in stats.traitsIncluded)
            {
                var itemName = $"{collection.Name}{item.Substring(0, 1).ToUpper() + item.Substring(1)}Rarity";
                sb.Append($"{OH(1, tabs)}context.Database.ExecuteSqlCommand($\"DELETE FROM[dbo].[{itemName}]\");");
                sb.Append($"{OH(1, tabs)}context.{itemName}.AddRange({item.ToLower()}Items);");
            }

            // Trait Count
            if (stats.includeTraitCount)
            {
                var itemName = $"{collection.Name}TraitCountRarity";
                sb.Append($"{OH(1, tabs)}context.Database.ExecuteSqlCommand($\"DELETE FROM[dbo].[{itemName}]\");");
                sb.Append($"{OH(1, tabs)}context.{itemName}.AddRange(traitCountItems);");
            }


            sb.Append($"{OH(1, tabs)}context.SaveChanges();");
            sb.Append($"{OH(1, tabs)}trans.Commit();");
            sb.Append($"{OH(1, tabs)}_ducky.Info($\"Updated {collection.Name}Rarity.\");");
            sb.Append($"{OH(1, --tabs)}" + "}");

            sb.Append($"{OH(1, tabs)}catch (Exception ex)");
            sb.Append($"{OH(1, tabs)}" + "{");
            sb.Append($"{OH(1, ++tabs)}_ducky.Error(\"RimeController\", \"UpdateKBotRarity_Pet\", ex.Message);");
            sb.Append($"{OH(1, tabs)}throw;");
            sb.Append($"{OH(1, --tabs)}" + "}");


            sb.Append($"{OH(1, --tabs)}" + "}");
            sb.Append($"{OH(1, --tabs)}" + "}");

            // GenerateCollectionRarity_SQL
            sb.Append($"{OH(2, tabs)}public override void GenerateCollectionRarity_SQL()");
            sb.Append($"{OH(1, tabs)}" + "{");
            sb.Append($"{OH(1, ++tabs)}using RimeADO rimeContext = new();");
            sb.Append($"{OH(1, tabs)}using BlockfrostADO bfContext = new();");
            sb.Append($"{OH(1, tabs)}var trans = rimeContext.Database.BeginTransaction();");
            sb.Append($"{OH(1, tabs)}try");
            sb.Append($"{OH(1, tabs)}" + "{");
            sb.Append($"{OH(1, ++tabs)}var blockfrostItems = bfContext.{collection.Name}.ToList();");
            sb.Append($"{OH(1, tabs)}var size = blockfrostItems.Count;");

            // Trait Count
            if (stats.includeTraitCount)
            {
                sb.Append($"{OH(1, tabs)}var traitCountRarity = rimeContext.{collection.Name}TraitCountRarity.ToList();");
            }

            foreach (var item in stats.traitsIncluded)
            {
                sb.Append($"{OH(1, tabs)}var {item.ToLower()}Rarity = rimeContext.{collection.Name}{item.Substring(0, 1).ToUpper() + item.Substring(1)}Rarity.ToList();");
            }

            sb.Append($"{OH(2, tabs)}var rimeItems = new List<{collection.Name}Rarity>();");
            sb.Append($"{OH(2, tabs)}foreach (var item in blockfrostItems)");
            sb.Append($"{OH(1, tabs)}" + "{");
            sb.Append($"{OH(1, ++tabs)}var rarity = new {collection.Name}Rarity()");
            sb.Append($"{OH(1, tabs)}" + "{");
            sb.Append($"{OH(1, ++tabs)}asset = item.asset,");
            sb.Append($"{OH(1, tabs)}name = item.name,");
            sb.Append($"{OH(1, tabs)}fingerprint = item.fingerprint");
            sb.Append($"{OH(1, --tabs)}" + "};");
            sb.Append($"{OH(2, tabs)}rarity.weighting = 0;");

            // Trait Count
            if (stats.includeTraitCount)
            {
                sb.Append($"{OH(1, tabs)}rarity.traitCount = MH(traitCountRarity.Where(i => i.traitCount == item.traitCount).FirstOrDefault().Count, size);");
                sb.Append($"{OH(2, tabs)}rarity.weighting += rarity.traitCount;");
            }

            foreach (var item in stats.traitsIncluded)
            {
                sb.Append($"{OH(1, tabs)}rarity.{item} = MH({item.ToLower()}Rarity.Where(i => i.{item} == item.{item}).FirstOrDefault().Count, size);");
                sb.Append($"{OH(2, tabs)}rarity.weighting += rarity.{item};");
            }

            sb.Append($"{OH(2, tabs)}rimeItems.Add(rarity);");
            sb.Append($"{OH(1, --tabs)}" + "}");

            sb.Append($"{OH(1, tabs)}rimeContext.Database.ExecuteSqlCommand($\"DELETE FROM {collection.Name}Rarity; \");");
            sb.Append($"{OH(1, tabs)}rimeContext.Database.ExecuteSqlCommand($\"DBCC CHECKIDENT('{collection.Name}Rarity', RESEED, 0); \");");
            sb.Append($"{OH(2, tabs)}rimeContext.{collection.Name}Rarity.AddRange(rimeItems);");
            sb.Append($"{OH(1, tabs)}trans.Commit();");
            sb.Append($"{OH(1, tabs)}rimeContext.SaveChanges();");
            sb.Append($"{OH(2, tabs)}_ducky.Info($\"Created rarity table for {collection.Name}.\");") ;
            sb.Append($"{OH(1, --tabs)}" + "}");

            sb.Append($"{OH(1, tabs)}catch (Exception ex)");
            sb.Append($"{OH(1, tabs)}" + "{");
            sb.Append($"{OH(1, ++tabs)}trans.Rollback();");
            sb.Append($"{OH(1, tabs)}_ducky.Error(\"{collection.Name}StatsHandler\", \"GenerateCollectionRarity_SQL\", ex.Message);");
            sb.Append($"{OH(1, tabs)}throw;");
            sb.Append($"{OH(1, --tabs)}" + "}");
            sb.Append($"{OH(1, --tabs)}" + "}");
            sb.Append($"{OH(1, --tabs)}" + "}");
            sb.Append($"{OH(1, --tabs)}" + "}");

            _fileIO.Write(sb.ToString(), fileLoc);
        }

        private void Update_StatsBuilder_PopulateAttributeTables(Collection collection)
        {
            {
                StringBuilder sb = new();

                string fileLoc = statsDir + "\\Builders\\StatsBuilder.cs";
                string startToken = _marker + "populate+";

                _fileIO.Read(fileLoc, out string readText);

                int startLoc = readText.IndexOf(startToken);
                string fileStartToMarker = readText.Substring(0, startLoc);
                string filerMarkerToEnd = readText.Substring(startLoc + startToken.Length);

                StringBuilder newTextsb = new();
                int tabs = 4;
                newTextsb.Append($"{OH(1, tabs)}{_marker}{collection.Name}+");// START MARKER

                newTextsb.Append($"{OH(1, tabs++)}case \"{collection.Name}\":");
                newTextsb.Append($"{OH(1, tabs)}_statsb.statsHandler = new {collection.Name}StatsHandler();");
                newTextsb.Append($"{OH(1, tabs)}_statsb.statsHandler.Handle();");
                newTextsb.Append($"{OH(1, tabs)}_statsb.statsHandler.GenerateCollectionRarity_SQL();");
                newTextsb.Append($"{OH(1, tabs--)}break;");

                newTextsb.Append($"{OH(1, tabs)}{_marker}{collection.Name}-");// END MARKER

                string newText = newTextsb.ToString();

                sb.Append(fileStartToMarker);
                sb.Append(startToken);
                sb.Append(newText);
                sb.Append(filerMarkerToEnd);

                _fileIO.Write(sb.ToString(), fileLoc);
            }
        }

        public bool CollectionExists(string path)
        {
            string fullPath = libraryDir + $"\\ADO\\Rime\\Models\\Rarity\\Token\\{path}";
            if (Directory.Exists(fullPath)) return true;
            return false;
        }

    }
}