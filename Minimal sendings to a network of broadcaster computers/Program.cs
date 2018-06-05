// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Homework 4
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Date: April 29, 2018
// :: Author: Abraham José Pacheco Becerra
// :: E-Mail: abraham.pacheco6319@gmail.com
// :: Description: Finds the minimum number of computers from which must be sent a
// :: message to reach the N computers of the network.
// :: Knowing that when a computer receives a message, this one immediately sends 
// :: the message to all the computers that is connected.
// :: Each connections is unidirectional.
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Compilation
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: It has been successfully proved in Visual Studio Community 2017 15.2
// :: Also https://repl.it/@jeweells/TAPHomeWork4
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Input
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: First line, a number N which defines the number of computers (1 <= N <= 50)
// :: after that follows N lines and each line must have N characters.
// :: These characters can only be 'S' or 'N'
// :: 'S': There's a connection from the computer (line i) to the computer (char j)
// :: 'N': There are no connections going out from the computer (line i) to the computer
// :: (char j)
// :: Example:
// :: 2
// :: NS
// :: NN
// :: Output: 1
// :: There's a connection from the computer 1 to 2, the message is sent to the computer
// :: 1 and the computer 1 sends a message to the computer 2, thus only 1 message is
// :: needed
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Explanation
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: This is as simple as getting the number of subgraph a graph has
// :: Getting a subgraph of the node i, visits and marks as visited each node
// :: that is connected to another node (this connection is bidirectional even though
// :: the computers are connected unidirectionally) until there are no nodes left 
// :: to visit. Each time a node is visited, this node is stored in a list.
// :: The result of this list is the subgraph that contains the node i.
// :: To get all the subgraphs of the graph, we start having the whole set A of nodes
// :: (1) Then we get a subgraph B that contains the first node of A. Now we eliminate 
// :: the nodes from A that are in B and replace A with them. (A = A - B).
// :: (1) will be repeated N times until A is empty, as a result N will be the minimal
// :: number of messages
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tarea4
{
	class Program
	{
		public static class Combination
		{
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
		class Graph<NodeValueType>
		{
			public class Node
			{
				List<Node> from;
				List<Node> to;
				NodeValueType value;
				List<Thread> visited;
				public Node(List<Node> from, List<Node> to, NodeValueType value)
				{
					From = from;
					To = to;
					Value = value;
					visited = new List<Thread>();
				}

				public NodeValueType Value { get { return value; } set { this.value = value; } }
				public List<Thread> Visited { get { return visited; } set { visited = value; } }

				public List<Node> From { get { return from; } set { from = value; } }
				public List<Node> To { get { return to; } set { to = value; } }
			}
			Node start;
			int countNodes;
			int countEdges;

			public Node Start { get { return start; } set { start = value; } }
			public int CountNodes { get { return countNodes; } set { countNodes = value; } }
			public int CountEdges { get { return countEdges; } set { countEdges = value; } }
		}
		class GraphT4 : Graph<int>
		{
			public static char CONNECTED = 'S';
			public static char DISCONNECTED = 'N';
			List<Node> nodes;
			public GraphT4(string[] matr, int n)
			{
				nodes = new List<Graph<int>.Node>();
				for (int i = 0; i < n; i++) // Creates all the nodes
				{
					nodes.Add(new Node(new List<Graph<int>.Node>(), new List<Graph<int>.Node>(), i));
				}
				Start = nodes[0];
				for (int i = 0; i < n; i++)
				{
					for (int j = 0; j < n; j++)
					{
						if (matr[i][j] == CONNECTED) // if it's connected, adds a connection
						{
							nodes[j].From.Add(nodes[i]);
							nodes[i].To.Add(nodes[j]);
						}
					}
				}
			}
			/// <summary>
			/// Returns the subgraph where elm belongs.
			/// It's important to know that Visited variable from Node will be changed
			/// </summary>
			/// <param name="elm">Node that belongs to the subgraph</param>
			/// <returns>List of nodes that form the subgraph where elm is</returns>			
			public IEnumerable<Node> SubGraph(Node elm)
			{
				if (elm != null && !elm.Visited.Contains(Thread.CurrentThread))
				{
					yield return elm;
					elm.Visited.Add(Thread.CurrentThread);
					foreach (Node item in elm.To.Union(elm.From))
					{
						foreach (Node it in SubGraph(item))
						{
							yield return it;
						}
					}
				}
			}

			// if especific is defined, performance might improve
			void ResetVisited(IEnumerable<Node> especific=null)
			{
				if(especific == null)
				{
					for (int i = 0; i < nodes.Count; i++)
					{
						nodes[i].Visited = new List<Thread>();
					}
				}
				else
				{
					for (int i = 0; i < especific.Count(); i++)
					{
						especific.ElementAt(i).Visited = new List<Thread>();
					}
				}
				
			}

			public int GetNumSubgraphs()
			{
				int counter = 0;
				IEnumerable<Node> nd = nodes;
				while(nd.Count() > 0)
				{
					nd = nd.Except(SubGraph(nd.ElementAt(0))).ToList();
					counter++;
				}
				return counter;
			}

			// This tries with all possible combinations (too slow, but it works)
			public int FillThePipe()
			{
				// Returns the minimal amount of messages that needs to be
				// sent until all the computers receives the message
				for (int i = 0; i < nodes.Count; i++)
				{ // Starts sending one message, if it doesn't send the message
				  // to all the computers, then send two messages
					if (canFill(i+1)) return i+1;
				}
				return 0;
			}
			bool canFill(int i)
			{ // i represents the number of messages to be sent
				if (i <= 0) return false;
				// Gets the combinations 
				IEnumerable<IEnumerable<Node>> comb = Combination.Get(nodes, i, false);
				// Prepares for parallel checking
				CancellationTokenSource cts = new CancellationTokenSource();
				ParallelOptions po = new ParallelOptions();
				po.CancellationToken = cts.Token;
				po.MaxDegreeOfParallelism = Environment.ProcessorCount;
#if DEBUG
				Console.WriteLine("Creando " + Environment.ProcessorCount + " threads");
#endif
				bool retvalue = false;
				try
				{
					Parallel.For(0, comb.Count(), po, () => false, (j, a, b) =>
					{
#if DEBUG
						string combstr = "";
						for (int k = 0; k < comb.ElementAt(j).Count(); k++)
						{
							combstr += comb.ElementAt(j).ElementAt(k).Value+ ",";
						}
						Console.WriteLine("Thread(" + Thread.CurrentThread.ManagedThreadId + ") probando combinacion ("+ combstr + ")");
#endif
						if (doesItFill(comb.ElementAt(j))) { retvalue = true; return true; }
						else return false;
					}, (x) => { if (retvalue) cts.Cancel(); });
				}
				catch
				{
#if DEBUG
					Console.WriteLine("For cancelled. Solution found.");
#endif
					// Cancelled
				}
				
				return retvalue;
			}
			bool doesItFill(IEnumerable<Node> comb)
			{
#if DEBUG
				Console.Write("Escribiendo la combinacion: ");
				foreach (Node n in comb)
				{
					Console.Write(n.Value + ",");
				}
				Console.Write("\n");
#endif
				int totalVisited = 0;
				if (totalVisited == nodes.Count) return true;
				IEnumerable<Node> visited = new List<Node>();
				for (int i = 0; i < comb.Count(); i++)
				{
#if DEBUG
					Console.WriteLine("Visitando todos desde " + comb.ElementAt(i).Value);
#endif
					visited = visited.Concat(VisitAllFrom(comb.ElementAt(i)));
#if DEBUG
					Console.WriteLine("Total visitados: " + visited.Count());
#endif
					if (visited.Count() == nodes.Count)
					{ // This only will happen at the last iteration
						// Due to it's protected with the function that calls this
						// Anyway in case it's called from another funcion, there this is
						ResetVisited(visited);
						return true;
					}
				}
#if DEBUG
				Console.WriteLine(visited.Count()+" elementos visitados.");
#endif
				ResetVisited(visited);
				return false;
			}
			IEnumerable<Node> VisitAllFrom(Node startPoint, List<Node> visited = null) // Returns the number of computers that were visited
			{
				Node t = startPoint;
				if (visited == null) visited = new List<Node>();
				if (!t.Visited.Contains(Thread.CurrentThread))
				{
					t.Visited.Add(Thread.CurrentThread);
					visited.Add(t);
				}
				else { return visited; }
				// If all the nodes were visited, the process is done
				if (visited.Count == nodes.Count) return visited;
				for (int i = 0; i < t.To.Count; i++)
				{
					if (visited.Count == nodes.Count) return visited;
					if (t.To[i].Visited.Contains(Thread.CurrentThread)) continue;
					VisitAllFrom(t.To[i], visited);
				}
				return visited;
			}
		}

		static void Main(string[] args)
		{
			nAgain:
			int n = Int32.Parse(Console.ReadLine());
			if(n < 1 || n > 50)
			{
				Console.WriteLine("Error al introducir N (1 <= N <= 50)");
				goto nAgain;
			}
			string[] matr = new string[n];
			for (int i = 0; i < n; i++)
			{
				string row = Console.ReadLine();
				if(row.Length != n)
				{
					Console.WriteLine("Error al escribir la matriz.");
					return;
				}
				for (int j = 0; j < n; j++)
				{
					if(row[j] != GraphT4.CONNECTED &&
						row[j] != GraphT4.DISCONNECTED)
					{
						Console.WriteLine("Error al escribir la matriz.");
						return;
					}
				}
				matr[i] = row;
			}
			GraphT4 t4 = new GraphT4(matr, n);
#if DEBUG
			TimeSpan start = new TimeSpan(DateTime.Now.Ticks);
			TimeSpan stop;

#endif
			Console.WriteLine(t4.GetNumSubgraphs());
#if DEBUG
			stop = new TimeSpan(DateTime.Now.Ticks);
			Console.WriteLine("Finished in " + (int) stop.Subtract(start).TotalMilliseconds + " ms");
#endif
		}
	}
}






