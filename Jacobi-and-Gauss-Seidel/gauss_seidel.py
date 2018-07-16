import numpy as np

def gauss_seidel(a, b, x0 = None, errorfactor = 0.01):
	a = np.array(a)
	b = np.array(b)
	l = np.zeros((len(a), len(a[0])))
	for i in range(len(a)):
		for j in range(i+1):
			l[i][j] = float(a[i][j]) # Get the lower matrix of a
	if x0 is None:
		x0 = [0.0] * len(a[0])
	xprev = np.array(x0)
	prevnorm = None
	while True:
		z = np.inner(a, xprev)
		r = b - z
		rnorm = np.linalg.norm(r)
		if prevnorm is None:
			prevnorm = rnorm
		elif prevnorm < rnorm: # It's going far away
			return xprev
		ebnorm = errorfactor * np.linalg.norm(b)
		if ebnorm > rnorm:
			return xprev
		# Solve Lz = r
		for i in range(len(a)): # Forward substitution
			cum = 0.0
			for j in range(i):
				cum += float(l[i][j] * z[j])
			z[i] = (r[i] - cum)/float(l[i][i])
		xprev += z