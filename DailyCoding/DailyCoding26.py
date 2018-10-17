
"""
Given a singly linked list and an integer k, remove the kth last element from the list. k is guaranteed to be smaller than the length of the list.

The list is very long, so making more than one pass is prohibitively expensive.

Do this in constant space and in one pass.
"""

class node:
	next_node = None
	value = None
	def __init__(self, value):
		self.value = value
class linked_list:
	start = None
	last = None
	length = 0
	def __init__(self):
		pass

def remove_kth_last(lst, k):
	if k < 0:
		return
	idx = lst.length - k
	prevprev = lst.start
	prev = prevprev.next_node
	curr = 2
	if idx == 1:
		lst.start = prev
	else:
		while(curr != idx):
			prevprev = prev
			prev = prev.next_node
			curr += 1
		prevprev.next_node = prev.next_node
	lst.length -= 1;



ll = linked_list()
ll.last = ll.start = node(1)
ll.length = 1
for i in range(2, 11):
	ll.last.next_node = node(i)
	ll.last = ll.last.next_node
	ll.length += 1


def print_array(ll):
	n = ll.start
	r = []
	while(n != None):
		r.append(n.value)
		n = n.next_node
	print(r)
print_array(ll)
remove_kth_last(ll, 5)
print_array(ll)