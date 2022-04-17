using WenRarityLibrary;
using Rime.ViewModels.Asset;
using System.Text;

namespace Rime.Builders
{
    internal class FrameworkWriter
    {
        private static FrameworkWriter instance;
        public static FrameworkWriter Instance => instance ?? (instance = new FrameworkWriter());
        private static Ducky _ducky = Ducky.Instance;
        private static readonly string _marker = @"// ##_:";

        private string DirADOToken;
        private string DirADO;
        private string DirVM;
        private string DirBuilders;
        private FrameworkWriter()
        {
            Start();
        }

        public void Execute(string className, AssetViewModel asset)
        {
            BuildViewModel(className, asset);
            BuildModel(className, asset);
            UpdateFile_RimeDb(className);
            UpdateFile_AssetBuilder(className);
        }


        private void BuildViewModel(string className, AssetViewModel asset)
        {
            StringBuilder sb = new StringBuilder();
            var vmName = className + "ViewModel";

            // Using Statments
            sb.Append("using Rime.ADO;");

            // Namespace
            sb.Append("\nnamespace Rime.ViewModels.Asset.Token" +
                "\n{");
            // Constructor
            sb.Append($"\n\tpublic class {vmName} : OnChainMetaDataViewModel" +
                "\n\t{");
            sb.Append("\n");
            // Model
            sb.Append(Model_String(className, vmName, asset));
            // Add
            sb.Append(Add_String(className, vmName));
            // Get
            sb.Append(Get_String(className, vmName));

            sb.Append("\n\t\t}");
            sb.Append("\n\t}");
            sb.Append("\n}");

            WriteViewModel(vmName, sb.ToString());
        }

        public void BuildModel(string className, AssetViewModel asset)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("namespace Rime.ADO");
            sb.Append("\n{");
            sb.Append($"\n\tpublic partial class {className} : OnChainMetaData");
            sb.Append("\n\t{");

            foreach (var attribute in asset.onchain_metadata.attributes)
            {
                int valueSize = 0;
                if (attribute.Value.Length > 0) valueSize = 1;
                string attributeName = attribute.Value.ToString().ToUpper() + attribute.Value.Substring(valueSize);
                sb.Append($"\n\t\tpublic string { attribute.Key}");
                sb.Append(" { get; set; }");
            }

            sb.Append("\n\t}");
            sb.Append("\n}");

            WriteModel(className, sb.ToString());
        }

        private string Model_String(string className, string vmName, AssetViewModel asset)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{Tabs(2)}public override OnChainMetaData Model()");
            sb.Append("\n\t\t{" +
                $"\n{Tabs(3)}" +
                $"{className} m = new();" +
                $"\n");

            // Abstract attributes
            sb.Append($"\n{Tabs(3)}m.name = name;");
            sb.Append($"\n{Tabs(3)}m.image = image;");
            sb.Append($"\n{Tabs(3)}m.mediaType = mediaType;");
            sb.Append($"\n{Tabs(3)}m.policy_id = policy_id;");
            sb.Append($"\n{Tabs(3)}m.asset = asset;");

            // Model specific attributes
            foreach (var attribute in asset.onchain_metadata.attributes)
            {
                int valueSize = 0;
                if (attribute.Value.Length > 0) valueSize = 1;
                string attributeName = attribute.Value.ToString().ToUpper() + attribute.Value.Substring(valueSize);
                sb.Append($"\n{Tabs(3)}m.{attribute.Key} = attributes.GetValueOrDefault(\"{attribute.Key}\");");
            }

            sb.Append($"\n\n{Tabs(3)}return m;");
            sb.Append($"\n{Tabs(2)}" +
                "}");
            sb.Append("\n");
            return sb.ToString();
        }

        private string Add_String(string className, string vmName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{Tabs(2)}public override void Add()");
            sb.Append("\n\t\t{" +
                $"\n{Tabs(3)}" +
                $"OnChainMetaData m = Model();" +
                $"\n");
            // Try
            sb.Append($"\n{Tabs(3)}using RimeDb context = new();");
            sb.Append($"\n\n{Tabs(3)}var trans = context.Database.BeginTransaction();");
            sb.Append($"\n{Tabs(3)}try" +
                $"\n{Tabs(3)}" +
                "{");

            sb.Append($"\n{Tabs(4)}" +
                $"context.{className}s.Add(({className})m);");

            sb.Append($"\n{Tabs(4)}trans.Commit();");
            sb.Append($"\n{Tabs(4)}context.SaveChanges();");
            sb.Append($"\n{Tabs(3)}" +
                "}");

            // Catch
            sb.Append($"\n{Tabs(3)}catch (Exception ex)" +
                $"\n{Tabs(3)}" +
                "{");
            sb.Append($"\n{Tabs(4)}trans.Rollback();");
            sb.Append($"\n{Tabs(4)}_ducky.Error(\"KBotViewModel\", \"Add()\", ex.Message);");
            sb.Append($"\n{Tabs(3)}" +
                "}");
            sb.Append($"\n{Tabs(2)}" + "}");
            return sb.ToString();
        }
        private string Get_String(string className, string vmName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"\n{Tabs(2)}public override void Get(out Dictionary<string, OnChainMetaData> assets)");
            sb.Append($"\n{Tabs(2)}" +
                "{" +
                $"\n{Tabs(3)}" +
                $"assets = new();");

            // Try
            sb.Append($"\n\n{Tabs(3)}using RimeDb context = new();");
            sb.Append($"\n\n{Tabs(3)}var all = context.{className}s.ToList();");
            sb.Append($"\n{Tabs(3)}try" +
                $"\n{Tabs(3)}" +
                "{");

            sb.Append($"\n{Tabs(4)}" +
                $"foreach (var item in all)");
            sb.Append($"\n{Tabs(4)}assets.Add(item.asset, item);");
            sb.Append($"\n{Tabs(3)}" +
             "}");

            // Catch
            sb.Append($"\n{Tabs(3)}catch (Exception ex)" +
                $"\n{Tabs(3)}" +
                "{");
            sb.Append($"\n{Tabs(4)}_ducky.Error(\"{vmName}\", \"Get()\", ex.Message);");
            sb.Append($"\n{Tabs(3)}" +
                "}");
            return sb.ToString();
        }


        #region Write
        public void WriteViewModel(string className, string classString)
        {
            //if (File.Exists($"${DirVM}\\{className}\\{className}.cs")) File.Delete(DirVM);
            using (StreamWriter sw = new StreamWriter(DirVM + $"\\{className}\\{className}.cs"))
            {
                sw.WriteLine(classString);
            }
            _ducky.Info($"Wrote {className} to file.");
        }

        public void WriteModel(string className, string classString)
        {
            //if (File.Exists($"${DirADOToken}{className}.cs")) File.Delete(DirADOToken);
            using (StreamWriter sw = new(DirADOToken + $"\\{className}\\{className}.cs"))
            {
                sw.WriteLine(classString);
            }
            _ducky.Info($"Wrote {className} to file.");
        }

        private void FileWriter(string text, string location)
        {
            //using StreamWriter sw = new(location + ".txt");
            using StreamWriter sw = new(location);
            sw.WriteLine(text);
            //sw.Close();
        }

        #endregion Write

        private void UpdateFile_RimeDb(string className)
        {
            string fileText = "";
            string fileLoc = DirADO + "RimeDb.cs";
            if (File.Exists(fileLoc))
            {
                using(StreamReader sr = new StreamReader(fileLoc))
                {
                    string readText = sr.ReadToEnd();
                    StringBuilder sb = new StringBuilder();
                    // Find index of the special marker
                    int markerLoc = readText.IndexOf(_marker);

                    sb.Append(readText.Substring(0, markerLoc));
                    string insert = $"public virtual DbSet<{className}> {className}" +
                        "{ get; set; }";
                    sb.Append(insert);
                    sb.Append($"\n{Tabs(2)}");
                    sb.Append(readText.Substring(markerLoc));
                    fileText = sb.ToString();
                }
            }
            FileWriter(fileText, fileLoc);
        }
        private void UpdateFile_AssetBuilder(string className)
        {
            string fileText = "";
            string fileLoc = DirBuilders + "AssetBuilder.cs";
            if (File.Exists(fileLoc))
            {
                using (StreamReader sr = new StreamReader(fileLoc))
                {
                    string readText = sr.ReadToEnd();
                    StringBuilder sb = new StringBuilder();
                    // Find index of the special marker
                    int markerLoc = readText.IndexOf(_marker);

                    sb.Append(readText.Substring(0, markerLoc));
                    string insert = $"case \"{className}\": return JsonConvert.DeserializeObject<{className}ViewModel>(json);";
                    sb.Append(insert);
                    sb.Append($"\n{Tabs(4)}");
                    sb.Append(readText.Substring(markerLoc));
                    fileText = sb.ToString();
                }
            }
            FileWriter(fileText, fileLoc);
        }

        private string Tabs(int count)
        {
            string tabs = "";
            for (int i = 0; i < count; i++)
                tabs += "\t";
            return tabs;
        }



        #region Start
        public void Start()
        {
            DirectorySafetyChecks();
        }

        private void DirectorySafetyChecks()
        {
            string cwd = "";
            string[] splitCwd = Environment.CurrentDirectory.Split('\\');
            bool complete = false;

            foreach (string item in splitCwd)
            {
                if (!complete) cwd += $"\\{item}";
                if (item.Equals("Rime")) complete = true;
            }

            cwd = cwd.Substring(1, cwd.Length - 1);

            DirADOToken = $@"{cwd}\ADO\Token\";
            DirADO = $@"{cwd}\ADO\";
            DirVM = $@"{cwd}\ViewModels\Asset\Token\";
            DirBuilders = $@"{cwd}\Builders\";

            if (!Directory.Exists(DirADOToken))
            {
                try
                {
                    Directory.CreateDirectory(DirADOToken);
                    _ducky.Info($"Created directory: {DirADOToken}.");
                }
                catch (Exception ex)
                {
                    _ducky.Error("RimeWriter", "DirectorySafetyChecks", ex.Message);
                    throw;
                }
            }

            if (!Directory.Exists(DirVM))
            {
                try
                {
                    Directory.CreateDirectory(DirVM);
                    _ducky.Info($"Created directory: {DirVM}.");
                }
                catch (Exception ex)
                {
                    _ducky.Error("RimeWriter", "DirectorySafetyChecks", ex.Message);
                    throw;
                }
            }
        }
        #endregion Start
    }
}
