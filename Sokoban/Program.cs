#undef SHOWDEQUEUEDSTATE
#undef SHOWREMOVEGOALSRESULT
#undef DISPLAYDIAGONALDEADLOCKS
#undef USEONECORE // IF THIS IS UNDEFINED, DEBUG MAP PREVIEWS WON'T APPEAR
#define DISPLAYCORRALDEADLOCK
#undef FOCUSONEBOXTOGOAL

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



#if DEBUG
using Testing;
#endif
namespace Sokoban
{
	class PriorityQueue<ValueT>
	{
		object lockEnqDeq = new object();
		public enum OrderType { ASC, DESC };
		OrderType ord = OrderType.ASC;
		public enum NodeMode { Queue, Stack }
		NodeMode mod = NodeMode.Queue;
		class Node
		{
			public int pos;
			public List<ValueT> values = new List<ValueT>();
			public Node prev;
			public Node next;
			public Node(int pos, ValueT value, Node prev, Node next)
			{
				this.prev = prev;
				this.next = next;
				this.pos = pos;
				this.values.Add(value);
			}
		}
		int count = 0; // Counts all elements that have been inserted
		int dcount = 0; // Counts all nodes (Works if you want to know the number of different positions in the queue)
		int smallestpos; // Helps searching
		int biggestpos; // Helps searching
		Node first;
		Node last;
		/// <summary>
		/// Way in which the values of the same 'pos' are taken out
		/// </summary>
		public NodeMode Mode { get { return mod;  } set { mod = value; } }
		public OrderType Order { get { return ord; } set { ord = value; } }
		public int Count { get { return count; } }
		public int SmallestPos { get { return smallestpos; } }
		public int BiggestPos { get { return biggestpos; } }
		public int DCount { get { return dcount; } }

		public ValueT Dequeue()
		{
			lock(lockEnqDeq)
			{
				if (ord == OrderType.ASC)
					return DequeueAsc();
				else
					return DequeueDesc();
			}
		}
		ValueT DequeueAsc()
		{
			if (first == null) return default(ValueT);
			int idx = (mod == NodeMode.Queue) ? 0 : first.values.Count() - 1;
			ValueT v = first.values.ElementAt(idx);
			first.values.RemoveAt(idx);
			if (first.values.Count() == 0)
			{
				first = first.next;
				dcount--;
				if (first != null)
					smallestpos = first.pos;
				else
				{
					last = null;
					smallestpos = 0;
					biggestpos = 0;
				}
			}
			count--;
			return v;
		}
		ValueT DequeueDesc()
		{
			if (last == null) return default(ValueT);
			int idx = (mod == NodeMode.Stack) ? 0 : last.values.Count() - 1;
			ValueT v = last.values.ElementAt(idx);
			last.values.RemoveAt(idx);
			if (last.values.Count() == 0)
			{
				last = last.prev;
				dcount--;
				if (last != null)
					biggestpos = last.pos;
				else
				{
					first = null;
					smallestpos = 0;
					biggestpos = 0;
				}
			}
			count--;
			return v;
		}

		public void  Enqueue(int pos, ValueT value)
		{
			lock (lockEnqDeq)
			{
				if (last == null)
				{
					Node t = new Node(pos, value, null, null);
					first = t;
					last = t;
					count++;
					dcount++;
					smallestpos = pos;
					biggestpos = pos;
				}
				else
				{
					if (biggestpos < pos) // insert last
					{
						Node t = new Node(pos, value, last, null);
						last.next = t;
						last = t;
						count++;
						dcount++;
						biggestpos = pos;
					}
					else if (pos < smallestpos) // insert first
					{
						Node t = new Node(pos, value, null, first);
						first.prev = t;
						first = t;
						count++;
						dcount++;
						smallestpos = pos;
					}
					else if (biggestpos - pos <= pos - smallestpos) // It's closer to the biggest pos
					{
						Node tmp = last;
						while (tmp != null)
						{
							if (tmp.pos <= pos)
							{
								if (tmp.pos == pos) // It already exists
								{
									tmp.values.Add(value);
									count++;
								}
								else // it doesn't exist
								{
									Node t = new Node(pos, value, tmp, tmp.next);
									tmp.next = t;
									t.next.prev = t;
									count++;
									dcount++;
								}
								break;
							}
							tmp = tmp.prev;
						}
					}
					else // It's closer to smallest pos
					{
						Node tmp = first;
						while (tmp != null)
						{
							if (tmp.pos >= pos)
							{
								if (tmp.pos == pos) // It already exists
								{
									tmp.values.Add(value);
									count++;
								}
								else // it doesn't exist
								{
									Node t = new Node(pos, value, tmp.prev, tmp);
									tmp.prev = t;
									t.prev.next = t;
									count++;
									dcount++;
								}
								break;
							}
							tmp = tmp.next;
						}
					}
				}
			}
		}
	}
	
	public class SokobanSolver
	{
		HashSet<int> nodeadlocks = new HashSet<int>();
		int[][] boxtogoals;
		class State
		{
			public bool[] playerreachablearea;
			public int boxpushes;
			public State previousState = null;
			public int[] boxpositions;
			//public Hashtable boxtogoals;
			public int playerposition;
			public int heuristic;
			public int f;
			public int cumulatedlength;

			public State Clone()
			{
				State s = new State();
				s.playerposition = playerposition;
				s.playerreachablearea = playerreachablearea.ToArray();
				s.previousState = previousState;
				s.boxpushes = boxpushes;
				s.boxpositions = boxpositions.ToArray();
				s.heuristic = heuristic;
				s.f = f;
				s.cumulatedlength = cumulatedlength;
				s.hashcode = hashcode;
				s.hashcodecalculated = hashcodecalculated;
				return s;
			}
			public static bool operator ==(State a, State b)
			{
				if ((object)a == null && (object)b == null) return true;
				else if ((object)a != null)
					return a.Equals(b);
				else
					return b.Equals(a);
			}
			public static bool operator !=(State a, State b)
			{
				return !(a == b);
			}
			public override bool Equals(object obj)
			{
				State s = ((State)obj);
				if (obj == null) return false;
				return playerposition == s.playerposition
						&& boxpositions.All(x => s.boxpositions.Contains(x));
			}
			public override int GetHashCode()
			{
				if (hashcodecalculated) return hashcode;
				hashcode = ZobrishHash.GetZobrishHash(this);
				hashcodecalculated = true;
				return hashcode;
			}
			public void SetHashCode()
			{
				hashcode = ZobrishHash.GetZobrishHash(this);
				hashcodecalculated = true;
			}
			public void SetHashCodeAfterMove(int before, int now)
			{
				hashcodecalculated = true;
				hashcode = ZobrishHash.GetZobrishHashAfterMove(this, before, now);
			}
			bool hashcodecalculated = false;
			public int hashcode;
		}
		enum Elements
		{
			WALL = '*', BOX = 'C', FREE = '.', PLAYER = 'J', GOAL = 'F'
		}
		static char[] Grid;
		int columns;
		State initial = new State();
		HashSet<int> goals = new HashSet<int>();
		//HashSet<int> nodeadlocks = new HashSet<int>();
		class StateComparerByPlayerZone : EqualityComparer<State>
		{
			object locktimesortplayertosideboxdistance = new object();
			public override bool Equals(State x, State y)
			{

				if (x == null || y == null) return false;
				if (InsideGrid(y.playerposition) && !x.playerreachablearea[y.playerposition])
				{
					return false;
				}
				return x.GetHashCode() == y.GetHashCode();
			}
			public override int GetHashCode(State obj)
			{
				return obj.GetHashCode();
			}
		}
		class StateComparer : EqualityComparer<State>
		{
			public override bool Equals(State x, State y)
			{
				return x == y;
			}
			public override int GetHashCode(State obj)
			{
				return base.GetHashCode();
			}
		}
		public SokobanSolver(char[] Grid, int columns)
		{
			if (columns <= 0) return;
			if (Grid.Length % columns != 0) return;
			this.columns = columns;
			SokobanSolver.Grid = new char[Grid.Length];
			List<int> boxpos = new List<int>();
			for (int i = 0; i < Grid.Length; i++)
			{
				if (Grid[i] == (char)Elements.BOX) { boxpos.Add(i); }
				else if (Grid[i] == (char)Elements.PLAYER) { initial.playerposition = i; }
				else if (Grid[i] == (char)Elements.WALL) { SokobanSolver.Grid[i] = (char)Elements.WALL; continue; }
				else if (Grid[i] == (char)Elements.GOAL) { SokobanSolver.Grid[i] = (char)Elements.GOAL; goals.Add(i); continue; }
				SokobanSolver.Grid[i] = (char)Elements.FREE;

				if (Grid[i] != (char)Elements.WALL) numberofnowalls++;
			}
			numberofboxes = boxpos.Count();
			initial.boxpositions = boxpos.ToArray();
			boxtogoals = new int[boxpos.Count][];
			canbesolved = CanReachBoxesAndFinals(initial);
			if (canbesolved)
			{
				canbesolved = FindSimpleDeadLocks(initial);
				if (canbesolved)
				{
					//canbesolved = CleanBoxToGoals(initial)

					//Hashtable hst = new Hashtable();
					//hst.Add(11, new HashSet<int>() { 2,3,4,5,6,7,8,9 });
					//hst.Add(12, new HashSet<int>() { 1,2,3,4,5,6,7,8 });
					//hst.Add(13, new HashSet<int>() { 1,2,3,8,9});
					//hst.Add(21, new HashSet<int>() { 5,8,9 });
					//hst.Add(22, new HashSet<int>() { 1,5,8});
					//hst.Add(23, new HashSet<int>() { 1,8,9 });
					//hst.Add(31, new HashSet<int>() { 2,5,8,9 });
					//hst.Add(32, new HashSet<int>() { 1,2,5,8 });
					//hst.Add(33, new HashSet<int>() { 6});


					//HashSet<int> hss = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
					//GraphForBoxGoals gfbg = new GraphForBoxGoals(hst, hss);

					GraphForBoxGoals gfbg = new GraphForBoxGoals(initial.boxpositions, boxtogoals, goals);
					boxtogoals = gfbg.RemoveUniques(out canbesolved);
#if DEBUG
					Console.Write("\n");
					if (canbesolved)
					{
						for (int i = 0; i < boxtogoals.Length; i++)
						{
							Console.Write(initial.boxpositions[i] + " -> ");
							foreach (int h in boxtogoals[i])
							{
								Console.Write(h + ",");
							}
							Console.Write("\n");
						}
					}
					else
					{

						Console.WriteLine("CAN'T BE SOLVED");

					}
					//Thread.Sleep(10000000);
#endif
				}
				SetPlayerReachableArea(initial);
			}
#if DEBUG && USEONECORE
			PrintDeadLocks();
#endif
		}

		public void PrintSolution()
		{
			if (solution != null)
				PrintStateChain(solution);
			else
				Console.WriteLine("There's no solution found");
		}

		bool canbesolved;

		void PrintDeadLocks()
		{
			Console.WriteLine("--DEADLOCKS-------");
			for (int i = 0; i < Grid.Length; i++)
			{
				if (i % columns == 0) Console.Write("\n");
				if (nodeadlocks.Contains(i)) Console.Write(".");
				else if (Grid[i] == (char)Elements.WALL) Console.Write(Grid[i]);
				else Console.Write("X");
			}
			Console.WriteLine("-- END DEADLOCKS-------");
		}
		bool CanReachBoxesAndFinals(State initial)
		{
			HashSet<int> pvisited = new HashSet<int>();
			HashSet<int> pendingp = new HashSet<int>();
			HashSet<int> playergoals = new HashSet<int>(goals);
			foreach (int k in initial.boxpositions)
			{
				playergoals.Add(k);
			}
			pendingp.Add(initial.playerposition);
			while (pendingp.Count() != 0)
			{
				int t = pendingp.ElementAt(0);
				pendingp.Remove(t);
				pvisited.Add(t);
				playergoals.Remove(t);
				if (playergoals.Count() == 0)
				{
#if DEBUG
					Console.WriteLine("It is posible to reach boxes and finals");
#endif
					break;
				}
				GenerateSuccessorsPlayerReachBoxesAndFinals(t, pendingp, pvisited);

			}
			if (playergoals.Count() > 0)
			{
#if DEBUG
				Console.WriteLine("¡¡It is NOT posible to reach boxes and finals!!");
#endif
				return false;
			}
			else return true;
		}
		class GraphForBoxGoals
		{
			public class Node
			{
				public bool isgoal;
				public int value;
				public List<Edge> edges = new List<Edge>();
			}
			public class Edge
			{
				public Node n;
			}
			int[][] boxtogoals;
			int[] boxpositions;
			public GraphForBoxGoals(int[] boxpositions, int[][] boxtogoals, HashSet<int> goals)
			{
				this.boxtogoals = boxtogoals;
				this.boxpositions = boxpositions;
				foreach (int k in goals) // Creating a node for each goal
				{
					Node t = new Node();
					t.isgoal = true;
					t.value = k;
					this.goals.Add(t);
				}
				List<Node> boxes = new List<Node>(); // Saving boxes apart
				for (int i = 0; i < boxpositions.Length; i++)
				{
					Node t = new Node(); // Create a node
					t.value = boxpositions[i];
					t.isgoal = false;
					foreach (int g in boxtogoals[i]) // Create an edge for each goal it points
					{
						Edge e = new Edge();
						Edge e2 = new Edge();
						e2.n = t;
						e.n = this.goals.Find(x => x.value == g); // To a goal
						t.edges.Add(e); // Add the edge to the box
						e.n.edges.Add(e2);// Add an edge to the goal
					}
					boxes.Add(t); // Save the box
				}
				
				this.boxes.AddRange(boxes); // Saves all boxes
#if DEBUG && SHOWREMOVEGOALSRESULT && USEONECORE
				foreach (Node n in this.boxes)
				{
					Console.Write(n.value + " -> ");
					foreach (Edge e in n.edges)
					{
						if (e.n == n) Console.Write(e.n.value + ",");
						else Console.Write(e.n.value + ",");
					}
					Console.Write("\n");
				}
#endif
			}
			public List<Node> boxes = new List<Node>();
			public List<Node> goals = new List<Node>();
			public int[][] RemoveUniques(out bool solvable)
			{
				solvable = false;
				HashSet<Node> checkeds = new HashSet<Node>();
				repeat:
				PriorityQueue<Node> pq = new PriorityQueue<Node>();
				foreach (Node n in boxes.Except(checkeds))
				{
					pq.Enqueue(n.edges.Count, n);
				}
				while(pq.Count != 0)
				{
					if (RemoveUniquesFrom(pq.Dequeue(), checkeds)) goto repeat;
				}
				int[][] boxtogoals2 = new int[boxtogoals.Length][];
				List<int> boxp = boxpositions.ToList();
				foreach (Node n in boxes)
				{
					List<int> hs = new List<int>();
					foreach (Edge e in n.edges)
					{
						hs.Add(e.n.value);
					}
					boxtogoals2[boxp.IndexOf(n.value)] = hs.ToArray();
					if (hs.Count == 0) return null;
				}
				solvable = true;
				return boxtogoals2;
			}
			bool RemoveUniquesFrom(Node n, HashSet<Node> checkeds)
			{
				HashSet<Node> tobevisited = new HashSet<Node>();
				foreach (Edge e in n.edges)
				{
					tobevisited.Add(e.n);
				}
				HashSet<Node> boxpairs = new HashSet<Node>(), goalpairs = new HashSet<Node>();
				if (DeepVisitNoRepeat(n, boxpairs, goalpairs, goals.Count - 1))
				{
#if DEBUG && SHOWREMOVEGOALSRESULT && USEONECORE
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
#endif
					foreach (Node nod in boxes)
					{
						if(boxpairs.Contains(nod)) // Deletes what it's not in goalpairs
						{
							for (int i = 0; i < nod.edges.Count; i++)
							{
								if (!goalpairs.Contains(nod.edges[i].n))
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
								if (goalpairs.Contains(nod.edges[i].n))
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
			private bool DeepVisitNoRepeat(Node start, HashSet<Node> boxpairs, HashSet<Node> goalpairs, int hiddensetmaxlength)
			{
				if (start.edges.Count == 0) return false;
				HashSet<Edge> visited = new HashSet<Edge>();
				Stack<State> pending = new Stack<State>();
				State s = new State() { n = start.edges[0].n, p = null };
				s.targets = new HashSet<Node>();
				for (int i = 0; i < start.edges.Count; i++)
				{
					s.targets.Add(start.edges[i].n);
				}
				s.rvisited = new HashSet<Node>();
				s.bvisited = new HashSet<Node>(){ start };
				pending.Push(s);
				int cases = 0;
				object lockstack = new object();
				ParallelOptions po = new ParallelOptions();
				po.MaxDegreeOfParallelism =
#if USEONECORE
					1;
#else
					Environment.ProcessorCount;
#endif
				while(pending.Count != 0)
				{
					cases++;
					State t = pending.Pop();
					if (t.targets.Count >= hiddensetmaxlength) continue;
					if(t.n.isgoal)
					{
						if (t.targets.Contains(t.n))
						{
							t.rvisited = new HashSet<Node>(t.rvisited);
							t.rvisited.Add(t.n);
						}
						if (t.targets.All(a => t.rvisited.Contains(a)) && s.targets.Contains(t.n))
						{
#if DEBUG && USEONECORE
							Console.Write("t=" + t.n.value + " -> ");
							foreach (Edge item in t.n.edges)
							{
								Console.Write(item.n.value + ", ");
							}
							Console.Write(" | Visited: ");
							foreach (Node item in t.rvisited)
							{
								Console.Write(item.value + ", ");
							}
							Console.Write(" | Targets: ");
							foreach (Node item in t.targets)
							{
								Console.Write(item.value + ", ");
							}
							Console.Write("\n");
							Console.WriteLine("Cases: " + cases);
#endif
							goalpairs.UnionWith(t.targets);
							boxpairs.UnionWith(t.bvisited);
							return true;
						}

					}
					Parallel.ForEach(t.n.edges, po, (e) =>
					{
						if (!e.n.isgoal) // e.n is a box and t.n is a goal
						{
							if (t.bvisited.Contains(e.n)) return;
							State s1 = new State()
							{
								n = e.n,
								p = t,
								rvisited = t.rvisited,
								targets = t.targets,
								bvisited = t.bvisited
							};
							bool copy = true;
							foreach (Edge e1 in e.n.edges) // Adds new targets
							{
								if (!t.targets.Contains(e1.n))
								{
									if (copy)
									{
										s1.targets = new HashSet<Node>(t.targets);
										copy = false;
									}
									s1.targets.Add(e1.n);
								}
							};
							lock(lockstack)
							{
								pending.Push(s1);
							}
						}
						else // e.n is a goal and t.n is a box
						{
							if (t.rvisited.Contains(e.n)) return;
							else if (!t.targets.Contains(e.n)) return;
							State s1 = new State()
							{
								n = e.n,
								p = t,
								rvisited = t.rvisited,
								targets = t.targets
							};
							if (!t.bvisited.Contains(t.n))
							{
								s1.bvisited = new HashSet<Node>(t.bvisited);
								s1.bvisited.Add(t.n);
							}
							else { s1.bvisited = t.bvisited; }
							lock (lockstack)
							{
								pending.Push(s1);
							}
						}
					});
				}
				return false;
			}
		}

		bool CleanBoxToGoals(State s) // Returns false if after cleaning a box has no goal set
		{
			
			// previous  CleanBoxToGoals version
			HashSet<int> uniques = new HashSet<int>();
			reset:
			for (int i = 0; i < boxtogoals.Length; i++)
			{
				if (boxtogoals[i].Length == 1) // It's unique
				{
					if (uniques.Contains(boxtogoals[i][0])) continue;
					uniques.Add(boxtogoals[i][0]);
					for (int j = 0; j < boxtogoals.Length; j++)
					{
						if (i == j) continue;
						if(boxtogoals[j].Contains(boxtogoals[i][0]))
						{
							if (boxtogoals[j].Length - 1 == 0) return false; // This box has no goal
							int[] narr = new int[boxtogoals[j].Length-1];
							int elm = 0;
							for (int k = 0; k < boxtogoals[j].Length; k++)
							{
								if (elm == 0 && boxtogoals[j][k + elm] == boxtogoals[i][0]) { elm = 1; continue; }
								narr[k] = boxtogoals[j][k + elm];
							}
						}
					}
					goto reset; // Find uniques again
				}
			}
			return true;
		}

		bool FindSimpleDeadLocks(State initial) // returns false if it can't finish
		{
			HashSet<int> pending = new HashSet<int>();
			for (int i = 0; i < initial.boxpositions.Length; i++) // Foreach box
			{
				HashSet<int> visited = new HashSet<int>();
				pending.Add(initial.boxpositions[i]);
				while (pending.Count() != 0)
				{
					int b = pending.ElementAt(0);
					pending.Remove(b);
					nodeadlocks.Add(b);
					visited.Add(b);
					if (goals.Contains(b))
					{
						if (boxtogoals[i] == null) boxtogoals[i] = new int[1];
						else
						{
							int[] prev = boxtogoals[i];
							boxtogoals[i] = new int[prev.Length + 1];
							prev.CopyTo(boxtogoals[i], 0);
						}
						boxtogoals[i][boxtogoals[i].Length - 1] = b;
					}
					GenerateSuccessorsForBox(b, pending, visited);
				}
				if (boxtogoals[i] == null)
					return false;
			}
			return true;
		}
		void GenerateSuccessorsPlayerReachBoxesAndFinals(int playerp, HashSet<int> pending, HashSet<int> visited)
		{
			int[] spos = new int[] { 1, -1, columns, -columns };
			foreach (int a in spos)
			{
				int npos = playerp + a;
				if (visited.Contains(npos) || pending.Contains(npos)) continue;
				int upperbound = Grid.Length, lowerbound = 0;
				if (a == 1) upperbound = columns * (playerp / columns + 1);
				else if (a == -1) lowerbound = columns * (playerp / columns);
				if (npos >= lowerbound && npos < upperbound && Grid[npos] != (char) Elements.WALL)
				{
					pending.Add(npos);
				}

			}
		}
		enum Direction { LEFT, RIGHT, UP, DOWN }
		bool CanBePushed(int bp, Direction dir)
		{
			int n = 0;
			int lowerbound = 0 , upperbound =  Grid.Length;
			switch (dir)
			{
				case Direction.LEFT:
					n = -1;
					lowerbound = columns * (bp / columns);
					upperbound = columns * (bp / columns + 1);
					break;
				case Direction.RIGHT:
					n = 1;
					lowerbound = columns * (bp / columns);
					upperbound = columns * (bp / columns + 1);
					break;
				case Direction.UP:
					n = -columns;
					break;
				case Direction.DOWN:
					n = columns;
					break;
			}
			int np = bp + n;
			int pushpos = bp - n;
			if (np < lowerbound || np >= upperbound ||
				pushpos < lowerbound || pushpos >= upperbound || Grid[pushpos] == (char) Elements.WALL || Grid[np] == (char) Elements.WALL) return false;
			return true;
		}

		bool IsBoxLockedByWalls(int bp)
		{
			if (bp < 0 || bp >= Grid.Length) return true;
			int up = bp - columns,
				down = bp + columns,
				left = bp - 1,
				right = bp + 1;
			bool movex = true, movey = true;
			if (up < 0 || Grid[up] == (char) Elements.WALL) movey = false;
			if (down >= Grid.Length || Grid[down] == (char)Elements.WALL) movey = false;
			if (left < 0 || left < columns * (bp / columns) || Grid[left] == (char)Elements.WALL) movex = false;
			if (right >= Grid.Length || right >= columns * (bp / columns + 1) || Grid[right] == (char)Elements.WALL) movex = false;
			return !movex && !movey;
		}
		void GenerateSuccessorsForBox(int boxp, HashSet<int> pending, HashSet<int> visited)
		{
			int boxl = boxp - 1;
			int boxr = boxp + 1;
			int boxu = boxp - columns;
			int boxd = boxp + columns;

			if (!pending.Contains(boxl) && !visited.Contains(boxl) && CanBePushed(boxp, Direction.LEFT))
			{
				if (goals.Contains(boxl) || !IsBoxLockedByWalls(boxl)) pending.Add(boxl);
				else visited.Add(boxl);
			}
			if (!pending.Contains(boxr) && !visited.Contains(boxr) && CanBePushed(boxp, Direction.RIGHT))
			{
				if (goals.Contains(boxr) || !IsBoxLockedByWalls(boxr)) pending.Add(boxr);
				else visited.Add(boxr);
			}
			if (!pending.Contains(boxu) && !visited.Contains(boxu) && CanBePushed(boxp, Direction.UP))
			{
				if (goals.Contains(boxu) || !IsBoxLockedByWalls(boxu)) pending.Add(boxu);
				else visited.Add(boxu);
			}
			if (!pending.Contains(boxd) && !visited.Contains(boxd) && CanBePushed(boxp, Direction.DOWN))
			{
				if (goals.Contains(boxd) || !IsBoxLockedByWalls(boxd)) pending.Add(boxd);
				else visited.Add(boxd);
			}
		}

		void PrintStateChain(State s)
		{
			
			Console.Clear();
			Stack<State> sortedStates = new Stack<State>();

			for (State a = s; a != null; a = a.previousState)
			{
				sortedStates.Push(a);
			}
			while (true)
			{
				int j = 0;
				foreach (State a in sortedStates)
				{
					Console.Write(" Step " + j + "\n");
					PrintState(a);
					j++;
#if DEBUG
					Console.Write("{0, -50} {1, 10} ms \n", "Time elapsed: ", solvingtimemeasurer.ElapsedMilliseconds);
					Console.WriteLine("{0}", "".PadLeft(100, '#'));
					Console.Write("{0, -50} {1, 10} ms this is {2, 8:P2} called {3, 10} \n", "Time wasted in finding reachableplayerarea:", reachableareameasurer.ElapsedMilliseconds,(double) reachableareameasurer.ElapsedMilliseconds / solvingtimemeasurer.ElapsedMilliseconds, reachableareameasurer.CountCalls);
					Console.Write("{0, -50} {1, 10} ms this is {2, 8:P2} called {3, 10} \n", "Time wasted in pushing box randomly:", pushboxmeasurer.ElapsedMilliseconds, (double)pushboxmeasurer.ElapsedMilliseconds / solvingtimemeasurer.ElapsedMilliseconds, pushboxmeasurer.CountCalls);
					Console.Write("{0, -50} {1, 10} ms this is {2, 8:P2} called {3, 10} \n", "Time wasted in pushing one box to a goal:", boxtogoalmeasurer.ElapsedMilliseconds, (double)boxtogoalmeasurer.ElapsedMilliseconds / solvingtimemeasurer.ElapsedMilliseconds, boxtogoalmeasurer.CountCalls);
					Console.WriteLine("{0}", "".PadLeft(100, '#'));
					Console.Write("{0, -50} {1, 10} ms this is {2, 8:P2} called {3, 10} \n", "Time wasted in calculating deadlocks:", deadlockmeasurer.ElapsedMilliseconds, (double)deadlockmeasurer.ElapsedMilliseconds / solvingtimemeasurer.ElapsedMilliseconds, deadlockmeasurer.CountCalls);
					Console.WriteLine("{0}", "".PadLeft(100, '#'));
					deadlockmeasurer.PrintReturns();
					Console.WriteLine("{0}", "".PadLeft(100, '#'));
					Console.Write("{0, 50} {1, 10} ms this is {2, 8:P2} called {3, 10} \n", "diagonal deadlocks:", diagonaldeadlockmeasurer.ElapsedMilliseconds, (double)diagonaldeadlockmeasurer.ElapsedMilliseconds / solvingtimemeasurer.ElapsedMilliseconds, diagonaldeadlockmeasurer.CountCalls);
					Console.WriteLine("{0}", "".PadLeft(100, '#'));
					diagonaldeadlockmeasurer.PrintReturns();
					Console.WriteLine("{0}", "".PadLeft(100, '#'));
					Console.Write("{0, 50} {1, 10} ms this is {2, 8:P2} called {3, 10} \n", "simple deadlocks:", simpledeadlockmeasurer.ElapsedMilliseconds, (double)simpledeadlockmeasurer.ElapsedMilliseconds / solvingtimemeasurer.ElapsedMilliseconds, simpledeadlockmeasurer.CountCalls);
					Console.WriteLine("{0}", "".PadLeft(100, '#'));
					simpledeadlockmeasurer.PrintReturns();
					Console.WriteLine("{0}", "".PadLeft(100, '#'));
					Console.Write("{0, 50} {1, 10} ms this is {2, 8:P2} called {3, 10} \n", "freeze deadlocks:", freezedeadlockmeasurer.ElapsedMilliseconds, (double)freezedeadlockmeasurer.ElapsedMilliseconds / solvingtimemeasurer.ElapsedMilliseconds, freezedeadlockmeasurer.CountCalls);
					Console.WriteLine("{0}", "".PadLeft(100, '#'));
					freezedeadlockmeasurer.PrintReturns();
					Console.WriteLine("{0}", "".PadLeft(100, '#'));
					Console.WriteLine("{0}", "".PadLeft(100, '#'));
					Console.Write("{0, -50} {1, 10} ms this is {2, 8:P2} called {3, 10} \n", "Time wasted in checking if visited contains:", visitedmeasurer.ElapsedMilliseconds, (double)visitedmeasurer.ElapsedMilliseconds / solvingtimemeasurer.ElapsedMilliseconds, visitedmeasurer.CountCalls);
					Console.Write("{0, -50} {1, 10} ms this is {2, 8:P2} called {3, 10} \n", "Time wasted in smartplayerarea:", smartplayearreachableareameasurer.ElapsedMilliseconds, (double)smartplayearreachableareameasurer.ElapsedMilliseconds / solvingtimemeasurer.ElapsedMilliseconds, smartplayearreachableareameasurer.CountCalls);

					
#endif
					Thread.Sleep(100);
					Console.SetCursorPosition(0,0);
				}
				Console.Clear();
			}
		}

		void PrintState(State a, int[] positionshighlighted = null)
		{
			if (positionshighlighted == null) positionshighlighted = new int[] { -1 };
			Console.ResetColor();
			Console.Write("\n");
			for (int i = 0; i < columns; i++)
			{
				Console.BackgroundColor = ConsoleColor.Black;
				Console.ForegroundColor = ConsoleColor.Black;
				Console.Write("-");
				Console.ResetColor();
			}
			Console.Write("\n");
			for (int i = 0; i < Grid.Length; i++)
			{
				if (i % columns == 0) Console.Write("\n");
				if (Grid[i] == (char)Elements.WALL)
				{
					if(positionshighlighted.Contains(i))
					{

						Console.BackgroundColor = ConsoleColor.Cyan;
						Console.ForegroundColor = ConsoleColor.Cyan;
					}
					else
					{
						Console.BackgroundColor = ConsoleColor.Black;
						Console.ForegroundColor = ConsoleColor.Black;
					}
					Console.Write(Grid[i]);
				}
				else if (Grid[i] == (char)Elements.FREE || Grid[i] == (char)Elements.GOAL)
				{
					if (a.boxpositions.Contains(i))
					{
						if(positionshighlighted.Contains(i))
						{

							Console.BackgroundColor = ConsoleColor.Cyan;
							Console.ForegroundColor = ConsoleColor.Cyan;
						}
						else
						{
							Console.BackgroundColor = ConsoleColor.DarkGreen;
							Console.ForegroundColor = ConsoleColor.DarkGreen;
						}
						Console.Write((char)Elements.BOX);
					}
					else if (a.playerposition == i)
					{
						if (positionshighlighted.Contains(i))
						{

							Console.BackgroundColor = ConsoleColor.Cyan;
							Console.ForegroundColor = ConsoleColor.Cyan;
						}
						else
						{
							Console.BackgroundColor = ConsoleColor.White;
							Console.ForegroundColor = ConsoleColor.DarkGreen;
						}
						Console.Write((char)Elements.PLAYER);
					}
					else if (Grid[i] == (char)Elements.GOAL)
					{
						if (positionshighlighted.Contains(i))
						{

							Console.BackgroundColor = ConsoleColor.Cyan;
							Console.ForegroundColor = ConsoleColor.Cyan;
						}
						else
						{
							Console.BackgroundColor = ConsoleColor.DarkRed;
							Console.ForegroundColor = ConsoleColor.DarkRed;
						}
						Console.Write((char)Elements.GOAL);
					}
					else
					{
						if (positionshighlighted.Contains(i))
						{

							Console.BackgroundColor = ConsoleColor.Cyan;
							Console.ForegroundColor = ConsoleColor.Cyan;
						}
						else
						{
							Console.BackgroundColor = ConsoleColor.Gray;
							Console.ForegroundColor = ConsoleColor.Gray;
						}
						Console.Write(Grid[i]);
					}
				}
				Console.ResetColor();
			}

			Console.ResetColor();
			Console.Write("Pushes: {0}\n", a.boxpushes);
		}
		bool IsInALockedBoxPool(State s, int box, HashSet<int> walls)
		{
			if (box < 0 || box >= Grid.Length) return true;
			HashSet<int> checking = new HashSet<int>();
			walls.Add(box);
			bool movex = true, movey = true;
			int up = box - columns, down = box + columns, left = box - 1, right = box + 1;
			if (up < 0 || Grid[up] == (char)Elements.WALL || walls.Contains(up))
			{
				movey = false;
			}
			else if (s.boxpositions.Contains(up))
			{
				if (IsInALockedBoxPool(s, up, walls)) movey = false;
			}
			if (down >= Grid.Length || Grid[down] == (char)Elements.WALL || walls.Contains(down))
			{
				movey = false;
			}
			else if (s.boxpositions.Contains(down))
			{
				if (IsInALockedBoxPool(s, down, walls)) movey = false;
			}


			if (left < 0 || left < columns * (box / columns) || Grid[left] == (char)Elements.WALL || walls.Contains(left))
			{
				movex = false;
			}
			else if (s.boxpositions.Contains(left))
			{
				if (IsInALockedBoxPool(s, left, walls)) movex = false;
			}
			if (right >= Grid.Length || right >= columns * (box / columns + 1) || Grid[right] == (char)Elements.WALL || walls.Contains(right))
			{
				movex = false;
			}
			else if (s.boxpositions.Contains(right))
			{
				if (IsInALockedBoxPool(s, right, walls)) movex = false;
			}
			return !movex && !movey;
		}
		
		bool IsDeadLockBetweenBoxes(State s)
		{
			foreach (int box in s.boxpositions)
			{
				if(!goals.Contains(box) && IsInALockedBoxPool(s, box, new HashSet<int>())) return true;
			}
			return false;
		}

		private bool DeadLock(State tmpstate)
		{
			bool locked = true; // Checks if all boxes are locked
			foreach (int boxp in tmpstate.boxpositions)
			{
				int upperboundud = Grid.Length, lowerboundud = 0;
				int upperboundlr = columns * (boxp / columns + 1), lowerboundlr = columns * (boxp / columns);
				int up = boxp - columns, down = boxp + columns,
					left = boxp - 1, right = boxp + 1;
				bool canup = false, candown = false, canleft = false, canright = false;
				bool bup = false, bdown = false, bleft = false, bright = false;
				bool isgoal = goals.Contains(boxp);
				if (up >= lowerboundud)
				{
					if(tmpstate.boxpositions.Contains(up)) bup = true;
					else if (Grid[up] != (char)Elements.WALL) canup = true;
				}
				if(down < upperboundud)
				{
					if (tmpstate.boxpositions.Contains(down)) bdown = true;
					else if (Grid[down] != (char)Elements.WALL) candown= true;
				}
				if(left >= lowerboundlr)
				{
					if (tmpstate.boxpositions.Contains(left)) bleft = true;
					else if (Grid[left] != (char)Elements.WALL) canleft = true;
				}
				if(right < upperboundlr)
				{
					if (tmpstate.boxpositions.Contains(right)) bright = true;
					else if (Grid[right] != (char)Elements.WALL) canright = true;
				}
				if (!canup && !canleft) // Can't move the box
				{
					if (!bup && !bleft && !isgoal) return true; // It's a wall 
					locked = locked && true;// It's a box, might be moved
				}
				else if (!canup && !canright) // Can't move the box
				{
					if (!bup && !bright && !isgoal) return true; // It's a wall
					locked = locked && true;// It's a box, might be moved
				}
				else if (!candown && !canleft) // Can't move the box
				{
					if (!bdown && !bleft && !isgoal) return true; // It's a wall
					locked = locked && true; // It's a box, might be moved
				}
				else if (!candown && !canright) // Can't move the box
				{
					if (!bdown && !bright && !isgoal) return true; // It's a wall
					locked = locked && true; // It's a box, might be moved
				}
				else // Can move the box
					locked = locked && false;
			}
			return locked;
		}

		State solution = null;

#if DEBUG
		TimeSpan solvingtime;
#endif
		public IEnumerable<int> SolveParallel()
		{
			IEnumerable<int> l;

#if (DEBUG)
			l = solvingtimemeasurer.AddTimeElapsedIn(() => { return SolveParallelAux(); });
#else
			l = SolveParallelAux();
#endif
			return l;
		}

		bool CanAllBoxesReachFinalEvenWithBoxes(State s)
		{
			foreach (int box in s.boxpositions)
			{
				if (!CanBoxReachAFinalEvenWithBoxes(s, box)) return false;
			}
			return true;
		}

		bool CanBoxReachAFinalEvenWithBoxes(State s, int box)
		{
			if (goals.Contains(box)) return true;
			HashSet<int> otherboxes = new HashSet<int>(s.boxpositions);
			if (!otherboxes.Contains(box)) return false;
			otherboxes.Remove(box);
			otherboxes.IntersectWith(goals);
			if (otherboxes.Count == 0) return true;
			HashSet<int> pending = new HashSet<int>();
			HashSet<int> visited = new HashSet<int>();
			pending.Add(box);
			while (pending.Count() != 0)
			{
				int b = pending.ElementAt(0);
				pending.Remove(b);
				visited.Add(b);
				if (goals.Contains(b))
				{
					return true;
				}
				GenerateSuccessorsForBoxEvenWithBoxes(b, pending, visited, otherboxes);
			}
			return false;
		}
		void GenerateSuccessorsForBoxEvenWithBoxes(int boxp, HashSet<int> pending, HashSet<int> visited, HashSet<int> otherboxes)
		{
			int boxl = boxp - 1;
			int boxr = boxp + 1;
			int boxu = boxp - columns;
			int boxd = boxp + columns;

			if (!pending.Contains(boxl) && !visited.Contains(boxl) && CanBePushed(boxp, Direction.LEFT) && !otherboxes.Contains(boxl))
			{
				if (goals.Contains(boxl) || !IsBoxLockedByWalls(boxl)) pending.Add(boxl);
				else visited.Add(boxl);
			}
			if (!pending.Contains(boxr) && !visited.Contains(boxr) && CanBePushed(boxp, Direction.RIGHT) && !otherboxes.Contains(boxr))
			{
				if (goals.Contains(boxr) || !IsBoxLockedByWalls(boxr)) pending.Add(boxr);
				else visited.Add(boxr);
			}
			if (!pending.Contains(boxu) && !visited.Contains(boxu) && CanBePushed(boxp, Direction.UP) && !otherboxes.Contains(boxu))
			{
				if (goals.Contains(boxu) || !IsBoxLockedByWalls(boxu)) pending.Add(boxu);
				else visited.Add(boxu);
			}
			if (!pending.Contains(boxd) && !visited.Contains(boxd) && CanBePushed(boxp, Direction.DOWN) && !otherboxes.Contains(boxd))
			{
				if (goals.Contains(boxd) || !IsBoxLockedByWalls(boxd)) pending.Add(boxd);
				else visited.Add(boxd);
			}
		}
		object locktime = new object();
		static bool InsideGrid(int i)
		{
			return i >= 0 && i < Grid.Length;
		}
		void SetPlayerReachableArea(State s, int boxidxmoved = -1)
		{
			if (boxidxmoved != -1)
			{
				bool a, b, c, d, e, f, g, h, i; // A box/wall is there
				HashSet<int> positions = new HashSet<int>(ValidSquareNegativeDiagonal(s.boxpositions[boxidxmoved]));
				positions.UnionWith(ValidSquarePositiveDiagonal(s.boxpositions[boxidxmoved]));
				positions.UnionWith(ValidSquareNegativeDiagonal(s.playerposition));
				positions.UnionWith(ValidSquarePositiveDiagonal(s.playerposition));
				positions.UnionWith(ValidSquareSides(s.boxpositions[boxidxmoved]));
				if (!smartplayearreachableareameasurer.AddTimeElapsedIn(() =>
				{
					bool calculate = true;
					int direction = s.playerposition - s.boxpositions[boxidxmoved];
					if (direction == 1) // | |P|
					{
						// a|b|c|d
						// e|B|P|
						// f|g|h|i
						int
							n = s.boxpositions[boxidxmoved] - columns - 1;
						a = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
						n = s.boxpositions[boxidxmoved] - columns;
						b = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
						n = s.boxpositions[boxidxmoved] - columns + 1;
						c = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
						n = s.boxpositions[boxidxmoved] - columns + 2;
						d = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
						n = s.boxpositions[boxidxmoved] - 1;
						e = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
						n = s.boxpositions[boxidxmoved] + columns - 1;
						f = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
						n = s.boxpositions[boxidxmoved] + columns;
						g = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
						n = s.boxpositions[boxidxmoved] + columns + 1;
						h = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
						n = s.boxpositions[boxidxmoved] + columns + 2;
						i = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
					}
					else if (direction == -1) // |P|B|
					{
						// d|c|b|a
						//  |P|B|e
						// i|h|g|f
						int
							n = s.boxpositions[boxidxmoved] - columns + 1;
						a = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
						n = s.boxpositions[boxidxmoved] - columns;
						b = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
						n = s.boxpositions[boxidxmoved] - columns - 1;
						c = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
						n = s.boxpositions[boxidxmoved] - columns - 2;
						d = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
						n = s.boxpositions[boxidxmoved] + 1;
						e = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
						n = s.boxpositions[boxidxmoved] + columns + 1;
						f = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
						n = s.boxpositions[boxidxmoved] + columns;
						g = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
						n = s.boxpositions[boxidxmoved] + columns - 1;
						h = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
						n = s.boxpositions[boxidxmoved] + columns - 2;
						i = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
					}
					else if (direction == columns)
					{
						// f|e|a
						// g|B|b
						// h|P|c
						// i| |d
						int
							n = s.boxpositions[boxidxmoved] - columns + 1;
						a = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
						n = s.boxpositions[boxidxmoved] + 1;
						b = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
						n = s.boxpositions[boxidxmoved] + columns + 1;
						c = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
						n = s.boxpositions[boxidxmoved] + 2 * columns + 1;
						d = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
						n = s.boxpositions[boxidxmoved] - columns;
						e = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
						n = s.boxpositions[boxidxmoved] - columns - 1;
						f = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
						n = s.boxpositions[boxidxmoved] - 1;
						g = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
						n = s.boxpositions[boxidxmoved] + columns - 1;
						h = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
						n = s.boxpositions[boxidxmoved] + 2 * columns - 1;
						i = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
					}
					else if (direction == -columns)
					{
						// d| |i
						// c|P|h
						// b|B|g
						// a|e|f
						int
							n = s.boxpositions[boxidxmoved] + columns - 1;
						a = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
						n = s.boxpositions[boxidxmoved] - 1;
						b = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
						n = s.boxpositions[boxidxmoved] - columns - 1;
						c = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
						n = s.boxpositions[boxidxmoved] - 2 * columns - 1;
						d = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
						n = s.boxpositions[boxidxmoved] + columns;
						e = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
						n = s.boxpositions[boxidxmoved] + columns + 1;
						f = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
						n = s.boxpositions[boxidxmoved] + 1;
						g = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
						n = s.boxpositions[boxidxmoved] - columns + 1;
						h = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
						n = s.boxpositions[boxidxmoved] - 2 * columns + 1;
						i = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
					}
					else
					{
						a = b = c = d = e = f = g = h = i = true;
					}
					if (!(
							(a && !b && c) ||
							(a && !e && f && (!b || !g)) ||
							(a && !e && h && !b) ||
							(a && !g && h && !b) ||
							(b && !h && i) ||
							(b && !c && d) ||
							(!b && c && f && !g) ||
							(!c && d && i) ||
							(!c && d && g) ||
							(c && !h && i) ||
							(!c && d && h) ||
							(c && !e && f && !g) ||
							(d && !h && i) ||
							(f && !g && h) ||
							(g && !h && i)))
					{
						calculate = false;
					}
					if (!calculate)
					{
						s.playerreachablearea = s.playerreachablearea.ToArray();
						s.playerreachablearea[s.playerposition] = true;
						s.playerreachablearea[s.boxpositions[boxidxmoved]] = false;

					}
					return calculate;
				}))
				{
					//Console.WriteLine("Smart guessed");
					//PrintState(s, s.boxpositions[boxidxmoved]);
					return;
				}

			}
			State scpy = new State();
			scpy.playerposition = s.playerposition;
			scpy.boxpositions = s.boxpositions;
			bool[] area = new bool[Grid.Length];
			HashSet<int> visited = new HashSet<int>();
			Queue<int> pending = new Queue<int>();
			pending.Enqueue(scpy.playerposition);
			while(pending.Count != 0)
			{
				int tmp = pending.Dequeue();
				if (visited.Contains(tmp)) continue;
				area[tmp] = true;
				visited.Add(tmp);
				scpy.playerposition = tmp;
				int up = tmp - columns, 
					down = tmp + columns, 
					left = tmp - 1,
					right = tmp + 1;
				if (InsideGrid(up) && Grid[up] != (char)Elements.WALL && !scpy.boxpositions.Contains(up))
					pending.Enqueue(up);
				if (InsideGrid(down) && Grid[down] != (char)Elements.WALL && !scpy.boxpositions.Contains(down))
					pending.Enqueue(down);
				if (InsideGrid(left) && Grid[left] != (char)Elements.WALL && !scpy.boxpositions.Contains(left))
					pending.Enqueue(left);
				if (InsideGrid(right) && Grid[right] != (char)Elements.WALL && !scpy.boxpositions.Contains(right))
					pending.Enqueue(right);
			}
			s.playerreachablearea = area;
		}

		IEnumerable<int> ValidSquareSides(int squarepos)
		{
			if (squarepos < 0 || squarepos >= Grid.Length) yield break;
			int left = squarepos - 1;
			int right = squarepos + 1;
			int up = squarepos - columns;
			int down = squarepos + columns;
			if (left >= columns * (squarepos / columns) && left >= 0) yield return left;
			if (right < columns * (squarepos / columns + 1) && right < Grid.Length) yield return right;
			if (up >= 0) yield return up;
			if (down < Grid.Length) yield return down;
		}
		/// <summary>
		/// This diagonal /
		/// </summary>
		/// <param name="squarepos"></param>
		/// <returns></returns>
		IEnumerable<int> ValidSquarePositiveDiagonal(int squarepos)
		{
			if (squarepos < 0 || squarepos >= Grid.Length) yield break;
			int leftup = squarepos - 1 - columns;
			int rightdown = squarepos + columns + 1;
			if (leftup >= columns * (squarepos / columns) - columns && leftup >= 0) yield return leftup;
			if (rightdown < columns * (squarepos / columns + 1) + columns && rightdown < Grid.Length) yield return rightdown;
		}
		/// <summary>
		/// This diagonal \
		/// </summary>
		/// <param name="squarepos"></param>
		/// <returns></returns>
		IEnumerable<int> ValidSquareNegativeDiagonal(int squarepos)
		{
			if (squarepos < 0 || squarepos >= Grid.Length) yield break;
			int rightup = squarepos + 1 - columns;
			int leftdown = squarepos + columns - 1;
			if (rightup < columns * (squarepos / columns + 1) - columns && rightup >= 0) yield return rightup;
			if (leftdown >= columns * (squarepos / columns) + columns && leftdown < Grid.Length) yield return leftdown;
		}
		/// <summary>
		/// True if square pos is surrounded by walls
		/// If squarepos is not free, then this returns true immediately
		/// </summary>
		/// <param name="s"></param>
		/// <param name="squarepos"></param>
		/// <returns></returns>
		bool IsSquareClosed(State s, int squarepos)
		{
			if (squarepos < 0 || squarepos >= Grid.Length || s.boxpositions.Contains(squarepos)
					|| Grid[squarepos] == (char)Elements.WALL) return true;
			bool sideclosed = true;
			foreach (int squareside in ValidSquareSides(squarepos)) // |B|Side|Squareside| (surrounding Side)
			{
				sideclosed &= (s.boxpositions.Contains(squareside)
					|| Grid[squareside] == (char)Elements.WALL);
			}
			return sideclosed;
		}
		bool IsEmptySquare(int squarepos)
		{
			if (squarepos < 0 || squarepos >= Grid.Length || Grid[squarepos] == (char)Elements.WALL) return false;
			else return true;
		}

		/// <summary>
		/// Checks if the positive diagonal starting from square is closed
		/// </summary>
		/// <param name="s"></param>
		/// <param name="square"></param>
		/// <param name="positivediagonal">True: Checks if the positive diagonal is closed, False: Checks if the negative diagonal is closed</param>
		/// <returns></returns>
		bool IsDiagonalClosed(State s, int square, bool positivediagonal)
		{
			if (!IsEmptySquare(square)) return false;
			HashSet<int> squareschecked = new HashSet<int>();
			Queue<int> pendingsquares = new Queue<int>();
			pendingsquares.Enqueue(square);
			byte sidesclosed = 0;
			while (pendingsquares.Count != 0)
			{
				int tmpsquare = pendingsquares.Dequeue();
				squareschecked.Add(tmpsquare);

				foreach (int diagsq in 
					(positivediagonal) ?  
						ValidSquarePositiveDiagonal(tmpsquare) 
						: ValidSquareNegativeDiagonal(tmpsquare))
				{
					if (squareschecked.Contains(diagsq)) continue;
					if (IsEmptySquare(diagsq))
					{
						if (!goals.Contains(diagsq) && IsSquareClosed(s, diagsq))
							pendingsquares.Enqueue(diagsq);
						else
							return false;
					}
					else
					{
						sidesclosed++;
						if (sidesclosed == 2) return true;
					}
				}
			}
			return false;
		}

		bool DeadLock(State s, int boxmoved)
		{
			// These deadlocks ought to be sorted by performance, from less expensive to more expensive
			bool simpledeadlock = simpledeadlockmeasurer.AddTimeElapsedIn(() => !s.boxpositions.All(x => nodeadlocks.Contains(x) || goals.Contains(x)));
			if (simpledeadlock) return true;

			bool betweenboxesdeadlock = freezedeadlockmeasurer.AddTimeElapsedIn(() => IsDeadLockBetweenBoxes(s));
			if (betweenboxesdeadlock) return true;

			bool diagonaldeadlock = diagonaldeadlockmeasurer.AddTimeElapsedIn(() => IsDiagonalDeadLock(s, boxmoved));
			if (diagonaldeadlock) return true;
			return false;
		}

		bool IsDiagonalDeadLock(State s, int boxmoved)
		{
			foreach (int side in ValidSquareSides(boxmoved).Union(ValidSquarePositiveDiagonal(boxmoved).Union(ValidSquareNegativeDiagonal(boxmoved)))) // |B|Side| (surrounding B)
			{
				if (side == s.playerposition || goals.Contains(side)) continue;
				if(IsEmptySquare(side) && !s.boxpositions.Contains(side) && IsSquareClosed(s, side))
				{
					if (IsDiagonalClosed(s, side, true) || IsDiagonalClosed(s, side, false))
					{
#if DEBUG && DISPLAYDIAGONALDEADLOCKS && USEONECORE
						Console.WriteLine("DIAGONAL DEADLOCK:");
						PrintState(s, boxmoved);
#endif
						return true;
					}
				}
			}
			return false;
		}

		bool IsBlockedIn(State s, HashSet<int> validpositions, int positiontocheck)
		{
			if (validpositions.Contains(positiontocheck))
			{
				if (Grid[positiontocheck] == (char)Elements.WALL ||
					s.boxpositions.Contains(positiontocheck))
				{
					return true;
				}
			}
			else return true;
			return false;
		}

		IEnumerable<int> FindPosibleUnreachablePosByPlayer(State s, int boxasideplayer)
		{
			HashSet<int> sides = new HashSet<int>(
			ValidSquareSides(boxasideplayer).Union(
				ValidSquareSides(s.playerposition).Union(
					ValidSquarePositiveDiagonal(boxasideplayer).Union(
						ValidSquareNegativeDiagonal(boxasideplayer)))));
			bool[] squarehasobstacle = new bool[7];
			int[] positionstocheck = new int[7];
			int firstobst = -1, lastobst = -1;
			bool isdone = false;
			if (boxasideplayer - 1 == s.playerposition) // Box is right to the player
			{
				// Makes a trace like this:
				//  x x x
				// |P|B|x
				//  x x x
				// Visiting each x, it must find at least two obstacles, if it doesn't, then this returns
				// If it finds at least two obstacles it returns the first one free between two obstacles
				positionstocheck[0] = s.playerposition - columns;
				positionstocheck[1] = boxasideplayer - columns;
				positionstocheck[2] = boxasideplayer - columns + 1;
				positionstocheck[3] = boxasideplayer + 1;
				positionstocheck[4] = boxasideplayer + columns + 1;
				positionstocheck[5] = boxasideplayer + columns;
				positionstocheck[6] = s.playerposition + columns;
			}
			else if(boxasideplayer + 1 == s.playerposition) // Box is left to the player
			{
				// Makes a trace like this:
				// x x x
				// x|B|P|
				// x x x
				positionstocheck[0] = s.playerposition - columns;
				positionstocheck[1] = boxasideplayer - columns;
				positionstocheck[2] = boxasideplayer - columns - 1;
				positionstocheck[3] = boxasideplayer - 1;
				positionstocheck[4] = boxasideplayer + columns - 1;
				positionstocheck[5] = boxasideplayer + columns;
				positionstocheck[6] = s.playerposition + columns;
			}
			else if(boxasideplayer - columns == s.playerposition) // Box is below the player
			{
				// Makes a trace like this:
				// x|P|x 
				// x|B|x
				// x x x
				positionstocheck[0] = s.playerposition - 1;
				positionstocheck[1] = boxasideplayer - 1;
				positionstocheck[2] = boxasideplayer + columns - 1;
				positionstocheck[3] = boxasideplayer + columns;
				positionstocheck[4] = boxasideplayer + columns + 1;
				positionstocheck[5] = boxasideplayer + 1;
				positionstocheck[6] = s.playerposition + 1;
			}
			else if(boxasideplayer + columns == s.playerposition) // Box is above the player
			{
				// Makes a trace like this:
				// x x x 
				// x|B|x
				// x|P|x
				positionstocheck[0] = s.playerposition - 1;
				positionstocheck[1] = boxasideplayer - 1;
				positionstocheck[2] = boxasideplayer - columns - 1;
				positionstocheck[3] = boxasideplayer - columns;
				positionstocheck[4] = boxasideplayer - columns + 1;
				positionstocheck[5] = boxasideplayer + 1;
				positionstocheck[6] = s.playerposition + 1;
			}
			for (int i = 0; i < 7; i++)
			{
				squarehasobstacle[i] = IsBlockedIn(s, sides, positionstocheck[i]);
				if (squarehasobstacle[i])
				{
					lastobst = i;
					if (firstobst == -1) firstobst = i;
				}
			}
			if (firstobst < 0 || firstobst == lastobst) yield break;
			for (int i = firstobst + 1; i < lastobst; i++)
			{
				if (!squarehasobstacle[i])
				{
					if (!isdone)
					{
						yield return positionstocheck[i];
						isdone = true;
					}
				}
				else isdone = false;
			}
			yield break;
		}
		HashSet<int> UnreachableAreaByPlayer(State s, int boxasideplayer)
		{
			
			foreach (int square in FindPosibleUnreachablePosByPlayer(s, boxasideplayer))
			{
				HashSet<int> zone = GetFreeZoneWithBoxesFrom(square, s);
				if (zone != null) return zone;
			}
			return null;
			
		}

		private HashSet<int> GetFreeZoneWithBoxesFrom(int square, State s)
		{
			State stmp = new State();
			stmp.boxpositions = s.boxpositions;
			HashSet<int> zone = new HashSet<int>();
			Queue<int> pending = new Queue<int>();
			pending.Enqueue(square);
			while (pending.Count != 0)
			{
				int tmp = pending.Dequeue();
				if (tmp == s.playerposition) return null;
				if (zone.Contains(tmp)) continue;
				zone.Add(tmp);
				stmp.playerposition = tmp;
				int up = tmp - columns, down = tmp + columns, left = tmp - 1, right = tmp + 1;
				if (CanBeMoved(stmp, Direction.UP)) pending.Enqueue(up);
				else if (stmp.boxpositions.Contains(up)) zone.Add(up);

				if (CanBeMoved(stmp, Direction.DOWN)) pending.Enqueue(down);
				else if (stmp.boxpositions.Contains(down)) zone.Add(down);

				if (CanBeMoved(stmp, Direction.LEFT)) pending.Enqueue(left);
				else if (stmp.boxpositions.Contains(left)) zone.Add(left);

				if (CanBeMoved(stmp, Direction.RIGHT)) pending.Enqueue(right);
				else if (stmp.boxpositions.Contains(right)) zone.Add(right);
			}
			return zone;
		}
		private StateOneBox PushBoxTo(State s, int i, int target)
		{
			if (s.boxpositions.Contains(target) || 
				target < 0 || target >= Grid.Length || 
				Grid[target] == (char) Elements.WALL || 
				i < 0 || i>= Grid.Length || !s.boxpositions.Contains(i)) return null;
			PriorityQueue<StateOneBox> pendinglocal = new PriorityQueue<StateOneBox>();
			StateOneBox sob = new StateOneBox(s);
			sob.currentbox = i;
			HashSet<StateOneBox> visited = new HashSet<StateOneBox>(new StateOneBoxComparer());
			pendinglocal.Enqueue(s.f, sob);
			while (pendinglocal.Count != 0)
			{
				StateOneBox tmp = pendinglocal.Dequeue();
				if (visited.Contains(tmp)) continue;
				visited.Add(tmp);
				if (tmp.currentbox == target)
				{
					return tmp;
				}
				GenerateSuccesorsPushBoxToGoals(tmp, pendinglocal);
			}
			return null;
		}


		static class ZobrishHash
		{
			public static int[,] array;
			static Random r = new Random();
			public static void Initialize()
			{
				array = new int[Grid.Length, 2];
				for (int i = 0; i < Grid.Length; i++)
				{
					for (int j = 0; j < 2; j++)
					{
						array[i, j] = r.Next();
					}
				}
			}
			public static int GetZobrishHash(State s)
			{
				int zkey = 0;
				for (int i = 0; i < Grid.Length; i++)
				{
					if (s.boxpositions.Contains(i)) zkey ^= array[i, 1];
					else zkey ^= array[i, 0];
				}
				return zkey;
			}
			public static int GetZobrishHashAfterMove(State s, int before, int now)
			{
				return s.GetHashCode() ^ array[before, 0] ^ array[before, 1] ^ array[now, 0] ^ array[now, 1];
			}
		}
		int numberofnowalls = 0;
		int numberofboxes = 0;
		Test.TimeMeasurer boxtogoalmeasurer = new Test.TimeMeasurer();
		Test.TimeMeasurer pushboxmeasurer = new Test.TimeMeasurer();
		Test.TimeMeasurer deadlockmeasurer = new Test.TimeMeasurer(true);
		Test.TimeMeasurer diagonaldeadlockmeasurer = new Test.TimeMeasurer(true);
		Test.TimeMeasurer simpledeadlockmeasurer = new Test.TimeMeasurer(true);
		Test.TimeMeasurer freezedeadlockmeasurer = new Test.TimeMeasurer(true);
		Test.TimeMeasurer visitedmeasurer = new Test.TimeMeasurer();
		Test.TimeMeasurer reachableareameasurer = new Test.TimeMeasurer();
		Test.TimeMeasurer solvingtimemeasurer = new Test.TimeMeasurer();
		Test.TimeMeasurer smartplayearreachableareameasurer = new Test.TimeMeasurer();
		IEnumerable<int> SolveParallelAux()
		{
			if (!canbesolved) return new List<int>();
			PriorityQueue<State> pending = new PriorityQueue<State>();
			pending.Mode = PriorityQueue<State>.NodeMode.Queue;
			ZobrishHash.Initialize();
			HashSet<State> visited = new HashSet<State>(new StateComparerByPlayerZone());
			pending.Enqueue(0, initial);
			initial.heuristic = GetHeuristicBoxesToGoal(initial);
			initial.SetHashCode();
			ParallelOptions po = new ParallelOptions();
			po.MaxDegreeOfParallelism =
#if USEONECORE
				1;
#else
			Environment.ProcessorCount;
#endif
			while (pending.Count != 0)
			{
				State s = pending.Dequeue();
				if (visitedmeasurer.AddTimeElapsedIn(() => visited.Contains(s))) continue;
				visited.Add(s);
#if DEBUG && SHOWDEQUEUEDSTATE && USEONECORE
				
				//Console.Write("DE QUEUING f: "+s.f + "\n");
				PrintState(s);
#endif
				if (goals.SetEquals(s.boxpositions))
				{
					List<int> r = new List<int>();
					solution = s;
					while(s != null)
					{
						r.Insert(0, s.playerposition);
						s = s.previousState;
					}
					return r;
				}
				PriorityQueue<int> pq = SortBoxesToGoalByDistance(s);
				Parallel.For(0, pq.Count, po, (ind1) =>
				{
					int boxidx = pq.Dequeue();
					PriorityQueue<int> sq = SortPlayerToSideBoxDistance(s.playerposition, s.boxpositions[boxidx]);
					Parallel.For(0, sq.Count, po, (ind2, lo) => {
						int sidebox = sq.Dequeue();
						State s1 = JumpPlayerTo(s, sidebox);
						if (s1 == null) return; // The player doesn't have access
#if FOCUSONEBOXTOGOAL
						State s3 = boxtogoalmeasurer.AddTimeElapsedIn(() => PushBoxToClosestGoal(s1, boxidx));
						if (s3 != null)
						{
							s3.f = Int32.MinValue + s3.f;
							pending.Enqueue(s3.f, s3);
						}
#endif
						
						State s2 = pushboxmeasurer.AddTimeElapsedIn(() => PushBox(s1, boxidx));
						if (s2 != null)
						{
							if (deadlockmeasurer.AddTimeElapsedIn(()=>!DeadLock(s2, s2.boxpositions[boxidx])))
							{
								reachableareameasurer.AddTimeElapsedIn(() => SetPlayerReachableArea(s2, boxidx));
								if (goals.Contains(s1.boxpositions[boxidx]))
								{
									s2.cumulatedlength = 0;
									s2.f = 0;
								}
								pending.Enqueue(s2.f, s2);
							}
						}
					});
				});
			}
			return new List<int>();
		}

		private State PushBoxToClosestGoal(State state, int boxidx)
		{
			PriorityQueue<State> pending = new PriorityQueue<State>();
			pending.Mode = PriorityQueue<State>.NodeMode.Queue;
			HashSet<State> visited = new HashSet<State>(new StateComparerByPlayerZone());
			pending.Enqueue(state.f, state);
			ParallelOptions po = new ParallelOptions();
			po.MaxDegreeOfParallelism =
#if USEONECORE
				1;
#else
			Environment.ProcessorCount;
#endif
			while (pending.Count != 0)
			{
				State s = pending.Dequeue();
				if (visited.Contains(s)) continue;
				visited.Add(s);
				if (goals.Contains(s.boxpositions[boxidx]))
				{
					return s;
				}

				PriorityQueue<int> sq = SortPlayerToSideBoxDistance(s.playerposition, s.boxpositions[boxidx]);
				Parallel.For(0, sq.Count, po, (ind2, lo) => {
					int sidebox = sq.Dequeue();
					State s1 = JumpPlayerTo(s, sidebox);
					if (s1 == null) return; // The player doesn't have access
					State s2 = PushBox(s1, boxidx);
					if (s2 != null)
					{
						if (!DeadLock(s2, s2.boxpositions[boxidx]))
						{
							SetPlayerReachableArea(s2);
							pending.Enqueue(s2.f, s2);
						}
					}
				});
			}
			return null;
		}
		
		class StateOneBox : State
		{
			public StateOneBox() { }
			/// <summary>
			/// This doesn't copy, only sets references as if it were s wihtout StateOneBox attributes initialized
			/// </summary>
			/// <param name="s"></param>
			public StateOneBox(State s) 
			{
				if(s != null)
				{
					playerreachablearea = s.playerreachablearea;
					boxpushes = s.boxpushes;
					previousState = s.previousState;
					boxpositions = s.boxpositions;
					playerposition = s.playerposition;
					heuristic = s.heuristic;
					f = s.f;
					cumulatedlength = s.cumulatedlength;
				}
			}
			public int currentbox;
		}
		class StateOneBoxComparer : EqualityComparer<StateOneBox>
		{
			public override bool Equals(StateOneBox x, StateOneBox y)
			{
				return x.playerreachablearea[y.playerposition] && x.currentbox == y.currentbox;
			}
			public override int GetHashCode(StateOneBox obj)
			{
				return base.GetHashCode();
			}
		}



		private void GenerateSuccesorsPushBoxToGoals(StateOneBox tmp, PriorityQueue<StateOneBox> pending)
		{
			PriorityQueue<int> sq = SortPlayerToSideBoxDistance(tmp.playerposition, tmp.currentbox);
			while(sq.Count != 0)
			{
				int playerpos = sq.Dequeue();
				State s = PushBox(JumpPlayerTo(tmp, playerpos), tmp.currentbox);
				if (s == null) continue;
				StateOneBox sob = new StateOneBox(s);
				sob.currentbox = 2*tmp.currentbox - playerpos;
				pending.Enqueue(sob.f, sob);
			}
		}
		
		State PushBox(State s, int boxidx)
		{
			if (s == null) return null;
			int boxpos = s.boxpositions[boxidx];
			int t = s.playerposition - boxpos;
			State tn = new State();
			tn.heuristic = s.heuristic;
			tn.hashcode = s.hashcode;
			tn.playerreachablearea = s.playerreachablearea;
			tn.boxpushes = s.boxpushes + 1;
			tn.playerposition = boxpos;
			tn.boxpositions = s.boxpositions.ToArray();//Copying it
			tn.cumulatedlength = s.cumulatedlength + 1;
			tn.previousState = s;
			if (t == 1) // Move player/box left
			{
				int npos = boxpos - 1;
				if (tn.boxpositions.Contains(npos)) return null;
				tn.SetHashCodeAfterMove(boxpos, npos);
				//tn.currentbox = boxpos - 1;
				tn.boxpositions[boxidx] = npos;
			}
			else if(t == -1) // Move player/box right
			{
				int npos = boxpos + 1;
				if (tn.boxpositions.Contains(npos)) return null;
				tn.SetHashCodeAfterMove(boxpos, npos);
				//tn.currentbox = boxpos + 1;
				tn.boxpositions[boxidx]  = npos;
			}
			else if(t == columns) // Move player/box up
			{
				int npos = boxpos - columns;
				if (tn.boxpositions.Contains(npos)) return null;
				tn.SetHashCodeAfterMove(boxpos, npos);
				//tn.currentbox = boxpos - columns;
				tn.boxpositions[boxidx] = npos;
			}
			else if(t == -columns) // Move player/box down
			{
				int npos = boxpos + columns;
				if (tn.boxpositions.Contains(npos)) return null;
				tn.SetHashCodeAfterMove(boxpos, npos);
				//tn.currentbox = boxpos + columns;
				tn.boxpositions[boxidx] = npos;
			}
			tn.heuristic = GetHeuristicBoxesToGoal(tn, boxpos, boxidx);
			tn.f = tn.heuristic + tn.cumulatedlength;
			
			return tn;
		}


		// Sum of the minimal distance of a box to a goal. THIS ONLY TAKES THE LAST BOX MOVED
		int GetHeuristicBoxesToGoal(State s, int boxprevpos, int boxnewidx)
		{
			int distprev = Int32.MaxValue, distnow = Int32.MaxValue;
			foreach (int g in boxtogoals[boxnewidx])
			{
				int dprev = ManhattanDistance(g, boxprevpos);
				int dnow = ManhattanDistance(g, s.boxpositions[boxnewidx]);
				if (dprev > distprev) distprev = dprev;
				if (dnow > distnow) distnow = dnow;
			}
			return s.heuristic - distprev + distnow;
		}


		// Sum of the minimal distance of a box to a goal
		int GetHeuristicBoxesToGoal(State s)
		{
			int total = 0;
			for (int i = 0; i < boxtogoals.Length; i++)
			{
				int h = Int32.MaxValue;
				foreach (int d in boxtogoals[i]) // Minimal distance from a box to its reachable(boxes in the path are ignored) goal
				{
					int dist = ManhattanDistance(d, s.boxpositions[i]);
					if (dist < h) h = dist;
				}
				total += h;
			}
			return total;
		}
		/// <summary>
		/// This is faster than moving the player, it uses the player reachable area to check if it can jump
		/// and then gives a new state with the only difference of the position of the player
		/// and the previous state is the one passed by parameters
		/// </summary>
		/// <param name="s"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		State JumpPlayerTo(State s, int target)
		{
			if (InsideGrid(target) && s.playerreachablearea[target])
			{
				State r = new State();
				r.boxpushes = s.boxpushes;
				r.boxpositions = s.boxpositions;
				r.cumulatedlength = s.cumulatedlength;
				r.f = s.f;
				r.heuristic = s.heuristic;
				r.playerposition = target;
				r.playerreachablearea = s.playerreachablearea;
				r.previousState = s;
				r.hashcode = s.hashcode;
				return r;
			}
			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="initials"></param>
		/// <param name="target"></param>
		/// <param name="returninitialdistance">Helps to find the minimal amount of pushes (outside this function) if true, helps to find the minimal amount of movements if false</param>
		/// <returns></returns>
		State MovePlayerTo(State initials, int target, bool returninitialdistance)
		{
			//if (!initials.playerreachablearea.Contains(target)) return null;
			if (target < 0 || target >= Grid.Length || Grid[target] == (char)Elements.WALL || initials.boxpositions.Contains(target)) return null;
			PriorityQueue<State> pending = new PriorityQueue<State>();
			HashSet<int> visited = new HashSet<int>();
			pending.Enqueue(initials.f, initials);
			while(pending.Count != 0)
			{
				State t = pending.Dequeue();
				visited.Add(t.playerposition);
				if(t.playerposition == target)
				{
					if (returninitialdistance)
					{
						t.f = initials.f;
						t.cumulatedlength = initials.cumulatedlength;
						t.heuristic = initials.heuristic;
					}
					return t;
				}
				GenerateSuccesorsMPTo(t, pending, visited, target);
			}
			return null;
		}
		// Checks if the player can be moved to a specific direction
		// Takes goals and walls into account when deciding where to move
		bool CanBeMoved(State t, Direction dir)
		{
			int n = 0;
			int lowerbound = 0, upperbound = Grid.Length;
			if (t.playerposition < lowerbound || t.playerposition >= upperbound) return false;
			switch (dir)
			{
				case Direction.LEFT:
					n = t.playerposition - 1;
					
					break;
				case Direction.RIGHT:
					n = t.playerposition + 1;
					break;
				case Direction.UP:
					n = t.playerposition - columns;
					break;
				case Direction.DOWN:
					n = t.playerposition + columns;
					break;
			}
			if(dir == Direction.LEFT || dir == Direction.RIGHT)
			{
				upperbound = columns * (t.playerposition / columns+1);
				lowerbound = columns * (t.playerposition / columns);
			}
			if (n < lowerbound || n >= upperbound || Grid[n] == (char)Elements.WALL || t.boxpositions.Contains(n)) return false;
			else return true;
		}

		private void GenerateSuccesorsMPTo(State t, PriorityQueue<State> pending, HashSet<int> visited, int target)
		{
			for (int i = 0; i < 4; i++)
			{
				Direction d = (Direction)i;
				int phase = 0;
				switch (d)
				{
					case Direction.LEFT:
						phase = -1;
						break;
					case Direction.RIGHT:
						phase = 1;
						break;
					case Direction.UP:
						phase = -columns;
						break;
					case Direction.DOWN:
						phase = columns;
						break;
				}
				if (!visited.Contains(t.playerposition + phase) && CanBeMoved(t, d))
				{
					State tn = new State();
					tn.playerreachablearea = t.playerreachablearea;
					tn.boxpushes = t.boxpushes;
					tn.playerposition = t.playerposition + phase;
					tn.boxpositions = t.boxpositions;
					tn.cumulatedlength = t.cumulatedlength + 1;
					tn.heuristic = ManhattanDistance(tn.playerposition, target);
					tn.previousState = t;
					tn.f = tn.heuristic + tn.cumulatedlength;
					pending.Enqueue(tn.f, tn);
				}
			}
		}

		PriorityQueue<int> SortPlayerToSideBoxDistance(int player, int box)
		{
			PriorityQueue<int> r = new PriorityQueue<int>();
			if(CanBePushed(box, Direction.UP))
			{
				// Pushed up from down
				int npos = box + columns;
				r.Enqueue(ManhattanDistance(player, npos), npos);
			}
			if(CanBePushed(box, Direction.DOWN))
			{
				int npos = box - columns;
				r.Enqueue(ManhattanDistance(player, npos), npos);
			}
			if (CanBePushed(box, Direction.LEFT))
			{
				int npos = box + 1;
				r.Enqueue(ManhattanDistance(player, npos), npos);
			}
			if (CanBePushed(box, Direction.RIGHT))
			{
				int npos = box - 1;
				r.Enqueue(ManhattanDistance(player, npos), npos);
			}
			return r;
		}
		PriorityQueue<int> SortBoxesToGoalByDistance(State s)
		{
			PriorityQueue<int> r = new PriorityQueue<int>();
			for (int i = 0; i < s.boxpositions.Length; i++) // Foreach box
			{
				// Foreach goal reachable from that box
				// Find the minimal distance
				int minimald = Int32.MaxValue;
				foreach (int v in boxtogoals[i])
				{
					int dist = ManhattanDistance(s.boxpositions[i], v);
					if (minimald > dist) minimald = dist;
				}
				r.Enqueue(minimald, i);
			}
			return r;
		}
		int ManhattanDistance(int p1, int p2)
		{
			int x1 = p1 / columns;
			int y1 = p1 % columns;
			int x2 = p2 / columns;
			int y2 = p2 % columns;
			int r = Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
			return r;
		}
	}
	class Program
	{
		static void Main(string[] args)
		{
#if DEBUG
			string filename = "input.txt";
			if (!System.IO.File.Exists(filename)) return;
			System.IO.StreamReader sr = new System.IO.StreamReader(filename);
			while (!sr.EndOfStream)
			{
				char[] grid = sr.ReadLine().ToArray();
				int columns = (int)Math.Sqrt(grid.Length);
				if (grid.Length <= 0 || columns * columns != grid.Length) continue;
				SokobanSolver ss = new SokobanSolver(grid, columns);
				CancellationTokenSource cts = new CancellationTokenSource();
				Thread th = new Thread(() =>
				{
					ss.SolveParallel().ToList<int>().ForEach(x =>
					{

						Console.WriteLine("({0},{1})", x / columns, x % columns);

					});
					ss.PrintSolution();
				});
				th.Start();
				Task rk = Task.Run(() => Console.ReadKey());
				rk.Wait();
				try
				{
					th.Abort();
				}
				catch(ThreadAbortException) { }
				Console.Clear(); GC.Collect();
			}
#endif
		}
	}
}
