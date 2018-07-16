using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIA
{
	class Sentence
	{
		public string Subject { get; set; }
		public string Auxiliar { get; set; }
		public string Adverb { get; set; }
		public string Adjective { get; set; }
		public string Verb { get; set; }
		public string Noun { get; set; }
		public List<string> Objects { get; set; }
		public Sentence Complement { get; set; }
		public string Other { get; set; }
		public Sentence() { }
		public Sentence(string sentence)
		{
			string[] words = sentence.Split(' ');
			int i = 0;

			if(words[i++].Same(Verbs.Get)) // Order
			{
				string word = words[i];
				// Follows something else
				more_objects:
				while((words[i].Same(Determiners.Get) || words[i].Same(Adjectives.Get)) && i < words.Length)
				{
					word += " " + words[i];
					i++;
				}
				Objects.Add(word);
				if(i < words.Length && words[i].Same(new List<string>() { "and", "or" }))
				{
					i++;
					goto more_objects;
				}
				else if(words[i].EndsWith(","))
				{
					if(i+1 < words.Length && !words[i+1].Same(Adverbs.Get))
					{
						goto more_objects;
					}
				}
			}
			else if(words[0].Same(Subjects.Get)) // Information
			{

			}
			else // Maybe a subject, such names, etc
			{

			}
		}
	}
}
