using BlockfrostQuery.Util;
using System;
using System.IO;

namespace Rime.Util
{
    public static class Setup
    {
        private static string Root;
        public static string LoggerStr;
        public static string DataDir;

        static Setup()
        {
            Start();
        }

        public static void Start()
        {
            DirectorySafetyChecks();
        }

        private static void DirectorySafetyChecks()
        {
            string cwd = "";
            string[] splitCwd = Environment.CurrentDirectory.Split("\\");
            bool complete = false;

            foreach (string item in splitCwd)
            {
                if (!complete) cwd += $"\\{item}";
                if (item.Equals("Rime"))
                {
                    //cwd += $"\\Data\\";
                    complete = true;
                }
            }

            cwd = cwd.Substring(1, cwd.Length - 1);

            Root = cwd;
            LoggerStr = @$"{Root}\Logs\";
            DataDir = $@"{Root}\Data\";

            if (!Directory.Exists(LoggerStr))
            {
                try
                {
                    Directory.CreateDirectory(LoggerStr);
                    Logger.Info($"Created Directory {LoggerStr}");
                }
                catch (Exception ex)
                {
                    Logger.Error("Setup", "Start", ex.Message);
                    throw;
                }
            }
            if (!Directory.Exists(DataDir))
            {
                try
                {
                    Directory.CreateDirectory(DataDir);
                    Logger.Info($"Created Directory {LoggerStr}");
                }
                catch (Exception ex)
                {
                    Logger.Error("Setup", "Start", ex.Message);
                    throw;
                }
            }
        }
    }
}
