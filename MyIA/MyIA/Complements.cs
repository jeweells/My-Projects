using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace MyIA
{
	static class Complements
	{
		public static HashSet<string> Get = new HashSet<string>();
		static Complements()
		{
			try
			{
				foreach (string file in Directory.EnumerateFiles(Paths.Sentences.Complements.GetInDataDirectory(), "*.data"))
				{
					using (StreamReader sr = new StreamReader(file))
					{
						Get.Add(sr.ReadLine());
					}
				}
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine("No complements were found");
			}
		}
	}
}
