using System.Text;
using WenRarityLibrary.ADO.Blockfrost.Models;
using WenRarityLibrary.Generic;

namespace WenRarityLibrary.Builders
{
    public class RimeFrameworkBuilder : FrameworkBuilder
    {
        private static RimeFrameworkBuilder instance;
        public static RimeFrameworkBuilder Instance => instance ?? (instance = new RimeFrameworkBuilder());
        private RimeFrameworkBuilder()
        {
            DirectorySafetyChecks();
        }

        private string tokenDir = "";
        private string thisAssetDir = "";

        /// <summary>
        /// Create the directory, if it does not exist.
        /// Remove existing collection information, if it exists.
        /// Add the Rarity Token
        /// Add the Rarity Token attributes classes for the database.
        /// Update the ADO.
        /// </summary>
        /// <param name="statsContainer"></param>
        public void CreateToken(StatsContainer statsContainer)
        {
            tokenDir = libraryDir + $"\\ADO\\Rime\\Models\\Rarity\\Token\\";

            CreateDirectory(statsContainer.collection);

            Remove_Rarity(statsContainer.collection);

            AddRarityToken(statsContainer);
            AddAttributeTokens(statsContainer);
            Update_RimeADO(statsContainer);
        }

        /// <summary>
        /// Update RimeADO in the Library Class
        /// </summary>
        /// <param name="statsContainer"></param>
        private void Update_RimeADO(StatsContainer statsContainer)
        {
            StringBuilder sb = new();

            string fileLoc = libraryDir + "\\ADO\\Rime\\RimeADO.cs";
            string startToken = _marker;

            _fileIO.Read(fileLoc, out string readText);

            int startLoc = readText.IndexOf(startToken);
            string fileStartToMarker = readText.Substring(0, startLoc).TrimStart();
            string filerMarkerToEnd = readText.Substring(startLoc + startToken.Length).TrimEnd();

            // Attributes
            StringBuilder attributeBuilder = new();
            int tabs = 2;

            attributeBuilder.Append($"{OH(1, tabs)}{_marker}{statsContainer.collection.Name}+");// START MARKER
            attributeBuilder.Append($"{OH(1, tabs)}public virtual DbSet<{statsContainer.collection.Name}Rarity> {statsContainer.collection.Name}Rarity " + "{ get; set; }");
            attributeBuilder.Append($"{OH(1, tabs)}public virtual DbSet<{statsContainer.collection.Name}TraitCount> {statsContainer.collection.Name}TraitCount" + "{ get; set; }");

            foreach (var item in statsContainer.traitsIncluded)
            {
                attributeBuilder.Append($"{OH(1, tabs)}public virtual DbSet<{statsContainer.collection.Name}{item.Substring(0, 1).ToUpper() + item.Substring(1) }> {statsContainer.collection.Name}{item.Substring(0, 1).ToUpper() + item.Substring(1) } " +
                "{ get; set; }");
            }

            attributeBuilder.Append($"{OH(1, tabs)}{_marker}{statsContainer.collection.Name}-");// END MARKER

            string newText = attributeBuilder.ToString();

            sb.Append(fileStartToMarker);
            sb.Append(startToken);
            sb.Append(newText);
            sb.Append(filerMarkerToEnd);

            _fileIO.Write(sb.ToString(), fileLoc);
        }

        /// <summary>
        /// Add the Rarity Token to the local asset directory.
        /// </summary>
        /// <param name="statsContainer"></param>
        private void AddRarityToken(StatsContainer statsContainer)
        {
            StringBuilder sb = new();
            int tabs = 1;
            string fileLoc = thisAssetDir + $"{ statsContainer.collection.Name}Rarity.cs";

            // Header
            sb.Append("using System.ComponentModel.DataAnnotations;");
            sb.Append($"{OH(1,tabs)}using System.ComponentModel.DataAnnotations.Schema;");
            sb.Append($"{OH(2, 0)}namespace WenRarityLibrary.ADO.Rime.Models.Rarity.Token");
            sb.Append($"{OH(1, 0)}" + "{");
            sb.Append($"{OH(1, tabs)}[Table(\"{statsContainer.collection.Name}Rarity\")]");
            sb.Append($"{OH(1, tabs)}public partial class {statsContainer.collection.Name}Rarity : OnChainRarity");
            sb.Append($"{OH(1, tabs++)}" + "{");

            sb.Append(EasyID());

            // Attributes
            foreach (var item in statsContainer.traitsIncluded)
            {
                sb.Append($"{OH(1, tabs)}public double {item} " + "{ get; set; }");
            }

            if(statsContainer.includeTraitCount) sb.Append($"{OH(1, tabs)}public double traitCount " + "{ get; set; }");

            sb.Append($"{OH(1, 1)}" + "}");
            sb.Append($"{OH(1, 0)}" + "}");

            _fileIO.Write(sb.ToString(), fileLoc);
        }

        /// <summary>
        /// Add the related Attribute Class and Table containers.
        /// Includes Trait Count.
        /// </summary>
        /// <param name="statsContainer"></param>
        private void AddAttributeTokens(StatsContainer statsContainer)
        {
            StringBuilder sb = new();
            int tabs = 1;
            string attributeName = "";

            // Attributes
            foreach (var item in statsContainer.traitsIncluded)
            {
                tabs = 1;
                sb.Clear();
                attributeName = $"{ statsContainer.collection.Name }{ item.Substring(0,1).ToUpper() + item.Substring(1) }";

                string fileLoc = thisAssetDir + $"{ attributeName }.cs";

                // Header
                sb.Append("using System.ComponentModel.DataAnnotations;");
                sb.Append($"{OH(1, tabs)}using System.ComponentModel.DataAnnotations.Schema;");
                sb.Append($"{OH(2, 0)}namespace WenRarityLibrary.ADO.Rime.Models.Rarity.Token");
                sb.Append($"{OH(1, 0)}" + "{");
                sb.Append($"{OH(1, tabs)}[Table(\"{attributeName}\")]");
                sb.Append($"{OH(1, tabs)}public partial class {attributeName}");
                sb.Append($"{OH(1, tabs++)}" + "{");

                // Attribute - Count
                sb.Append(EasyID());
                sb.Append($"{OH(1, tabs)}public string {item}" + " { get; set; }");
                sb.Append($"{OH(1, tabs)}public int Count" + " { get; set; }");
                sb.Append($"{OH(1, 1)}" + "}");
                sb.Append($"{OH(1, 0)}" + "}");

                _fileIO.Write(sb.ToString(), fileLoc);
            }

            // Trait Count
            if (statsContainer.includeTraitCount)
            {
                tabs = 1;
                sb.Clear();

                attributeName = $"{ statsContainer.collection.Name }TraitCount";

                string fileLoc = thisAssetDir + $"{ attributeName }.cs";

                // Header
                sb.Append("using System.ComponentModel.DataAnnotations;");
                sb.Append($"{OH(1, tabs)}using System.ComponentModel.DataAnnotations.Schema;");
                sb.Append($"{OH(2, 0)}namespace WenRarityLibrary.ADO.Rime.Models.Rarity.Token");
                sb.Append($"{OH(1, 0)}" + "{");
                sb.Append($"{OH(1, tabs)}[Table(\"{attributeName}\")]");
                sb.Append($"{OH(1, tabs)}public partial class {attributeName}");
                sb.Append($"{OH(1, tabs++)}" + "{");

                // Attribute - Count
                sb.Append(EasyID());
                sb.Append($"{OH(1, tabs)}public int traitCount" + " { get; set; }");
                sb.Append($"{OH(1, tabs)}public int Count" + " { get; set; }");
                sb.Append($"{OH(1, 1)}" + "}");
                sb.Append($"{OH(1, 0)}" + "}");

                _fileIO.Write(sb.ToString(), fileLoc);
            }
        }


        /// <summary>
        /// Remove all code blocks in the Class Library with the given collection information.
        /// </summary>
        /// <param name="collection"></param>
        protected void Remove_Rarity(Collection collection)
        {
            _ducky.Info($"Removing file information for {collection.Name} rarity in the Libary Project...");

            List<string> projectFiles = new(Directory.GetFiles(libraryDir + "\\ADO\\Rime", "*.cs", SearchOption.AllDirectories));

            foreach (var projectFile in projectFiles)
            {
                _fileIO.Read(projectFile, out string readText);
                string markerStr = $"{ _marker }{ collection.Name }";

                if (readText.Contains(markerStr))
                {
                    StringBuilder sb = new();
                    try
                    {
                        int indexOfPlus = readText.IndexOf($"{markerStr}+");
                        int indexOfMinus = readText.IndexOf($"{markerStr}-");

                        sb.Append(readText.AsSpan(0, indexOfPlus));

                        sb.Append(readText.AsSpan(indexOfMinus + markerStr.Length + 1));
                        _fileIO.Write(sb.ToString(), projectFile);
                    }
                    catch (Exception ex)
                    {
                        _ducky.Error("RimeFrameworkBuilder", "Remove_Rarity", ex.Message);
                    }
                }
            }
            _ducky.Info($"File audit success for {collection.Name} rarity Libary Project files.");
        }

        /// <summary>
        /// Helper method for creating a directory if it does not exist.
        /// </summary>
        /// <param name="collection"></param>
        private void CreateDirectory(Collection collection)
        {
            thisAssetDir = tokenDir + $"{collection.Name}\\";
            if (!Directory.Exists(thisAssetDir)) Directory.CreateDirectory(thisAssetDir);
        }
    }
}