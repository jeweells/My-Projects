def fermat(n:int):
	ar = range(1, n-1)
	for a in ar:
		if a**(n-1)% n != 1:
			return False
	return True