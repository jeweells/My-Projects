using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graph
{
	public class Edge<T, TInfo>
	{
		public int NodeFromId;
		public TInfo Info;
		public Node<T, TInfo> Node;
	}
}