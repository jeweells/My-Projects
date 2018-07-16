using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MyIA
{
	static class Instructions
	{
		public static HashSet<string> Get = new HashSet<string>()
		{
			"add"
		};
		public static void Perform(Sentence sentence)
		{
			// Reading senteces here can have mistakes so approximations are made
			if(sentence.Verb.Same("add"))
			{
				#region Verbs
				if(sentence.Object.Same("verbs"))
				{
					if(sentence.Object.Contains('s')) // Since verbs can have mistakes such as 
						// verb == verbs it's necessary to confirm it has a 's' will be enough to be plural
					{
						// Many objects
						Verbs.ReportedAdd(sentence.Other.Split(Language.Separators));
					}
					else
					{
						// One object
						Verbs.ReportedAdd(sentence.Other);
					}
				}
				else if (sentence.Object.Same("verb")) // It's not verbs, but might be ver == verb, 
					// since verbs and ver has 2 mistakes so is discarded
				{ // However, adding many objects are discarded
					Verbs.ReportedAdd(sentence.Other);
				}
				#endregion

				#region Greetings
				if (sentence.Object.Same("greetings"))
				{
					if (sentence.Object.Contains('s'))
					{
						// Many objects
						Greetings.ReportedAdd(sentence.Other);
						string ngreeting;
						while (!(ngreeting = Console.ReadLine()).Same(StopPhrases.Get))
						{
							Greetings.ReportedAdd(ngreeting);
						}
					}
					else
					{
						// One object
						Greetings.ReportedAdd(sentence.Other);
					}
				}
				else if (sentence.Object.Same("greeting"))
				{
					Greetings.ReportedAdd(sentence.Other);
				}
				#endregion
			}
		}
	}
}
