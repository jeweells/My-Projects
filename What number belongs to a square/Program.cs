// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Homework 3
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Date: April 25, 2018
// :: Author: Abraham José Pacheco Becerra
// :: E-Mail: abraham.pacheco6319@gmail.com
// :: Description: Sorting squares by the number that belongs to a square.
// :: From this number is only known its position and it belongs to a square
// :: only if the number is inside the square
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Compilation
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: It has been successfully proved in Visual Studio Community 2017 15.2
// :: Also https://repl.it/@jeweells/TAPHomework3
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Input
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Starting with an integer N
// :: Followed by N lines where each line will contain Name X1 Y1 X2 Y2
// :: Name is the name of the square
// :: X1 Y1 are the coordinates of the left top corner
// :: X2 Y2 are the coordinates of the right bottom corner
// :: Also, (0 <= X1 < X2 <= 1000) and (0 <= Y1 < Y2 <= 1000)
// :: Then this is followed by N lines again
// :: Each line will be as X Y N
// :: Where N is the number that belongs to some square
// :: and X Y represent the position of that number (This position will decide which square this number belongs to)
// :: Example:
// :: 4
// :: A 84 14 367 341
// :: B 41 199 292 528
// :: D 312 42 575 140
// :: C 184 218 626 291
// :: 185 159 1
// :: 338 268 2
// :: 331 95 4
// :: 223 267 3
// :: This will output : A C B D
// :: If there are more options for each square this will ouput IMPOSIBLE
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Explanation
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: First we need a start node which doesn't belong to the original nodes group
// :: Then each edge from that start node will point to all the nodes (squares) that
// :: can have the number 1 in it (the edge will have 1 as an identifier), 
// :: we save these nodes in a list and we proceed to set an edge from each
// :: element of this list to the node that can have the number 2,
// :: this list is substuted by a list of these nodes (without repetition)
// :: we keep doing this process until we get the n number
// :: There are some things we need to take into account.
// :: -A list of unique nodes is made in the process, this means when
// :: setting the edge of value 'i' if there's only one node that has this edge
// :: it becomes an unique node (due to the only way of having access to this node
// :: is from the edge of value 'i'), this list helps to discard all the numbers
// :: that are inside the square since they're not going to belong to it
// :: -None of these lists can have elements repeated
// :: Once we have the graph done we call a function Show which will recursively
// :: find all the solutions for paths that starts in the edge 1 and ends in the edge
// :: n passing through all the nodes without repetition
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

using System;
using System.Collections.Generic;

namespace Tarea3
{
	public class Program
	{
		class Graph<NodeValueType, EdgeValueType>
		{
			
			public class Node
			{
				public class Edge
				{
					Node from;
					Node to;
					EdgeValueType value;
					public Edge(Node from, Node to, EdgeValueType value)
					{
						From = from;
						To = to;
						Value = value;
					}
					public EdgeValueType Value { get { return value; } set { this.value = value; } }
					public Node From { get { return from; } set { from = value; } }
					public Node To { get { return to; } set { to = value; } }
				}
				List<Edge> edges;
				NodeValueType value;
				public Node(List<Edge> edges, NodeValueType value)
				{
					Edges = edges;
					Value = value;
				}

				public List<Edge> Edges { get { return edges; } set { edges = value; } }
				public NodeValueType Value { get { return value; } set { this.value = value; } }
			}
			Node start;
			int countNodes;
			int countEdges;

			public Node Start { get { return start; } set { start = value; } }
			public int CountNodes { get { return countNodes; } set { countNodes = value; } }
			public int CountEdges { get { return countEdges; } set { countEdges = value; } }
		}

		class GraphT3 : Graph<T3Square, int>
		{
			public int DESIREDRESULTS = 1; // This controls how many results are allowed (if there are more results than this, it returns IMPOSSIBLE)
			public GraphT3(List<T3Square> squares, List<T3Number> numbers)
			{
				numbers.Sort(delegate (T3Number x, T3Number y)
				{
					return x.Id.CompareTo(y.Id);
				});
				Start = new Node(new List<Node.Edge>(), new T3Square("START", new Point(), new Point()));
				List<Node> squareNodes = new List<Graph<T3Square, int>.Node>();
				for (int i = 0; i < squares.Count; i++)
				{
					// Creating a node for each square
					squareNodes.Add(new Node(new List<Node.Edge>(), squares[i]));
				}
				List<Node> ownerPreviousNumber = new List<Graph<T3Square, int>.Node>();
				List<Node> ownerCurrentNumber;
				List<Node> uniqueNodes = new List<Graph<T3Square, int>.Node>();
				List<Node>[] numberNodes = new List<Graph<T3Square, int>.Node>[numbers.Count];

				ownerPreviousNumber.Add(Start);
				CountEdges = 0;
				for (int i = 0; i < numbers.Count; i++)
				{
					ownerCurrentNumber = new List<Graph<T3Square, int>.Node>();
					numberNodes[i] = new List<Graph<T3Square, int>.Node>();
					for (int k = 0; k < ownerPreviousNumber.Count; k++)
					{
						for (int j = 0; j < squareNodes.Count; j++)
						{
							if (squareNodes[j] == ownerPreviousNumber[k]) continue; // Avoiding loops
							if (uniqueNodes.Contains(squareNodes[j])) continue; // Avoiding imposible connections
							if (Belongs(squares[j], numbers[i]))
							{
								// Adds an edge that goes to the respective node if
								// the number belongs to the respective square
								ownerPreviousNumber[k].Edges.Add(
									new Node.Edge(
										Start,
										squareNodes[j],
										numbers[i].Id
										)
									);
									// Stores the node that is reached by the current number
									// *Only if it's not in the list*
									if (!ownerCurrentNumber.Contains(squareNodes[j]))
										ownerCurrentNumber.Add(squareNodes[j]);
								// Store all the nodes for each edge
								// This can be used to count the edges
								numberNodes[i].Add(squareNodes[j]); 
							}
						}
					}
					// Updates the list to the list of nodes that can be reached by 
					// the previous number
					ownerPreviousNumber = ownerCurrentNumber;
					if (numberNodes[i].Count == 1) // Only one square has that number
						uniqueNodes.Add(numberNodes[i][0]); // It's unique

					CountEdges += numberNodes[i].Count; // Adding edges
				}
				CountNodes = squareNodes.Count; // Start isn't counted as node

			}
			public void Show()
			{
				List<List<Node>> result = new List<List<Node>>();
				int resultidx = 0;
				RecursiveShow(Start, 1, ref resultidx, result);
				if (result.Count == DESIREDRESULTS)
				{
					for (int j = 0; j < DESIREDRESULTS; j++)
					{
						for (int i = 0; i < result[0].Count - 1; i++)
						{
							Console.Write(result[j][i].Value.Name + " ");
						}
						Console.WriteLine(result[j][result[j].Count - 1].Value.Name);
					}
					
				}
				else
				{
					Console.WriteLine("IMPOSIBLE");
				}
			}
			bool RecursiveShow(Node s, int idx, ref int resultidx, List<List<Node>> r)
			{
				for (int i = 0; i < s.Edges.Count; i++)
				{
					if(resultidx >= r.Count)
					{
						r.Add(new List<Graph<T3Square, int>.Node>());
					}
					if (resultidx >= DESIREDRESULTS+1)// Delete this if all results are desired
					{
						return false; 
						// resultidx is the result that is in proccess, so if resulidx = 2, it means there are 2 results due to (0, 1) are done
						// we want only one result so if two of them are found, this can improve the 
					}
					if (s.Edges[i].Value == idx)
					{
						if(!r[resultidx].Contains(s.Edges[i].To))
						{
							if (r[resultidx].Count < idx)
								r[resultidx].Add(s.Edges[i].To);
							else
								r[resultidx][idx - 1] = s.Edges[i].To;
							if(RecursiveShow(r[resultidx][idx - 1], idx + 1, ref resultidx, r))
							{
								r.Add(new List<Graph<T3Square, int>.Node>());
								for (int j = 0; j < idx-1; j++)
								{
									r[resultidx].Add(r[resultidx-1][j]);
								}
							}
							else
							{
								while (r[resultidx].Count > idx)
								{
									r[resultidx].RemoveAt(r[resultidx].Count - 1);
								}
							}

						}
					}
				}
				if (r[resultidx].Count == CountNodes)
				{
					resultidx++;
					return true;
				}
				else
				{
					if (idx == 1) r.RemoveAt(r.Count - 1);
					return false;
				}
			}

			bool Belongs(T3Square square, T3Number number)
			{
				if((square.P1.X < number.P.X && square.P2.X < number.P.X) ||
					(square.P1.X > number.P.X && square.P1.X > number.P.X)) // If it's out in X's then it doesn't belong to the square
				{
					return false;
				}
				else if((square.P1.Y < number.P.Y && square.P2.Y < number.P.Y) ||
					(square.P1.Y > number.P.Y && square.P1.Y > number.P.Y)) // If it's out in Y's then it doesn't belong to the square
				{
					return false;
				}
				else
				{
					return true; // It it's not out in X's and Y's then it belongs to the square
				}
			}
		}
		class Point
		{
			int x;
			int y;
			public Point() { x = 0; y = 0; }
			public Point(int x, int y)
			{
				X = x;
				Y = y;
			}

			public int X { get { return x; } set { x = value; } }
			public int Y { get { return y; } set { y = value; } }
		}
		class T3Number
		{
			int id;
			Point p;
			public T3Number(int id, Point p)
			{
				P = p;
				Id = id;
			}

			public int Id { get { return id; } set { id = value; } }
			public Point P { get { return p; } set { p = value; } }
		}
		class T3Square
		{
			string name;
			Point p1;
			Point p2;
			public T3Square(string name, Point p1, Point p2)
			{
				Name = name;
				P1 = p1;
				P2 = p2;
			}

			public string Name { get { return name; } set { name = value; } }
			public Point P1 { get { return p1; } set { p1 = value; } }
			public Point P2 { get { return p2; } set { p2 = value; } }
		}
		static void ErrorDialog(int id)
		{
			switch(id)
			{
				case 0:
					// Input error
					Console.WriteLine("Error al introducir datos:\nNOMBRE X1 Y1 X2 Y2");
					Console.WriteLine("NOMBRE: String de hasta "+ MAXLETTERSNAME + " letras mayúsculas.");
					Console.WriteLine("X1 Y1: Coordenadas de la esquina superior izquierda. ("+MINLIMITCOORD+" <= X1, Y1 <= "+MAXLIMITCOORD+")");
					Console.WriteLine("X2 Y2: Coordenadas de la esquina inferior derecha. ("+MINLIMITCOORD+" <= X2, Y2 <= "+MAXLIMITCOORD+")\n");
					break;
				case 1:
					// Input error
					Console.WriteLine("Error al introducir datos:\nNX NY i");
					Console.WriteLine("NX NY: Coordenadas del número.("+ MINLIMITCOORD + " <= NX,NY <= "+ MAXLIMITCOORD + ")");
					Console.WriteLine("i: Numero. (1 <= i <= N)");
					break;
			}
			
		}
		static bool IsCorrectName(string n)
		{
			if (n.Length > MAXLETTERSNAME) return false;
			for (int i = 0; i < n.Length; i++)
			{
				if (!Char.IsUpper(n[i])) return false;
			}
			return true;
		}
		static bool IsCorrectCoord(int x, int y)
		{
			return (MINLIMITCOORD <= x && x <= MAXLIMITCOORD &&
					MINLIMITCOORD <= y && y <= MAXLIMITCOORD);
		}
		const int MAXLIMITCOORD = 1000;
		const int MINLIMITCOORD = 0;
		const int MAXLETTERSNAME = 10;
		public static void Main(string[] args)
		{
			List<T3Square> squares = new List<T3Square>();
			List<T3Number> numbers = new List<T3Number>();
			// Read N
			try
			{
				int n = Int32.Parse(Console.ReadLine());
				for (int i = 0; i < n; i++)// N lines of "Name X1 Y1 X2 Y2"
				{
					string[] lines = Console.ReadLine().Split(' ');
					int x1, x2, y1, y2;
					if (lines.Length != 5 || !IsCorrectName(lines[0]) ||
						!Int32.TryParse(lines[1], out x1) ||
						!Int32.TryParse(lines[2], out y1) ||
						!Int32.TryParse(lines[3], out x2) ||
						!Int32.TryParse(lines[4], out y2) ||
						x1 > x2 ||
						y1 > y2 ||
						!IsCorrectCoord(x1, y1) ||
						!IsCorrectCoord(x2, y2))
					{
						ErrorDialog(0);
					}
					else
					{
						squares.Add(new T3Square(lines[0], new Point(x1, y1), new Point(x2, y2)));
					}
				}
				for (int i = 0; i < n; i++)
				{
					string[] lines = Console.ReadLine().Split(' ');
					int x, y, id;
					if(lines.Length != 3 ||
						!Int32.TryParse(lines[0], out x) ||
						!Int32.TryParse(lines[1], out y) ||
						!Int32.TryParse(lines[2], out id) ||
						!IsCorrectCoord(x, y) ||
						id < 1 ||
						id > n)
					{
						ErrorDialog(1);
					}
					else
					{
						numbers.Add(new T3Number(id, new Point(x, y)));
					}
				}
			}
			catch
			{
				Console.WriteLine("Ha ocurrido un error al introducir datos, por favor revise que los datos estén introducidos correctamente e intente de nuevo.");
				return;
			}
			GraphT3 gt3 = new GraphT3(squares, numbers);
			gt3.Show();
		}
	}
}
