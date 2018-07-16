using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace MyIA
{
	static class Greetings
	{

		static Random r = new Random();
		public static HashSet<string> Get = new HashSet<string>();
		static Greetings()
		{
			try
			{
				foreach (string file in Directory.EnumerateFiles(Paths.Greetings.Normals.GetInDataDirectory(), "*.data"))
				{
					using (StreamReader sr = new StreamReader(file))
					{
						Get.Add(sr.ReadLine());
					}
				}
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine("No greetings were found");
			}
			
		}
		public static bool IsGreeting(this string word)
		{
			return Get.Contains(word);
		}

		public static string GetRandom()
		{
			return Get.ElementAt(r.Next() % Get.Count);
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
		public static void ReportedAdd(IEnumerable<string> greetings)
		{
			foreach (var item in greetings)
			{
				ReportedAdd(item);
			}
		}
		public static bool Add(string greeting)
		{
			try
			{
				using (StreamWriter sw = new StreamWriter($"{Paths.Greetings.Normals.GetInDataDirectory()}/{DateTime.Today.ToString().CleanFileName()}.data", true))
				{
					sw.WriteLine(greeting);
					Get.Add(greeting);
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
