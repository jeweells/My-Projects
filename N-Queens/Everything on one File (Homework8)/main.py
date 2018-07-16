# :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
# :: Homework 8
# :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
# :: Date: July 16, 2018
# :: Author: Abraham José Pacheco Becerra
# :: E-Mail: abraham.pacheco6319@gmail.com
# :: Description: Solves the N-Queens randomly using Las Vegas algorithm
# :: in two ways:
# :: 1) Undone the previous action when the solution isn't found
# :: 2) Start again when the solution isn't found
# :: It's used the best random generator chosen after performing a couple of tests
# :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
# :: Compilation
# :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
# :: It has been successfully proved in Visual Studio Community 2017 15.2
# :: Also https://repl.it/@jeweells/Homework8
# :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
# :: Input
# :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
# :: Only numbers will be inserted on the input, using the instructions shown in the output
# :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
# :: Explanation
# :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
# :: To N-Queens problem will make an algorithm as if it were brute force
# :: 
# :: We set a queen in one position on the current column index (it's chosen randomly for the available indexes, 
# :: since we're solving this randomly)
# :: 
# :: We create a set with all indexes for rows (these are the available indexes where randomly will be taken),
# ::  and we remove one index if there's a queen on that row
# :: 
# :: We check the queens we've placed and for each of them we remove the indexes that are, diagonally,
# :: intercepting the current column. This would indexes would be: qri + cci - qci 
# :: and qri - cci + qci
# :: where:
# :: qri: Row index of the queen chosen
# :: cci: Current column index we're deleting indexes
# :: qci: Column index of the queen chosen
# :: 
# :: if this set has no elements it means the current solution posed is not a solution
# :: 
# :: Here depending on the algorithm chosen we decide whether to start again (set the current column index to 0)
# :: or undone the previous action (substract 2 to the current column index(It's 2 since we haven't placed anything
# :: on the current column index and we need to undone the previous one))
# :: 
# :: Everything above is repeated until we finish placing queens on all the columns (a solution was found)
# :: 
# :: It's important to notice that it might repeat the same path again, so I decided to make it memorize
# :: each wrong solution, so that it will get closer to a solution each failure
# :: 
# :: For that a dictionary is made where the key of each path is a concatenation of each row index
# :: where the queen was placed on that column (reading it from left to right represent the columns)
# :: they're separated by a period ., on that dictionary is stored the following moves that made it to fail
# :: 
# :: Say the key "1": would store 3 if the program failed when it had placed
# :: the queens as 1 3 and on that step it was found that there were no posible queen to be placed
# :: 
# :: Then the next time it tries to solve it and it places the queen on 1, the next queen won't be 3
# :: 
# :: When deleting elements from the set that has the indexes of all rows, if the current key exists
# :: We delete the rows on that set (The dictionary can contain a set of rows that failed)
# :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

import time

# Linear congruence generator
class RandomLCG:
	_mode = 0
	__modeset = [
	{# Mode 0 Numerical Recipes
		"a": 1664525,
		"m": 2**32,
		"c": 1013904223
	},
	{# Mode 1 Borland C/C++
		"a": 22695477,
		"m": 2**32,
		"c": 1
	},
	{# Mode 2 glibc (used by GCC)
		"a": 1103515245,
		"m": 2**31,
		"c": 12345
	},
	{# Mode 3 Park y Miller
		"a": 16807,
		"m": 2147483647,
		"c": 0
	}]
	_seed = 0
	def __init__(self, seed=1, mode=0):
		self._mode = mode % len(self.__modeset)
		self._seed = seed
	def take_next_int(self):
		variables = self.__modeset[self._mode]
		self._seed = (variables["a"] * self._seed + variables["c"]) % variables["m"] # (a*x + c) mod m
		self._seed = abs(self._seed)
		return self._seed
	def take_next(self):
		return self.take_next_int() * 1.0/self.__modeset[self._mode]["m"]


class Ran1:
	_seed = 0
	_IA = 16807
	_IM = 2147483647
	_AM = (1.0/_IM)
	_IQ = 127773
	_IR = 2836
	_NTAB = 32
	_NDIV = (1+(_IM-1)/_NTAB)
	_EPS = 1.2e-7
	_RNMX = (1.0-_EPS)
	_iy = 0
	_iv = [0] * _NTAB
	def __init__(self, seed = 1):
		self._seed = seed
	def take_next(self):
		if (self._seed <= 0 or not self._iy):
			if -self._seed < 1:
				self._seed=1
			else: 
				self._seed = -self._seed
			for j in range(self._NTAB+7, -1, -1):
				k=int(self._seed/self._IQ)
				self._seed=self._IA*(self._seed - k*self._IQ)-self._IR*k
				if self._seed < 0:
					self._seed += self._IM
				if j < self._NTAB:
					self._iv[j] = self._seed
			self._iy=self._iv[0];
		k=int(self._seed/self._IQ)
		self._seed=self._IA * (self._seed- k * self._IQ) - self._IR * k
		if self._seed < 0:
			self._seed += self._IM;
		j = int(self._iy / self._NDIV);
		self._iy=self._iv[j];
		self._iv[j] = self._seed;
		temp = self._AM * self._iy
		if temp > self._RNMX:
			return self._RNMX
		return temp
	def take_next_int(self):
		return int(self.take_next() * self._IM)

class Ran0:
	_seed = 0
	_IA = 16807 # A
	_IM = 2147483647 # M
	_AM = (1.0/_IM) # M
	_IQ = 127773 # Q
	_IR = 2836 # R
	_MASK = 123459876
	def __init__(self, seed = 0):
		self._seed = seed
	def take_next_int(self):
		idum = self._seed ^ self._MASK
		k = int(idum / self._IQ)
		idum = self._IA * (idum - k * self._IQ) - self._IR * k
		if idum < 0: idum += self._IM;
		ans = idum
		idum ^= self._MASK
		self._seed = idum
		return ans
	# Returns a float number
	def take_next(self):
		return self._IA * self.take_next_int()

# Random class from C# https://referencesource.microsoft.com/#mscorlib/system/random.cs
class RandomCSharp:
	_inextp = int()
	_inext = int()
	_seed = int()
	__mseed = 161803398
	_seed_array = [int()] * 56
	__int32maxvalue = 0x7fffffff
	__int32minvalue = 0x80000000
	def __init__(self, seed = None):
		if(seed is None):
			seed = int(time.clock())
    # This algorithm comes from Numerical Recipes in C (2nd Ed.)
		substraction = int(self.__int32maxvalue if(seed == self.__int32minvalue) else abs(seed))
		mj = self.__mseed - substraction
		self._seed_array[55] = int(mj)
		mk = 1
		for i in range(1, 55):#Apparently the range [1..55] is special (Knuth) and so we're wasting the 0'th position.
			ii = (21*i) % 55
			self._seed_array[ii] = mk
			mk = mj - mk;
			if (mk<0):
				mk+= self.__int32maxvalue
				mj= self._seed_array[ii]
		for k in range(1,5):
			for i in range(1,56):
				self._seed_array[i] -= self._seed_array[1+(i+30)%55]
				if self._seed_array[i] < 0:
					self._seed_array[i] += self.__int32maxvalue;
		self._inext = 0
		self._inextp = 21
		self._seed = 1 

	def take_next_int(self):
		locINext = self._inext + 1
		locINextp = self._inextp + 1
		if (locINext >=56):
			locINext = 1
		if (locINextp >= 56):
			locINextp = 1
		retVal = self._seed_array[locINext] - self._seed_array[locINextp]
		if (retVal == self.__int32maxvalue):
			retVal -= 1          
		if (retVal<0):
			retVal += self.__int32maxvalue
		
		self._seed_array[locINext] = retVal
		self._inext = locINext
		self._inextp = locINextp
		# This value is in [0, self.__int32maxvalue)

		return retVal
	# Next float
	def take_next(self):
		return 1.0/self.__int32maxvalue * self.take_next_int()

# Linear congruence generator
class RandomSCH:
	_mode = 0
	__modeset = [
	{# Mode 0
		"a": 214362,
		"r": 1,
		"q": 17010,
		"m": 3646297621
	},
	{# Mode 1
		"a": 675250,
		"r": 4567,
		"q": 5184,
		"m": 3500500567
	},
	{# Mode 2
		"a": 85241,
		"q": 47979,
		"r": 7628,
		"m": 4089785567
	},
	{# Mode 3
		"a": 16807,
		"q": 127773,
		"r": 2836,
		"m": 2147483647
	}]
	_seed = 0
	def __init__(self, seed=1, mode=0):
		self._mode = mode % len(self.__modeset)
		if(seed <= 0):
			self._seed = 1
		else:
			self._seed = seed % self.__modeset[self._mode]["m"]
	def take_next_int(self):
		variables = self.__modeset[self._mode]
		self._seed = variables["a"] *( self._seed % variables["q"]) - variables["r"] * int(self._seed / variables["q"])
		self._seed = self._seed if self._seed > 0 else self._seed + variables["m"]
		return self._seed
	# Next float
	def take_next(self):
		return self.take_next_int() * 1.0 / self.__modeset[self._mode]["m"]

def chisquaretest(get_next_func, ntimes, maxnumber):
	mytestset = [0] * maxnumber
	for i in range(ntimes):
		tmp = get_next_func()
		mytestset[int(tmp%maxnumber)] += 1
	num1 = 0
	ndivr = ntimes/maxnumber
	for i in range(maxnumber):
		num1 += (mytestset[i] - ndivr)**2
	return num1 / ndivr

# If nomemory = True then it starts again when fails, otherwise goes one step back
def rsolve_n_queens(rand_int_generator_func, n, nomemory = True):
	array = [None] * n
	i = 0
	path = {}
	cur_path_keys = ["."] * (n+1) # Start our path keys
	rowset = range(n)
	rowsetnorows = set() # Same as rowset but rows where queens are are discarded
	while i < n:
		rowsleft = set(rowset)
		for j in range(i):
			x = i - j
			if array[j] + x in rowsleft:
				rowsleft.remove(array[j] + x)
			if array[j] - x in rowsleft:
				rowsleft.remove(array[j] - x)
		rowsleft = rowsleft - rowsetnorows # Erasing horizontal posibilities where queens have been placed
		if cur_path_keys[i] in path:
			rowsleft = rowsleft - path[cur_path_keys[i]] # Each element already visited
		setlen = len(rowsleft)
		if setlen == 0: # Solution no found
			if i == 0:
				return None # Impossible
			if nomemory is True:
				if i > 0:
					if cur_path_keys[i-1] not in path:
						path[cur_path_keys[i-1]] = set()
					path[cur_path_keys[i-1]].add(array[i-1])
				array = [None] * n
				rowsetnorows = set() # Restore all the rows deleted
				i = 0 # Go to the begining
				#print(path[cur_path_keys[i]])
			else:
				if i > 0:
					if cur_path_keys[i-1] not in path:
						path[cur_path_keys[i-1]] = set()
					path[cur_path_keys[i-1]].add(array[i-1])
					i -= 1 # Go 1 step back
					rowsetnorows.remove(array[i]) # Remove the number that indicated there was a queen on that row
					array[i] = None
				# print(str(array) + " pos: " + str(i))
			continue
		idx = 0
		target = rand_int_generator_func() % setlen # Selecting one random position from the set to place the queen
		for elm in rowsleft:
			if idx == target:
				array[i] = elm # New element
				rowsetnorows.add(elm) # Indicates there's a queen on this row (elm)
				cur_path_keys[i+1] = cur_path_keys[i] + "." + str(elm)
				break
			idx += 1
		i += 1
	return array # Solution found

def solvequeens(randgenerator):
	while True:
		testmode = input("""\tElija el tipo de test:\n
	1: Resolver con algoritmo las vegas fallo y reinicia
	2: Resolver con algoritmo las vegas fallo y un paso atrás
	3: Volver
	""")
		if testmode == "3":
			return
		modes = {
			"1": True, # Usando Las Vegas, con reinicio si falla
			"2": False} # Usando Las Vegas, se devuelve 1 paso atras si falla
		if testmode not in modes:
			print("Opción incorrecta")
			continue
		n = input("Introduzca N:")
		start = time.time()
		solu = rsolve_n_queens(randgenerator.take_next_int, int(n), modes[testmode])
		end = time.time()
		cur_time = ( end - start ) * 1000
		print("\tDuración {0} milisegundos. {1}".format(int(cur_time), "No se pudo resolver" if solu is None else "Resuelto."))
		if solu is not None:
			pointsol = []
			print("\tSolución: ")
			for i in range(int(n)):
				line = "\t\t"
				for j in range(int(n)):
					if solu[j] == i:
						line += "Q "
						pointsol.append((i+1,j+1))
					elif (i%2 == 0 and j%2 == 0) or (i%2 != 0 and j%2 != 0):
							line += "██"
					else:
							line += "  "
				print(line)
			print("Forma en puntos [1..."+str(n)+"]: " + str(pointsol))
		return


def dotest(randgenerator):
	while True:
		testmode = input("""\tElija el tipo de test:\n
	1: Resolver con algoritmo las vegas fallo y reinicia
	2: Resolver con algoritmo las vegas fallo y un paso atrás
	3: Volver
	""")
		if testmode == "3":
			return
		modes = {
			"1": True, # Usando Las Vegas, con reinicio si falla
			"2": False} # Usando Las Vegas, se devuelve 1 paso atras si falla
		if testmode not in modes:
			print("Opción incorrecta")
			continue 
		for ntest in [4,8,10,20,50,100,500]: # Testing cases for N using the best random generator
			maxtime = mintime = avgtime = None
			cum = 0
			#solved = False
			#while solved is False:
			for t in range(20): # 20 cases for each test
				print("Para n = {0} caso {1}".format(ntest, t+1))
				start = time.time()
				solu = rsolve_n_queens(randgenerator.take_next_int, ntest, modes[testmode])
				end = time.time()
				cur_time = ( end - start ) * 1000
				if maxtime is None:
					maxtime = mintime = avgtime = cur_time
				else:
					if maxtime < cur_time:
						maxtime = cur_time
					if mintime > cur_time:
						mintime = cur_time
					cum += cur_time
				print("\tDuración {0} milisegundos. {1}".format(int(cur_time), "No se pudo resolver" if solu is None else "Resuelto."))
				if solu is not None:
					print("\tSolución: ")
					for i in range(ntest):
						line = "\t\t"
						for j in range(ntest):
							if solu[j] == i:
								line += "Q "
							elif (i%2 == 0 and j%2 == 0) or (i%2 != 0 and j%2 != 0):
									line += "██"
							else:
									line += "  "

						print(line)
			avgtime = cum / 20.0
			print("Terminado n = {0}. Tmin = {1}, Tmax = {2}, Tavg = {3} (milisegundos)".format(ntest, mintime, maxtime, avgtime))
		return


def main():
	# Random generators
	rcsharp = RandomCSharp(int(time.time())) # From C#
	rran0 = Ran0(int(time.time())) # ran0 
	rran1 = Ran1(int(time.time())) # ran1
	rranlincongr0 = RandomLCG(seed = int(time.time()), mode = 0) # Linear congruence set 1
	rranlincongr1 = RandomLCG(seed = int(time.time()), mode = 1) # Linear congruence set 2
	rranlincongr2 = RandomLCG(seed = int(time.time()), mode = 2) # Linear congruence set 3
	rranschrage0 = RandomSCH(seed = int(time.time()), mode = 0) # Schrage set 1
	rranschrage1 = RandomSCH(seed = int(time.time()), mode = 1) # Schrage set 2
	rranschrage2 = RandomSCH(seed = int(time.time()), mode = 2) # Schrage set 3

	# Storing all generators to be iterated
	randgenerators = {
		"C#": rcsharp, 
		"ran0": rran0, 
		"ran1": rran1,
		"Linear congruence set 1": rranlincongr0,
		"Linear congruence set 2": rranlincongr1,
		"Linear congruence set 3": rranlincongr2, 
		"Schrage set 1": rranschrage0,
		"Schrage set 2": rranschrage1,
		"Schrage set 3": rranschrage2
		}
	r = int(3e1)
	n = int(10*r) # Using N = 10r
	twosqrtr = 2 * (r ** (1/2))
	minkey, minval, rdistance, mindifference = None, None, 1, r
	for key, generator in randgenerators.items():
		valuetest = chisquaretest(generator.take_next_int, n, r)
		difference = abs(valuetest - r)
		tmpdistance = difference / r # Zero means it's r, 1 means too far from r
		print("Probando "+key+" random generator")
		print("\t{0}% cerca de r = {1}".format((1.0-tmpdistance)*100.0, r))
		print("\tTest {0}. {1} < {2} = 2sqrt({3})".format("aprobado" if twosqrtr > difference else "reprobado",
		difference, twosqrtr, r))
		if difference < twosqrtr and (minval is None or rdistance > tmpdistance):
			minkey, minval, rdistance, mindifference = key, valuetest, tmpdistance, difference

	if(mindifference < twosqrtr):
		print("{0} elegido como el mejor con {1}% de cercanía a r".format(minkey, (1.0-rdistance)*100.0))
	else:
		print("Ningún generador pasó el test")
		return
	option = ""
	options = {
	"1": dotest,
	"2": solvequeens
	}
	while True:
		option = input("""Elija una opción:\n
	1: Hacer test para N = 4,8,10,20,50,100,500
	2: Introducir N para resolver las N-Reinas
	3: Salir
	""")
		if option == "3":
			return
		if option not in options:
			print("Opción incorrecta")
			continue
		options[option](randgenerators[minkey]) # Execute option

if __name__ == '__main__':
	main()