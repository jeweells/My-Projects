using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIA
{
	static class StopPhrases
	{
		public static HashSet<string> Get = new HashSet<string>();
		static StopPhrases()
		{
			try
			{
				foreach (string file in Directory.EnumerateFiles(Paths.StopPhrases.Normals.GetInDataDirectory(), "*.data"))
				{
					using (StreamReader sr = new StreamReader(file))
					{
						Get.Add(sr.ReadLine());
					}
				}
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine("No stop phrases were found");
			}
		}
		public static void ReportedAdd(string stopphrase)
		{
			if (Add(stopphrase))
			{
				Console.WriteLine($"{stopphrase} added.");
			}
			else
			{
				Console.WriteLine($"I couldn't add '{stopphrase}'");
			}
		}
		public static void ReportedAdd(IEnumerable<string> stopphrases)
		{
			foreach (var item in stopphrases)
			{
				ReportedAdd(item);
			}
		}
		public static bool Add(string stopphrase)
		{
			try
			{
				using (StreamWriter sw = new StreamWriter($"{Paths.StopPhrases.Normals.GetInDataDirectory()}/{DateTime.Today.ToString().CleanFileName()}.data", true))
				{
					sw.WriteLine(stopphrase);
					Get.Add(stopphrase);
				}
			}
			catch
			{
				return false;
			}
			return true;
		}
	}
}
