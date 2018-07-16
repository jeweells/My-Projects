# cons(a, b) constructs a pair, and car(pair) and cdr(pair) returns the first and last element of that pair. For example, car(cons(3, 4)) returns 3, and cdr(cons(3, 4)) returns 4.

# Given this implementation of cons:

# def cons(a, b):
#     def pair(f):
#         return f(a, b)
#     return pair
# Implement car and cdr.

def car(func):
	def f(a,b):
		return a
	return func(f)

def cdr(func):
	def f(a,b):
		return b
	return func(f)

def cons(a, b):
    def pair(f):
        return f(a, b)
    return pair

print(str(car(cons(10,5)))+", "+str(cdr(cons(10,5))))