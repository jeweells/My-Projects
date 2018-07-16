using System;
using System.Diagnostics;
namespace Testing
{
	public static class Test
	{
		public class TimeMeasurer
		{
			public TimeMeasurer() { }
			public TimeMeasurer(bool storereturns) { this.storereturns = storereturns; }
			bool storereturns = false;
			Stopwatch stopWatch = new Stopwatch();
			int timescalled = 0;
			public int CountCalls { get { return timescalled; } }
			object parallelController = new object();
			System.Collections.Hashtable returns = new System.Collections.Hashtable();
			public void PrintReturns()
			{
				if(storereturns)
				{
					Console.WriteLine("This object has returned {0} different results", returns.Count);
					foreach (System.Collections.DictionaryEntry de in returns)
					{
						string s;
						try
						{
							s = ((dynamic)de.Key).ToString();
						}
						catch { s = "{ToString() must be defined in this type}"; }
						Console.WriteLine("\t{0}, was returned {1} times", s, (int)de.Value);
					}
				}
			}
			public TResult AddTimeElapsedIn<TResult>(Func<TResult> proc)
			{
				timescalled++;
				TResult r;
				lock (parallelController)
				{
					stopWatch.Start();
					r = proc();
					stopWatch.Stop();
				}
				if(storereturns)
				{
					if (!returns.Contains(r)) returns.Add(r, 1);
					else returns[r] = (int)returns[r] + 1;
				}
				return r;
			}
			public void AddTimeElapsedIn(Action proc)
			{
				timescalled++;
				lock (parallelController)
				{
					stopWatch.Start();
					proc();
					stopWatch.Stop();
				}
			}
			public long ElapsedMilliseconds
			{
				get { lock (parallelController) return stopWatch.ElapsedMilliseconds; }
			}
			public long ElapsedTicks
			{
				get { lock (parallelController) return stopWatch.ElapsedTicks; }
			}
		}
		/// <summary>
		/// Returns the time passed after completing an function
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="proc"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		public static TimeSpan MeasureTime<TResult>(Func<TResult> proc, out TResult result)
		{
			DateTime start = DateTime.Now;
			result = proc();
			DateTime stop = DateTime.Now;
			return stop.Subtract(start);
		}
	}
}

