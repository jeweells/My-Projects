using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graph
{
	public interface IGraph<T, TInfo>
	{
		int CountNodes { get; }
		int CountEdges { get; }
		bool HasEdge(Node<T,TInfo> node_from, Node<T,TInfo> node_to);
		bool IsEmpty();
		bool HasNode(int node_id);
		Node<T, TInfo> AddNode(T value);
		bool AddEdge(Node<T, TInfo> node_from, Node<T, TInfo> node_to, TInfo info);
		Node<T, TInfo> Find(T value);
		Node<T, TInfo> GetNode(int node_id);
		Node<T, TInfo> GetNodeByIdx(int node_idx);
	}
}