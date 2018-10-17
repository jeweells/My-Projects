// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Homework 6
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Date: June 08, 2018
// :: Author: Abraham José Pacheco Becerra
// :: E-Mail: abraham.pacheco6319@gmail.com
// :: Description: Solves the sokoban game for minimal pushes using A* algorithm
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Compilation
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: It has been successfully proved in Visual Studio Community 2017 15.2
// :: Also https://repl.it/@jeweells/Sokoban
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Input
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: A line containing the map of the sokoban level, this level must be square, this means, the level has
// :: the same amount of columns and rows.
// :: How to define which elements are in the level?
// :: J player
// :: C box
// :: . empty square
// :: * wall
// :: F final for a box
// :: Example: ...................*.C.....*......CJ*.....F*.F..................
// :: That line has 64 characters, hence, sqrt(64) = 8 columns and rows
// :: Which represents the following map
// :: ........
// :: ........
// :: ...*.C..
// :: ...*....
// :: ..CJ*...
// :: ..F*.F..
// :: ........
// :: ........
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Output
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: This will return the directions the player must move in the first line and the number of pushes
// :: (minimal pushes) realized in the second line.
// :: Taking into account to read this ouput:
// :: n player moves up
// :: s player moves down
// :: e player moves right
// :: o player moves left
// :: Example:
// :: For the following input: FF*...FFC.C...*C*...CJ..............
// :: That line has 36 characters, hence, sqrt(36) = 6 columns and rows
// :: This represents the following map
// :: FF*...
// :: FFC.C.
// :: ..*C*.
// :: ..CJ..
// :: ......
// :: ......
// :: And the output will be:
// :: eennnoosoosoneeeneesoooossseenoenseennoooessosoonnensossenn
// :: 14
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Explanation
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: In order to find the best amount of pushes A* algorithm is required.
// :: A state is a 'photo' every time a move is made.
// :: A* uses, for every state, a heuristic of reaching the final state + the cumulated cost of reaching this state
// :: For the sokoban game, the cumulated cost is simple: if the player has been moved to the right 3 times,
// :: the cumulated cost is 3
// :: The heuristic used in this program is calculated as follows:
// :: -Find the distances of every box to the closest goal and sum them all
// ::
// :: Either the heuristic and the cumulated cost are summed and this represents 'f' a number which will be used
// :: to insert this state in a priority queue sorted by this f, where the minimal f will be dequeued first
// :: A* needs a structure where to store visited states so that it's not needed to pass through the same state every
// :: time a move is made.
// ::
// :: Two states are the same, for finding minimal pushes and some other cases, if the boxes are in the same position
// :: and the player is in the same reachable area.
// ::
// :: The reachable area is the area the player can move freely. HOWEVER, in this program the reachable area of
// :: the player will ONLY be the sides of boxes where the player can move to.
// ::
// :: This way a lot of memory and time is saved, since for every state this area needs to be calculated and stored
// ::
// :: The player will 'jump' to the side of a box (the cost of doing this is 0, since we are finding the least
// :: amount of pushes).
// ::
// :: No need to worry about the player jumping, once the solution is found, it is possible to fill the states where
// :: the player has jumped by moving the player normally from the state A to the state B (using another A*
// :: algorithm where the heuristic is the only thing that changes, and it is the minimal distance without
// :: taking into account the obstacles (Manhattan distance) from the player position to its 'goal').
// ::
// :: Zobrish hash, in order to store the visited states and be able to check quickly if the state X has been visited,
// :: we need to use a hash set and the zobrish hash. To implement this we generate two grids the same size as the
// :: sokoban grid and fill them with random numbers. One grid will indicate there's a free, wall, final or player
// :: (it doesn't matter) position while the other grid will indicate there's a box.
// ::
// :: Calculating this hash code will be divided in two parts in order to save time:
// :: Having a variable stores the hashcode starting in 0
// :: 1)The initial state is created and we need to calculate the hashcode by checking EVERY position.
// :: 	-If it's a wall, final, free or player position we take the number in this position from the first 'random' grid
// :: 	and XOR it with the variable
// :: 	-If it's a box we take the number in this position from the second 'random' grid and XOR it with the variable
// :: 2)A box was moved and the hashcode needs to be updated
// :: 	-Find the random number in the second grid where the box was before the move, we call this A
// :: 	-Find the random number in the second grid where the box is, we call this B
// :: 	-Find the random number in the first grid where the box was before the move, we call this C
// :: 	-Find the random number in the first grid where the box is, we call this D
// :: 	-Now what's left is XORing all that with the hashcode, so the result will be: hashcode = hashcode XOR A XOR B XOR C XOR D
// :: 	What did we do? Applying the following concept:
// :: 	-If hashcode = A XOR B
// :: 	-And we do hashcode XOR B, we will get A, in other words... A XOR B XOR A = B
// ::
// :: Deadlocks, what are they?
// ::
// :: A deadlock occurs when no matter what the player does it is not possible to solve the game.
// :: Hence, we can't let the player to waste time moving boxes when there's no a solution.
// :: There are a lot of deadlocks but in this program only few were applied
// ::
// :: -Simple deadlock
// :: This deadlock occurs when a box is pushed aside a wall and thanks to this it will no longer can be pushed
// :: Calculating this deadlock is simple and fast using BFS, this is done once.
// :: For each box, is moved taking into account that no matter the position this was moved to, there must be a free position
// :: in the opposite position (e.g. if moved horizontally, there must have free positions either left and right of the box to
// :: perform this move). Also, after pushing this box, we need to check this box is movable again, with this we can create the
// :: successors. Every time we make a move, we store that position, when this has finished we will have a zone where the boxes
// :: can move freely.
// ::
// :: -Diagonal deadlock
// :: This deadlock occurs when a box is pushed and if we try to push this box or any box ubicated diagonally to this box
// :: and we find that we'll no longer have the possibility of moving a box in this diagonall to a goal
// :: Calculating this deadlock is a bit harder for the time this uses, every time a box is pushed we check it's diagonals
// :: starting from the box position, if a free position isn't found in some of these two diagonals, then this turns out into a
// :: diagonal deadlock. Once a free position is found, this diagonal can't be a diagonal deadlock.
// :: We need to have into account that there can't be boxes in these diagonals, if we find a goal, it's taken as a free position.
// :: if this has a box above it's taken as a wall
// ::
// :: -Deadlock between boxes
// :: This deadlock occurs when pushing a box close to another box makes that all boxes can't be pushed to a goal anymore
// :: Calculating this deadlock is simple, everytime a box is pushed, we check if the box can be moved horizontally or vertically
// :: if this box can be moved because a box is there, we store this box and this box will act as a wall now and check if the box
// :: that is blocking this box can be moved horizontally or vertically. If there's a box blocking the movement of this box,
// :: we store this box and check if the other box can be moved and so on, until all a box can be moved (which means this is NOT
// :: a deadlock between boxes) or no nearby boxes can be moved (which means this IS a deadlock between boxes).
// :: If the box pushed is in a goal, this deadlock will no be computed
// ::
// :: Other characteristics
// ::
// :: -The grid is stored in a lineal array (it's faster to access a position and will use less memory when storing the positions)
// ::
// :: -Each state has a previous state so that the path can be formed once a solution is found
// ::
// :: -While finding the simple deadlocks it is possible to find which goals a box can reach. These are stored too.
// ::
// :: -The grid is padded by walls the whole surrounding
// :: e.g:
// :: for the following grid:
// :: ...
// :: JCF
// :: ...
// :: will be padded as:
// :: *****
// :: *...*
// :: *JCF*
// :: *...*
// :: *****
// :: Why? As we're using a lineal array to store the grid, moving left or right can be seen as jumping up or down to the next row:
// :: ...                    ..J
// :: JCF Moving it left ->  .CF
// :: ...                    ...
// :: Of course this can be calculated by bounds such as columns * (int)(position/columns) and columns * (int)(position/ columns + 1)
// :: But this takes more time than only padding the grid
// ::
// :: -As in the game of Sudoku, headsets can be found while boxes can reach some goals, but if there's a hidden set, we can know that
// :: it is imposible for a box to reach X goal because that will be occupied by another box.
// :: e.g: B1{1,2,3,4} B2{1,2} B3{1,2} B4{1,3,4}
// :: Here we see the box B1 can reach the goals 1,2,3,4 the box B2 can reach the goals 1,2 and so on
// :: but here there's a hidden set, since B2 and B3 can reach the goals 1,2. It only has these posibilities:
// :: B2 in 1
// :: B3 in 2
// :: or
// :: B2 in 2
// :: B3 in 1
// :: No more than that can happen, so, goals 1, 2 are occupied by B2 and B3 so in B1 the reachable goals will be {3,4} and not
// :: {1,2,3,4}, so will that in B4 as {3,4}
// :: Passing from B1{1,2,3,4} B2{1,2} B3{1,2} B4{1,3,4}
// :: 		  to B1{3,4} B2{1,2} B3{1,2} B4{3,4}
// :: Of course this is one of the simplest cases. This can be extended for more goals in every box and combinations such as (1,2)-(2,3)-(1,3)
// :: or (1,2,3,4)-(1,2,3,5)-(1,2,4,5)-(1,3,4,5)-(2,3,4,5)
// ::
// :: How to find these hidden sets?
// ::
// :: First we need to build a graph, identifying which node is a goal and which is not.
// :: The edge will go from a box to its reachable goal and vice-versa (bidirectional)
// :: We need a set of boxes selected.
// :: (1)
// :: Create a queue of boxes selected that are not in the set of boxes selected
// :: Now we need to walk throughout the graph, starting from one box (this will be done for each box)
// :: We use states where will be stored
// :: 	-A set of goals visited
// :: 	-A set of boxes visited (this will have the selected box to start with)
// :: 	-A set of current targets (starting from the goals the box selected can reach)
// :: 	-And of course the current box or goal
// :: The way we walk throughout the graph will be as follows:
// :: 	-Starting from the box we go to all reachable goals that are not in the set of goals visited, pushing each state onto a stack,
// :: 	for each state a goal will be stored in goals visited
// :: 	-Now we go to all boxes that are not in the boxes visited, pushing each state onto a stack, for each state a box will be stored in boxes visited
// :: 	-Before pushing a box, the state will be a goal and we check all the targets that box can reach, if there's one that is not in the list of targets
// :: 	we add it on current targets of that state
// :: 	-After popping a goal, we see if this goal was visited, if not we see if its goals visited are the same than its current targets
// :: 	if they are the same we need to see if this goal is part of the targets of the FIRST state, if this happens then we have a match
// :: 	-A match contains current targets and boxes visted. This means that all boxes that are not in boxes visited will no longer point
// :: 	to the goals that are in current targets, and the boxes visited will only point to the current targets
// :: 	-If a match never occurs then we start with another box in the queue of boxes selected
// :: This boxes visited are added into the set of boxes selected and we go to (1)
// :: If the queue is empty then we have no more hidden sets.
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

#undef HOMEWORK6

#if !HOMEWORK6
#undef DEBUG
#undef SHOWDEQUEUEDSTATE
#undef SHOWREMOVEGOALSRESULT
#undef DISPLAYDIAGONALDEADLOCKS
#undef DISPLAYPLAYERREACHABLEAREA
#undef DISPLAYSMARTPLAYERREACHABLEAREA
#define USEONECORE
#else
#undef DEBUG
#define USEONECORE
#endif
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
			while (pq.Count != 0)
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
					if (boxpairs.Contains(nod)) // Deletes what it's not in goalpairs
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
		// It is recommended to set hiddensetmaxLength less than N where N are the number of elements, otherwise this might get stuck or throw unexpected
		// results
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
			s.bvisited = new HashSet<Node>() { start };
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
			while (pending.Count != 0)
			{
				cases++;
				State t = pending.Pop();
				if (t.targets.Count >= hiddensetmaxlength) continue; // In case there are no hiddensets this is needed
				if (t.n.isgoal) // If this is a goal
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
						goalpairs.UnionWith(t.targets); // Add the targets that were needed to visit
						boxpairs.UnionWith(t.bvisited); // Add the boxes visited throughout the path
						return true;
					}

				}
				Parallel.ForEach(t.n.edges, po, (e) => // For each adyacent node
				{
					if (!e.n.isgoal) // e.n is a box and t.n is a goal
					{
						if (t.bvisited.Contains(e.n)) return; // if such box was visited... continue
						State s1 = new State() // The box wasn't visited so we add it
						{ // Copying references to save memory, only will copy if it's necessary
							n = e.n,
							p = t,
							rvisited = t.rvisited,
							targets = t.targets,
							bvisited = t.bvisited
						};
						bool copy = true;
						foreach (Edge e1 in e.n.edges) // Adds new targets if the box this goal aims points to a goal that is not in targets
						{
							if (!t.targets.Contains(e1.n)) // The goal is not in targets
							{
								if (copy) // A copy is needed
								{
									s1.targets = new HashSet<Node>(t.targets);
									copy = false;
								}
								s1.targets.Add(e1.n); // Adds the goal to the targets
							}
						};
						lock (lockstack)
						{
							pending.Push(s1); // Push the box to be checked
						}
					}
					else // e.n is a goal and t.n is a box
					{
						if (t.rvisited.Contains(e.n)) return; // if this box has visited such goal... continue
						else if (!t.targets.Contains(e.n)) return; // if this box doesn't contain that goal in its targets.. continue
						State s1 = new State() // The box is in targets and wasn't visited
						{ // Copies references to save memory
							n = e.n,
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


	public class SokobanSolver
	{
		State solution = null;
		int numberofnowalls = 0;
		int numberofboxes = 0;
		bool canbesolved;
		char[] Grid;
		int columns;
		State initial;
		HashSet<int> goals = new HashSet<int>();
		ZobrishHash zh;
		HashSet<int> nodeadlocks = new HashSet<int>();
		int[][] boxtogoals;
		class State
		{
			public State(ZobrishHash zh) { this.zh = zh; }
			ZobrishHash zh;
			public List<int> sideboxesreachable;
			public int boxpushes;
			public State previousState = null;
			public int[] boxpositions;
			//public Hashtable boxtogoals;
			public int playerposition;
			public int heuristic;
			public int f;
			public int cumulatedlength;

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
				hashcode = zh.GetZobrishHash(this);
				hashcodecalculated = true;
				return hashcode;
			}
			public void SetHashCode()
			{
				hashcode = zh.GetZobrishHash(this);
				hashcodecalculated = true;
			}
			public void SetHashCodeAfterMove(int before, int now)
			{
				hashcodecalculated = true;
				hashcode = zh.GetZobrishHashAfterMove(this, before, now);
			}
			bool hashcodecalculated = false;
			public int hashcode;
		}
		enum Elements
		{
			WALL = '*', BOX = 'C', FREE = '.', PLAYER = 'J', GOAL = 'F'
		}


		//HashSet<int> nodeadlocks = new HashSet<int>();
		class StateComparerByPlayerZone : EqualityComparer<State>
		{
			int gridlength;
			public StateComparerByPlayerZone(int gridlength)
			{
				this.gridlength = gridlength;
			}
			object locktimesortplayertosideboxdistance = new object();
			public override bool Equals(State x, State y)
			{

				if (x == null || y == null) return false;
				if (x.GetHashCode() != y.GetHashCode()) return false;
				if (!x.sideboxesreachable.Contains(y.playerposition))
				{
					return false;
				}
				if (x.boxpushes > y.boxpushes) return false;
				return true;
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
			this.columns = columns + 2;
			this.Grid = new char[Grid.Length + 2 * (this.columns + Grid.Length / columns)];
			for(int i = 0, endi = this.columns; i < endi; ++i) // Padds the grid top and bottom with walls
			{
				this.Grid[i] = this.Grid[i + this.Grid.Length - this.columns] = (char)Elements.WALL;
			}
			// Pads the grid left and right
			for (int i = this.columns; i < this.Grid.Length; i+= this.columns)
			{
				this.Grid[i] = this.Grid[i + this.columns - 1] = (char)Elements.WALL;
			}
			List<int> boxpos = new List<int>();
			int pads = 0;
			zh = new ZobrishHash(this.Grid.Length);
			initial = new State(zh);
			for (int i = 0; i < Grid.Length; i++)
			{
				while (this.Grid[i + pads] == (char)Elements.WALL) pads++;
				if (Grid[i] == (char)Elements.BOX) { boxpos.Add(i + pads); }
				else if (Grid[i] == (char)Elements.PLAYER) { initial.playerposition = i + pads; }
				else if (Grid[i] == (char)Elements.WALL) { this.Grid[i + pads] = (char)Elements.WALL; continue; }
				else if (Grid[i] == (char)Elements.GOAL) { this.Grid[i + pads] = (char)Elements.GOAL; goals.Add(i + pads); continue; }
				this.Grid[i + pads] = (char)Elements.FREE;

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
				if (npos >= lowerbound && npos < upperbound && Grid[npos] != (char) Elements.WALL)
				{
					pending.Add(npos);
				}
			}
		}
		public enum Direction { LEFT, RIGHT, UP, DOWN }
		bool CanBePushed(int bp, Direction dir)
		{
			int n = 0;
			switch (dir)
			{
				case Direction.LEFT:
					n = -1;
					break;
				case Direction.RIGHT:
					n = 1;
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
			if (!InsideGrid(np) && !InsideGrid(pushpos) ||
				Grid[pushpos] == (char) Elements.WALL || Grid[np] == (char) Elements.WALL) return false;
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
			if (left < 0 || Grid[left] == (char)Elements.WALL) movex = false;
			if (right >= Grid.Length || Grid[right] == (char)Elements.WALL) movex = false;
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
						Console.ForegroundColor = ConsoleColor.Black;
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
							Console.ForegroundColor = ConsoleColor.Black;
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
							Console.ForegroundColor = ConsoleColor.Black;
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
							Console.ForegroundColor = ConsoleColor.Black;
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
							Console.ForegroundColor = ConsoleColor.Black;
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
			if (!InsideGrid(box)) return true;
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


			if (left < 0 || Grid[left] == (char)Elements.WALL || walls.Contains(left))
			{
				movex = false;
			}
			else if (s.boxpositions.Contains(left))
			{
				if (IsInALockedBoxPool(s, left, walls)) movex = false;
			}
			if (right >= Grid.Length || Grid[right] == (char)Elements.WALL || walls.Contains(right))
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


#if DEBUG
		TimeSpan solvingtime;
#endif
		public IEnumerable<Direction> SolveParallel()
		{

			IEnumerable<Direction> l;

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
#if DEBUG
		object locktime = new object();
#endif
		bool InsideGrid(int i)
		{
			return i >= 0 && i < Grid.Length;
		}
		void SetPlayerReachableArea(State s, int boxidxmoved = -1)
		{
//			if (boxidxmoved != -1)
//			{
//				bool a, b, c, d, e, f, g, h, i; // A box/wall is there
//				HashSet<int> positions = new HashSet<int>(ValidSquareNegativeDiagonal(s.boxpositions[boxidxmoved]));
//				positions.UnionWith(ValidSquarePositiveDiagonal(s.boxpositions[boxidxmoved]));
//				positions.UnionWith(ValidSquareNegativeDiagonal(s.playerposition));
//				positions.UnionWith(ValidSquarePositiveDiagonal(s.playerposition));
//				positions.UnionWith(ValidSquareSides(s.boxpositions[boxidxmoved]));
//#if DEBUG
//				if (!smartplayearreachableareameasurer.AddTimeElapsedIn(() =>
//				{
//#endif
//					bool calculate = true;
//					int direction = s.playerposition - s.boxpositions[boxidxmoved];
//					if (direction == 1) // | |P|
//					{
//						// a|b|c|d
//						// e|B|P|
//						// f|g|h|i
//						int
//							n = s.boxpositions[boxidxmoved] - columns - 1;
//						a = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//						n = s.boxpositions[boxidxmoved] - columns;
//						b = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//						n = s.boxpositions[boxidxmoved] - columns + 1;
//						c = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//						n = s.boxpositions[boxidxmoved] - columns + 2;
//						d = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//						n = s.boxpositions[boxidxmoved] - 1;
//						e = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//						n = s.boxpositions[boxidxmoved] + columns - 1;
//						f = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//						n = s.boxpositions[boxidxmoved] + columns;
//						g = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//						n = s.boxpositions[boxidxmoved] + columns + 1;
//						h = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//						n = s.boxpositions[boxidxmoved] + columns + 2;
//						i = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//					}
//					else if (direction == -1) // |P|B|
//					{
//						// d|c|b|a
//						//  |P|B|e
//						// i|h|g|f
//						int
//							n = s.boxpositions[boxidxmoved] - columns + 1;
//						a = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//						n = s.boxpositions[boxidxmoved] - columns;
//						b = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//						n = s.boxpositions[boxidxmoved] - columns - 1;
//						c = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//						n = s.boxpositions[boxidxmoved] - columns - 2;
//						d = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//						n = s.boxpositions[boxidxmoved] + 1;
//						e = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//						n = s.boxpositions[boxidxmoved] + columns + 1;
//						f = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//						n = s.boxpositions[boxidxmoved] + columns;
//						g = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//						n = s.boxpositions[boxidxmoved] + columns - 1;
//						h = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//						n = s.boxpositions[boxidxmoved] + columns - 2;
//						i = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//					}
//					else if (direction == columns)
//					{
//						// f|e|a
//						// g|B|b
//						// h|P|c
//						// i| |d
//						int
//							n = s.boxpositions[boxidxmoved] - columns + 1;
//						a = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//						n = s.boxpositions[boxidxmoved] + 1;
//						b = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//						n = s.boxpositions[boxidxmoved] + columns + 1;
//						c = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//						n = s.boxpositions[boxidxmoved] + 2 * columns + 1;
//						d = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//						n = s.boxpositions[boxidxmoved] - columns;
//						e = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//						n = s.boxpositions[boxidxmoved] - columns - 1;
//						f = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//						n = s.boxpositions[boxidxmoved] - 1;
//						g = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//						n = s.boxpositions[boxidxmoved] + columns - 1;
//						h = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//						n = s.boxpositions[boxidxmoved] + 2 * columns - 1;
//						i = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//					}
//					else if (direction == -columns)
//					{
//						// d| |i
//						// c|P|h
//						// b|B|g
//						// a|e|f
//						int
//							n = s.boxpositions[boxidxmoved] + columns - 1;
//						a = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//						n = s.boxpositions[boxidxmoved] - 1;
//						b = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//						n = s.boxpositions[boxidxmoved] - columns - 1;
//						c = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//						n = s.boxpositions[boxidxmoved] - 2 * columns - 1;
//						d = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//						n = s.boxpositions[boxidxmoved] + columns;
//						e = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//						n = s.boxpositions[boxidxmoved] + columns + 1;
//						f = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//						n = s.boxpositions[boxidxmoved] + 1;
//						g = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//						n = s.boxpositions[boxidxmoved] - columns + 1;
//						h = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//						n = s.boxpositions[boxidxmoved] - 2 * columns + 1;
//						i = (positions.Contains(n) ? ((Grid[n] != (char)Elements.WALL) ? false : true) : true) || s.boxpositions.Contains(n);
//					}
//					else
//					{
//						a = b = c = d = e = f = g = h = i = true;
//					}
//					if (!(
//							(a && !b && c) ||
//							(a && !e && f && (!b || !g)) ||
//							(a && !e && h && !b) ||
//							(a && !g && h && !b) ||
//							(b && !h && i) ||
//							(b && !c && d) ||
//							(!b && c && f && !g) ||
//							(!c && d && i) ||
//							(!c && d && g) ||
//							(c && !h && i) ||
//							(!c && d && h) ||
//							(c && !e && f && !g) ||
//							(d && !h && i) ||
//							(f && !g && h) ||
//							(g && !h && i)))
//					{
//						calculate = false;
//					}
//					if (!calculate)
//					{
//						s.sideboxesreachable = s.sideboxesreachable.Clone(s.playerposition, s.boxpositions[boxidxmoved]);
//#if DISPLAYSMARTPLAYERREACHABLEAREA
//						List<int> thearea2 = new List<int>();
//						for (int ik = 0; ik < Grid.Length; ik++) {
//							if (s.playerreachablearea[ik]) thearea2.Add(ik);
//						}
//						PrintState(s, thearea2.ToArray());
//#endif
//					}
//#if !DEBUG
//					if (!calculate) return;
//#else
//					return calculate;

//				}))
//				{
//					//Console.WriteLine("Smart guessed");
//					//PrintState(s, s.boxpositions[boxidxmoved]);
//#if DISPLAYPLAYERREACHABLEAREA
//					List<int> thearea2 = new List<int>();
//					for (int ik = 0; ik < Grid.Length; ik++)
//					{
//						if (s.playerreachablearea.Contains(ik)) thearea2.Add(ik);
//					}
//					PrintState(s, thearea2.ToArray());
//#endif
//					return;
//				}
//#endif

//			}

			State scpy = new State(zh);
			scpy.playerposition = s.playerposition;
			scpy.boxpositions = s.boxpositions;
			List<int> area = new List<int>();
			bool[] visited = new bool[Grid.Length];
			Queue<int> pending = new Queue<int>();
			pending.Enqueue(scpy.playerposition);
			visited[scpy.playerposition] = true;
			HashSet<int> reachgoals = new HashSet<int>();
			foreach (int item in scpy.boxpositions)
			{
				reachgoals.UnionWith(ValidSquareSides(item));
			}
			reachgoals.SymmetricExceptWith(scpy.boxpositions);
			while (pending.Count != 0)
			{
				int tmp = pending.Dequeue();
				if (reachgoals.Remove(tmp))
				{
					area.Add(tmp);
				}
				if (reachgoals.Count == 0) break;
				scpy.playerposition = tmp;
				int up = tmp - columns,
					down = tmp + columns,
					left = tmp - 1,
					right = tmp + 1;
				if (InsideGrid(up) && !visited[up] && Grid[up] != (char)Elements.WALL && !scpy.boxpositions.Contains(up))
				{
					pending.Enqueue(up);
					visited[up] = true;
				}
				if (InsideGrid(down) && !visited[down] && Grid[down] != (char)Elements.WALL && !scpy.boxpositions.Contains(down))
				{
					pending.Enqueue(down);
					visited[down] = true;
				}
				if (InsideGrid(left) && !visited[left] && Grid[left] != (char)Elements.WALL && !scpy.boxpositions.Contains(left))
				{
					pending.Enqueue(left);
					visited[left] = true;
				}
				if (InsideGrid(right) && !visited[right] && Grid[right] != (char)Elements.WALL && !scpy.boxpositions.Contains(right))
				{
					pending.Enqueue(right);
					visited[right] = true;
				}
			}
			s.sideboxesreachable = area;

#if DISPLAYPLAYERREACHABLEAREA
			List<int> thearea = new List<int>();
			for (int ik = 0; ik < Grid.Length; ik++)
			{
				if (s.sideboxesreachable.Contains(ik)) thearea.Add(ik);
			}
			PrintState(s, thearea.ToArray());
#endif


		}

		IEnumerable<int> ValidSquareSides(int squarepos)
		{
			if (!InsideGrid(squarepos)) yield break;
			int left = squarepos - 1;
			int right = squarepos + 1;
			int up = squarepos - columns;
			int down = squarepos + columns;
			if (left >= 0) yield return left;
			if (right < Grid.Length) yield return right;
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
			if (!InsideGrid(squarepos)) yield break;
			int leftup = squarepos - 1 - columns;
			int rightdown = squarepos + columns + 1;
			if (leftup >= 0) yield return leftup;
			if (rightdown < Grid.Length) yield return rightdown;
		}
		/// <summary>
		/// This diagonal \
		/// </summary>
		/// <param name="squarepos"></param>
		/// <returns></returns>
		IEnumerable<int> ValidSquareNegativeDiagonal(int squarepos)
		{
			if (!InsideGrid(squarepos)) yield break;
			int rightup = squarepos + 1 - columns;
			int leftdown = squarepos + columns - 1;
			if (rightup >= 0) yield return rightup;
			if (leftdown < Grid.Length) yield return leftdown;
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
			if (!InsideGrid(squarepos) || s.boxpositions.Contains(squarepos)
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
			if (!InsideGrid(squarepos) || Grid[squarepos] == (char)Elements.WALL) return false;
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
#if DEBUG
			bool simpledeadlock = simpledeadlockmeasurer.AddTimeElapsedIn(() => !s.boxpositions.All(x => nodeadlocks.Contains(x) || goals.Contains(x)));
#else
			bool simpledeadlock = !s.boxpositions.All(x => nodeadlocks.Contains(x) || goals.Contains(x));
#endif
			if (simpledeadlock) return true;
#if DEBUG

			bool betweenboxesdeadlock = freezedeadlockmeasurer.AddTimeElapsedIn(() => IsDeadLockBetweenBoxes(s));
#else
			bool betweenboxesdeadlock = IsDeadLockBetweenBoxes(s);
#endif
			if (betweenboxesdeadlock) return true;

#if DEBUG
			bool diagonaldeadlock = diagonaldeadlockmeasurer.AddTimeElapsedIn(() => IsDiagonalDeadLock(s, boxmoved));
#else
			bool diagonaldeadlock = IsDiagonalDeadLock(s, boxmoved);
#endif
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

		class ZobrishHash
		{
			public int[,] array;
			int gridlength;
			Random r = new Random();
			public ZobrishHash(int gridlength) { this.gridlength = gridlength; }
			public void Initialize()
			{
				array = new int[gridlength, 2];
				for (int i = 0; i < gridlength; i++)
				{
					for (int j = 0; j < 2; j++)
					{
						array[i, j] = r.Next();
					}
				}
			}
			public int GetZobrishHash(State s)
			{
				int zkey = 0;
				for (int i = 0; i < gridlength; i++)
				{
					if (s.boxpositions.Contains(i)) zkey ^= array[i, 1];
					else zkey ^= array[i, 0];
				}
				return zkey;
			}
			public int GetZobrishHashAfterMove(State s, int before, int now)
			{
				return s.GetHashCode() ^ array[before, 0] ^ array[before, 1] ^ array[now, 0] ^ array[now, 1];
			}
		}
#if DEBUG
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
#endif
		IEnumerable<Direction> SolveParallelAux()
		{
			if (!canbesolved) return new List<Direction>();
			PriorityQueue<State> pending = new PriorityQueue<State>();
			pending.Mode = PriorityQueue<State>.NodeMode.Queue;
			zh.Initialize();
			HashSet<State> visited = new HashSet<State>(new StateComparerByPlayerZone(Grid.Length));
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
#if DEBUG
				if (visitedmeasurer.AddTimeElapsedIn(() => visited.Contains(s))) continue;
#else
				if (visited.Contains(s)) continue;
#endif
				visited.Add(s);
#if DEBUG && SHOWDEQUEUEDSTATE && USEONECORE

				//Console.Write("DEQUEUING f: "+s.f + "\n");
				PrintState(s);
#endif
				if (goals.SetEquals(s.boxpositions))
				{
					solution = s;
					return BringPlayerMoves(solution);
				}
				PriorityQueue<int> pq = SortBoxesToGoalByDistance(s);
				Parallel.For(0, pq.Count, po, (ind1) =>
				{
					int boxidx = pq.Dequeue();
					PriorityQueue<int> sq = SortPlayerToSideBoxDistance(s.playerposition, s.boxpositions[boxidx]);
					while(sq.Count != 0)
					{
						int sidebox = sq.Dequeue();
						State s1 = JumpPlayerTo(s, sidebox);
						if (s1 == null) continue; // The player doesn't have access
#if DEBUG
						State s2 = pushboxmeasurer.AddTimeElapsedIn(() => PushBox(s1, boxidx));
#else
						State s2 = PushBox(s1, boxidx);
#endif
						if (s2 != null)
						{
#if DEBUG
							if (deadlockmeasurer.AddTimeElapsedIn(()=>!DeadLock(s2, s2.boxpositions[boxidx])))
#else
							if (!DeadLock(s2, s2.boxpositions[boxidx]))
#endif
							{
#if DEBUG
								reachableareameasurer.AddTimeElapsedIn(() => SetPlayerReachableArea(s2, boxidx));
#else
								SetPlayerReachableArea(s2, boxidx);
#endif
								pending.Enqueue(s2.f, s2);
							}
						}
					}
				});
			}
			return new List<Direction>();
		}

		public IEnumerable<Direction> SolveDeep()
		{
			IEnumerable<Direction> l;

#if (DEBUG)
			l = solvingtimemeasurer.AddTimeElapsedIn(() => { return SolveDeepAux(); });
#else
			l = SolveDeepAux();
#endif
			return l;
		}

		// Goes deep and finds the best solution
		IEnumerable<Direction> SolveDeepAux()
		{
			if (!canbesolved) return new List<Direction>();
			Stack<State> pending = new Stack<State>();
			zh.Initialize();
			HashSet<State> visited = new HashSet<State>(new StateComparerByPlayerZone(Grid.Length));


			pending.Push(initial);
			initial.heuristic = GetHeuristicBoxesToGoal(initial);
			initial.SetHashCode();
			ParallelOptions po = new ParallelOptions();
			po.MaxDegreeOfParallelism =
#if USEONECORE
				1;
#else
			Environment.ProcessorCount;
#endif
			PriorityQueue<int> pq = SortBoxesToGoalByDistance(initial);
			int[] arrpq = new int[pq.Count];
			for (int i = 0, endi = pq.Count; i < endi; i++) arrpq[i] = pq.Dequeue();
			while (pending.Count != 0)
			{
				State s = pending.Pop();

#if DEBUG && SHOWDEQUEUEDSTATE && USEONECORE

				//Console.Write("DEQUEUING f: "+s.f + "\n");
				PrintState(s);
#endif
				if(solution != null && GetHeuristicBoxesToGoal(s) + s.boxpushes > solution.boxpushes)
				{
#if DEBUG
				//	Console.WriteLine("State with "+ s.boxpushes +" expected pushes, ignored");
					continue;
#endif
				}
				if (goals.SetEquals(s.boxpositions))
				{
					if (solution == null || solution.boxpushes > s.boxpushes)
					{
						solution = s;
						int rmv = visited.RemoveWhere((x) => GetHeuristicBoxesToGoal(x) + x.boxpushes >= solution.boxpushes);
#if DEBUG
						Console.WriteLine("Solution found: " + solution.boxpushes + " pushes, " + rmv + " elements were deleted from visited table ");

#endif
						GC.Collect();
					}
					continue;
				}

				Parallel.For(0, arrpq.Length, po, (ind1) =>
				{
					int boxidx = arrpq[ind1];
					PriorityQueue<int> sq = SortPlayerToSideBoxDistance(s.playerposition, s.boxpositions[boxidx]);
					while(sq.Count != 0)
					{
						int sidebox = sq.Dequeue();
						State s1 = JumpPlayerTo(s, sidebox);
						if (s1 == null) continue; // The player doesn't have access

#if DEBUG
						State s2 = pushboxmeasurer.AddTimeElapsedIn(() => PushBox(s1, boxidx));
#else
						State s2 = PushBox(s1, boxidx);
#endif
						if (s2 != null)
						{
#if DEBUG
							reachableareameasurer.AddTimeElapsedIn(() => SetPlayerReachableArea(s2, boxidx));
#else
							SetPlayerReachableArea(s2, boxidx);
#endif
#if DEBUG
							if (!visitedmeasurer.AddTimeElapsedIn(() => visited.Contains(s2)) && deadlockmeasurer.AddTimeElapsedIn(() => !DeadLock(s2, s2.boxpositions[boxidx])))
#else
							if (!visited.Contains(s2) && !DeadLock(s2, s2.boxpositions[boxidx]))
#endif
							{
								visited.Add(s2);
								pending.Push(s2);
							}
						}
					}
				});
			}
			return BringPlayerMoves(solution);
		}

		public int SolutionPushes { get { if (solution != null) return solution.boxpushes; else return -1; } }

		private IEnumerable<Direction> BringPlayerMoves(State s)
		{
			if (s == null) yield break;
			// Stores the movements in a stack
			Stack<Direction> final = new Stack<Direction>();
			for (State t = s; t != null; t = t.previousState) // Fills the movements of the player where he jumped a longer distance than posible
			{
				newstates:
				State prev = t.previousState;
				if (prev == null) break;
				int direction = t.playerposition - prev.playerposition;
				if (direction == 1) // Moved right
					final.Push(Direction.RIGHT);
				else if (direction == -1)// Moved left
					final.Push(Direction.LEFT);
				else if (direction == columns) // Moved down
					final.Push(Direction.DOWN);
				else if (direction == -columns) // Moved up
					final.Push(Direction.UP);
				else if (direction == 0) { continue; } // If this happens something weird happened
				else
				{ // The player has jumped a longer distance, so the states are computed again
					t.previousState = MovePlayerTo(prev, t.playerposition).previousState;
					goto newstates;
				}
			}
			while (final.Count != 0) yield return final.Pop();
		}

		State PushBox(State s, int boxidx)
		{
			if (s == null) return null;
			int boxpos = s.boxpositions[boxidx];
			int t = s.playerposition - boxpos;
			State tn = new State(zh);
			tn.heuristic = s.heuristic;
			tn.hashcode = s.hashcode;
			tn.sideboxesreachable = s.sideboxesreachable;
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
			if (s.playerposition == target) return s;
			if (InsideGrid(target) && s.sideboxesreachable.Contains(target))
			{
				State r = new State(zh);
				r.boxpushes = s.boxpushes;
				r.boxpositions = s.boxpositions;
				r.cumulatedlength = s.cumulatedlength;
				r.f = s.f;
				r.heuristic = s.heuristic;
				r.playerposition = target;
				r.sideboxesreachable = s.sideboxesreachable;
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
		State MovePlayerTo(State initials, int target)
		{
			//if (!initials.playerreachablearea.Contains(target)) return null;
			if (!InsideGrid(target) || Grid[target] == (char)Elements.WALL || initials.boxpositions.Contains(target)) return null;
			PriorityQueue<State> pending = new PriorityQueue<State>();
			HashSet<int> visited = new HashSet<int>();
			pending.Enqueue(initials.f, initials);
			while(pending.Count != 0)
			{
				State t = pending.Dequeue();
				visited.Add(t.playerposition);
				if(t.playerposition == target)
				{
					t.f = initials.f;
					t.cumulatedlength = initials.cumulatedlength;
					t.heuristic = initials.heuristic;
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
			if (!InsideGrid(t.playerposition)) return false;
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
			if (!InsideGrid(n) || Grid[n] == (char)Elements.WALL || t.boxpositions.Contains(n)) return false;
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
					State tn = new State(zh);
					tn.sideboxesreachable = t.sideboxesreachable;
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
				foreach (int v in boxtogoals[i].Intersect(s.boxpositions)) // Boxes in the goal are discarded
				{
					int dist = ManhattanDistance(s.boxpositions[i], v);
					if (minimald > dist) minimald = dist;
				}
				r.Enqueue(minimald, i); // If there are no goals for a box then it will have the max value, it means will be treated last
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
					ss.SolveParallel();
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
#else
			char[] grid = Console.ReadLine().ToArray();
			int columns = (int)Math.Sqrt(grid.Length);
			SokobanSolver ss = new SokobanSolver(grid, columns);
			List<SokobanSolver.Direction> sol = ss.SolveParallel().ToList();
			foreach (SokobanSolver.Direction dir in sol)
			{
				switch (dir)
				{
					case SokobanSolver.Direction.LEFT:
						Console.Write("o");
						break;
					case SokobanSolver.Direction.RIGHT:
						Console.Write("e");
						break;
					case SokobanSolver.Direction.UP:
						Console.Write("n");
						break;
					case SokobanSolver.Direction.DOWN:
						Console.Write("s");
						break;
					default:
						Console.Write("?");
						break;
				}
			}
			Console.Write("\n" + ss.SolutionPushes + "\n");
#endif
		}
	}
}
