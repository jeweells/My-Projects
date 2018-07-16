using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace MyIA
{

	static class Verbs
	{
		public static HashSet<string> Get = new HashSet<string>();
		static Verbs()
		{
			try
			{
				foreach (string file in Directory.EnumerateFiles(Paths.Sentences.Verbs.GetInDataDirectory(), "*.data"))
				{
					using (StreamReader sr = new StreamReader(file))
					{
						Get.Add(sr.ReadLine());
					}
				}
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine("No verbs were found");
			}
		}
		public static void ReportedAdd(string verb)
		{
			if (Verbs.Add(verb))
			{
				Console.WriteLine($"{verb} added.");
			}
			else
			{
				Console.WriteLine($"I couldn't add '{verb}'");
			}
		}
		public static void ReportedAdd(IEnumerable<string> verbs)
		{
			foreach (var item in verbs)
			{
				ReportedAdd(item);
			}
		}
		public static bool Add(string verb)
		{
			try
			{
				using (StreamWriter sw = new StreamWriter($"{Paths.Sentences.Verbs.GetInDataDirectory()}/{DateTime.Today.ToString().CleanFileName()}.data", true))
				{
					sw.WriteLine(verb);
					Get.Add(verb);
				}
			}
			catch
			{
				return false;
			}
			return true;
		}

		public IEnumerator<string> GetEnumerator()
		{
			throw new NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}
}
