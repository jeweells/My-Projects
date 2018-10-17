def is_balanced(sec):
	__doc__ = """
Given a string of round, curly, and square open and closing brackets, return whether the brackets are balanced (well-formed).

For example, given the string "([])[]({})", you should return true.

Given the string "([)]" or "((()", you should return false."""
	mystack = []
	for c in sec:
		if c == '{' or c == '(' or c == '[':
			mystack.append(c)
		elif (c == '}' and mystack[-1] == '{') or (c == ')' and mystack[-1] == '(') or (c == ']' and mystack[-1] == '['):
			mystack.pop()
		else:
			return False
	return len(mystack) == 0

print(is_balanced("([])[]({})"))
print(is_balanced("([)]"))
print(is_balanced("((()"))
