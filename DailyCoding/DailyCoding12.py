# There exists a staircase with N steps, and you can climb up either 1 or 2 steps at a time. Given N, write a function that returns the number of unique ways you can climb the staircase. The order of the steps matters.

# For example, if N is 4, then there are 5 unique ways:

# 1, 1, 1, 1
# 2, 1, 1
# 1, 2, 1
# 1, 1, 2
# 2, 2
# What if, instead of being able to climb 1 or 2 steps at a time, you could climb any number from a set of positive integers X? For example, if X = {1, 3, 5}, you could climb 1, 3, or 5 steps at a time.
def CountStairs(staircases, possible_steps):
	minofsteps = min(possible_steps) # Smallest jump will tell us where to start
	if(minofsteps > staircases): return 0;
	x = [0] * (staircases + 1) # the biggest jump is n so the array must have n + 1 elements
	x[minofsteps] = 1 # If the stair is the length minofsteps, the maximal amount of steps is 1
	n = minofsteps + 1 # Do not check x[minofsteps] again
	while(n <= staircases): # We'll jump smaller stairs until we can jump this stair
		x[n] = 1 if(n in possible_steps) else 0 # If the stair is the length n, the maximal amount of steps is 1 only if that's a possible step
		# For the simple case where steps are {1,2}
		# The solution would be x(n) = x(n-1) + x(n-2)
		# Notice -1 and -2 are part of the set
		# This will increase if there are more possible steps
		# And will have the corresponding value of such step length
		# Hence, if the steps are : {x0, x1,...,xm}
		# we'll have x(n) = x(n-x0) + x(n-x1) + ... + x(n-xm)
		# n-xi never will ve less than zero nor zero due to i < n
		# (Negative staircases aren't posible)
		for i in possible_steps: 
			if i < n and i > 0: # Only positive integers
				x[n] += x[n-i]
		n += 1
	print(x)
	return x[n-1]

print(CountStairs(6, {1,2}))
