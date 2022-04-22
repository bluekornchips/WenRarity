using System.Text;

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

        //protected void Remove_LibrayFiles(Collection collection)
        //{
        //    _ducky.Info($"Removing file information for {collection.Name} in the Class Library Project...");
        //    List<string> projectFiles = new(Directory.GetFiles(libraryDir, "*.cs", SearchOption.AllDirectories));

        //    foreach (var projectFile in projectFiles)
        //    {
        //        _fileIO.Read(projectFile, out string readText);
        //        string markerStr = $"{ _marker }{ collection.Name }";

        //        if (readText.Contains(markerStr))
        //        {
        //            try
        //            {
        //                int indexOfPlus = readText.IndexOf($"{markerStr}+");
        //                int indexOfMinus = readText.IndexOf($"{markerStr}-");

        //                string spliced = Splice(readText.AsSpan(0, indexOfPlus).ToString(), readText.AsSpan(indexOfMinus + markerStr.Length + 1).ToString());

        //                _fileIO.Write(spliced, projectFile);
        //            }
        //            catch (Exception ex)
        //            {
        //                _ducky.Error("FrameworkBuilder", "Remove_LibrayFiles", ex.Message);
        //            }
        //        }
        //    }
        //    _ducky.Info($"File audit success for {collection.Name}.");
        //}

        /// <summary>
        /// Helper method for combining two strings
        /// Used mainly for removing existing collection information in files.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        protected string Splice(string start, string end)
        {
            StringBuilder sb = new();
            sb.Append(start);
            sb.Append(end);
            return sb.ToString();
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