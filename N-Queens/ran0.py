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