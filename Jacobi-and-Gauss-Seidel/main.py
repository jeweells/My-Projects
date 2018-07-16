import jacobi as j
import numpy as np
import gauss_seidel as gs
a = [[5,1,1],
		[1,5,1],
		[1,1,5]]
a = [
	[1,2,-2],
	[1,1,1],
	[2,2,1]
]
b = [7,7,7]
b = [1,2,5]
x = j.jacobi(a, b)
x = gs.gauss_seidel(a,b)
print(x)
print(np.inner(a, x))