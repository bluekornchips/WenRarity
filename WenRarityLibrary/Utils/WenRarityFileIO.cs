﻿using CsvHelper;
using System.Globalization;

namespace WenRarityLibrary.Utils
{
    public class WenRarityFileIO
    {
        private static WenRarityFileIO instance;
        public static WenRarityFileIO Instance => instance ?? (instance = new WenRarityFileIO());
        private WenRarityFileIO() { }

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

        public void Write_CSV(IEnumerable<object> ordered, string csvDir)
        {
            using (var writer = new StreamWriter(csvDir))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(ordered);
            }
        }
    }
}
