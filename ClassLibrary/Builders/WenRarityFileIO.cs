namespace WenRarityLibrary.Builders
{
    public class WenRarityFileIO
    {
        private static WenRarityFileIO instance;
        public static WenRarityFileIO Instance => instance ?? (instance = new WenRarityFileIO());
        private WenRarityFileIO() { }

        private static Ducky _ducky = Ducky.Instance;

        /// <summary>
        /// Write file - Overwrites.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="location"></param>
        public void Write(string text, string location)
        {
            using StreamWriter sw = new(location);
            sw.WriteLine(text);
        }

        /// <summary>
        /// Read file if it exists.
        /// </summary>
        /// <param name="fileLoc"></param>
        /// <param name="fileText"></param>
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
}
