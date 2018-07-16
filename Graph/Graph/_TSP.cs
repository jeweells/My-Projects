using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graph
{
	public static class _TSP
	{
		public class TSPInfo
		{
			public int Distance = 0;
			public int Ways = 1;
			public int Time = 0;

			public TSPInfo(int distance, int ways, int time)
			{
				Distance = distance;
				Ways = ways;
				Time = time;
			}
		}
		static TSPInfo TSPAux<T, TInfo>(this Graph<T, TInfo> graph, Dictionary<string, TSPInfo> dict, string bitstr, int target_id, int maxtime, Func<TInfo, int> distance, Func<TInfo, int> time)
		{
			if (target_id == 0 && bitstr.Contains('0') && bitstr.Contains('1'))
			{
				return null;
			}
			string key = bitstr + target_id;
			if (dict.ContainsKey(key)) return dict[key];
			TSPInfo xpath = null;
			int cur_ways = 1;
			foreach (var edg in graph[target_id])
			{
				if (edg.Node.Id != 0 && bitstr[edg.Node.Id] == '0') continue;
				string tmp = "";
				for (int i = 0, endi = bitstr.Length; i < endi; i++)
				{
					if (edg.Node.Id == i)
						tmp += '0';
					else
						tmp += bitstr[i];
				}
				if (bitstr == "1101")
					Console.WriteLine("Now");
				TSPInfo currpath = TSPAux(graph, dict, tmp, edg.Node.Id, maxtime, distance, time);

				
				if (currpath == null) continue;
				int dist = currpath.Distance + distance(edg.Info);
				int ctime = currpath.Time + time(edg.Info);
				if (ctime > maxtime) continue;
				if (xpath == null || xpath.Distance > dist)
				{
					xpath = new TSPInfo(dist, currpath.Ways, ctime);
					cur_ways = 1;
				}
				else if(xpath.Distance == dist)
				{
					cur_ways++;
				}
			}
			if(xpath != null)
			{
				xpath.Ways *= cur_ways;
			}
			dict.Add(key, xpath);
			return xpath;
		}
		public static TSPInfo TSP<T,TInfo>(this Graph<T,TInfo> graph, int start_node_id, int maxtime, Func<TInfo, int> distance, Func<TInfo, int> time)
		{
			Dictionary<string, TSPInfo> dict = new Dictionary<string, TSPInfo>();
			dict.Add("".PadLeft(graph.CountNodes+1, '0'), new TSPInfo(0, 1, 0)); // Going to the first node from the first node is zero
			//foreach (var item in graph[start_node_id])
			//{
			//	dict.Add("".PadLeft(graph.CountNodes, '0')/*"".PadLeft(item.Node.Id, '0') +'1' + "".PadLeft(graph.CountNodes - item.Node.Id - 1, '0')*/ + item.Node.Id, distance(item.Info));
			//}
			return TSPAux(graph, dict, "".PadLeft(graph.CountNodes, '1'), 0, maxtime, distance, time);
			
			//List<State> paths = new List<State>();
			//Stack<State> next = new Stack<State>();
			//next.Push(new State(start_node_id, 0, 0, null));
			//State solution = null;
			//while(next.Count != 0)
			//{
			//	State tmp = next.Pop();
			//	State cmp;
			//	if(tmp.deep == graph.CountNodes - 1) // Maybe a solution
			//	{
			//		foreach (var item in graph[tmp.node])
			//		{
			//			if(item.Node.Id == start_node_id)
			//			{
			//				State ns = new State(start_node_id, tmp.distance + distance(item.Info), tmp.deep+1, tmp);
			//				if(solution == null)
			//				{
			//					solution = ns;
			//				}
			//				else if(solution.distance > ns.distance)
			//				{
			//					solution = ns;
			//				}
			//				break;
			//			}
			//		}
			//		continue;
			//	}
			//	if((cmp = paths.Find((x) => x.node == tmp.node && x.deep == tmp.deep)) != null)
			//	{
			//		if(cmp.distance > tmp.distance)
			//		{
			//			cmp.distance = tmp.distance;
			//			cmp.previous = tmp.previous;
			//			tmp = cmp;
			//		}
			//		else
			//		{
			//			continue;
			//		}
			//	}
			//	else
			//	{
			//		paths.Insert(0, tmp);
			//	}
			//	foreach (var item in graph[tmp.node])
			//	{
			//		if (tmp.FindNode(item.Node.Id) != null) continue;
			//		State ns = new State(item.Node.Id, tmp.distance + distance(item.Info), tmp.deep + 1, tmp);
			//		next.Push(ns);
			//	}
			//}
			//List<int> result = new List<int>();
			//if (solution != null)
			//{
			//	int dist = solution.distance;
			//	while(solution != null)
			//	{
			//		result.Insert(0, solution.node);
			//		solution = solution.previous;
			//	}
			//	result.Insert(0, dist);
			//}
			//return result;
		}
		class State
		{
			
			public int node;
			public int deep;
			public int distance;
			public State previous;

			public State(int node, int distance, int deep, State previous)
			{
				this.deep = deep;
				this.node = node;
				this.distance = distance;
				this.previous = previous;
			}
			public State FindNode(int idnode)
			{
				State tmp = this;
				while(tmp != null)
				{
					if (tmp.node == idnode) return tmp;
					tmp = tmp.previous;
				}
				return null;
			}
		}
	}
}