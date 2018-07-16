using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knapsack
{


	public class Bag : IEnumerable
	{
		public class Item
		{
			public int Id { get; set; } // Important if repeated items will be stored in the bag
			public object Obj { get; set; }
			public int Weight { get; set; }
			public int Value { get; set; }
			public Item(object obj, int weight, int value)
			{
				Obj = obj;
				Weight = weight;
				Value = value;
			}
		}
		public Bag(int capacity, int maxitems)
		{
			MaxItems = maxitems;
			Capacity = capacity;
		}

		public void UpdateInfo()
		{
			weight = 0;
			value = 0;
			foreach (Item it in items.Values)
			{
				weight += it.Weight;
				value += it.Value;
			}
		}
		Dictionary<object, Item> items = new Dictionary<object, Item>();
		int weight = 0;
		int value = 0;
		int capacity;
		int maxitems;
		public int CountItems
		{
			get
			{
				return items.Count;
			}
		}
		public int Value { get { return value; } }
		public int Weight { get { return weight; } }
		public int MaxItems
		{
			get
			{
				return maxitems;
			}
			set
			{
				if (value < 0) Console.WriteLine("Error: The max amount of items can't be negative");
				if (CountItems > value) Console.WriteLine("Error: The max amount of items can't be reduced, delete some items in order to reduce it");
				maxitems = value;
			}
		}
		public int Capacity
		{
			get
			{
				return capacity;
			}
			set
			{
				if (value < 0) Console.WriteLine("Error: The capacity can't be negative");
				if (weight > value) Console.WriteLine("Error: The bag capacity can't be reduced, delete some items in order to reduce it");
				capacity = value;
			}
		}
		public bool Add(Item item)
		{
			if (items.ContainsKey(item.Obj) || Weight + item.Weight > Capacity || CountItems >= MaxItems) return false;

			items.Add(item.Obj, item);
			weight += item.Weight;
			value += item.Value;
			return true;
		}
		public bool Remove(Item item)
		{
			if (!items.Remove(item.Obj)) return false;
			weight -= item.Weight;
			value -= item.Value;
			return true;
		}
		public bool Contains(Item item)
		{
			return items.ContainsKey(item.Value);
		}

		public Bag Clone()
		{
			Bag b = new Bag(capacity, maxitems);
			foreach (Item i in items.Values)
			{
				b.Add(i);
			}
			return b;
		}

		bool ItemsEquals(Bag y)
		{
			if (items.Count != y.items.Count) return false;
			foreach (object key in items.Keys)
			{
				if (!y.items.ContainsKey(key)) return false;
			}
			return true;
		}

		public override bool Equals(object obj)
		{
			Bag y = (Bag)obj;
			return this.capacity == y.capacity && this.weight == y.weight && ItemsEquals(y);
		}

		public IEnumerator GetEnumerator()
		{
			return items.Values.GetEnumerator();
		}
		public override int GetHashCode()
		{
			int tmp = 0 ^ capacity.GetHashCode() ^ maxitems.GetHashCode() ^ weight.GetHashCode() ^ value.GetHashCode();
			foreach (object obj in items) tmp ^= obj.GetHashCode();
			return tmp;
		}
	}
	public class Knapsack
	{
		

		class BagComparer : IEqualityComparer<Bag>
		{
			public bool Equals(Bag x, Bag y)
			{
				return x.Equals(y);
			}

			public int GetHashCode(Bag obj)
			{
				return obj.GetHashCode();
			}
		}

		Dictionary<object, Bag.Item> items = new Dictionary<object, Bag.Item>();
		/// <summary>
		/// Expensive for bags that have the biggest value possible
		/// Big for bags that have the maximum amount of items possible
		/// Heavy for bags that are the heaviest
		/// </summary>
		public enum BagsType { Expensive, Big, Heavy }

		/// <summary>
		/// Get the bags that has the same value acording to type
		/// </summary>
		/// <param name="bagmaxweight">Max weight the bag can carry on</param>
		/// <param name="maxelements">Max elements the bag can carry on</param>
		/// <param name="type"></param>
		/// <returns>All these bags have the same value defined in 'type' but might differ on other characteristics</returns>
		public IEnumerable<Bag> Pack(int bagmaxweight, int maxelements, BagsType type)
		{
			HashSet<Bag> visited = new HashSet<Bag>(new BagComparer());
			PriorityQueue<Bag> open = new PriorityQueue<Bag>();
			List<Bag> bestbag = new List<Bag>();
			bestbag.Add(new Bag(bagmaxweight, maxelements));
			open.Enqueue(0, bestbag[0]);
			while (open.Count != 0)
			{
				Bag tmpbag = open.Dequeue();
				switch (type)
				{
					case BagsType.Expensive:
						if (bestbag[0].Value < tmpbag.Value) bestbag = new List<Bag>() { tmpbag };
						else if (bestbag[0].Value == tmpbag.Value) bestbag.Add(tmpbag);
						break;
					case BagsType.Big:
						if (bestbag[0].CountItems < tmpbag.CountItems) bestbag = new List<Bag>() { tmpbag };
						else if (bestbag[0].CountItems == tmpbag.CountItems) bestbag.Add(tmpbag);
						break;
					case BagsType.Heavy:
						if (bestbag[0].Weight < tmpbag.Weight) bestbag = new List<Bag>() { tmpbag };
						else if (bestbag[0].Weight == tmpbag.Weight) bestbag.Add(tmpbag);
						break;
					default:
						break;
				}
				foreach (Bag.Item bi in items.Values)
				{
					Bag newbag = tmpbag.Clone();
					if (newbag.Add(bi) && !visited.Contains(newbag))
					{
						open.Enqueue(newbag.Weight, newbag);
						visited.Add(newbag);
					}
				}
			}
			return bestbag;
		}

		public bool AddItem(object item, int value, int weight, int id = 0)
		{
			if (items.ContainsKey(item)) return false;
			items.Add(item, new Bag.Item(item, weight, value) { Id = id });
			return true;
		}
		public int CountItems { get { return items.Count; } }
		public bool RemoveItem(object item)
		{
			return items.Remove(item);
		}
		public bool ContainsItem(object item)
		{
			return items.ContainsKey(item);
		}
		public bool UpdateItem(object item, int value, int weight, int id = 0)
		{
			if (!ContainsItem(item)) return false;
			Bag.Item bi = items[item];
			bi.Value = value;
			bi.Weight = weight;
			bi.Id = id;
			return true;
		}
		static void Main(string[] args)
		{
			Knapsack ks = new Knapsack();
			ks.AddItem("Pera", 10, 5);
			ks.AddItem("Manzana", 10, 5);
			ks.AddItem("Celular", 12000, 30);
			ks.AddItem("Anillo", 1000000, 10);
			ks.AddItem("CD", 30, 3);
			ks.AddItem("Impresora", 4000, 50);
			List<Bag> iee = ks.Pack(70, int.MaxValue, BagsType.Expensive).ToList();
			foreach (Bag ie in iee)
			{
				foreach (Bag.Item x in ie)
				{
					Console.WriteLine("{0,-25} value: {1,25} weight: {2,25}", x.Obj, x.Value, x.Weight);
				}
				Console.WriteLine("Bag weight: {0,10}/{1} hashcode {2}", ie.Weight, ie.Capacity, ie.GetHashCode());
				Console.WriteLine();
				Console.WriteLine();
				Console.WriteLine();
			}
		}
	}
}
