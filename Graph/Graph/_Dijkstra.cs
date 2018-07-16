using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graph
{
	public static class _Dijkstra
	{
		/// <summary>
		/// Returns the minimal distance from a node to any node
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TInfo"></typeparam>
		/// <param name="g"></param>
		/// <param name="node_from_id">Id of the node where this algorithm starts</param>
		/// <param name="edgedistance">This funcion will receive an edge and ought to return the distance that belongs to that edge</param>
		/// <returns>A hashtable where the keys are the node's id and the values are the distances</returns>
		public static Dictionary<int, int> Dijkstra<T, TInfo>(this Graph<T, TInfo> g, int node_from_id, Func<Edge<T, TInfo>, int> edgedistance)
		{
			Dictionary<int, int> distance = new Dictionary<int, int>();
			HashSet<Node<T, TInfo>> seen = new HashSet<Node<T, TInfo>>();
			foreach (Node<T, TInfo> item in g)
			{
				distance.Add(item.Id, int.MaxValue);
			}
			distance[node_from_id] = 0;
			int n_seen = 1;
			while (n_seen != g.CountNodes)
			{
				Node<T, TInfo> vertex = null;
				foreach (Node<T, TInfo> item in g)
				{
					if (vertex == null)
					{
						if (!seen.Contains(item)) vertex = item;
					}
					else if (distance[item.Id] < distance[vertex.Id] && !seen.Contains(item)) vertex = item;
				}
				if (vertex == null) break;
				seen.Add(vertex);
				foreach (Edge<T, TInfo> edge in vertex)
				{
					if (edgedistance(edge) < 0) continue;
					int nd = edgedistance(edge) + distance[vertex.Id];
					if (distance[edge.Node.Id] > nd)
					{
						distance[edge.Node.Id] = nd;
					}
				}
			}
			return distance;
		}
	}
}