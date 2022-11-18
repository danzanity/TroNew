using System.Globalization;

namespace ClientSourceGenerator
{
	public static class StringExtensions
	{
		public static string ToCamelCase(this string text)
		{
			if (string.IsNullOrEmpty(text)) return text;
			if (text.Contains("-"))
			{
				var ti = new CultureInfo("en-US", false).TextInfo;
				text = ti.ToTitleCase(text).Replace("-", "");
			}
			return $"{text.Substring(0, 1).ToLower()}{text.Substring(1)}";
		}

		public static string[] SplitByNewline(this string text)
		{
			return text
				.Replace("\r\n", "\n")
				.Replace("\r", "\n")
				.Split('\n');
		}
	}
}