using System.Text;
using WenRarityLibrary.ADO.Blockfrost.Models;

namespace WenRarityLibrary.Builders
{
    public class FrameworkBuilder
    {
        protected static Ducky _ducky = Ducky.Instance;
        protected static WenRarityFileIO _fileIO = WenRarityFileIO.Instance;

        protected static readonly string _marker = @"//##_:";
        protected static string blockFrostDir = "";
        protected static string libraryDir = "";
        protected static string statsDir = "";

        /// <summary>
        /// Remove all related collection information in the .cs files.
        /// </summary>
        /// <param name="collection"></param>
        public void RemoveAllCollectionInfoFromFiles(Collection collection)
        {
            Remove_LibrayFiles(collection);
            Remove_BlockfrostFiles(collection);
            Remove_StatsFiles(collection);
        }

        protected void Remove_LibrayFiles(Collection collection)
        {
            _ducky.Info($"Removing file information for {collection.Name} in the Class Library Project...");
            List<string> projectFiles = new(Directory.GetFiles(libraryDir, "*.cs", SearchOption.AllDirectories));

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
                        _ducky.Error("FrameworkBuilder", "Remove_LibrayFiles", ex.Message);
                    }
                }
            }
            _ducky.Info($"File audit success for {collection.Name}.");
        }

        protected void Remove_BlockfrostFiles(Collection collection)
        {
            _ducky.Info($"Removing file information for {collection.Name} in the Blockfrost Project...");
            List<string> projectFiles = new(Directory.GetFiles(blockFrostDir, "*.cs", SearchOption.AllDirectories));

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
                        _ducky.Error("FrameworkBuilder", "Remove_BlockfrostFiles", ex.Message);
                    }
                }
            }
            _ducky.Info($"File audit success for {collection.Name} Blockfrost files.");
        }

        protected void Remove_StatsFiles(Collection collection)
        {
            _ducky.Info($"Removing file information for {collection.Name} in the Stats Project...");
            List<string> projectFiles = new(Directory.GetFiles(statsDir, "*.cs", SearchOption.AllDirectories));

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
                        _ducky.Error("FrameworkBuilder", "Remove_StatsFiles", ex.Message);
                    }
                }
            }
            _ducky.Info($"File audit success for {collection.Name} Stats Project files.");
        }


        /// <summary>
        /// Output Handler for proper newlines and tabs.
        /// </summary>
        /// <param name="newLine"></param>
        /// <param name="tabs"></param>
        /// <returns></returns>
        protected string OH(int newLine, int tabs)
        {
            string output = "";
            for (int i = 0; i < newLine; i++) output += "\r\n";
            for (int i = 0; i < tabs; i++) output += "\t";
            return output;
        }

        protected string EasyID()
            => $"{OH(1, 2)}[Key]{OH(1, 2)}[DatabaseGenerated(DatabaseGeneratedOption.Identity)]{OH(1, 2)} public int id" + "{ get; set; }";

        protected void DirectorySafetyChecks()
        {
            string cwd = "";
            string[] splitCwd = Environment.CurrentDirectory.Split('\\');
            bool complete = false;

            foreach (string item in splitCwd)
            {
                if (!complete) cwd += $"\\{item}";
                if (item.Equals("WenRarity")) complete = true;
            }

            cwd = cwd.Substring(1, cwd.Length - 1);
            cwd += "\\";

            blockFrostDir = cwd + "Blockfrost";
            libraryDir = cwd + "ClassLibrary";
            statsDir = cwd + "Stats";


            //if (!Directory.Exists(DirVM))
            //{
            //    try
            //    {
            //        Directory.CreateDirectory(DirVM);
            //        _ducky.Info($"Created directory: {DirVM}.");
            //    }
            //    catch (Exception ex)
            //    {
            //        _ducky.Error("RimeWriter", "DirectorySafetyChecks", ex.Message);
            //        throw;
            //    }
            //}
        }

    }
}