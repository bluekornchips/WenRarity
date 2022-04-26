using System.Text;
using WenRarityLibrary.Utils;

namespace WenRarityLibrary.Builders
{
    public class FrameworkBuilder
    {
        protected static Ducky _ducky = Ducky.Instance;
        protected static WenRarityFileIO _fileIO = WenRarityFileIO.Instance;

        protected static readonly string _marker = @"//##_:";

        protected static string blockFrostDir = "";
        protected static string rimeLibraryDir = "";
        protected static string blockfrostLibraryDir = "";
        protected static string statsDir = "";

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

        /// <summary>
        /// Helper method for easy sql friendly identity attribute
        /// </summary>
        /// <returns></returns>
        protected string EasyID()
            => $"{OH(1, 2)}[Key]{OH(1, 2)}[DatabaseGenerated(DatabaseGeneratedOption.Identity)]{OH(1, 2)}public int id" + "{ get; set; }";

        /// <summary>
        /// Helper method for ensuring directory paths
        /// </summary>
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
            rimeLibraryDir = cwd + "RimeLibrary";
            blockfrostLibraryDir = cwd + "BlockfrostLibrary";
            statsDir = cwd + "Stats";
        }
    }
}