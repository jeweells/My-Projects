using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Combination;

namespace Graph
{
	public static class _HeldKarp
	{
		/// <summary>
		///  Finds a tour of N cities in a country (assuming all cities to be visited are reachable), the tour should 
		/// (a) visit every city just once 
		/// (b) return to the starting point
		/// (c) be of minimum distance
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TInfo"></typeparam>
		/// <param name="graph"></param>
		/// <param name="distancefunc">This funcion ought to return the distance from one node to another, by receiving the TInfo of the edge</param>
		/// <returns>First position has the length of the path</returns>
		public static IEnumerable<int> HeldKarp<T,TInfo>(this Graph<T,TInfo> graph, int start_node_id, Func<TInfo, int> distancefunc)
		{
			if (!graph.HasNode(start_node_id)) yield break;
			Dictionary<DynamicMem, Pair> dynamicMem = new Dictionary<DynamicMem, Pair>(new DynamicMemComp());
			HashSet<int> nodesid = new HashSet<int>();

			foreach (Node<T, TInfo> n in graph)
				if (n.Id == start_node_id) continue;
				else nodesid.Add(n.Id);
			for (int k = 0; k < graph.CountNodes; k++)
			{
				//Console.WriteLine($"For k = {k}");
				foreach (Node<T,TInfo> n in graph)
				{
					if (n.Id == start_node_id) continue;
					HashSet<int> tmpids = new HashSet<int>(nodesid);
					tmpids.Remove(n.Id);
					foreach (var item in Combination.Combination.Get(tmpids, k, false))
					{
						//int value =
						G(graph, new DynamicMem(n.Id, new HashSet<int>(item)), start_node_id, dynamicMem, distancefunc);
						//try
						//{
						//	Console.WriteLine($"g({n.Id}, ({string.Join(",", item.ToArray())}) ) = {dynamicMem[new DynamicMem(n.Id, new HashSet<int>(item))].dist}");
						//	Console.WriteLine($"p({n.Id}, ({string.Join(",", item.ToArray())}) ) = {dynamicMem[new DynamicMem(n.Id, new HashSet<int>(item))].chosen_id}");
						//	Console.WriteLine();
						//}
						//catch { }
					}
				}
			}
			Dictionary<int, int> result = new Dictionary<int, int>();
			
			foreach (var item in Combination.Combination.Get(nodesid, graph.CountNodes - 1, false))
			{
				DynamicMem dm = new DynamicMem(start_node_id, nodesid);
				G(graph, dm, start_node_id, dynamicMem, distancefunc);

				Pair tmpPair = dynamicMem[dm];
				yield return tmpPair.dist;
				yield return start_node_id;
				yield return tmpPair.chosen_id;
				HashSet<int> tmpids = new HashSet<int>(nodesid);
				while(tmpids.Count != 0)
				{
					tmpids.Remove(tmpPair.chosen_id);
					tmpPair = dynamicMem[new DynamicMem(tmpPair.chosen_id, tmpids)];
					yield return tmpPair.chosen_id;
				}
				yield break;
			}
		}


		/// G(x,S): Starting from start_node_id, path min cost ends at vertex x, passing vertices in set S exactly once
		static int G<T, TInfo>(Graph<T, TInfo> graph, DynamicMem dym, int start_node_id, Dictionary<DynamicMem, Pair> dynamicMem, Func<TInfo, int> distancefunc)
		{
			if (dynamicMem.ContainsKey(dym))
			{
				return dynamicMem[dym].dist;
			}
			else
			{
				if(dym.S.Count == 0)
				{
					int tmp = Dist(graph, start_node_id, dym.x, distancefunc);
					dynamicMem.Add(dym, new Pair(tmp, start_node_id));
					return tmp;
				}
				else
				{
					int min = int.MaxValue;
					int min_id = -1;
					foreach (int id in dym.S)
					{
						HashSet<int> tmphs = new HashSet<int>(dym.S);
						tmphs.Remove(id);
						int dist = Dist(graph, id, dym.x, distancefunc);
						int tmp = G(graph, new DynamicMem(id, tmphs), start_node_id, dynamicMem, distancefunc);
						if (min > (tmp = dist + tmp))
						{
							min = tmp;
							min_id = id;
						}
					}
					dynamicMem.Add(dym, new Pair(min, min_id));
					return min;
				}
			}
		}

		static int Dist<T,TInfo>(Graph<T, TInfo> graph, int from_id, int to_id, Func<TInfo, int> distancefunc)
		{
			Node<T, TInfo> from = graph[from_id];
			Node<T, TInfo> to = graph[to_id];
			foreach (Edge<T, TInfo> ed in from)
			{
				if (ed.Node.Id == to_id)
				{
					return distancefunc(ed.Info);
				}
			}
			return -1;
		}
		class Pair
		{
			public Pair(int dist, int chosen_id)
			{
				this.dist = dist;
				this.chosen_id = chosen_id;
			}
			public int dist;
			public int chosen_id;
		}

		class DynamicMem
		{
			public DynamicMem(int x, HashSet<int> S)
			{
				this.x = x;
				this.S = S;
			}
			public int x;
			public HashSet<int> S;
		}
		class DynamicMemComp : IEqualityComparer<DynamicMem>
		{
			public bool Equals(DynamicMem x, DynamicMem y)
			{

				if (x == null || y == null || 
					x.x != y.x ||
					!x.S.SetEquals(y.S)) return false;
				return true;
			}

			public int GetHashCode(DynamicMem obj)
			{
				return obj.x.GetHashCode() ^ obj.S.Count.GetHashCode();
			}
		}
	}
}