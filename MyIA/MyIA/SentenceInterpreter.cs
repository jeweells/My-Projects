using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIA
{
	static class SentenceInterpreter
	{
		public static void Interpret(Sentence sentence)
		{
			if(sentence.Other.Same(Greetings.Get))
			{
				Console.WriteLine(Greetings.GetRandom());
			}
		}
	}
}
