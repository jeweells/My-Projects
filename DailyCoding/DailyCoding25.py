def match(re, word):
	__doc__ = """ 
Daily Coding 25

Implement regular expression matching with the following special characters:

. (period) which matches any single character
* (asterisk) which matches zero or more of the preceding element
That is, implement a function that takes in a string and a valid regular expression and returns whether or not the string matches the regular expression.

For example, given the regular expression "ra." and the string "ray", your function should return true. The same regular expression on the string "raymond" should return false.

Given the regular expression ".*at" and the string "chat", your function should return true. The same regular expression on the string "chats" should return false.
"""
	widx = 0
	ridx = 0

	def clean_and_count(re):
		ask_count = 0
		last_ask = False
		new_re = ""
		for c in re:
			if c == '*':
				if last_ask is True:
					continue
				last_ask = True
				new_re += c
				ask_count += 1
			else:
				new_re += c
				last_ask = False
		return new_re, ask_count
	re, ask_count = clean_and_count(re)
	
	if (ask_count == 0 and len(re) != len(word)) or (len(re) - ask_count > len(word)):
		return False
	def aux(re, word, ridx, widx):
		while(widx != len(word)):
			if ridx >= len(re):
				return False
			if re[ridx] == '.':
				widx += 1
				ridx += 1
				continue # A character is ignored
			if re[ridx] == '*':
				ridx += 1
				if ridx == len(re):
					return True
				while(widx != len(word)):
					if aux(re, word, ridx, widx) is True:
						return True
					widx += 1
				return False
			if re[ridx] != word[widx]:
				return False
			widx += 1
			ridx += 1
		return True

	return aux(re, word, ridx, widx)

print(match("", ""))