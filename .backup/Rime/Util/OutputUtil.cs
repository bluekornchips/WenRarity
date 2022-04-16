using BlockfrostQuery.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace Rime.Util
{
    public class OutputUtil
    {
        /// <summary>
        /// Write all the SQL rows to file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="rows"></param>
        public static void Write(string path, List<string> rows)
        {
            try
            {
                File.WriteAllLines(path + ".txt", rows);
                Logger.Info($"Wrote {rows.Count} to {path}.txt");
            }
            catch (Exception ex)
            {
                Logger.Error("CSV", "Write", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Write all the Tokens to a json file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="tokens"></param>
        public static void WriteJson(string path, List<JToken> tokens)
        {
            try
            {
                var converted = JsonConvert.SerializeObject(tokens, Formatting.Indented);
                File.WriteAllText(path + ".json", converted);
                Logger.Info($"Wrote {tokens.Count} to {path}.json");
            }
            catch (Exception ex)
            {
                Logger.Error("CSV", "Write", ex.Message);
                throw;
            }
        }
    }
}
