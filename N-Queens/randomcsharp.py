import time
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