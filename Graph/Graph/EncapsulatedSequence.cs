using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class EncapsulatedSequence<T> : IEnumerable<T>
{
	class Node
	{
		public EncapsulatedSequence<T> Prev;
		public T Value;
		public Node(T Value, EncapsulatedSequence<T> Prev)
		{
			this.Value = Value;
			this.Prev = Prev;
		}
	}
	Node current;
	public EncapsulatedSequence(T Value)
	{
		current = new Node(Value, null);
	}
	public EncapsulatedSequence(EncapsulatedSequence<T> Secuence, T NextValue)
	{
		current = new Node(NextValue, Secuence);
		count += Secuence.Count;
	}
		
	public EncapsulatedSequence<T> Clone()
	{
		EncapsulatedSequence<T> es = new EncapsulatedSequence<T>(current.Value);
		es.count = count;
		Node tmp = current;
		Node tmpnew = es.current;
		while(tmp != null)
		{
			tmpnew.Prev = tmp.Prev;
			tmp = tmpnew = tmpnew.Prev.current;
		}
		return es;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="index"></param>
	/// <returns>Reference of the sub-secuence, careful: all bigger secuences depends on this</returns>
	public EncapsulatedSequence<T> SubSequenceFrom(int index)
	{
		if (index < 0 || index >= count) throw new IndexOutOfRangeException($"{index} must be between in [0, {count})");
		int i = 0;
		EncapsulatedSequence<T> secuence = this;
		Node tmp = current;
		while (i++ != index && tmp != null)
		{
			secuence = tmp.Prev;
			tmp = tmp.Prev.current;
		}
		if (tmp == null) throw new ArgumentNullException($"Unknown error, inconsistency in the number of elements of this structure");
		return secuence;
	}

	public T FirstValue()
	{
		return this[count-1];
	}

	/// <summary>
	/// Returns the element indicated starting from the last
	/// </summary>
	/// <param name="index">0 to Count(exclusive)</param>
	/// <returns></returns>
	public T this[int index]
	{
		get
		{
			if (index < 0 || index >= count) throw new IndexOutOfRangeException($"{index} must be between in [0, {count})");
			int i = 0;
			Node tmp = current;
			while(i++ != index && tmp != null)
				tmp = tmp.Prev.current;
			if (tmp == null) throw new ArgumentNullException($"Unknown error, inconsistency in the number of elements of this structure");
			return tmp.Value;
		}
	}
	int count = 1;
	public int Count { get { return count; } }

	static void Main(string[] args)
	{
	}

	public IEnumerator<T> GetEnumerator()
	{
		return new Enumerator(this);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return new Enumerator(this);
	}
	public class Enumerator : IEnumerator, IEnumerator<T>
	{
		Node startNode;
		Node currNode;
		public Enumerator(EncapsulatedSequence<T> node)
		{
			startNode = currNode = new Node(default(T), node);
		}
		public object Current => currNode.Value;

		T IEnumerator<T>.Current => currNode.Value;

		public void Dispose()
		{
			return;
		}

		public bool MoveNext()
		{
			if (currNode == null || currNode.Prev == null) return false;
			currNode = currNode.Prev.current;
			return true;
		}

		public void Reset()
		{
			currNode = startNode;
		}
	}
}

