using MarketWatcher.Discord;
using MarketWatcher.Discord.Webhooks;
using System;
using System.Text;

namespace MarketWatcher.Utility
{
    public partial class Ducky
    {
        private static Ducky _instance = null;
        private static DiscordBot _discord = null;
        private static bool _discordOutput = true;
        private QuackType _logLevel = QuackType.DEBUG;
        private static readonly int _lineWidth = 60;
        private static DateTime _now = DateTime.Now;

        private Ducky() { }

        public static void Start()
        {
            _instance = new Ducky();
            _discord = DiscordBot.Instance;
            _now = DateTime.Now;

            SolidLine();
            CenteredSentence("Starting MarketWatcher");
            SolidLine();
            CenteredSentence($" Date: {_now}");
            SolidLine();
        }

        public static Ducky GetInstance()
        {
            if (_instance == null) Start();
            return _instance;
        }

        public void Info(string details)
        {
            QuackConsole(details, QuackType.INFO);
        }

        public void Debug(string className, string methodName, string details)
        {
            string line = $"[{QuackType.DEBUG}] >>> [{className}.{methodName}] >>> {details}";
            QuackConsole(line, QuackType.DEBUG);
        }

        public void Error(string className, string methodName, string details)
        {
            string line = $"[{QuackType.ERROR}] >>> [{className}.{methodName}] >>> {details}";
            QuackConsole(line, QuackType.ERROR);
        }

        public void Critical(string className, string methodName, string details)
        {
            string line = $"[{QuackType.CRITICAL}] >>> [{className}.{methodName}] >>> {details}";
            QuackConsole(line, QuackType.CRITICAL);
        }

        public void QuackDiscord(string quackType, string className, string methodName, string details)
        {
            if (!_discordOutput) return;
            _now = DateTime.Now;
            IWebhookContainer cm = new ContentMessage(quackType, _now, className, methodName, details);
            _discord.DuckyOutput(cm);
        }

        private void QuackConsole(string line, QuackType quackType)
        {
            _now = DateTime.Now;
            if (quackType <= _logLevel) Console.WriteLine($"{_now} >>> {line}");
        }

        /// <summary>
        /// Output a line of solid dashes.
        /// </summary>
        private static void SolidLine()
        {
            StringBuilder sb = new StringBuilder();
            int charCount = 1;
            sb.Append("+");
            while (charCount < _lineWidth)
            {
                sb.Append('-');
                ++charCount;
            }
            sb.Append("+");
            Console.WriteLine(sb.ToString());
            sb.Clear();
        }

        /// <summary>
        /// Output a line with centered text.
        /// </summary>
        /// <param name="sentence"></param>
        private static void CenteredSentence(string sentence = "")
        {
            int widthMinusChars = 0;
            int halfway = 0;
            int charCount = 1;
            StringBuilder sb = new StringBuilder();
            widthMinusChars = _lineWidth - sentence.Length;
            halfway = widthMinusChars / 2;

            sb.Append("|");
            while (charCount < halfway)
            {
                sb.Append(" ");
                ++charCount;
            }
            sb.Append(sentence);

            charCount += sentence.Length;

            while (charCount < _lineWidth)
            {
                sb.Append(" ");
                ++charCount;
            }
            sb.Append("|");

            Console.WriteLine(sb.ToString());
        }
    }
}
