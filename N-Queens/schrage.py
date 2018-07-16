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
