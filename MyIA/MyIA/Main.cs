using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIA
{
	class MainIA
	{
		static void Main(string[] args)
		{
			Paths.CreatePaths();
			string message = "";
			while(message != "bye")
			{
				message = Console.ReadLine();
			}
		}
	}
}
