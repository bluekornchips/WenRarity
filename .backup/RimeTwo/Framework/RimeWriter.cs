using RimeTwo.ADO.Asset;
using RimeTwo.Util;
using RimeTwo.ViewModels;
using System;
using System.IO;
using System.Text;

namespace RimeTwo
{
    public class RimeWriter
    {
        private static RimeWriter instance;
        public static RimeWriter Instance => instance ?? (instance = new RimeWriter());
        private static Ducky _ducky = Ducky.Instance;

        private string DirADO;
        private string DirVM;
        private RimeWriter()
        {
            Start();
        }

        public void BuildViewModel(string className, AssetViewModel asset)
        {
            StringBuilder sb = new StringBuilder();
            var ogName = className;
            className += "ViewModel";
            sb.Append("using RimeTwo.ADO.Asset.Token;");
            sb.Append("\nnamespace RimeTwo.ViewModels.Asset.Token");
            sb.Append("\n{");
            sb.Append($"\n\tpublic class {className} : OnChainMetaDataViewModel");
            sb.Append("\n\t{");

            foreach (var attribute in asset.onchain_metadata.attributes)
            {
                string attributeName = attribute.Value.ToString().ToUpper() + attribute.Value.Substring(1);
                sb.Append($"\n\t\tpublic string { attribute.Key}");
                sb.Append(" { get; set; }");
            }

            sb.Append($"\n\t\tpublic {className}()");
            sb.Append("{ " +
                "}");
            sb.Append("\n\t}");
            sb.Append("\n}");

            WriteViewModel(className, sb.ToString());
        }

        public void BuildModel(string className, AssetViewModel asset)
        {
            StringBuilder sb = new StringBuilder();
            className += "Model";

            var model = asset.AsModel();
            sb.Append("namespace RimeTwo.ADO.Asset.Token");
            sb.Append("\n{");
            sb.Append($"\n\tpublic class {className} : OnChainMetaDataModel");
            sb.Append("\n\t{");

            foreach (var attribute in asset.onchain_metadata.attributes)
            {
                string attributeName = attribute.Value.ToString().ToUpper() + attribute.Value.Substring(1);
                sb.Append($"\n\t\tpublic string { attribute.Key}");
                sb.Append(" { get; set; }");
            }

            sb.Append($"\n\t\tpublic {className}()");
            sb.Append("{ }");
            sb.Append("\n\t}");
            sb.Append("\n}");

            WriteModel(className, sb.ToString());
        }

        public void WriteViewModel(string className, string classString)
        {
            if (File.Exists($"${DirVM}{className}.cs")) File.Delete(DirVM);
            using (StreamWriter sw = new StreamWriter(DirVM + $"{className}.cs"))
            {
                sw.WriteLine(classString);
            }
            _ducky.Info($"Wrote {className} to file.");
        }

        public void WriteModel(string className, string classString)
        {
            if (File.Exists($"${DirADO}{className}.cs")) File.Delete(DirADO);
            using (StreamWriter sw = new StreamWriter(DirADO + $"{className}.cs"))
            {
                sw.WriteLine(classString);
            }
            _ducky.Info($"Wrote {className} to file.");
        }

        public void Start()
        {
            DirectorySafetyChecks();
        }

        private void DirectorySafetyChecks()
        {
            string cwd = "";
            string[] splitCwd = Environment.CurrentDirectory.Split("\\");
            bool complete = false;

            foreach (string item in splitCwd)
            {
                if (!complete) cwd += $"\\{item}";
                if (item.Equals("RimeTwo")) complete = true;
            }

            cwd = cwd.Substring(1, cwd.Length - 1);

            DirADO = $@"{cwd}\ADO\Asset\Token\";
            DirVM = $@"{cwd}\ViewModels\Asset\Token\";

            if (!Directory.Exists(DirADO))
            {
                try
                {
                    Directory.CreateDirectory(DirADO);
                    _ducky.Info($"Created directory: {DirADO}.");
                }
                catch (Exception ex)
                {
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
                    throw;
                }
            }
        }
    }
}