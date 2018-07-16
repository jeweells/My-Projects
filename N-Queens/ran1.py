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