import numpy as np

# errorfactor means how close 
def jacobi(a, b, x0 = None, errorfactor = 0.01):
	if x0 is None:
		x0 = [0.0]*len(a[0])
	xprev = np.array(x0) # Convert it to a numpy array
	a = np.array(a) # Convert it to a numpy array
	b = np.array(b) # Convert it to a numpy array
	d = np.diag(a) # Diagonal of A
	prevnorm = None
	while True:
		z = np.inner(a, xprev)
		r = b - z # Find the remainder
		rnorm = np.linalg.norm(r)
		if prevnorm is None:
			prevnorm = rnorm
		elif prevnorm < rnorm: # It's going far away
			return xprev
		ebnorm = errorfactor * np.linalg.norm(b)
		if ebnorm > rnorm: # See if we've got an acceptable solution
			return xprev # If we have it, we return the solution
		for i in range(len(z)):
			z[i] = r[i]/float(d[i]) # Solve Dz = r, where D is the diagonal of the matrix. We have D and r computed. What's only needed is to 
		xprev += z # Do: xn = xn-1 + z
	# Repeat until we've got a solution