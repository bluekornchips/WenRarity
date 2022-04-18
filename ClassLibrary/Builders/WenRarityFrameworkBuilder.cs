using System.Text;
using WenRarityLibrary.ADO.Rime.Models;
using WenRarityLibrary.ViewModels;

namespace WenRarityLibrary.Builders
{
    public class WenRarityFileIO
    {
        private static WenRarityFileIO instance;
        public static WenRarityFileIO Instance => instance ?? (instance = new WenRarityFileIO());
        private WenRarityFileIO() { }
        private static Ducky _ducky = Ducky.Instance;

        public void Write(string text, string location)
        {
            using StreamWriter sw = new(location);
            sw.WriteLine(text);
            //sw.Close();
        }

        public void Read(string fileLoc, out string fileText)
        {
            fileText = "";
            if (!File.Exists(fileLoc)) return;
            else
            {
                using StreamReader sr = new StreamReader(fileLoc);
                fileText = sr.ReadToEnd();
            }
        }
    }

    public class WenRarityFrameworkBuilder
    {
        private static WenRarityFrameworkBuilder instance;
        public static WenRarityFrameworkBuilder Instance => instance ?? (instance = new WenRarityFrameworkBuilder());
        private WenRarityFrameworkBuilder()
        {
            DirectorySafetyChecks();
        }
        private static WenRarityFileIO _fileIO = WenRarityFileIO.Instance;
        private static Ducky _ducky = Ducky.Instance;
        private static readonly string _marker = @"//##_:";

        private string blockFrostDir = "";
        private string libraryDir = "";

        public void Build(Collection collection, OnChainMetaDataViewModel vm)
        {
            //Update_BlockfrostADO(collection);
            //Update_ViewModelSwitch(collection);
            AddToken(collection, vm);
        }

        private void Update_BlockfrostADO(Collection collection)
        {
            StringBuilder sb = new();

            string fileLoc = blockFrostDir + "\\ADO\\BlockfrostADO.cs";
            string startToken = _marker + "tokens+";

            _fileIO.Read(fileLoc, out string readText);
            
            int startLoc = readText.IndexOf(startToken);
            string fileStartToMarker = readText.Substring(0, startLoc);
            string filerMarkerToEnd = readText.Substring(startLoc + startToken.Length);

            string newText = $"{OH(1,2)}public virtual DbSet<{collection.Name}> {collection.Name} " +
                "{ get; set; };";

            sb.Append(fileStartToMarker);
            sb.Append(startToken);
            sb.Append(newText);
            sb.Append(filerMarkerToEnd);

            _fileIO.Write(sb.ToString(), fileLoc);
        }

        public void Update_ViewModelSwitch(Collection collection)
        {
            StringBuilder sb = new();

            string fileLoc = libraryDir + "\\ViewModels\\OnChainMetaDataViewModel.cs";
            string startToken = _marker;

            _fileIO.Read(fileLoc, out string readText);

            int startLoc = readText.IndexOf(startToken);
            string fileStartToMarker = readText.Substring(0, startLoc);
            string filerMarkerToEnd = readText.Substring(startLoc + startToken.Length);

            string newText = $"{OH(1, 4)}case \"KBot\": return ({collection.Name})model;";

            sb.Append(fileStartToMarker);
            sb.Append(startToken);
            sb.Append(newText);
            sb.Append(filerMarkerToEnd);

            _fileIO.Write(sb.ToString(), fileLoc + ".txt");
        }

        private void AddToken(Collection collection, OnChainMetaDataViewModel vm)
        {
            StringBuilder sb = new();
            int tabs = 1;

            // Header
            sb.Append("using System.ComponentModel.DataAnnotations.Schema;");
            sb.Append($"{OH(2,0)}namespace WenRarityLibrary.ADO.Rime.Models.OnChainMetaData.Token");
            sb.Append($"{OH(1,0)}" + "{");
            sb.Append($"{OH(1,tabs)}[Table(\"{collection.Name}\")]");
            sb.Append($"{OH(1,tabs++)}public partial class {collection.Name} : OnChainMetaData");
            sb.Append($"{OH(1,0)}" + "{");

            sb.Append($"{OH(1, 1)}" + "}");
            sb.Append($"{OH(1, 0)}" + "}");

        }

        public string OH(int newLine, int tabs)
        {
            string output = "";
            for (int i = 0; i < newLine; i++) output += "\r\n";
            for (int i = 0; i < tabs; i++) output += "\t";
            return output;
        }

        private void DirectorySafetyChecks()
        {
            string cwd = "";
            string[] splitCwd = Environment.CurrentDirectory.Split('\\');
            bool complete = false;

            foreach (string item in splitCwd)
            {
                if (!complete) cwd += $"\\{item}";
                if (item.Equals("Blockfrost")) complete = true;
            }

            cwd = cwd.Substring(1, cwd.Length - 1);

            blockFrostDir = cwd;
            libraryDir = cwd.Replace("Blockfrost", "ClassLibrary");

            //DirADOToken = $@"{cwd}\ADO\Token\";
            //DirADO = $@"{cwd}\ADO\";
            //DirVM = $@"{cwd}\ViewModels\Asset\Token\";
            //DirBuilders = $@"{cwd}\Builders\";

            //if (!Directory.Exists(DirADOToken))
            //{
            //    try
            //    {
            //        Directory.CreateDirectory(DirADOToken);
            //        _ducky.Info($"Created directory: {DirADOToken}.");
            //    }
            //    catch (Exception ex)
            //    {
            //        _ducky.Error("RimeWriter", "DirectorySafetyChecks", ex.Message);
            //        throw;
            //    }
            //}

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
