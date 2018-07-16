using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MyIA
{
	static class Subjects
	{
		public static HashSet<string> Get = new HashSet<string>();
		static Subjects()
		{
			try
			{
				foreach (string file in Directory.EnumerateFiles(Paths.Sentences.Subjects.GetInDataDirectory(), "*.data"))
				{
					using (StreamReader sr = new StreamReader(file))
					{
						Get.Add(sr.ReadLine());
					}
				}
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine("No subjects were found");
			}
		}
	}
}
