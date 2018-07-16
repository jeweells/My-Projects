using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CombinationVariations;

namespace MyIA
{
	static class Word
	{
		public static IEnumerable<int> Differents(this string a, string b)
		{
			int dist = 0;
			int auxbound;
			if (a.Length <= b.Length)
			{
				auxbound = a.Length;
				dist += a.Length - b.Length;
			}
			else
			{
				auxbound = b.Length;
				dist += b.Length - a.Length;
			}
			if(dist > 0)
			{
				for (int i = auxbound, endi = auxbound + dist; i < endi; i++) yield return i;
			}
			for (int i = 0; i < auxbound; i++)
			{
				if (a[i] != b[i]) yield return i;
			}
		}
		static bool CanMatch(string a, string b, IEnumerable<int> indexes, int distance = 1)
		{
			List<int> normalindexes = indexes.ToList();
			// Sort the indexes since the indexes must be from the less to the greater
			normalindexes.Sort((x, y) =>
			{
				if (x == y) return 0;
				if (x < y) return -1;
				else return 1;
			});
			foreach (var tmpindexes in Variations.Get(indexes, indexes.Count(), false))
			{
				char[] a_arr = a.ToCharArray();
				for (int i = 0; i < normalindexes.Count; i++)
				{
					char tmpc = a_arr[normalindexes[i]];
					a_arr[normalindexes[i]] = a_arr[tmpindexes.ElementAt(i)];
					a_arr[tmpindexes.ElementAt(i)] = tmpc;
				}
				string str = new string(a_arr);
				if (String.Compare(str, b, ignoreCase: true) == 0) return true;
				else
				{
					int ndif = str.Differents(b).ToArray().Length;
					if (ndif == distance) return true;
				}
			}
			return false;
		}
		public static bool Same(this string a, string b, int distance = 1)
		{
			if (String.Compare(a, b, ignoreCase: true) == 0) return true;
			else
			{ 
				int[] indexes = a.Differents(b).ToArray();
				int dist = indexes.Length;
				if (dist == distance) return true; 
				else if(dist == distance + 1)
				{
					if(CanMatch(a,b, indexes, distance)) return true; // Permutations can match string a in b
					// Also those permutations can accept 'distance' errors so as this function does
				}
			}
			return false;
		}
		public static bool Same(this string b, IEnumerable<string> list,  int distance = 1)
		{
			foreach (var item in list)
			{
				if (b.Same(item)) return true;
			}
			return false;
		}
	}
}
