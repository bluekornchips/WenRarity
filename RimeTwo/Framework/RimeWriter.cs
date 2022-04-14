using System;
using System.IO;
using System.Text;

namespace RimeTwo
{
    public class RimeWriter
    {
        private static RimeWriter instance;
        public static RimeWriter Instance => instance ?? (instance = new RimeWriter());
        private string DataDir;
        private RimeWriter()
        {
            Start();
        }

        public void Build(string className, Asset asset)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("namespace RimeTwo");
            sb.Append("\n{");
            sb.Append($"\n\tpublic class {className} : Asset");
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

            Write(className, sb.ToString());
        }

        public void Write(string className, string classString)
        {
            if (File.Exists($"${DataDir}{className}.cs")) File.Delete(DataDir);
            using (StreamWriter sw = new StreamWriter(DataDir + $"{className}.cs"))
            {
                sw.WriteLine(classString);
            }
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

            DataDir = $@"{cwd}\Asset\Token\";

            if (!Directory.Exists(DataDir))
            {
                try
                {
                    Directory.CreateDirectory(DataDir);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}