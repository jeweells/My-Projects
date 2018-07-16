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
		return take_next_int() * 1.0/self.__modeset[self._mode]["m"]
