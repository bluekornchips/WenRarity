using System.Text;
using WenRarityLibrary.ADO.Blockfrost.Models;
using WenRarityLibrary.ViewModels;

namespace WenRarityLibrary.Builders
{
    public class BlockfrostFrameworkBuilder : FrameworkBuilder
    {
        private static BlockfrostFrameworkBuilder instance;
        public static BlockfrostFrameworkBuilder Instance => instance ?? (instance = new BlockfrostFrameworkBuilder());
        private BlockfrostFrameworkBuilder()
        {
            DirectorySafetyChecks();
        }

        /// <summary>
        /// First step, creating the Token.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="vm"></param>
        public void CreateToken(Collection collection, OnChainMetaDataViewModel vm)
        {
            AddToken(collection, vm);
        }

        /// <summary>
        /// Complete framework build, updates files.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="vm"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool Build(Collection collection, OnChainMetaDataViewModel vm, out string status)
        {
            status = "";

            if (!CheckTokenExists(collection))
            {
                status = $"Token {collection.Name}.cs does not exist.";
                _ducky.Error("WenRarityFrameworkBuilder", "Build", status);
                return false;
            }

            // Greasy stop to prevent doubling up the existing code when I move between computers
            // TODO: Make this programmatic to check whether the files contain the info already or not.
            //if (!update) return true;

            Update_BlockfrostADO(collection);
            Update_ViewModelSwitch(collection);
            UpdateJsonBuilder_Handle(collection, vm);
            UpdateJsonBuilder_Switch(collection, vm);
            Update_OnChainMetaDataModelHandler(collection);
            Update_BlockfrostController(collection);

            return true;
        }

        /// <summary>
        /// Safety measure for checking if the token exists before completing framework build.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        private bool CheckTokenExists(Collection collection)
        {
            string fileLoc = libraryDir + $"\\ADO\\Blockfrost\\Models\\OnChainMetaData\\Token\\{collection.Name}.cs";

            if (File.Exists(fileLoc)) return true;
            return false;
        }

        /// <summary>
        /// Adds the new token to the Class Library.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="vm"></param>
        private void AddToken(Collection collection, OnChainMetaDataViewModel vm)
        {
            StringBuilder sb = new();
            int tabs = 1;
            string fileLoc = libraryDir + $"\\ADO\\Blockfrost\\Models\\OnChainMetaData\\Token\\{collection.Name}.cs";

            // Header
            sb.Append("using System.ComponentModel.DataAnnotations.Schema;");
            sb.Append($"{OH(2, 0)}namespace WenRarityLibrary.ADO.Blockfrost.Models.OnChainMetaData.Token");
            sb.Append($"{OH(1, 0)}" + "{");
            sb.Append($"{OH(1, tabs)}[Table(\"{collection.Name}\")]");
            sb.Append($"{OH(1, tabs)}public partial class {collection.Name} : OnChainMetaData");
            sb.Append($"{OH(1, tabs++)}" + "{");

            // CIP attributes
            foreach (var item in vm.model.attributes)
            {
                sb.Append($"{OH(1, tabs)}public string {item.Key} " + "{ get; set; }");
            }

            // Attributes
            foreach (var item in vm.attributes)
            {
                if (item.Key != "attributes")
                    sb.Append($"{OH(1, tabs)}public string {item.Key} " + "{ get; set; }");
            }

            sb.Append($"{OH(1, 1)}" + "}");
            sb.Append($"{OH(1, 0)}" + "}");

            _fileIO.Write(sb.ToString(), fileLoc);
        }


        private void Update_BlockfrostController(Collection collection)
        {
            StringBuilder sb = new();

            string fileLoc = blockFrostDir + "\\Controller\\BlockfrostController.cs";
            string startToken = _marker;

            _fileIO.Read(fileLoc, out string readText);

            int startLoc = readText.IndexOf(startToken);
            string fileStartToMarker = readText.Substring(0, startLoc);
            string filerMarkerToEnd = readText.Substring(startLoc + startToken.Length);

            // GetOnChainMetaData
            StringBuilder sbHandler = new();
            int tabs = 5;
            sbHandler.Append($"{OH(1, tabs)}{_marker}{collection.Name}+");// START MARKER

            sbHandler.Append($"{OH(1, tabs++)}case \"{collection.Name}\" :");
            sbHandler.Append($"{OH(1, tabs)}var found{collection.Name} = context.{collection.Name}.ToList();");
            sbHandler.Append($"{OH(1, tabs)}foreach (var item in found{collection.Name}) items.Add(item.asset, item);");
            sbHandler.Append($"{OH(1, tabs--)}break;");

            sbHandler.Append($"{OH(1, tabs)}{_marker}{collection.Name}-");// END MARKER

            string newText = sbHandler.ToString();

            sb.Append(fileStartToMarker);
            sb.Append(startToken);
            sb.Append(newText);
            sb.Append(filerMarkerToEnd);

            _fileIO.Write(sb.ToString(), fileLoc);
        }


        private void Update_OnChainMetaDataModelHandler(Collection collection)
        {
            StringBuilder sb = new();

            string fileLoc = blockFrostDir + "\\Builder\\OnChainMetaDataModelHandler.cs";
            string startToken = _marker;

            _fileIO.Read(fileLoc, out string readText);

            int startLoc = readText.IndexOf(startToken);
            string fileStartToMarker = readText.Substring(0, startLoc);
            string filerMarkerToEnd = readText.Substring(startLoc + startToken.Length);

            // Add
            StringBuilder sbHandler = new();
            int tabs = 2;

            sbHandler.Append($"{OH(1, tabs)}{_marker}{collection.Name}+");// START MARKER

            sbHandler.Append($"{OH(1, tabs)}public void Add({collection.Name} item)");
            sbHandler.Append($"{OH(1, tabs++)}" + "{");
            sbHandler.Append($"{OH(1, tabs)}using BlockfrostADO context = new();");
            sbHandler.Append($"{OH(1, tabs)}var trans = context.Database.BeginTransaction();");
            sbHandler.Append($"{OH(1, tabs)}try");
            sbHandler.Append($"{OH(1, tabs++)}" + "{");
            sbHandler.Append($"{OH(1, tabs)}context.{collection.Name}.Add(item);");
            sbHandler.Append($"{OH(1, tabs)}trans.Commit();");
            sbHandler.Append($"{OH(1, tabs--)}context.SaveChanges();");
            sbHandler.Append($"{OH(1, tabs)}" + "}");
            sbHandler.Append($"{OH(1, tabs)}catch (Exception ex)");
            sbHandler.Append($"{OH(1, tabs++)}" + "{");
            sbHandler.Append($"{OH(1, tabs)}trans.Rollback();");
            sbHandler.Append($"{OH(1, tabs--)}_ducky.Error(\"OnChainMetaDataModelHandler\", \"Add({collection.Name})\", ex.Message);");
            sbHandler.Append($"{OH(1, tabs--)}" + "}");

            sbHandler.Append($"{OH(1, tabs)}" + "}");

            sbHandler.Append($"{OH(1, tabs)}{_marker}{collection.Name}-");// END MARKER

            string newText = sbHandler.ToString();

            sb.Append(fileStartToMarker);
            sb.Append(startToken);
            sb.Append(newText);
            sb.Append(filerMarkerToEnd);

            _fileIO.Write(sb.ToString(), fileLoc);
        }

        private void Update_BlockfrostADO(Collection collection)
        {
            StringBuilder sb = new();

            string fileLoc = libraryDir + "\\ADO\\Blockfrost\\BlockfrostADO.cs";
            string startToken = _marker + "tokens+";

            _fileIO.Read(fileLoc, out string readText);
            
            int startLoc = readText.IndexOf(startToken);
            string fileStartToMarker = readText.Substring(0, startLoc);
            string filerMarkerToEnd = readText.Substring(startLoc + startToken.Length);

            string newText = $"{OH(1, 2)}{_marker}{collection.Name}+";// START MARKER

            newText += $"{OH(1,2)}public virtual DbSet<{collection.Name}> {collection.Name} " +
                "{ get; set; }";

            newText +=$"{OH(1, 2)}{_marker}{collection.Name}-";// END MARKER

            sb.Append(fileStartToMarker);
            sb.Append(startToken);
            sb.Append(newText);
            sb.Append(filerMarkerToEnd);

            _fileIO.Write(sb.ToString(), fileLoc);
        }

        private void Update_ViewModelSwitch(Collection collection)
        {
            StringBuilder sb = new();

            string fileLoc = libraryDir + "\\ViewModels\\OnChainMetaDataViewModel.cs";
            string startToken = _marker;

            _fileIO.Read(fileLoc, out string readText);

            int startLoc = readText.IndexOf(startToken);
            string fileStartToMarker = readText.Substring(0, startLoc);
            string filerMarkerToEnd = readText.Substring(startLoc + startToken.Length);

            string newText = $"{OH(1, 4)}{_marker}{collection.Name}+";// START MARKER

            newText += $"{OH(1, 4)}case \"{collection.Name}\": return ({collection.Name})model;";

            newText += $"{OH(1, 4)}{_marker}{collection.Name}-";// END MARKER

            sb.Append(fileStartToMarker);
            sb.Append(startToken);
            sb.Append(newText);
            sb.Append(filerMarkerToEnd);

            _fileIO.Write(sb.ToString(), fileLoc);
        }


        private void UpdateJsonBuilder_Switch(Collection collection, OnChainMetaDataViewModel vm)
        {
            StringBuilder sb = new();
            int tabs = 2;

            string fileLoc = libraryDir + $"\\Builders\\JsonBuilder.cs";
            string startToken = _marker + "switch+";

            _fileIO.Read(fileLoc, out string readText);

            int startLoc = readText.IndexOf(startToken);
            string fileStartToMarker = readText.Substring(0, startLoc);
            string filerMarkerToEnd = readText.Substring(startLoc + startToken.Length);

            string newText = $"{OH(1, 4)}{_marker}{collection.Name}+";// START MARKER

            newText += $"{OH(1,4)}case \"{collection.Name}\":return Handle{collection.Name}(json);";

            newText += $"{OH(1, 4)}{_marker}{collection.Name}-";// END MARKER

            sb.Append(fileStartToMarker);
            sb.Append(startToken);
            sb.Append(newText);
            sb.Append(filerMarkerToEnd);

            _fileIO.Write(sb.ToString(), fileLoc);
        }


        private void UpdateJsonBuilder_Handle(Collection collection, OnChainMetaDataViewModel vm)
        {
            StringBuilder sb = new();
            int tabs = 2;

            string fileLoc = libraryDir + $"\\Builders\\JsonBuilder.cs";
            string startToken = _marker + "handle+";

            _fileIO.Read(fileLoc, out string readText);

            int startLoc = readText.IndexOf(startToken);
            string fileStartToMarker = readText.Substring(0, startLoc);
            string filerMarkerToEnd = readText.Substring(startLoc + startToken.Length);


            // Attributes
            StringBuilder sbHandler = new();

            sbHandler.Append($"{OH(1, tabs)}{_marker}{collection.Name}+");// START MARKER

            sbHandler.Append($"{OH(1, tabs)}private {collection.Name} Handle{collection.Name}(string json)");
            sbHandler.Append($"{OH(1, tabs++)}" + "{");
            sbHandler.Append($"{OH(1, tabs)}{collection.Name} model = JsonConvert.DeserializeObject<{collection.Name}>(json);");

            foreach (var item in vm.model.attributes)
                sbHandler.Append($"{OH(1, tabs)}model.{item.Key} = model.attributes.GetValueOrDefault(\"{item.Key}\");");

            sbHandler.Append($"{OH(1, tabs--)}return model;");
            sbHandler.Append($"{OH(1, tabs)}" + "}");
            sbHandler.Append($"{OH(1, 0)}");

            sbHandler.Append($"{OH(1, tabs)}{_marker}{collection.Name}-");// END MARKER

            string newText = sbHandler.ToString();

            sb.Append(fileStartToMarker);
            sb.Append(startToken);
            sb.Append(newText);
            sb.Append(filerMarkerToEnd);

            _fileIO.Write(sb.ToString(), fileLoc);
        }
    }
}