using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PriorityQueue;

namespace SudokuSolver
{
	class HiddenSets<T>
	{
		class Node
		{
			
			public int id;
			public bool final;
			public T value;
			public List<Node> edges;

			public Node(int id, bool final, T value, List<Node> edges)
			{
				this.id = id;
				this.final = final;
				this.value = value;
				this.edges = edges;
			}
		}
		List<IEnumerable<T>> setofsets;
		List<Node> finals;
		List<Node> normals;
		int deep;
		public int Deep {
			get { return deep; }
			set
			{
				if (value > finals.Count - 1)
					deep = finals.Count - 1;
				else if (value <= 0) deep = 1;
				else deep = value;
			}
		}
		public HiddenSets(IEnumerable<IEnumerable<T>> setofsets)
		{
			this.setofsets = setofsets.ToList();
			HashSet<T> possiblefinals = new HashSet<T>();
			foreach (var ie in setofsets)
				foreach (T v in ie) possiblefinals.Add(v);
			finals = new List<Node>(possiblefinals.Count);
			int id;
			for (id = 0; id < possiblefinals.Count; id++)
				finals.Add(new Node(id, true, possiblefinals.ElementAt(id), new List<Node>()));
			normals = new List<Node>(setofsets.Count());
			for(;id < setofsets.Count() + possiblefinals.Count; id++)
				normals.Add(new Node(id, false, default(T), new List<Node>()));
			for(int i = 0; i < setofsets.Count(); i++)
			{
				foreach (T v in setofsets.ElementAt(i))
				{
					Node tmp = finals.First((x) => x.value.Equals(v));
					normals[i].edges.Add(tmp);
					tmp.edges.Add(normals[i]);
				}
			}
			Deep = finals.Count - 1;
		}
		public IEnumerable<IEnumerable<T>> RemoveUniques()
		{
			HashSet<Node> checkeds = new HashSet<Node>();
			repeat:
			PriorityQueue<Node> pq = new PriorityQueue<Node>();
			foreach (Node n in normals.Except(checkeds))
			{
				pq.Enqueue(n.edges.Count, n);
			}
			while (pq.Count != 0)
			{
				if (RemoveUniquesFrom(pq.Dequeue(), checkeds)) goto repeat;
			}
			List<List<T>> setofsets2 = new List<List<T>>();
			for (int i = 0; i < normals.Count; i++)
			{
				Node n = normals[i];
				setofsets2.Add(new List<T>());
				foreach (Node e in n.edges)
				{
					setofsets2[i].Add(e.value);
				}
			}
			return setofsets2;
		}
		bool RemoveUniquesFrom(Node n, HashSet<Node> checkeds)
		{
			HashSet<Node> tobevisited = new HashSet<Node>();
			foreach (Node e in n.edges)
			{
				tobevisited.Add(e);
			}
			HashSet<Node> boxpairs = new HashSet<Node>(), goalpairs = new HashSet<Node>();
			if (DeepVisitNoRepeat(n, boxpairs, goalpairs, Deep))
			{
					Console.WriteLine("BoxPairs");
					foreach (Node item in boxpairs)
					{
						Console.WriteLine(item.value);
					}
					Console.WriteLine("GoalPairs");
					foreach (Node item in goalpairs)
					{
						Console.WriteLine(item.value);
					}

				foreach (Node nod in normals)
				{
					if (boxpairs.Contains(nod)) // Deletes what it's not in goalpairs
					{
						for (int i = 0; i < nod.edges.Count; i++)
						{
							if (!goalpairs.Contains(nod.edges[i]))
							{

								nod.edges.RemoveAt(i);
								i--;
							}
						}
					}
					else // Delets what it's in goalpairs
					{
						for (int i = 0; i < nod.edges.Count; i++)
						{
							if (goalpairs.Contains(nod.edges[i]))
							{
								nod.edges.RemoveAt(i);
								i--;
							}
						}
					}
				}
				checkeds.UnionWith(boxpairs);
				return true;
			}
			return false;
		}
		class State
		{
			public Node n;
			public HashSet<Node> bvisited;
			public HashSet<Node> rvisited;
			public HashSet<Node> targets;
			public State p;
		}
		// It is recommended to set hiddensetmaxLength less than N where N are the number of elements, otherwise this might get stuck or throw unexpected
		// results
		private bool DeepVisitNoRepeat(Node start, HashSet<Node> boxpairs, HashSet<Node> goalpairs, int hiddensetmaxlength)
		{
			if (start.edges.Count == 0) return false;
			HashSet<Node> visited = new HashSet<Node>();
			Stack<State> pending = new Stack<State>();
			State s = new State() { n = start.edges[0], p = null };
			s.targets = new HashSet<Node>();
			for (int i = 0; i < start.edges.Count; i++)
			{
				s.targets.Add(start.edges[i]);
			}
			s.rvisited = new HashSet<Node>();
			s.bvisited = new HashSet<Node>() { start };
			pending.Push(s);
			object lockstack = new object();
			ParallelOptions po = new ParallelOptions();
			po.MaxDegreeOfParallelism =
#if USEONECORE
					1;
#else
					Environment.ProcessorCount;
#endif
			while (pending.Count != 0)
			{
				State t = pending.Pop();
				if (t.targets.Count >= hiddensetmaxlength) continue; // In case there are no hiddensets this is needed
				if (t.n.final) // If this is a goal
				{
					if (t.targets.Contains(t.n)) // if this is a goal we need to visit
					{
						t.rvisited = new HashSet<Node>(t.rvisited);
						t.rvisited.Add(t.n); // Add it as visited
					}
					// If we have visited all the goals we needed to visit and this goal is one of the ones we need to visit (this is needed so that
					// this can finish in goal that the starting box can reach)
					if (t.targets.All(a => t.rvisited.Contains(a)) && s.targets.Contains(t.n))
					{
						goalpairs.UnionWith(t.targets); // Add the targets that were needed to visit
						boxpairs.UnionWith(t.bvisited); // Add the boxes visited throughout the path
						return true;
					}

				}
				Parallel.ForEach(t.n.edges, po, (e) => // For each adyacent node
				{
					if (!e.final) // e.n is a box and t.n is a goal
					{
						if (t.bvisited.Contains(e)) return; // if such box was visited... continue
						State s1 = new State() // The box wasn't visited so we add it
						{ // Copying references to save memory, only will copy if it's necessary
							n = e,
							p = t,
							rvisited = t.rvisited,
							targets = t.targets,
							bvisited = t.bvisited
						};
						bool copy = true;
						foreach (Node e1 in e.edges) // Adds new targets if the box this goal aims points to a goal that is not in targets
						{
							if (!t.targets.Contains(e1)) // The goal is not in targets
							{
								if (copy) // A copy is needed
								{
									s1.targets = new HashSet<Node>(t.targets);
									copy = false;
								}
								s1.targets.Add(e1); // Adds the goal to the targets
							}
						};
						lock (lockstack)
						{
							pending.Push(s1); // Push the box to be checked
						}
					}
					else // e.n is a goal and t.n is a box
					{
						if (t.rvisited.Contains(e)) return; // if this box has visited such goal... continue
						else if (!t.targets.Contains(e)) return; // if this box doesn't contain that goal in its targets.. continue
						State s1 = new State() // The box is in targets and wasn't visited
						{ // Copies references to save memory
							n = e,
							p = t,
							rvisited = t.rvisited,
							targets = t.targets
						};
						if (!t.bvisited.Contains(t.n)) // If this box wasn't visited
						{
							s1.bvisited = new HashSet<Node>(t.bvisited); // Copies the boxes visited
							s1.bvisited.Add(t.n); // Adds this box as visited
						}
						else { s1.bvisited = t.bvisited; } // If this box was visited, only copies the reference
						lock (lockstack)
						{
							pending.Push(s1); // Pushes the goal
						}
					}
				});
			}
			return false;
		}
	}


	class Sudoku
	{
		public int[,] Grid = new int[9,9];
		class Point
		{
			public Point(int i, int j)
			{
				this.i = i;
				this.j = j;
			}
			public int i { get; set; }
			public int j { get; set; }
			public override bool Equals(object obj)
			{
				Point y = (Point)obj;
				return y.i == i && y.j == j;
			}
			public override int GetHashCode()
			{
				return i.GetHashCode() ^ j.GetHashCode();
			}
		}
		public Sudoku(int[,] Grid)
		{
			this.Grid = Grid;
			if (!IsWellStructured(Grid))
			{
				Console.WriteLine("This sudoku isn't well structured. Correct the sudoku and try again.");
				return;
			}
		}

		bool IsWellStructured(int[,] Grid)
		{
			if (Grid.Length != 81) return false;
			for(int i = 0; i < 9; i++)
			{
				if (!CheckRow(i)) return false;
				if (!CheckCol(i)) return false;
			}
			for(int i = 0; i < 9; i+= 3)
			{
				for(int j = 0; j < 9; j+= 3)
				{
					if(!CheckSquareOf(new Point(i, j))) return false;
				}
			}
			return true;
		}
		bool CheckRow(int i)
		{
			bool[] pos = new bool[9];
			for(int j = 0; j < 9; j++)
			{
				int b = Grid[i, j];
				if (IsDigit(b))
				{
					if (pos[b - 1]) return false;
					else pos[b - 1] = true;
				}
			}
			return true;
		}


		bool CheckCol(int i)
		{
			bool[] pos = new bool[9];
			for (int j = 0; j < 9; j++)
			{
				int b = Grid[j, i];

				if (IsDigit(b))
				{
					if (pos[b - 1]) return false;
					else pos[b - 1] = true;
				}
			}
			return true;
		}

		bool CheckSquareOf(Point p)
		{
			int i = p.i, j = p.j;
			bool[] pos = new bool[9];
			for (int u = 3*(i/3), endu = 3 * (i / 3 + 1); u < endu; u++)
			{
				for (int k = 3 * (j / 3), endk = 3 * (j / 3 + 1); k < endk; k++)
				{
					int b = Grid[u, k];
					if (IsDigit(b))
					{
						if (pos[b - 1]) return false;
						else pos[b - 1] = true;
					}
				}
			}
			return true;
		}

		bool IsDigit(int b)
		{
			return b < 10 && b > 0;
		}
		bool CanBeInRow(int[,] Grid, int i, int value)
		{
			bool[] pos = new bool[9];
			pos[value - 1] = true;
			for (int j = 0; j < 9; j++)
			{
				int b = Grid[i, j];
				if (IsDigit(b))
				{
					if (pos[b - 1]) return false;
					else pos[b - 1] = true;
				}
			}
			return true;
		}
		bool CanBeInCol(int[,] Grid, int i, int value)
		{
			bool[] pos = new bool[9];
			pos[value - 1] = true;
			for (int j = 0; j < 9; j++)
			{
				int b = Grid[j, i];

				if (IsDigit(b))
				{
					if (pos[b - 1]) return false;
					else pos[b - 1] = true;
				}
			}
			return true;
		}

		bool CanBeInSquareOf(int[,] Grid, Point p, int value)
		{
			int i = p.i, j = p.j;
			bool[] pos = new bool[9];
			pos[value - 1] = true;
			for (int u = 3 * (i / 3), endu = 3 * (i / 3 + 1); u < endu; u++)
			{
				for (int k = 3 * (j / 3), endk = 3 * (j / 3 + 1); k < endk; k++)
				{
					int b = Grid[u, k];
					if (IsDigit(b))
					{
						if (pos[b - 1]) return false;
						else pos[b - 1] = true;
					}
				}
			}
			return true;
		}

		bool CanBeIn(int[,] grid, Point p, int value)
		{
			if (!CanBeInRow(grid, p.i, value)) return false;
			if (!CanBeInCol(grid, p.j, value)) return false;
			if (!CanBeInSquareOf(grid, p, value)) return false;
			return true;
		}

		Point FindNextEmpty(int[,] grid)
		{
			for(int i = 0; i < 9; i++)
			{
				for(int j = 0; j < 9; j++)
				{
					if (!IsDigit(grid[i, j])) return new Point(i, j);
				}
			}
			return null;
		}

		class State
		{
			public Point p;
			public int value;
			public State prev;
			public int[,] BuildGrid(int[,] basegrid)
			{
				int[,] r = (int[,]) basegrid.Clone();
				State tmp = this;
				while(tmp.prev != null)
				{
					r[tmp.p.i, tmp.p.j] = tmp.value;
					tmp = tmp.prev;
				}
				return r;
			}
			int hashcode;
			public void SetHashCode(int[,] zhash, int[] zhashvalues)
			{
				if(prev != null)
				{
					int h = prev.GetHashCode();
					h ^= zhash[p.i, p.j] ^ zhashvalues[value - 1];
				}
			}
			public override int GetHashCode()
			{
				return hashcode;
			}
		}

		int[,] zhash = new int[9,9];
		int[] zhashvalues = new int[9];

		void InitializeHashes()
		{
			Random random = new Random();
			for(int i = 0; i < 9; i ++)
				for(int j = 0; j < 9; j++)
					zhash[i,j] = random.Next();
			for(int i = 0; i < 9; i++) zhashvalues[i] = random.Next();

		}

		class StateComparer : IEqualityComparer<State>
		{
			public bool Equals(State x, State y)
			{
				return x.value == y.value && x.p == y.p && x.GetHashCode() == y.GetHashCode();
			}

			public int GetHashCode(State obj)
			{
				return obj.GetHashCode();
			}
		}

		bool CanBeInRow(State s, int i, int value)
		{
			bool[] pos = new bool[9];
			pos[value - 1] = true;

			State tmp = s;
			while (tmp.prev != null)
			{
				if (tmp.p.i == i)
				{
					if (IsDigit(tmp.value))
					{
						if (pos[tmp.value - 1]) return false;
						else pos[tmp.value - 1] = true;
					}
				}
				tmp = tmp.prev;
			}
			for (int j = 0; j < 9; j++)
			{
				int b = Grid[i, j];
				if (IsDigit(b))
				{
					if (pos[b - 1]) return false;
					else pos[b - 1] = true;
				}
			}
			return true;
		}
		bool CanBeInCol(State s, int i, int value)
		{
			bool[] pos = new bool[9];
			pos[value - 1] = true;
			State tmp = s;
			while (tmp.prev != null)
			{
				if (tmp.p.j == i)
				{
					if (IsDigit(tmp.value))
					{
						if (pos[tmp.value - 1]) return false;
						pos[tmp.value - 1] = true;
					}
				}
				tmp = tmp.prev;
			}
			for (int j = 0; j < 9; j++)
			{
				int b = Grid[j, i];

				if (IsDigit(b))
				{
					if (pos[b - 1]) return false;
					else pos[b - 1] = true;
				}
			}
			return true;
		}

		bool CanBeInSquareOf(State s, Point p, int value)
		{
			int i = p.i, j = p.j;
			bool[] pos = new bool[9];
			pos[value - 1] = true;
			int limu = 3 * (i / 3), endlimu = 3 * (i / 3 + 1);
			int limk = 3 * (j / 3), endlimk = 3 * (j / 3 + 1);
			State tmp = s;
			while (tmp.prev != null)
			{
				if (tmp.p.i >= limu && tmp.p.i < endlimu &&
					tmp.p.j >= limk && tmp.p.j < endlimk)
				{
					if (IsDigit(tmp.value))
					{
						if (pos[tmp.value - 1]) return false;
						else pos[tmp.value - 1] = true;
					}
				}
				tmp = tmp.prev;
			}
			for (int u = limu; u < endlimu; u++)
			{
				for (int k = limk; k < endlimk; k++)
				{
					int b = Grid[u, k];
					if (IsDigit(b))
					{
						if (pos[b - 1]) return false;
						else pos[b - 1] = true;
					}
				}
			}
			return true;
		}

		bool CanBeIn(State s, Point p, int value)
		{
			if (!CanBeInRow(s, p.i, value)) return false;
			if (!CanBeInCol(s, p.j, value)) return false;
			if (!CanBeInSquareOf(s, p, value)) return false;
			return true;
		}
		Point FindNextEmpty(State s)
		{
			HashSet<Point> points = new HashSet<Point>();
			State tmp = s;
			while(tmp.prev != null)
			{
				points.Add(tmp.p);
				tmp = tmp.prev;
			}
			for(int i = 0; i < 9; i++)
			{
				for(int j = 0; j < 9; j++)
				{
					if(!IsDigit(Grid[i,j]))
					{
						Point p = new Point(i, j);
						if (!points.Contains(p)) return p;
					}
				}
			}
			return null;
		}

		public Sudoku SolveEfficiently()
		{
			Stack<State> open = new Stack<State>();
			//HashSet<State> visited = new HashSet<State>(new StateComparer());
			open.Push(new State());
			while (open.Count != 0)
			{
				State s = open.Pop();
				int[,] tmpgrid = s.BuildGrid(Grid);
				Point ne = FindNextEmpty(tmpgrid);
				if (ne == null) return new Sudoku(tmpgrid);
				for (int i = 1; i <= 9; i++)
				{
					if (CanBeIn(tmpgrid, ne, i))
					{
						//Console.WriteLine("BEFORE::::");
						//Print(tmpgrid.BuildGrid(Grid));
						//Console.WriteLine("AFTER::::");
						State newgrid = new State() { p = ne, prev = s, value = i };
						newgrid.SetHashCode(zhash, zhashvalues);
						//Print(newgrid.BuildGrid(Grid));
						//if (!visited.Contains(newgrid))
						//{
							open.Push(newgrid);
							//visited.Add(newgrid);
						//}
					}
				}
			}
			return null;
		}


		public Sudoku Solve()
		{
			Stack<int[,]> open = new Stack<int[,]>();
			//HashSet<int[,]> visited = new HashSet<int[,]>();
			open.Push(Grid);
			while(open.Count != 0)
			{
				int[,] tmpgrid = open.Pop();

				Point ne = FindNextEmpty(tmpgrid);
				if (ne == null) return new Sudoku(tmpgrid);
				for (int i = 1; i <= 9; i++)
				{
					if(CanBeIn(tmpgrid, ne, i))
					{
						//Console.WriteLine("BEFORE::::");
						//Print(tmpgrid);
						//Console.WriteLine("AFTER::::");
						int[,] newgrid =(int[,]) tmpgrid.Clone();
						newgrid[ne.i, ne.j] = i;
						//Print(newgrid);
						//if (visited.Add(newgrid))
					//	{
							open.Push(newgrid);
						//}
					}
				}
			}
			return null;
		}

		public void Print(int[,] Grid)
		{
			for (int u = 0; u < 3; u++)
			{
				for (int i = 3 * u; i < 3 * (u + 1); i++)
				{
					for (int k = 0; k < 3; k++)
					{
						for (int j = 3 * k; j < 3 * (k + 1); ++j)
						{
							Console.Write($" {Grid[i, j]} ");
						}
						if (k != 2) Console.Write("█");
					}
					Console.WriteLine();
				}
				if (u != 2) Console.WriteLine("".PadLeft(29, '█'));
			}
		}
		public void Print()
		{
			for(int u = 0; u < 3; u++)
			{
				for (int i = 3*u; i < 3*(u+1); i++)
				{
					for (int k = 0; k < 3; k++)
					{
						for (int j = 3*k; j < 3*(k+1); ++j)
						{
							Console.Write($" {Grid[i,j]} ");
						}
						if(k != 2) Console.Write("█");
					}
					Console.WriteLine();
				}
				if (u != 2) Console.WriteLine("".PadLeft(29, '█'));
			}
		}

		static void Main(string[] args)
		{
			string[] lines = new string[9];
			for(int i = 0; i < 9; i ++)
			{
				lines[i] = Console.ReadLine();
			}
			int[,] linesint = new int[9, 9];
			for(int i = 0; i < 9; i ++)
			{
				char[] charr = lines[i].ToArray();
				
				for (int j = 0; j < 9; j++)
				{
					linesint[i, j] = Int32.Parse("0"+charr[j]);
				}
			}
			Testing.Test.TimeMeasurer timeMeasurer = new Testing.Test.TimeMeasurer();
			
			Sudoku sudoku = new Sudoku(linesint);

			HiddenSets<int> hiddenSets = new HiddenSets<int>(new int[][]{
				new int[]{ 1,2,3 },
				new int[]{ 1,2,3 },
				new int[]{1,2,3},
				new int[]{3,4,5 },
				new int[]{3,4,1}
			});
			List<IEnumerable<int>> li = hiddenSets.RemoveUniques().ToList();
			Console.WriteLine();
			foreach(var ie in li)
			{
				foreach (int i in ie)
					Console.Write(i+", ");
				Console.WriteLine();
			}
			//timeMeasurer.AddTimeElapsedIn(() => sudoku.Solve().Print());
			//Console.WriteLine(timeMeasurer.ElapsedMilliseconds);
			//timeMeasurer = new Testing.Test.TimeMeasurer();
			//timeMeasurer.AddTimeElapsedIn(() => sudoku.SolveEfficiently().Print());
			//Console.WriteLine(timeMeasurer.ElapsedMilliseconds);
		}
	}
}
