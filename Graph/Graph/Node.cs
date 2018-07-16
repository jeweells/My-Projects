using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graph
{
	public class Node<T, TInfo> : IEnumerable<Edge<T, TInfo>>
	{
		private int id;
		private T value;
		private List<Edge<T, TInfo>> edges = new List<Edge<T, TInfo>>();
		public Node() { }
		public Node(int id)
		{
			this.id = id;
		}
		public Node(int id, T value)
		{ 
			this.id = id;
			this.value = value;
		}
		public Node(int id, T value, List<Edge<T, TInfo>> edges)
		{
			this.id = id;
			this.value = value;
			this.edges = edges;
		}

		public int Id
		{
			get
			{
				return id;
			}
			set
			{
				id = value;
			}
		}
		public int CountEdges { get { return edges.Count(); } }
		public static implicit operator T(Node<T,TInfo> n) { return n.value; }
		public Edge<T, TInfo> this[int edge_idx]
		{
			get => edges[edge_idx];
		}
		public List<Edge<T,TInfo>> Edges { get; set; }

		public T Value
		{
			get => value;
			set
			{
			}
		}

		public IEnumerator<Edge<T, TInfo>> GetEnumerator()
		{
			return edges.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return edges.GetEnumerator();
		}

	}
}