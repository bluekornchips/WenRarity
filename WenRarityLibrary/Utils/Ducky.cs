using System.Text;

namespace WenRarityLibrary
{
    public class Ducky
    {
        private static Ducky _instance;
        public static Ducky Instance => _instance ?? (_instance = new Ducky());
        private QuackType _logLevel = QuackType.DEBUG;
        private static readonly int _lineWidth = 60;
        private static DateTime _now = DateTime.Now;

        private Ducky()
        {
            Start();
        }

        private void Start()
        {
            _now = DateTime.Now;

            SolidLine();
            CenteredSentence("Starting RimeTwo");
            SolidLine();
            CenteredSentence($" Date: {_now}");
            SolidLine();
        }

        /// <summary>
        /// Quick information.
        /// </summary>
        /// <param name="details"></param>
        public void Info(string details)
        {
            QuackConsole(details, QuackType.INFO);
        }

        /// <summary>
        /// Debug output
        /// </summary>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <param name="details"></param>
        public void Debug(string className, string methodName, string details)
        {
            string line = $"[{QuackType.DEBUG}] >>> [{className}.{methodName}] >>> {details}";
            QuackConsole(line, QuackType.DEBUG);
        }

        /// <summary>
        /// Error output.
        /// </summary>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <param name="details"></param>
        public void Error(string className, string methodName, string details)
        {
            string line = $"[{QuackType.ERROR}] >>> [{className}.{methodName}] >>> {details}";
            QuackConsole(line, QuackType.ERROR);
        }

        /// <summary>
        /// Critical error, required remediation.
        /// Always throws.
        /// </summary>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <param name="details"></param>
        /// <exception cref="Exception"></exception>
        public void Critical(string className, string methodName, string details)
        {
            string line = $"[{QuackType.CRITICAL}] >>> [{className}.{methodName}] >>> {details}";
            QuackConsole(line, QuackType.CRITICAL);
        }

        /// <summary>
        /// Display results to console if the log level is valid.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="quackType"></param>
        private void QuackConsole(string line, QuackType quackType)
        {
            _now = DateTime.Now;
            if (quackType <= _logLevel) Console.WriteLine($"{_now} >>> {line}");
        }

        /// <summary>
        /// Output a line of solid dashes.
        /// </summary>
        private void SolidLine()
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
        private void CenteredSentence(string sentence = "")
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

    enum QuackType
    {
        DEBUG = 3,
        ERROR = 2,
        CRITICAL = 1,
        INFO = 0
    }
}