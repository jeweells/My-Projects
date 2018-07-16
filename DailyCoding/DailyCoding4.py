# Given an array of integers, find the first missing positive integer in linear time and constant space. In other words, find the lowest positive integer that does not exist in the array. The array can contain duplicates and negative numbers as well.

# For example, the input [3, 4, -1, 1] should give 2. The input [1, 2, 0] should give 3.

# You can modify the input array in-place.

# Finds the lowest missing number in the array, starting from start_from (inclusive) in O(n) time, O(1) space
# WARNING: This function modifies the array
# starting from start_from (inclusive) to start_from + arr.Length (inclusive)
def FindLowestMissing(arr, start_from = 0):
	length = len(arr);
	isTheFirst = True;
	for i in range(length-1, -1, -1):
		if(arr[i] == start_from):
			isTheFirst = False;
			continue
		if (arr[i] < start_from or arr[i] >= start_from + length):
			arr[i] = start_from # Sets numbers out of the possible returned value range as the minimal number to be returned
	if (isTheFirst):
		return start_from # if the first element wasn't found, it means this is the missing number
	helper = length + start_from
	for i in range(0, length):
		if(arr[i] >= helper): # Check if it's out of the range
			if (arr[arr[i] - helper] < helper): # Access the position indicated in its value restoring that number by putting it into the range and shiftening it to fit in 0 to length
				arr[arr[i] - helper] += length; # Take out of the range the number if it's not out of the range
		else:
			if (arr[arr[i] - start_from] < helper): # Just shift the value in the array to fit in 0 to length and access it
				arr[arr[i] - start_from] += length # Take out of the range the number if it's not out of the range
	for i in range(0, length):
		if (arr[i] < helper): # If there's one number that's still in the range 0 to length, that's the minimum
			return i + start_from
	return length + start_from # There are no numbers in the range 0 to length so the minimum is the next number that follows the biggest 

assert(FindLowestMissing([3, 4, -1, 1],1))
assert(FindLowestMissing([1, 2, 0], 1))