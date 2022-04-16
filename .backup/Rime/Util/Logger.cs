using Rime.Util;
using System;
using System.IO;

namespace BlockfrostQuery.Util
{
    public static class Logger
    {
        /// <summary>
        /// Informative
        /// </summary>
        /// <param name="details"></param>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        public static void Info(string details)
        {
            Write("[INFO] ", details);
        }

        /// <summary>
        /// Problem finding.
        /// </summary>
        /// <param name="details"></param>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        public static void Debug(string className, string methodName, string details)
        {
            Write("[DEBUG]", $"[{className}:{methodName}] >>> {details}");
        }

        /// <summary>
        /// Critical errors.
        /// </summary>
        /// <param name="details"></param>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        public static void Error(string className, string methodName, string exMessage)
        {
            Write("[ERROR]", $"[{className}:{methodName}] >>> {exMessage}");
        }

        /// <summary>
        /// Write logs to file and console.
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="details"></param>
        private static void Write(string logType, string details)
        {
            string output = $"[{DateTime.Now}] >>> {logType} >>> {details}\n";
            Console.Write(output);

            if (!Directory.Exists(Setup.LoggerStr)) Directory.CreateDirectory(Setup.LoggerStr);
            File.AppendAllText($"{Setup.LoggerStr}\\log.txt", output);
        }
    }
}
