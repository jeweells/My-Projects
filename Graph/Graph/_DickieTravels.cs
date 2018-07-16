// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Homework 7
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Date: July 05, 2018
// :: Author: Abraham José Pacheco Becerra
// :: E-Mail: abraham.pacheco6319@gmail.com
// :: Description: Finds the number of ways of visiting all the cities and returning to the start city taking no more 
// :: than a T time and spending the least amount of money possible (TSP modified)
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Compilation
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: It has been successfully proved in Visual Studio Community 2017 15.7.3
// :: Also https://repl.it/@jeweells/Homework7
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Input
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: The first line will have N and M (2 <= N <= 10, N <= M <= 100) where N is the number of airports including the
// :: local one. Then M lines describing the nmber of air routes that connect two airports A and B, followed by the
// :: number of conexions with a cost C and a flight time t (0 <= C <= 10^5 and 0 <= t <= 30) and finally T, (1<=T<=60)
// :: describing the maximum flight time accepted.
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Output
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Minimal cost of the path that visits all the cities once and returns to the start point taking no more than T 
// :: time
// :: e.g:
// :: Input:
// :: 3 3
// :: 0 2 1 3
// :: 0 1 2 3
// :: 2 1 1 3
// :: 5
// :: Output:
// :: Impossible
// :: 
// :: Input:
// :: 3 3
// :: 0 2 1 3
// :: 0 1 2 3
// :: 2 1 1 3
// :: 10
// ::
// :: Output: 
// :: 4 2
// :: 
// :: Input:
// :: 4 8
// :: 0 2 2 1
// :: 0 2 1 10
// :: 0 3 1 10
// :: 0 1 2 1
// :: 2 3 1 1
// :: 2 1 1 6
// :: 2 1 6 1
// :: 3 1 10 1
// :: 10
// :: 
// :: Output:
// :: 15 2
// :: 
// :: Input:
// :: 4 8
// :: 0 2 2 1
// :: 0 2 1 10
// :: 0 3 1 10
// :: 0 1 2 1
// :: 2 3 1 1
// :: 2 1 1 6
// :: 2 1 6 1
// :: 3 1 10 1
// :: 18
// :: 
// :: Output:
// :: 5 2
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Explanation
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: The simple idea is basically:
// :: Building an undirected graph acording to the input, say 0,1,2,3 as nodes
// :: We take 0 as the startup point and store in a list the paths that goes from 0 to any other node
// :: they would be: 01, 02, 03
// :: For each node stored we do the same, for example for 01 we go to 2 and 3, resulting into 012, 013
// :: If we do this with the rest of the nodes we will have stored:
// :: 012 013
// :: 021 023
// :: 031 032
// :: Now we can notice the length of the path has reached its maximum so what's left is to go to the startup point
// :: resulting into:
// :: 0120 0130
// :: 0210 0230
// :: 0310 0320
// :: What's left is to get the paths that have the least cost and accomplish the constraint of the max time
// :: 
// :: In order to apply dynamic programming, reducing the amount of memory this takes and the speed
// :: First we divide the list in two parts
// :: The first part will have the pending paths which have length n-1 (be n the current length of a path in a moment n)
// :: and the last part of the list will have paths with length n
// :: Once we take a path of length n-1 to get paths of length n from it, we delete that path of length n-1 from the
// :: list. For this a linked list do the thing.
// :: 
// :: A hash set is implemented too which will store the same paths of length n, this part is important:
// :: We manage how the paths will be inserted into the set, if the path is in the set (explained below) then we don't
// :: insert the path onto the list either
// :: 
// :: To see if a path already on the hash set we need to know if the path has an inner permutation, this means
// :: both start at the same node, both finish at the same node and the rest of the nodes are the same (but not the same order)
// :: This can be done by filling a bool array which each index represents the node and the value is true if it's
// :: on the path, false otherwise. If both array are the same, they have an inner permutation
// :: With this we need to see if the path costs the same, costs more, less, has more time, less time, etc.
// :: Depending on that, the path in the set can be modified (there're more ways to get this path), replaced (a better
// :: path than this in an inner permutation), also the new path can be ignored, or added in such case (e.g: costs more
// :: but has less time)
// :: An example of inner permutation would be 0123 and 0213, they reach the same node starting from the same node
// :: and one is either better, worse or an alternative way
// :: 
// :: 
// :: 
// :: 
// :: 
// :: 
// :: 
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Fact
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Since the all edges are bidirectional, the number of ways will always be even. The half of the paths will be
// :: the way back of the other half
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::


using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph
{
	public static class _DickieTravels
	{
		#region Auxiliary
		/// <summary>
		/// Implement this interface in your edge type if you want to get DickieTravels result
		/// </summary>
		public interface IDickieEdgeInfo
		{
			int Cost { get; set; }
			int Time { get; set; }
		}
		/// <summary>
		/// Result given by DickieTravels function
		/// </summary>
		public class DickieInfo<NodeType, EdgeType> where EdgeType : IDickieEdgeInfo
		{
			public List<Path<NodeType, EdgeType>> Paths;
			public int MinCost;
			public DickieInfo(List<Path<NodeType, EdgeType>> paths, int minCost)
			{
				this.Paths = paths;
				this.MinCost = minCost;
			}
		}
		/// <summary>
		/// Use this class in case you're creating a new graph as an edge type to get results of dickie travels
		/// </summary>
		public class DickieEdgeInfo : IDickieEdgeInfo
		{
			int cost;
			int time;

			public DickieEdgeInfo(int cost, int time)
			{
				this.cost = cost;
				this.time = time;
			}

			public int Cost { get { return cost; } set { cost = value; } }
			public int Time { get { return time; } set { time = value; } }
		}
		/// <summary>
		/// Represents a sequence of nodes connected
		/// </summary>
		/// <typeparam name="NodeType"></typeparam>
		public class Path<NodeType, EdgeType> where EdgeType : IDickieEdgeInfo
		{
			/// <summary>
			/// Number of ways of reaching this path
			/// </summary>
			public int totalways = 1;
			/// <summary>
			/// This variable doesn't provide any useful value for the last output, it's used for internal purposes
			/// </summary>
			public int lastway = 1;
			public int cost = 0;
			public int time = 0;
			/// <summary>
			/// Take into account that the position 0 of an encapsulated sequence is the end point of a sequence
			/// </summary>
			public EncapsulatedSequence<Node<NodeType, EdgeType>> path;
			public bool[] boolpath = null;
			/// <summary>
			/// The hash code is based on the bool path
			/// </summary>
			public int Hashcode;

			public Path(int cost, int time, EncapsulatedSequence<Node<NodeType, EdgeType>> path, bool[] boolpath, int hashcode)
			{
				this.cost = cost;
				this.time = time;
				this.path = path;
				this.Hashcode = hashcode;
				this.boolpath = boolpath;
			}

			public Path(int cost, int time, EncapsulatedSequence<Node<NodeType, EdgeType>> path)
			{
				this.cost = cost;
				this.time = time;
				this.path = path;
			}
			/// <summary>
			/// Updates the hashcode with the boolpath passed
			/// </summary>
			/// <param name="path"></param>
			public void SetBoolPath(bool[] path, int[,] codes)
			{
				boolpath = path;
				Hashcode = 0;
				for (int i = 0, endi = boolpath.Length; i < endi; i++)
				{
					if (boolpath[i]) Hashcode ^= codes[i, 1];
					else Hashcode ^= codes[i, 0];
				}
			}
		}
		/// <summary>
		/// Giveng a path returns an array of length newids.Count, where each node ID is mapped with newids and then
		/// is accessed to the array to mark it True (This means the node is in the path)
		/// </summary>
		/// <param name="path">Path used to get the bool path</param>
		/// <param name="newids">Keys represent the each node id and the value should be in [0, newids.Count)</param>
		static bool[] BoolPath<NodeType, EdgeType>(Path<NodeType, EdgeType> path, Dictionary<int, int> newids) where EdgeType : IDickieEdgeInfo
		{
			bool[] tmp = new bool[newids.Count];
			foreach (var item in path.path)
				tmp[newids[item.Id]] = true;
			return tmp;
		}

		/// <summary>
		/// Used to discard path permutations and modify the ones already stored
		/// </summary>
		private class PathComparer<NodeType, EdgeType> : IEqualityComparer<Path<NodeType, EdgeType>> where EdgeType : IDickieEdgeInfo
		{
			public bool Equals(Path<NodeType, EdgeType> item, Path<NodeType, EdgeType> np)
			{
				if (item.path.Count != np.path.Count) return false;
				if (item.path[0].Id != np.path[0].Id) return false;
				for (int i = 0, endi = item.boolpath.Length; i < endi; i++)
				{
					if (item.boolpath[i] != np.boolpath[i]) return false;
				}
				bool dontadd = false;
				if (np.time == item.time)
				{
					dontadd = true;
					if (np.cost == item.cost)
					{
						item.lastway++; // More ways of reaching this path
										//Console.WriteLine("More ways");
					}
					else
					{
						if (np.cost < item.cost)
						{
							item.cost = np.cost;
							item.totalways = np.totalways;
							item.path = np.path;
							item.lastway = np.lastway;
							//Console.WriteLine("Replaced");
						}
					}
				}
				else if (np.time > item.time)
				{
					if (np.cost > item.cost)
					{
						dontadd = true;
						//Console.WriteLine("Ignore");
					}
				}
				else // np.time < item.time
				{
					if (np.cost < item.cost)
					{
						dontadd = true;
						item.cost = np.cost;
						item.totalways = np.totalways;
						item.path = np.path;
						item.lastway = np.lastway;
						item.time = np.time;
						//Console.WriteLine("Replace");
					}
				}
				return dontadd;// Returning true, means don't add that element which is the same that saying the element exists ( in a set )
			}

			public int GetHashCode(Path<NodeType, EdgeType> obj)
			{
				return obj.Hashcode;// Personalized hashcode
			}
		}
		#endregion

		/// <summary>
		/// Finds the number of ways of visiting all the cities and returning to the start city taking no more 
		/// than a T time and spending the least amount of money possible (TSP modified)
		/// </summary>
		/// <typeparam name="EdgeType">The edge type must implement IDickieEdgeInfo interface in order to use this function</typeparam>
		/// <param name="G"></param>
		/// <param name="startNode">Name of the node where the path starts and finishes</param>
		/// <param name="maxTime">Maximum amount of time allowed to accomplish visit all the cities</param>
		/// <returns>Null if no path was found, information about the result otherwise</returns>
		public static DickieInfo<NodeType, EdgeType> DickieTravels<NodeType, EdgeType>(this Graph<NodeType, EdgeType> G, NodeType startNode, int maxTime)
			where EdgeType : IDickieEdgeInfo
		{
			Node<NodeType, EdgeType> start = G[value: startNode]; // Start and End node
			Dictionary<int, int> newids = new Dictionary<int, int>(); // Ids mapped to [0, G.CountNodes)
			Random r = new Random();
			int[,] codes = new int[G.CountNodes, 2]; // Each path will have a hashcode
			int nextid = 1; // Since the startNode will always have our mapped id as 0, we start from 1 after setting our start node
			newids.Add(start.Id, 0);
			foreach (var node in G)
			{
				codes[nextid, 0] = r.Next(); // Represets a node that doesn't have been visited
				codes[nextid, 1] = r.Next(); // Represets a node that has been visited
				if (node.Id == start.Id) continue;
				newids.Add(node.Id, nextid++);
			}
			LinkedList<Path<NodeType, EdgeType>> paths = new LinkedList<Path<NodeType, EdgeType>>();//Using a linked list improves the performance but increases the memory usage
			paths.AddLast(new Path<NodeType, EdgeType>(0, 0, new EncapsulatedSequence<Node<NodeType, EdgeType>>(start)));
			// What is an EncapsulatedSecuence ? it's basically a queue that can't be popped out
			// Also, the previous element is another EncapsulatedSequence, this helps to reduce memory usage
			// since all paths starts from 0, 0 wont' be repeated for each path
			paths.Last.Value.SetBoolPath(BoolPath(paths.Last.Value, newids), codes); // Using a bool array to represent the positions that are already visited
			int currentElms;
			DickieInfo<NodeType, EdgeType> di = new DickieInfo<NodeType, EdgeType>(new List<Path<NodeType, EdgeType>>(), int.MaxValue); ; // This object will have information about the paths found
			while ((currentElms = paths.Count) != 0 && paths.First.Value.path.Count <= G.CountNodes)
			{
				HashSet<Path<NodeType, EdgeType>> nopermutationpaths = new HashSet<Path<NodeType, EdgeType>>(new PathComparer<NodeType, EdgeType>());
				// Might sound repeatitive but this hashset, even if it consumes memory since the paths are stored
				// in the linked list, accessing to each path is way too fast comparing to using only the linked list
				// The pathcomparer will take charge of inserting new paths, ignoring them or modifying the paths already on the hashset
				// (Increasing the number of ways, or giving a better path than the one stored)
				for (int i = 0; i < currentElms; i++)
				{
					Path<NodeType, EdgeType> tmppath = paths.First(); // This linked list is divided in two parts
																	  // from 0 to currentElms will be stored the paths of length n-1
																	  // the rest of the list will have paths of length n
					paths.RemoveFirst();
					tmppath.totalways *= tmppath.lastway;// Reaches the same path with this amount of ways (permutating inner nodes)
					tmppath.lastway = 1;
					foreach (var edge in tmppath.path[0])
					{
						bool finalpath = (tmppath.path.Count == G.CountNodes) ? true : false;
						int newNodeMappedId = newids[edge.Node.Id];
						if (finalpath)
						{
							if (edge.Node.Id != start.Id) continue; // Only going to the same start point is required
						}
						else if (tmppath.boolpath[newNodeMappedId]) continue; // It has already visited and visited only once is required
						int ntime = tmppath.time + edge.Info.Time;
						if (ntime > maxTime) continue;
						int ncost = tmppath.cost + edge.Info.Cost;
						if (finalpath)
							if (ncost <= di.MinCost) di.MinCost = ncost; // Not wasting time on larger paths
							else continue;
						Path<NodeType, EdgeType> np = new Path<NodeType, EdgeType>(ncost, ntime, new EncapsulatedSequence<Node<NodeType, EdgeType>>(tmppath.path, edge.Node), tmppath.boolpath.ToArray(), tmppath.Hashcode);
						if (!finalpath)
						{
							np.boolpath[newNodeMappedId] = true;
							np.Hashcode ^= codes[newNodeMappedId, 0] ^ codes[newNodeMappedId, 1]; // Modifying the hashcode according to the boolpath
						}

						if (nopermutationpaths.Add(np)) // If the path was added to the set, we add it to the linked list
						{
							paths.AddLast(np);

						}
					}
				}
			}
			if (paths.Count == 0)
				return null; // No paths found
			while ((currentElms = paths.Count) != 0) // Only going to the last node left
			{
				Path<NodeType, EdgeType> tmppath = paths.First(); // Popping out the last paths
				paths.RemoveFirst();
				if (di.MinCost == tmppath.cost) // Discarding paths that are larger than the minimal
				{
					tmppath.totalways *= tmppath.lastway; // Reaches the same path with this amount of ways (permutating inner nodes)
					tmppath.lastway = 1;
					di.Paths.Add(tmppath); // Adding the more paths
				}
			}
			return di;
		}



		/// <summary>
		/// Homework 7 implementation
		/// </summary>
		public static void Main()
		{
			insertNnM:
			string[] firstline = Console.ReadLine().Split(new[] { ' ' }, 2);
			int n, // Number of airports 
				m; // Numer of routes
			if (!int.TryParse(firstline[0], out n) || n < 2 || n > 10 ||
				!int.TryParse(firstline[1], out m) || m < n || m > 100)
			{
				Console.WriteLine("Error al insertar N y M:");
				Console.WriteLine("\tRecuerde introducir sólo números separados por un espacio en la forma: N M");
				Console.WriteLine("\t\tN: Número de aeropuertos distintos. 2 <= N <= 10");
				Console.WriteLine("\t\tM: Número de conexiones entre aeropuertos. N <= M <= 100\n");
				Console.WriteLine("Inserte de nuevo N y M:");
				goto insertNnM;
			}
			Graph<string, DickieEdgeInfo> graph = new Graph<string, DickieEdgeInfo>();
			graph.mode = Graph<string, DickieEdgeInfo>.Mode.UNDIRECTED;
			Node<string, DickieEdgeInfo>[] nodes = new Node<string, DickieEdgeInfo>[n];
			for (int i = 0; i < n; i++)
			{
				nodes[i] = graph.AddNode(i.ToString());
			}
			for (int i = 0; i < m; i++)
			{
				routeAgain:
				string[] mline = Console.ReadLine().Split(new[] { ' ' }, 4);
				try
				{
					int C = int.Parse(mline[2]);
					int t = int.Parse(mline[3]);
					if (C < 0 || C > 100000 || t < 0 || t > 30 ||
						!graph.AddEdge(graph[mline[0]], graph[mline[1]], new DickieEdgeInfo(C, t)))
						throw new FormatException();
				}
				catch (FormatException)
				{
					Console.WriteLine("Error al describir las rutas de los aeropuertos");
					Console.WriteLine("\tRecuerde escribirlas de la forma: A B C T");
					Console.WriteLine("\t\tA: Aeropuerto que va hacia B");
					Console.WriteLine("\t\tB: Aeropuerto que va hacia A");
					Console.WriteLine("\t\tC: Costo de viajar entre A y B con esta aerolínea. 0 <= C <= 10^5");
					Console.WriteLine("\t\tt: Tiempo de viajar entre A y B con esta aerolínea. 0 <= t <= 30");
					Console.WriteLine("\tLos aeropuertos están enumerados de 0 a N-1");
					Console.WriteLine("Inserte de nuevo la ruta:");
					goto routeAgain;
				}
			}
			tAgain:
			int Tmax;
			if (!int.TryParse(Console.ReadLine(), out Tmax) || Tmax < 1 || Tmax > 60)
			{
				Console.WriteLine("Error al insertar el tiempo máximo:");
				Console.WriteLine("\t1 <= T <= 60");
				Console.WriteLine("Inserte de nuevo el tiempo máximo:");
				goto tAgain;
			}
			DickieInfo<string, DickieEdgeInfo> di = graph.DickieTravels("0", Tmax);
			int numWays = 0;

			if (di == null) Console.WriteLine("Imposible");
			else
			{
				foreach (var p in di.Paths) numWays += (p.totalways);
				Console.WriteLine($"{di.MinCost} {numWays}");
				// Prints the path with less cost, less or equal than the max time and the number of ways of doing it with that cost
			}
		}
	}
}
