using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Graph
{
	static class TSPTESTCOORDINATES
	{
		public static void Main()
		{
			int id = 0;
			string name;
			while (File.Exists((name = $"input{++id}.dickietravels"))) ;
			using (StreamWriter sw = new StreamWriter(name, false))
			{
				List<string> lines = new List<string>();
				string line;
				while ((line = Console.ReadLine()) != "end")
				{
					lines.Add(line);
				}
				double[,] coordinates = new double[lines.Count, 2];
				for (int i = 0; i < lines.Count; i++)
				{
					string[] arr = lines[i].Split(' ');
					coordinates[i, 0] = double.Parse(arr[1]);
					coordinates[i, 1] = double.Parse(arr[2]);
				}
				sw.WriteLine(lines.Count + " " + ((lines.Count * lines.Count - lines.Count) / 2));
				for(int i = 0; i < lines.Count; i ++)
				{
					for(int j = i+1; j < lines.Count; j++)
					{
						double a = coordinates[i, 0] - coordinates[j, 0];
						double b = coordinates[i, 1] - coordinates[j, 1];
						int dist = (int)Math.Sqrt(a * a + b * b);
						sw.WriteLine($"{i} {j} {dist/ 1237919} {dist/ 1237919 /j}");
					}
				}
				sw.WriteLine(int.MaxValue);
			}
				
		}
	}
}
