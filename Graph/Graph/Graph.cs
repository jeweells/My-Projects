using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graph
{
	public class Graph<T, TInfo> : IGraph<T, TInfo>, IEnumerable<Node<T, TInfo>>
	{
		protected Dictionary<int, Node<T, TInfo>> nodes = new Dictionary<int, Node<T, TInfo>>();
		protected Dictionary<int, List<Edge<T, TInfo>>> edges = new Dictionary<int, List<Edge<T, TInfo>>>();
		protected int next_node_id = 0;
		protected int num_edges = 0;
		public enum Mode { DIRECTED, UNDIRECTED }
		public Mode mode = Mode.DIRECTED;
		public Node<T, TInfo> this[int id]
		{
			get
			{
				if (nodes.ContainsKey(id)) return nodes[id];
				else return null;
			}
		}
		public Node<T, TInfo> this[T value]
		{
			get
			{
				return Find(value);
			}
		}

		public Node<T, TInfo> AddNode(T value)
		{
			List<Edge<T, TInfo>> edges = new List<Edge<T, TInfo>>();
			this.edges.Add(next_node_id, edges);
			Node<T, TInfo> n = new Node<T, TInfo>(next_node_id, value, edges);
			nodes.Add(next_node_id++, n);
			return n;
		}

		public int CountEdges
		{
			get { return num_edges; }
		}

		public int CountNodes
		{
			get { return nodes.Count; }
		}

		public Node<T, TInfo> Find(T value)
		{
			foreach (Node<T, TInfo> item in this)
			{
				if (item.Value.Equals(value)) return item;
			}
			return null;
		}

		public bool IsEmpty()
		{
			return nodes.Count == 0;
		}

		public IEnumerator<Node<T, TInfo>> GetEnumerator()
		{
			return new GraphIterator(nodes);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new GraphIterator(nodes);
		}

		public bool HasEdge(Node<T, TInfo> node_from, Node<T, TInfo> node_to)
		{
			if (!edges.ContainsKey(node_from.Id) || 
				edges[node_from.Id].First(x => x.Node == node_to) == null)
				return false;
			else
				return true;
		}

		public bool HasNode(int node_id)
		{
			return nodes.ContainsKey(node_id);
		}

		public bool AddEdge(Node<T, TInfo> node_from, Node<T, TInfo> node_to, TInfo info = default(TInfo))
		{
			if (node_from == null || 
				node_to == null ||
				!nodes.ContainsKey(node_from.Id) ||
				!nodes.ContainsKey(node_to.Id)) return false;
			Edge<T, TInfo> edge = new Edge<T, TInfo>() { Info = info, Node = node_to, NodeFromId = node_from.Id };
			edges[node_from.Id].Add(edge);

			if (mode == Mode.UNDIRECTED)
			{
				edge = new Edge<T, TInfo>() { Info = info, Node = node_from, NodeFromId = node_to.Id };
				edges[node_to.Id].Add(edge);
			}
			num_edges++;
			return true;
		}

		public Node<T, TInfo> GetNode(int node_id)
		{
			return this[node_id];
		}

		public Node<T, TInfo> GetNodeByIdx(int node_idx)
		{
			return nodes.ElementAt(node_idx).Value;
		}

		public void PrintGraph(Func<T, string> nodesStr, Func<TInfo, string> edgeStr)
		{
			foreach (Node<T,TInfo> item in this)
			{
				Console.WriteLine("Node("+nodesStr(item.Value) + ") => ");
				foreach (Edge<T,TInfo> edges in item)
				{
					Console.WriteLine("\tEdge(" + edgeStr(edges.Info) + ") => Node("+ nodesStr(edges.Node.Value) + ")");
				}
			}
			Console.WriteLine();
			Console.WriteLine("Number of nodes: " + CountNodes);
			Console.WriteLine("Number of edges: " + CountEdges);
		}


		public class GraphIterator : IEnumerator<Node<T, TInfo>>
		{
			IDictionaryEnumerator enumerator;
			protected Dictionary<int, Node<T, TInfo>> nodes;
			public GraphIterator(Dictionary<int, Node<T, TInfo>> nodes)
			{
				enumerator = nodes.GetEnumerator();
				this.nodes = nodes;
			}
			Node<T, TInfo> IEnumerator<Node<T, TInfo>>.Current => (Node <T,TInfo>) enumerator.Value;

			object IEnumerator.Current => enumerator.Value;

			public void Dispose()
			{
				return;
			}

			public bool MoveNext()
			{
				return enumerator.MoveNext();
			}

			public void Reset()
			{
				enumerator.Reset();
			}
		}

	}
}