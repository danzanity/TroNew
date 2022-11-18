using System;
using System.IO;

namespace TRO.Tests
{
	public class DotEnv
	{
		public static void Load()
		{
			var root = AppDomain.CurrentDomain.BaseDirectory;
			var filePath = Path.Combine(root, ".env");
			if (!File.Exists(filePath))
			{
				return;
			}

			foreach (var line in File.ReadAllLines(filePath))
			{
				if (line.TrimStart().StartsWith("#"))
				{
					continue;
				}

				var parts = line.Split('=', StringSplitOptions.RemoveEmptyEntries);
				if (parts.Length != 2)
				{
					continue;
				}

				Environment.SetEnvironmentVariable(parts[0], parts[1]);
			}
		}
	}
}