using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graph
{
	public static class _BFS
	{
		public static IEnumerable<Node<T, TInfo>> BFS<T, TInfo>(this Graph<T, TInfo> g, int start_node_idx)
		{
			Node<T, TInfo> start = g[start_node_idx];
			HashSet<Node<T, TInfo>> visited = new HashSet<Node<T, TInfo>>();
			Queue<Node<T, TInfo>> pending = new Queue<Node<T, TInfo>>();
			pending.Enqueue(start);
			while (pending.Count != 0)
			{
				Node<T, TInfo> tmp = pending.Dequeue();
				yield return tmp;
				foreach (Edge<T, TInfo> e in tmp)
					if (visited.Add(e.Node)) pending.Enqueue(e.Node);
			}
			yield break;
		}
		public static Node<T, TInfo> BFS<T, TInfo>(this Graph<T, TInfo> g, int start_node_idx, Predicate<Node<T, TInfo>> pred)
		{
			Node<T, TInfo> start = g[start_node_idx];
			HashSet<Node<T, TInfo>> visited = new HashSet<Node<T, TInfo>>();
			Queue<Node<T, TInfo>> pending = new Queue<Node<T, TInfo>>();
			pending.Enqueue(start);
			while (pending.Count != 0)
			{
				Node<T, TInfo> tmp = pending.Dequeue();
				if (pred(tmp)) return tmp;
				foreach (Edge<T, TInfo> e in tmp)
					if (visited.Add(e.Node)) pending.Enqueue(e.Node);
			}
			return null;
		}

		public static IEnumerable<TResult> BFS<T, TInfo, TResult>(this Graph<T, TInfo> g, int start_node_idx, Func<Node<T, TInfo>, TResult> func)
		{
			Node<T, TInfo> start = g[start_node_idx];
			HashSet<Node<T, TInfo>> visited = new HashSet<Node<T, TInfo>>();
			Queue<Node<T, TInfo>> pending = new Queue<Node<T, TInfo>>();
			pending.Enqueue(start);
			while (pending.Count != 0)
			{
				Node<T, TInfo> tmp = pending.Dequeue();
				yield return func(tmp);
				foreach (Edge<T, TInfo> e in tmp)
					if (visited.Add(e.Node)) pending.Enqueue(e.Node);
			}
			yield break;
		}
		public static void BFS<T, TInfo>(this Graph<T, TInfo> g, int start_node_idx, Action<Node<T, TInfo>> act)
		{
			Node<T, TInfo> start = g[start_node_idx];
			HashSet<Node<T, TInfo>> visited = new HashSet<Node<T, TInfo>>();
			Queue<Node<T, TInfo>> pending = new Queue<Node<T, TInfo>>();
			pending.Enqueue(start);
			while (pending.Count != 0)
			{
				Node<T, TInfo> tmp = pending.Dequeue();
				act(tmp);
				foreach (Edge<T, TInfo> e in tmp)
					if (visited.Add(e.Node)) pending.Enqueue(e.Node);
			}
		}
	}
}