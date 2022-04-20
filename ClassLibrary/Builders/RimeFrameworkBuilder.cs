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

        public void CreateToken(StatsContainer statsContainer)
        {
            tokenDir = libraryDir + $"\\ADO\\Rime\\Models\\Rarity\\Token\\";

            CreateDirectory(statsContainer.collection);
            AddRarityToken(statsContainer);
            AddAttributeTokens(statsContainer);
            Update_RimeADO(statsContainer);
        }

        private void Update_RimeADO(StatsContainer statsContainer)
        {
            StringBuilder sb = new();

            string fileLoc = libraryDir + "\\ADO\\Rime\\RimeADO.cs";
            string startToken = _marker;

            _fileIO.Read(fileLoc, out string readText);

            int startLoc = readText.IndexOf(startToken);
            string fileStartToMarker = readText.Substring(0, startLoc);
            string filerMarkerToEnd = readText.Substring(startLoc + startToken.Length);

            // Attributes
            StringBuilder attributeBuilder = new();
            int tabs = 2;

            attributeBuilder.Append($"{OH(1, tabs)}public virtual DbSet<{statsContainer.collection.Name}Rarity> {statsContainer.collection.Name}Rarity " + "{ get; set; }");
            
            foreach (var item in statsContainer.traitsIncluded)
            {
                attributeBuilder.Append($"{OH(1, tabs)}public virtual DbSet<{statsContainer.collection.Name}{item}Rarity> {statsContainer.collection.Name}{item}Rarity " +
                "{ get; set; }");
            }

            string newText = attributeBuilder.ToString();

            sb.Append(fileStartToMarker);
            sb.Append(startToken);
            sb.Append(newText);
            sb.Append(filerMarkerToEnd);

            _fileIO.Write(sb.ToString(), fileLoc);
        }

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
                attributeName = $"{ statsContainer.collection.Name }{ item }";

                string fileLoc = thisAssetDir + $"{ attributeName }Rarity.cs";

                // Header
                sb.Append("using System.ComponentModel.DataAnnotations;");
                sb.Append($"{OH(1, tabs)}using System.ComponentModel.DataAnnotations.Schema;");
                sb.Append($"{OH(2, 0)}namespace WenRarityLibrary.ADO.Rime.Models.Rarity.Token");
                sb.Append($"{OH(1, 0)}" + "{");
                sb.Append($"{OH(1, tabs)}[Table(\"{attributeName}Rarity\")]");
                sb.Append($"{OH(1, tabs)}public partial class {attributeName}Rarity");
                sb.Append($"{OH(1, tabs++)}" + "{");

                // Attribute - Count
                sb.Append(EasyID());
                sb.Append($"{OH(1, tabs)}public string {item}" + " { get; set; }");
                sb.Append($"{OH(1, tabs)}public int Count" + " { get; set; }");
                sb.Append($"{OH(1, 1)}" + "}");
                sb.Append($"{OH(1, 0)}" + "}");

                _fileIO.Write(sb.ToString(), fileLoc);
            }
        }
        private string EasyID()
            => $"{OH(1, 2)}[Key]{OH(1, 2)}[DatabaseGenerated(DatabaseGeneratedOption.Identity)]{OH(1, 2)} public int id" + "{ get; set; }";

        private void CreateDirectory(Collection collection)
        {
            thisAssetDir = tokenDir + $"{collection.Name}\\";
            if (!Directory.Exists(thisAssetDir)) Directory.CreateDirectory(thisAssetDir);
        }
    }
}