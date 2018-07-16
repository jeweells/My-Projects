# Given the root to a binary tree, implement serialize(root), which serializes the tree into a string, and deserialize(s), which deserializes the string back into the tree.

# For example, given the following Node class

# class Node:
#     def __init__(self, val, left=None, right=None):
#         self.val = val
#         self.left = left
#         self.right = right
# The following test should pass:

# node = Node('root', Node('left', Node('left.left')), Node('right'))
# assert deserialize(serialize(node)).left.left.val == 'left.left'

from collections import deque

separator = "\n"
nullindicator = "null"
class Node:
    def __init__(self, val, left=None, right=None):
        self.val = val
        self.left = left
        self.right = right

def Serialize(root):
	if(root is None): return nullindicator # Throws null node
	return root.val + separator + Serialize(root.left) + separator + Serialize(root.right)
	# separator : Separates each Node
	# Serialize(root.left/right) Go left and right

def Deserialize(stri):
	nodes = deque()
	for s in stri.split(separator):
		nodes.append(s) # Having each node
	def DeserializeAux():
		val = nodes.popleft()
		if(val == nullindicator):
			return None
		return Node(val, DeserializeAux(), DeserializeAux())
	return DeserializeAux()

btree = Node("root", Node("left", Node("leftleft")), Node("right"))
assert(Deserialize(Serialize(btree)).left.left.val == "leftleft")