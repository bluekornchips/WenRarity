using System.Text;
using BlockfrostLibrary.ADO.Models;
using BlockfrostLibrary.ADO.Models.Collection;
using BlockfrostLibrary.ViewModels;
using WenRarityLibrary.Builders;

namespace BlockfrostLibrary.Builders
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
        /// Create the Token if it does not exist already.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="vm"></param>
        /// <param name="overwrite"></param>
        /// <param name="isNewCollection"></param>
        public void CreateToken(BlockfrostCollection collection, OnChainMetaDataViewModel vm, bool overwrite, out bool isNewCollection)
        {
            // If the token already exists and overwrite is false, we don't want to change it.
            string fileLoc = blockfrostLibraryDir + $"\\ADO\\Models\\OnChainMetaData\\Token\\{collection.Name}.cs";
            isNewCollection = false;
            if (File.Exists(fileLoc) && !overwrite) return;
            AddToken(collection, vm, fileLoc);
            isNewCollection = true;
        }

        /// <summary>
        /// Complete framework build, updates files.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="vm"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool Build(BlockfrostCollection collection, OnChainMetaDataViewModel vm)
        {
            // Remove
            RemoveCollectionFiles(collection);


            // Update
            Update_BlockfrostADO(collection);
            Update_ViewModelSwitch(collection);
            UpdateJsonBuilder_Handle(collection, vm);
            UpdateJsonBuilder_Switch(collection);
            Update_OnChainMetaDataModelHandler(collection);
            Update_BlockfrostController(collection);

            return true;
        }

        /// <summary>
        /// Safety measure for checking if the token exists before completing framework build.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        private bool CheckTokenExists(BlockfrostCollection collection)
        {
            string fileLoc = blockfrostLibraryDir + $"\\ADO\\Models\\OnChainMetaData\\Token\\{collection.Name}.cs";

            if (File.Exists(fileLoc)) return true;
            return false;
        }

        /// <summary>
        /// Adds the new token to the Class Library.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="vm"></param>
        private void AddToken(BlockfrostCollection collection, OnChainMetaDataViewModel vm, string fileLoc)
        {
            StringBuilder sb = new();
            int tabs = 1;

            // Header
            sb.Append("using System.ComponentModel.DataAnnotations.Schema;");
            sb.Append($"{OH(2, 0)}namespace BlockfrostLibrary.ADO.Models.OnChainMetaData.Token");
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

        /// <summary>
        /// Update the BlockfrostController file with the new Collection items
        /// </summary>
        /// <param name="collection"></param>
        private void Update_BlockfrostController(BlockfrostCollection collection)
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

            sbHandler.Append($"{OH(1, tabs)}{_marker}{collection.Name}+"); // START MARKER
            sbHandler.Append($"{OH(1, tabs++)}case \"{collection.Name}\" :");
            sbHandler.Append($"{OH(1, tabs)}var found{collection.Name} = context.{collection.Name}.ToList();");
            sbHandler.Append($"{OH(1, tabs)}foreach (var item in found{collection.Name}) items.Add(item.asset, item);");
            sbHandler.Append($"{OH(1, tabs--)}break;");
            sbHandler.Append($"{OH(1, tabs)}{_marker}{collection.Name}-"); // END MARKER

            string newText = sbHandler.ToString();

            sb.Append(fileStartToMarker);
            sb.Append(startToken);
            sb.Append(newText);
            sb.Append(filerMarkerToEnd);

            _fileIO.Write(sb.ToString(), fileLoc);
        }

        /// <summary>
        /// Update the OnChainMetaDataModelHandler director.
        /// </summary>
        /// <param name="collection"></param>
        private void Update_OnChainMetaDataModelHandler(BlockfrostCollection collection)
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

            sbHandler.Append($"{OH(1, tabs)}{_marker}{collection.Name}+"); // START MARKER
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
            sbHandler.Append($"{OH(1, tabs)}{_marker}{collection.Name}-"); // END MARKER

            string newText = sbHandler.ToString();

            sb.Append(fileStartToMarker);
            sb.Append(startToken);
            sb.Append(newText);
            sb.Append(filerMarkerToEnd);

            _fileIO.Write(sb.ToString(), fileLoc);
        }

        /// <summary>
        /// Update the BlockfrostADO file
        /// NOT in the Blockfrost Project - in the Class Library
        /// </summary>
        /// <param name="collection"></param>
        private void Update_BlockfrostADO(BlockfrostCollection collection)
        {
            StringBuilder sb = new();

            string fileLoc = blockfrostLibraryDir + "\\ADO\\BlockfrostADO.cs";
            string startToken = _marker + "tokens+";

            _fileIO.Read(fileLoc, out string readText);
            
            int startLoc = readText.IndexOf(startToken);
            string fileStartToMarker = readText.Substring(0, startLoc);
            string filerMarkerToEnd = readText.Substring(startLoc + startToken.Length);

            string newText = $"{OH(1, 2)}{_marker}{collection.Name}+"; // START MARKER
            newText += $"{OH(1,2)}public virtual DbSet<{collection.Name}> {collection.Name} " +
                "{ get; set; }";
            newText +=$"{OH(1, 2)}{_marker}{collection.Name}-"; // END MARKER

            sb.Append(fileStartToMarker);
            sb.Append(startToken);
            sb.Append(newText);
            sb.Append(filerMarkerToEnd);

            _fileIO.Write(sb.ToString(), fileLoc);
        }

        /// <summary>
        /// Update the ViewModel Switch statement
        /// </summary>
        /// <param name="collection"></param>
        private void Update_ViewModelSwitch(BlockfrostCollection collection)
        {
            StringBuilder sb = new();

            string fileLoc = blockfrostLibraryDir + "\\ViewModels\\OnChainMetaDataViewModel.cs";
            string startToken = _marker;

            _fileIO.Read(fileLoc, out string readText);

            int startLoc = readText.IndexOf(startToken);
            string fileStartToMarker = readText.Substring(0, startLoc);
            string filerMarkerToEnd = readText.Substring(startLoc + startToken.Length);

            string newText = $"{OH(1, 4)}{_marker}{collection.Name}+"; // START MARKER
            newText += $"{OH(1, 4)}case \"{collection.Name}\": return ({collection.Name})model;";
            newText += $"{OH(1, 4)}{_marker}{collection.Name}-"; // END MARKER

            sb.Append(fileStartToMarker);
            sb.Append(startToken);
            sb.Append(newText);
            sb.Append(filerMarkerToEnd);

            _fileIO.Write(sb.ToString(), fileLoc);
        }


        /// <summary>
        /// Update the JsonBuilder Switch statement.
        /// </summary>
        /// <param name="collection"></param>
        private void UpdateJsonBuilder_Switch(BlockfrostCollection collection)
        {
            StringBuilder sb = new();

            string fileLoc = blockFrostDir + $"\\Builder\\JsonBuilder.cs";
            string startToken = _marker + "switch+";

            _fileIO.Read(fileLoc, out string readText);

            int startLoc = readText.IndexOf(startToken);
            string fileStartToMarker = readText.Substring(0, startLoc);
            string filerMarkerToEnd = readText.Substring(startLoc + startToken.Length);

            string newText = $"{OH(1, 4)}{_marker}{collection.Name}+"; // START MARKER
            newText += $"{OH(1,4)}case \"{collection.Name}\":return Handle{collection.Name}(json);";
            newText += $"{OH(1, 4)}{_marker}{collection.Name}-"; // END MARKER

            sb.Append(fileStartToMarker);
            sb.Append(startToken);
            sb.Append(newText);
            sb.Append(filerMarkerToEnd);

            _fileIO.Write(sb.ToString().TrimEnd(), fileLoc);
        }

        /// <summary>
        /// Update the JsonBuilder file Handle area.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="vm"></param>
        private void UpdateJsonBuilder_Handle(BlockfrostCollection collection, OnChainMetaDataViewModel vm)
        {
            StringBuilder sb = new();
            int tabs = 2;

            string fileLoc = blockFrostDir + $"\\Builder\\JsonBuilder.cs";
            string startToken = _marker + "handle+";

            _fileIO.Read(fileLoc, out string readText);

            int startLoc = readText.IndexOf(startToken);
            string fileStartToMarker = readText.Substring(0, startLoc);
            string filerMarkerToEnd = readText.Substring(startLoc + startToken.Length);

            // Attributes
            StringBuilder sbHandler = new();

            sbHandler.Append($"{OH(1, tabs)}{_marker}{collection.Name}+"); // START MARKER
            sbHandler.Append($"{OH(1, tabs)}private {collection.Name} Handle{collection.Name}(string json)");
            sbHandler.Append($"{OH(1, tabs++)}" + "{");
            sbHandler.Append($"{OH(1, tabs)}{collection.Name} model = JsonConvert.DeserializeObject<{collection.Name}>(json);");

            foreach (var item in vm.model.attributes)
                sbHandler.Append($"{OH(1, tabs)}model.{item.Key} = model.attributes.GetValueOrDefault(\"{item.Key}\");");

            sbHandler.Append($"{OH(1, tabs--)}return model;");
            sbHandler.Append($"{OH(1, tabs)}" + "}");
            sbHandler.Append($"{OH(1, 0)}");
            sbHandler.Append($"{OH(1, tabs)}{_marker}{collection.Name}-"); // END MARKER

            string newText = sbHandler.ToString();

            sb.Append(fileStartToMarker);
            sb.Append(startToken);
            sb.Append(newText);
            sb.Append(filerMarkerToEnd);

            _fileIO.Write(sb.ToString(), fileLoc);
        }


        /// <summary>
        /// Remove all code blocks in the Blockfrost Collection for the given Collection
        /// </summary>
        /// <param name="collection"></param>
        protected void RemoveCollectionFiles(BlockfrostCollection collection)
        {
            _ducky.Info($"Removing file information for {collection.Name} in the Blockfrost Project...");
            List<string> projectFiles = new(Directory.GetFiles(blockFrostDir, "*.cs", SearchOption.AllDirectories));

            projectFiles.Add(@"C:\blackbox\Projects\WenRarity\ClassLibrary\ADO\Blockfrost\BlockfrostADO.cs");
            projectFiles.Add(@"C:\blackbox\Projects\WenRarity\ClassLibrary\ViewModels\OnChainMetaDataViewModel.cs");

            foreach (var projectFile in projectFiles)
            {
                _fileIO.Read(projectFile, out string readText);
                string markerStr = $"{ _marker }{ collection.Name }";

                if (readText.Contains(markerStr))
                {
                    try
                    {
                        int indexOfPlus = readText.IndexOf($"{markerStr}+");
                        int indexOfMinus = readText.IndexOf($"{markerStr}-");
                        string spliced = Splice(readText.AsSpan(0, indexOfPlus).ToString(), readText.AsSpan(indexOfMinus + markerStr.Length + 1).ToString());
                        
                        do
                        {
                            indexOfPlus = spliced.IndexOf($"{markerStr}+");
                            if(indexOfPlus != -1)
                            {
                                indexOfMinus = spliced.IndexOf($"{markerStr}-");
                                spliced = Splice(spliced.AsSpan(0, indexOfPlus).ToString(), spliced.AsSpan(indexOfMinus + markerStr.Length + 1).ToString());
                            }
                        } while (indexOfPlus != -1);
                        
                        _fileIO.Write(spliced, projectFile);
                    }
                    catch (Exception ex)
                    {
                        _ducky.Error("FrameworkBuilder", "Remove_BlockfrostFiles", ex.Message);
                        throw;
                    }
                }
            }
            _ducky.Info($"File audit success for {collection.Name} Blockfrost files.");
        }
    }
}