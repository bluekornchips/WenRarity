using System;
using System.Text;

namespace MarketWatcher.Utility
{
	public class Ducky
	{
		private int _logLevel = 3;
		private static readonly int _lineWidth = 60;
		private static Ducky _ducky = null;

		private Ducky() { }

		private static void Start()
		{
			_ducky = new Ducky();
			DateTime date = DateTime.Now;

			SolidLine();
			Sentence("Starting MarketWatcher");
			SolidLine();
			Sentence($" Date: {date}");
			SolidLine();
		}
		public static Ducky getInstance()
		{
			if (_ducky == null) Start();
			return _ducky;
		}

		public void Info(string details)
		{
			Quack(details, 0);
		}
		public void Debug(string className, string methodName, string details)
		{
			string line = "";
			line = $"[DEBUG] >>> [{className}.{methodName}] >>> {details}";
			Quack(line, 3);
		}

		public void Error(string className, string methodName, string details)
		{
			string line = "";
			line = $"[ERROR] >>> [{className}.{methodName}] >>> {details}";
			Quack(line, 1);
		}

		private void Quack(string line, int logLevel)
		{
			DateTime date = DateTime.Now;
			if (logLevel <= _logLevel)
			{
				Console.WriteLine($"{date} >>> {line}");
			}
		}

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

		private static void Sentence(string sentence = "")
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
