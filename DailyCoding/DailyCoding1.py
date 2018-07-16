# Given a list of numbers and a number k, return whether any two numbers from the list add up to k.

# For example, given [10, 15, 3, 7] and k of 17, return true since 10 + 7 is 17.

def AddUpto(array, k):
	rang = range(0, len(array))
	for i in rang:
		for j in rang[i+1:]: # No need to start from 0, since the sum is commutative
			if (array[i] + array[j] == k):
				return True
	return False