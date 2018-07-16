using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
	class Testing
	{
		public static void Main()
		{
			#region :: Testing Dijstra
			//Graph<int, int> graph = new Graph<int, int>();
			//graph.mode = Graph<int, int>.Mode.UNDIRECTED;
			//Node<int,int> n1 = graph.AddNode(1);
			//Node<int, int> n2 = graph.AddNode(2);
			//Node<int, int> n3 = graph.AddNode(3);
			//Node<int, int> n4 = graph.AddNode(4);
			//Node<int, int> n5 = graph.AddNode(5);
			//Node<int, int> n6 = graph.AddNode(6);
			//graph.AddEdge(n1, n2, 7);
			//graph.AddEdge(n1, n6, 14);
			//graph.AddEdge(n1, n3, 9);
			//graph.AddEdge(n2, n3, 10);
			//graph.AddEdge(n2, n4, 15);
			//graph.AddEdge(n3, n4, 11);
			//graph.AddEdge(n3, n6, 2);
			//graph.AddEdge(n6, n5, 9);
			//graph.AddEdge(n5, n4, 6);
			////graph.PrintGraph((x) => x.ToString(), (x) => x.ToString());
			//foreach (KeyValuePair<int,int> d in graph.Dijkstra(n1.Id, (x)=> x.Info))
			//{
			//	Console.WriteLine(graph[d.Key].Value + " => " + d.Value);
			//}
			#endregion

			#region :: Testing HeldKarp part 1
			//Graph<int, int> graph = new Graph<int, int>();
			//Node<int, int> n1 = graph.AddNode(1);
			//Node<int, int> n2 = graph.AddNode(2);
			//Node<int, int> n3 = graph.AddNode(3);
			//Node<int, int> n4 = graph.AddNode(4);
			//graph.AddEdge(n1, n2, 1);
			//graph.AddEdge(n1, n3, 15);
			//graph.AddEdge(n1, n4, 6);

			//graph.AddEdge(n2, n1, 2);
			//graph.AddEdge(n2, n3, 7);
			//graph.AddEdge(n2, n4, 3);

			//graph.AddEdge(n3, n1, 9);
			//graph.AddEdge(n3, n2, 6);
			//graph.AddEdge(n3, n4, 12);

			//graph.AddEdge(n4, n1, 10);
			//graph.AddEdge(n4, n2, 4);
			//graph.AddEdge(n4, n3, 8);

			//int i = -1;
			//foreach (var item in graph.HeldKarp(0, (x) => x))
			//{
			//	Console.WriteLine(i++ +" => "+ item);
			//}
			#endregion

			#region :: Testing HeldKarp part 2
			//Graph<string, int> graph = new Graph<string, int>();
			//Node<string, int> n1 = graph.AddNode("White");
			//Node<string, int> n2 = graph.AddNode("Yellow");
			//Node<string, int> n3 = graph.AddNode("Orange");
			//Node<string, int> n4 = graph.AddNode("Red");
			//Node<string, int> n5 = graph.AddNode("Black");
			//graph.AddEdge(graph["White"], graph["Yellow"], 170);
			//graph.AddEdge(graph["White"], graph["Orange"], 200);
			//graph.AddEdge(graph["White"], graph["Red"], 220);
			//graph.AddEdge(graph["White"], graph["Black"], 300);

			//graph.AddEdge(graph["Yellow"], graph["White"], 150);
			//graph.AddEdge(graph["Yellow"], graph["Orange"], 170);
			//graph.AddEdge(graph["Yellow"], graph["Red"], 190);
			//graph.AddEdge(graph["Yellow"], graph["Black"], 210);

			//graph.AddEdge(graph["Orange"], graph["White"], 120);
			//graph.AddEdge(graph["Orange"], graph["Yellow"], 110);
			//graph.AddEdge(graph["Orange"], graph["Red"], 100);
			//graph.AddEdge(graph["Orange"], graph["Black"], 180);


			//graph.AddEdge(graph["Red"], graph["White"], 100);
			//graph.AddEdge(graph["Red"], graph["Yellow"], 90);
			//graph.AddEdge(graph["Red"], graph["Orange"], 80);
			//graph.AddEdge(graph["Red"], graph["Black"], 130);


			//graph.AddEdge(graph["Black"], graph["White"], 110);
			//graph.AddEdge(graph["Black"], graph["Yellow"], 100);
			//graph.AddEdge(graph["Black"], graph["Orange"], 100);
			//graph.AddEdge(graph["Black"], graph["Red"], 90);


			//int i = -1;
			//foreach (var item in graph.HeldKarp(graph["Orange"].Id, (x) => x))
			//{
			//	if(i == -1)
			//	{
			//		Console.WriteLine("Dist => " + item);
			//		i++;
			//	}
			//	else
			//		Console.WriteLine(i++ + " => " + graph[item].Value);
			//}
			#endregion

			#region :: Testing TSP
			Graph<string, int> graph = new Graph<string, int>();
			Node<string, int> n1 = graph.AddNode("Las Lomitas");
			Node<string, int> n2 = graph.AddNode("El Reyuno");
			Node<string, int> n3 = graph.AddNode("San Homero");
			Node<string, int> n4 = graph.AddNode("Cubillos");
			Node<string, int> n5 = graph.AddNode("Dolores");
			Node<string, int> n6 = graph.AddNode("Quilapaguay");
			graph.AddEdge(graph["Las Lomitas"], graph["El Reyuno"], 8);
			graph.AddEdge(graph["Las Lomitas"], graph["Cubillos"], 3);
			graph.AddEdge(graph["Las Lomitas"], graph["Quilapaguay"], 4);

			graph.AddEdge(graph["El Reyuno"], graph["Las Lomitas"], 8);
			graph.AddEdge(graph["El Reyuno"], graph["San Homero"], 1);
			graph.AddEdge(graph["El Reyuno"], graph["Cubillos"], 5);
			graph.AddEdge(graph["El Reyuno"], graph["Dolores"], 9);

			graph.AddEdge(graph["San Homero"], graph["El Reyuno"], 1);
			graph.AddEdge(graph["San Homero"], graph["Cubillos"], 7);
			graph.AddEdge(graph["San Homero"], graph["Dolores"], 2);
			graph.AddEdge(graph["San Homero"], graph["Quilapaguay"], 21);

			graph.AddEdge(graph["Cubillos"], graph["El Reyuno"], 5);
			graph.AddEdge(graph["Cubillos"], graph["Las Lomitas"], 3);
			graph.AddEdge(graph["Cubillos"], graph["San Homero"], 7);
			graph.AddEdge(graph["Cubillos"], graph["Quilapaguay"], 3);

			graph.AddEdge(graph["Dolores"], graph["El Reyuno"], 9);
			graph.AddEdge(graph["Dolores"], graph["San Homero"], 2);
			graph.AddEdge(graph["Dolores"], graph["Quilapaguay"], 35);

			graph.AddEdge(graph["Quilapaguay"], graph["Las Lomitas"], 4);
			graph.AddEdge(graph["Quilapaguay"], graph["San Homero"], 21);
			graph.AddEdge(graph["Quilapaguay"], graph["Cubillos"], 3);
			graph.AddEdge(graph["Quilapaguay"], graph["Dolores"], 35);
			
			#endregion
		}
	}
}
