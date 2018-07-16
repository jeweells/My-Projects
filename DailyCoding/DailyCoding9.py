# Given a list of integers, write a function that returns the largest sum of non-adjacent numbers. Numbers can be 0 or negative.

# For example, [2, 4, 6, 2, 5] should return 13, since we pick 2, 6, and 5. [5, 1, 1, 5] should return 10, since we pick 5 and 5.
def LargestSum(array):
	index = 0
	length = len(array)
	maxin = max(array)
	if(maxin <= 0):	# if they all are negatives
		return maxin # Only one element is the largest sum
	# Imagine a pointer, which starts at a in
	# [a, b, c, d, _, a, ...] (it's repeated)
	# This patern is repeated is iteration which gives us to this conclusion:
	# Taking these elements
	# [a,_,c,_,*] (a+c)
	# [a,_,_,d,_,*] (a+d)
	# [_,b,_,d,_,*] (b+d)
	# If we know which is the biggest
	# we will start at * on the next iteration depending on the case and * will be the new a
	max_sum = 0
	while(index < length):
		a = array[index]
		b = array[index + 1] if (index + 1 < length and array[index + 1] > 0) else 0
		c = array[index + 2] if (index + 2 < length and array[index + 2] > 0) else 0
		d = array[index + 3] if (index + 3 < length and array[index + 3] > 0) else 0
		cur_max = max(a+c, a+d, b+d)
		if(cur_max == a+c):
			index += 4 # _
		else:
			index += 5 # second a
		max_sum += cur_max
	return max_sum

print(LargestSum([2, 4, 6, 2, 5]))