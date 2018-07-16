# Given an array of integers, return a new array such that each element at index i of the new array is the product of all the numbers in the original array except the one at i.

# For example, if our input was [1, 2, 3, 4, 5], the expected output would be [120, 60, 40, 30, 24]. If our input was [3, 2, 1], the expected output would be [2, 3, 6].

# Follow-up: what if you can't use division?

# Without using division
def MultiplyAllButI(array):
	length = len(array)
	if(length == 1):
		return [0]
	# Following this, it's noticeable that the matrix diagonal are sequently multiplied discarding the last element
	# <- This column is the array
	# 1-__1 ___ ___ ___ ___ <- 1 just helps multiplying
	# 2-__2 __1 ___ ___ ___
	# 3-__6 __3 __2 ___ ___
	# 4-_24 _12 __8 __6 ___
	# 5-120 _60 _40 _30 _24 <- Answer
	
	# Building the matrix length * length
	matr = [[0] * length] * length
	matr[0][0] = 1
	# First, we build the diagonal
	for i in range(1, length):
		matr[i][i] = matr[i-1][i-1] * array[i-1]
	# Then the rest is calculated using this diagonal
  # Multiplying the array position by the previous element
	for i in range(0, length):
		for j in range(i+1, length):
			matr[j][i] = matr[j-1][i] * array[j]
	return matr[length-1];




# Using division
def MultiplyAllButIDiv(array):
	if(len(array) == 1):
		return [0]
	multiply = 1
	zeroes = 0
	for e in array:
		if(e == 0):
			zeroes += 1
			continue
		multiply *= e
	result = []
	# If there is one zeroe, then where the zeroe is, there must be the multiplication of all of them but the zeroe and the rest will be zeroe
	if(zeroes == 1):
		for i in array:
			if(i == 0):
				result.append(multiply)
			else:
				result.append(0)
	elif(zeroes > 1):
		# There are more than 1 zeroe the result will be a bunch of zeroes
		for i in array:
			result.append(0)
	else:
		for i in array:
			result.append(multiply / i)
	return result

