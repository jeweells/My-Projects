using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriorityQueue
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
		public NodeMode Mode { get { return mod; } set { mod = value; } }
		public OrderType Order { get { return ord; } set { ord = value; } }
		public int Count { get { return count; } }
		public int SmallestPos { get { return smallestpos; } }
		public int BiggestPos { get { return biggestpos; } }
		public int DCount { get { return dcount; } }

		public ValueT Dequeue()
		{
			lock (lockEnqDeq)
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

		public void Enqueue(int pos, ValueT value)
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
}
