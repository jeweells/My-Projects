def Test(get_next_func, ntimes, maxnumber):
	mytestset = [0] * maxnumber
	for i in range(ntimes):
		tmp = get_next_func()
		mytestset[int(tmp%maxnumber)] += 1
	num1 = 0
	ndivr = ntimes/maxnumber
	for i in range(maxnumber):
		num1 += (mytestset[i] - ndivr)**2
	return num1 / ndivr