using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graph
{
	public static class _DFS
	{
		public static IEnumerable<Node<T, TInfo>> DFS<T, TInfo>(this Graph<T, TInfo> g, int start_node_idx)
		{
			Node<T, TInfo> start = g[start_node_idx];
			HashSet<Node<T, TInfo>> visited = new HashSet<Node<T, TInfo>>();
			Stack<Node<T, TInfo>> pending = new Stack<Node<T, TInfo>>();
			pending.Push(start);
			while(pending.Count != 0)
			{
				Node<T,TInfo> tmp = pending.Pop();
				yield return tmp;
				foreach (Edge<T,TInfo> e in tmp)
					if (visited.Add(e.Node)) pending.Push(e.Node);
			}
			yield break;
		}
		public static Node<T, TInfo> DFS<T, TInfo>(this Graph<T, TInfo> g, int start_node_idx, Predicate<Node<T, TInfo>> pred)
		{
			Node<T, TInfo> start = g[start_node_idx];
			HashSet<Node<T, TInfo>> visited = new HashSet<Node<T, TInfo>>();
			Stack<Node<T, TInfo>> pending = new Stack<Node<T, TInfo>>();
			pending.Push(start);
			while (pending.Count != 0)
			{
				Node<T, TInfo> tmp = pending.Pop();
				if (pred(tmp)) return tmp;
				foreach (Edge<T, TInfo> e in tmp)
					if (visited.Add(e.Node)) pending.Push(e.Node);
			}
			return null;
		}

		public static IEnumerable<TResult> DFS<T, TInfo, TResult>(this Graph<T, TInfo> g, int start_node_idx, Func<Node<T, TInfo>, TResult> func)
		{
			Node<T, TInfo> start = g[start_node_idx];
			HashSet<Node<T, TInfo>> visited = new HashSet<Node<T, TInfo>>();
			Stack<Node<T, TInfo>> pending = new Stack<Node<T, TInfo>>();
			pending.Push(start);
			while (pending.Count != 0)
			{
				Node<T, TInfo> tmp = pending.Pop();
				yield return func(tmp);
				foreach (Edge<T, TInfo> e in tmp)
					if (visited.Add(e.Node)) pending.Push(e.Node);
			}
			yield break;
		}
		public static void DFS<T, TInfo>(this Graph<T, TInfo> g, int start_node_idx, Action<Node<T, TInfo>> act)
		{
			Node<T, TInfo> start = g[start_node_idx];
			HashSet<Node<T, TInfo>> visited = new HashSet<Node<T, TInfo>>();
			Stack<Node<T, TInfo>> pending = new Stack<Node<T, TInfo>>();
			pending.Push(start);
			while (pending.Count != 0)
			{
				Node<T, TInfo> tmp = pending.Pop();
				act(tmp);
				foreach (Edge<T, TInfo> e in tmp)
					if (visited.Add(e.Node)) pending.Push(e.Node);
			}
		}
		
	}
}