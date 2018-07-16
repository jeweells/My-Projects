# Given the mapping a = 1, b = 2, ... z = 26, and an encoded message, count the number of ways it can be decoded.
# For example, the message '111' would give 3, since it could be decoded as 'aaa', 'ka', and 'ak'.

def CountWays(message):
	nowords = 0
	if(len(message) == 0):
		return 0
	if(message[0] == '0'):
		print("Input error")
		return
	zero = 0
	for i in list(range(0,len(message)-1)):
		if(not(9 < int(message[i]+message[i+1]) < 27)):
			nowords+=1
		if(int(message[i]) == 0):
			zero = 1
	def f(n):
		if(n <= 1):
			return 0
		arr = [0, 1]
		for i in list(range(2, n)):
			arr[1], arr[0] = arr[0], arr[1]
			arr[1] += arr[0] + 1
		return arr[1]
	return f(len(message)-nowords) + 1 - zero



