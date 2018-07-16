/*
Given the root to a binary tree, implement serialize(root), which serializes the tree into a string, and deserialize(s), which deserializes the string back into the tree.

For example, given the following Node class

class Node:
    def __init__(self, val, left=None, right=None):
        self.val = val
        self.left = left
        self.right = right
The following test should pass:

node = Node('root', Node('left', Node('left.left')), Node('right'))
assert deserialize(serialize(node)).left.left.val == 'left.left'
*/


using System;
using System.Collections.Generic;
class DailyCoding3 {
	class BTree
	{
		public static string separator = "\n";
		public static string nullindicator = "null";
		public class Node{
			public Node(string val, Node left = null, Node right = null)
			{
				this.val = val;
				this.left = left;
				this.right = right;
			}
			public string val;
			public Node left;
			public Node right;
		}
		public static string Serialize(Node root)
		{
			if(root == null) return nullindicator; // Throws null node
			return root.val + 
			separator + // Separates each node
			Serialize(root.left) + 
			separator + 
			Serialize(root.right); // Go left and right
		}
		public static Node Deserialize(string str)
		{
			Queue<string> nodes = new Queue<string>(str.Split(new string[]{separator} ,StringSplitOptions.None)); // Having each node
			return DeserializeAux(nodes, nodes.Dequeue()); // Passing the queue address
		}
		public static Node DeserializeAux(Queue<string> nodes, string val){
			if(val == nullindicator) return null; // It's a null node
			return new Node(val, 
			DeserializeAux(nodes, nodes.Dequeue()), 
			DeserializeAux(nodes, nodes.Dequeue())); // When this function is called, the queue will have a right node on the top
		}
	}
	

  public static void Main (string[] args) {
    BTree.Node nod = new BTree.Node("root", new BTree.Node("left", new BTree.Node("leftleft")), new BTree.Node("right"));
		Console.WriteLine(BTree.Deserialize(BTree.Serialize(nod)).left.left.val);
  }
}