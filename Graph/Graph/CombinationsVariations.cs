using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Combination
{
	public static class Combination
	{ 
		/// <summary>
		/// Calls "function" for each combination
		/// </summary>
		/// <typeparam name="T">Elements type</typeparam>
		/// <typeparam name="TResult">Return type of the function</typeparam>
		/// <param name="elements">Set from where the combinations are gotten</param>
		/// <param name="depth">The amount of elements that are taken from elements</param>
		/// <param name="repeatedElements">Set true if the combinations allow repeated elements</param>
		/// <param name="function">Function to be called</param>
		/// <returns>Returned values of "function" for each combination</returns>
		public static IEnumerable<TResult> Do<T, TResult>(IEnumerable<T> elements, int depth, bool repeatedElements, Func<IEnumerable<T>, TResult> function)
		{
			IEnumerable<IEnumerable<T>> comb = Get(elements, depth, repeatedElements);
			foreach (IEnumerable<T> item in comb)
			{
				yield return function(item);
			}
		}
		public static int Cardinality(int whole, int depth, bool repeatedElements = false)
		{
			if (!repeatedElements)
			{
				if (depth > whole) return 0;
				int wmp = whole - depth;
				int deletedNumber = (wmp > depth) ? wmp : depth;
				int denominator = (wmp <= depth) ? wmp : depth;
				int numerator = 1;
				for (int i = whole; i > deletedNumber; i--)
				{
					numerator *= i;
				}
				for (int i = denominator - 1; i >= 1; i--)
				{
					denominator *= i;
				}
				if (denominator == 0) denominator = 1;
				if (numerator == 0) numerator = 1;
				return numerator / denominator;
			}
			else
			{
				// CR(w,d) ? = C(w+d-1, d)
				return Cardinality(whole + depth - 1, depth, false);
			}
		}
		/// <summary>
		/// This function gets the kth combination of the whole set of possible combinations
		/// for the elements with depth depth.
		/// You can use this function to calculate the whole set of combinations 
		/// when Cardinality(depth) is less than 1000, otherwise use Get() for better perfomance
		/// </summary>
		/// <param name="k">This belongs to [0, CalcComb(elements.Count,depth))</param>
		/// <param name="depth"></param>
		/// <param name="maxK">This helps performance if the maximum combinations are generated before and set here</param>
		/// <returns></returns>
		public static IEnumerable<T> GetKth<T>(IEnumerable<T> elements,int k, int depth, bool repeatedElements = false, int maxK = 0)
		{
			k++;
			List<T> result = new List<T>();
			if (maxK == 0)
			{
				maxK = Cardinality(elements.Count(), depth, repeatedElements);
			}
			if (k <= 0 || k > maxK)
			{
				return result;
			}
			int previous = 0;
			int acumulated = 0;
			int i = 0;
			while (result.Count() != depth)
			{
				acumulated = previous;
				while (k > acumulated)
				{
					if (result.Count() + 1 == depth)
					{
						int h = k - acumulated;
						i = i + h;
						break;
					}
					previous = acumulated;
					int phase = (repeatedElements) ? 0 : 1;
					acumulated += Cardinality((elements.Count() - i - phase),
						depth - result.Count() - 1, repeatedElements);
					i++;
				}
				result.Add(elements.ElementAt(i - 1));
				if (repeatedElements) i--;
			}
			return result;
		}
		public static IEnumerable<IEnumerable<T>> Get<T>(IEnumerable<T> elements, int depth, bool repeatedElements = false)
		{
			if (repeatedElements)
			{
				return cwithrep(elements, depth);
			}
			else
			{
				return cwithoutrep(elements, depth);
			}
		}

		static IEnumerable<IEnumerable<T>> cwithoutrep<T>(IEnumerable<T> elements, int depth, int aux = 0)
		{
			if (depth <= 0)
				yield return new List<T>();
			else
			{
				for (int i = aux; i < elements.Count(); i++)
				{
					foreach (IEnumerable<T> c in cwithoutrep(elements, depth - 1, i + 1))
					{
						List<T> list = new List<T>();
						list.Add(elements.ElementAt(i));
						list.AddRange(c);
						yield return list;
					}
				}
			}
		}

		static IEnumerable<IEnumerable<T>> cwithrep<T>(IEnumerable<T> elements, int depth, int aux = 0)
		{
			if (depth <= 0)
				yield return new List<T>();
			else
			{
				for (int i = aux; i < elements.Count(); i++)
				{
					foreach (IEnumerable<T> c in cwithrep(elements, depth - 1, i))
					{
						List<T> list = new List<T>();
						list.Add(elements.ElementAt(i));
						list.AddRange(c);
						yield return list;
					}
				}
			}
		}
	}

	public static class Variations
	{
		/// <summary>
		/// Calls "function" for each possible variation
		/// </summary>
		/// <typeparam name="T">Elements type</typeparam>
		/// <typeparam name="TResult">Return type of the function</typeparam>
		/// <param name="elements">Set from where the variations are gotten</param>
		/// <param name="depth">The amount of elements that are taken from elements</param>
		/// <param name="repeatedElements">Set true if the variations allow repeated elements</param>
		/// <param name="function">Function to be called</param>
		/// <returns>Returned values of "function" for each variation</returns>
		public static IEnumerable<TResult> Do<T, TResult>(IEnumerable<T> elements, int depth, bool repeatedElements, Func<IEnumerable<T>, TResult> function)
		{
			IEnumerable<IEnumerable<T>> vari = Get(elements, depth, repeatedElements);
			foreach (IEnumerable<T> item in vari)
			{
				yield return function(item);
			}
		}
		/// <summary>
		/// This function gets the kth combination of the whole set of possible combinations
		/// for the elements with depth depth.
		/// You can use this function to calculate the whole set of combinations 
		/// when Cardinality(depth) is less than 1000, otherwise use Get() for better perfomance
		/// </summary>
		/// <param name="k">This belongs to [0, CalcComb(elements.Count,depth))</param>
		/// <param name="depth"></param>
		/// <param name="maxK">This helps performance if the maximum combinations are generated before and set here</param>
		/// <returns></returns>
		public static IEnumerable<T> GetKth<T>(IEnumerable<T> elements, int k, int depth, bool repeatedElements = false, int maxK = 0)
		{
			k++;
			List<T> result = new List<T>();
			if (maxK == 0)
			{
				maxK = Cardinality(elements.Count(), depth, repeatedElements);
			}
			if (k <= 0 || k > maxK)
			{
				return result;
			}
			int previous = 0;
			int acumulated = 0;
			int i = 0;
			while (result.Count() != depth)
			{
				acumulated = previous;
				while (k > acumulated)
				{
					if (result.Count() + 1 == depth)
					{
						int h = k - acumulated;
						i = i + h;
						break;
					}
					previous = acumulated;
					int phase = (repeatedElements) ? -1 : result.Count();
					acumulated += Cardinality((elements.Count() - 1 - phase),
						depth - result.Count() - 1, repeatedElements);
					i++;
				}
				if (repeatedElements)
				{
					result.Add(elements.ElementAt(i - 1));
					i = 0;
				}
				else
				{
					int selected = 0;
					for (int j = 0; j < i; j++)
					{
						while (result.Contains(elements.ElementAt(selected))) selected++;
						selected++;
					}
					result.Add(elements.ElementAt(selected - 1));
					i = 0;
				}
			}
			return result;
		}
		// Calculates potencies efficently
		static int powEff(int bas, int exp)
		{
			if (exp < 0) return 0;
			if (exp == 0) return 1;
			if(exp == 1)
			{
				return bas;
			}
			else
			{
				int u = powEff(bas, exp / 2);
				int r = u * u;
				if (exp % 2 == 1) r *= bas;
				return r;
			}
		}
		public static int Cardinality(int whole, int depth, bool repeatedElements = false)
		{
			if(repeatedElements)
			{
				return powEff(whole, depth);
			}
			else
			{
				if (depth > whole) return 0;
				int lim = whole - depth;
				for (int i = whole-1; i > lim; i--)
				{
					whole *= i;
				}
				return whole;
			}

		}
		public static IEnumerable<IEnumerable<T>> Get<T>(IEnumerable<T> elements, int depth, bool repeatedElements = false)
		{
			if (repeatedElements)
			{
				return varwithrep(elements, depth);
			}
			else
			{
				return varwithoutrep(elements, depth);
			}
		}
		/// <summary>
		/// Returns a set of variations with repetitions, this is elements.Count^depth elements
		/// </summary>
		/// <param name="elements"></param>
		/// <param name="depth"></param>
		/// <returns></returns>
		static IEnumerable<IEnumerable<T>> varwithrep<T>(IEnumerable<T> elements, int depth)
		{
			if (depth <= 0)
				yield return new List<T>();
			else
			{
				foreach (T i in elements)
					foreach (IEnumerable<T> c in varwithrep(elements, depth - 1))
					{
						List<T> list = new List<T>();
						list.Add(i);
						list.AddRange(c);
						yield return list;
					}
			}
		}
		static IEnumerable<IEnumerable<T>> varwithoutrep<T>(IEnumerable<T> elements, int depth)
		{
			if (depth <= 0)
				yield return new List<T>();
			else
			{
				for (int i = 0; i < elements.Count(); i++)
				{
					foreach (IEnumerable<T> c in varwithoutrep(elements, depth - 1))
					{
						if (c.Contains(elements.ElementAt(i))) continue;
						List<T> list = new List<T>();
						list.Add(elements.ElementAt(i));
						list.AddRange(c);
						yield return list;
					}
				}
			}
		}
	}


	class Program
	{
		// ;'(
		//public List<List<T>> GetSlow(int depth, bool repeatedElements = false)
		//{
		//	if (depth <= 0) return new List<List<T>>();
		//	if (depth > elements.Count)
		//		depth = elements.Count;
		//	List<List<T>> result = new List<List<T>>();
		//	List<T> tempSolution = new List<T>();
		//	// First I organize them normally
		//	List<int> idxCounter = new List<int>();
		//	List<int> limitPosition = new List<int>();
		//	for (int i = 0; i < depth; i++)
		//	{
		//		idxCounter.Add(i);
		//		limitPosition.Add(elements.Count - depth + i);
		//		tempSolution.Add(elements[i]);
		//	}
		//	result.Add(tempSolution);

		//	for (int i = depth - 1; i >= 0; i--)
		//	{
		//		idxCounter[i]++;
		//		for (int j = i + 1; j < depth; j++)
		//		{
		//			idxCounter[j] = idxCounter[j - 1] + 1;
		//		}
		//		// Adds the solution
		//		tempSolution = new List<T>();
		//		for (int j = 0; j < idxCounter.Count; j++)
		//		{
		//			tempSolution.Add(elements[idxCounter[j]]);
		//		}
		//		result.Add(tempSolution);
		//		for (int j = depth - 1; j >= 0; j--)
		//		{
		//			if (limitPosition[j] > idxCounter[j]) { i = j + 1; break; }
		//		}
		//	}
		//	return result;

		//}

		static string WriteElem<T>(IEnumerable<T> comb)
		{
			string outStr = "";
			foreach (var item in comb)
			{
				outStr += "-" + item;
			}
			return outStr;
		}
		static void Main(string[] args)
		{
			List<int> k = new List<int>();
			int depth = 3;
			for (int i = 0; i < 5; i++)
			{
				k.Add(i + 1);
			}
			IEnumerable<string> w = Combination.Do(k, depth, false, WriteElem);
			foreach (string item in w)
			{
				Console.WriteLine(item);
			}
			//int card = Combination.Cardinality(k.Count, depth, false);
			//IEnumerable<IEnumerable<int>> cbrep = Combination<int>.Get(k, depth, true);
			//IEnumerable<IEnumerable<int>> cbnorep = Combination<int>.Get(k, depth, false);

			//IEnumerable<IEnumerable<int>> varirep = Variations<int>.Get(k, depth, true);
			//IEnumerable<IEnumerable<int>> varinorep = Variations<int>.Get(k, depth, false);

			//Console.WriteLine("Comb with rep");
			//foreach (IEnumerable<int> t in cbrep)
			//{
			//	foreach (int v in t)
			//	{
			//		Console.Write(v+",");
			//	}
			//	Console.Write("\n");
			//}
			//Console.WriteLine("elements:"+cbrep.Count());
			//Console.WriteLine("Comb without rep");
			//foreach (IEnumerable<int> t in cbnorep)
			//{
			//	foreach (int v in t)
			//	{
			//		Console.Write(v + ",");
			//	}
			//	Console.Write("\n");
			//}
			//Console.WriteLine("elements:" + cbnorep.Count());
			//Console.WriteLine("Vari with rep");
			//foreach (IEnumerable<int> t in varirep)
			//{
			//	foreach (int v in t)
			//	{
			//		Console.Write(v + ",");
			//	}
			//	Console.Write("\n");
			//}
			//Console.WriteLine("elements:" + varirep.Count());
			//Console.WriteLine("Vari without rep");
			//foreach (IEnumerable<int> t in varinorep)
			//{
			//	foreach (int v in t)
			//	{
			//		Console.Write(v + ",");
			//	}
			//	Console.Write("\n");
			//}
			//Console.WriteLine("elements:" + varinorep.Count());
			//int card2;
			//Console.WriteLine("Vari with rep GETKTH");
			//card = Variations<int>.Cardinality(k.Count, depth, true);
			//card2 = 0;
			//for (int i = 0; i < card; i++)
			//{
			//	foreach (int v in Variations<int>.GetKth(k, i, depth, true, card))
			//	{
			//		Console.Write(v + ",");
			//	}
			//	card2++;
			//	Console.Write("\n");
			//}
			//Console.WriteLine("elements: real:"+card+" maybereal:" + card2);
			//Console.WriteLine("Vari without rep GETKTH");
			//card = Variations<int>.Cardinality(k.Count, depth, false);
			//card2 = 0;
			//for (int i = 0; i < card; i++)
			//{
			//	foreach (int v in Variations<int>.GetKth(k, i, depth, false, card))
			//	{
			//		Console.Write(v + ",");
			//	}
			//	card2++;
			//	Console.Write("\n");
			//}
			//Console.WriteLine("elements: real:" + card + " maybereal:" + card2);


			//TimeSpan stop;
			//TimeSpan start;



			//start = new TimeSpan(DateTime.Now.Ticks);
			//IEnumerable<IEnumerable<int>> cbrep = Combination.Get(k, depth, true);
			//stop = new TimeSpan(DateTime.Now.Ticks);
			//Console.WriteLine("cbrep: "+stop.Subtract(start).TotalMilliseconds+" real: "+cbrep.Count()+" maybereal:"+Combination.Cardinality(k.Count, depth, true));

			//card = Combination.Cardinality(k.Count, depth, true);
			//start = new TimeSpan(DateTime.Now.Ticks);
			//List<IEnumerable<int>> cbrepK = new List<IEnumerable <int>>();
			//for (int i = 0; i < card; i++)
			//{
			//	cbrepK.Add(Combination.GetKth(k, i, depth, true, card));
			//}
			//stop = new TimeSpan(DateTime.Now.Ticks);
			//Console.WriteLine("cbrep KTH: " + stop.Subtract(start).TotalMilliseconds + " real: " + cbrepK.Count() + " maybereal:" + Combination.Cardinality(k.Count, depth, true));



			//start = new TimeSpan(DateTime.Now.Ticks);
			//IEnumerable<IEnumerable<int>> cbnorep = Combination.Get(k, depth, false);
			//stop = new TimeSpan(DateTime.Now.Ticks);
			//Console.WriteLine("cbnorep: " + stop.Subtract(start).TotalMilliseconds + " real: " + cbnorep.Count() + " maybereal:" + Combination.Cardinality(k.Count, depth, false));

			//card = Combination.Cardinality(k.Count, depth, false);
			//start = new TimeSpan(DateTime.Now.Ticks);
			//List<IEnumerable<int>> cbnorepK = new List<IEnumerable<int>>();
			//for (int i = 0; i < card; i++)
			//{
			//	cbnorepK.Add(Combination.GetKth(k, i, depth, false, card));
			//}
			//stop = new TimeSpan(DateTime.Now.Ticks);
			//Console.WriteLine("cbnorep KTH: " + stop.Subtract(start).TotalMilliseconds + " real: " + cbnorepK.Count() + " maybereal:" + Combination.Cardinality(k.Count, depth, false));


			//start = new TimeSpan(DateTime.Now.Ticks);
			//IEnumerable<IEnumerable<int>> varirep = Variations.Get(k, depth, true);
			//stop = new TimeSpan(DateTime.Now.Ticks);
			//Console.WriteLine("varirep: " + stop.Subtract(start).TotalMilliseconds + " real: " + varirep.Count() + " maybereal:" + Variations.Cardinality(k.Count, depth, true));

			//card = Variations.Cardinality(k.Count, depth, true);
			//start = new TimeSpan(DateTime.Now.Ticks);
			//List<IEnumerable<int>> varirepK = new List<IEnumerable<int>>();
			//for (int i = 0; i < card; i++)
			//{
			//	varirepK.Add(Variations.GetKth(k, i, depth, true, card));
			//}
			//stop = new TimeSpan(DateTime.Now.Ticks);
			//Console.WriteLine("varirep KTH: " + stop.Subtract(start).TotalMilliseconds + " real: " + varirepK.Count() + " maybereal:" + Variations.Cardinality(k.Count, depth, true));


			//start = new TimeSpan(DateTime.Now.Ticks);
			//IEnumerable<IEnumerable<int>> varinorep = Variations.Get(k, depth, false);
			//stop = new TimeSpan(DateTime.Now.Ticks);
			//Console.WriteLine("varinorep: " + stop.Subtract(start).TotalMilliseconds + " real: " + varinorep.Count() + " maybereal:" + Variations.Cardinality(k.Count, depth, false));

			//card = Variations.Cardinality(k.Count, depth, false);
			//start = new TimeSpan(DateTime.Now.Ticks);
			//List<IEnumerable<int>> varinorepK = new List<IEnumerable<int>>();
			//for (int i = 0; i < card; i++)
			//{
			//	varinorepK.Add(Variations.GetKth(k, i, depth, false, card));
			//}
			//stop = new TimeSpan(DateTime.Now.Ticks);
			//Console.WriteLine("varinorep KTH: " + stop.Subtract(start).TotalMilliseconds + " real: " + varinorepK.Count() + " maybereal:" + Variations.Cardinality(k.Count, depth, false));

			//foreach (IEnumerable<int> item in t)
			//{
			//	foreach (int a in item)
			//	{
			//		Console.Write(a + ",");
			//	}
			//	Console.Write("\n");
			//}



			//rs = cmb.Get(3);
			//for (int i = 0; i < rs.Count; i++)
			//{
			//	Console.Write((i + 1) + ": ");
			//	for (int j = 0; j < rs[i].Count; j++)
			//	{
			//		Console.Write(rs[i][j]+",");
			//	}
			//	Console.Write("\n");
			//}
			//for (int i = 0; i < rs.Count; i++)
			//{
			//	Console.Write((i + 1) + "!: ");
			//	for (int j = 0; j < rs[i].Count; j++)
			//	{
			//		Console.Write(cmb.GetKth(i,3)[j] + ",");
			//	}
			//	Console.Write("\n");
			//}
		}
	}
}
