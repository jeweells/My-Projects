def justify(words, k):
	__doc__ = """Write an algorithm to justify text. Given a sequence of words and an integer line length k, return a list of strings which represents each line, fully justified.

More specifically, you should have as many words as possible in each line. There should be at least one space between each word. Pad extra spaces when necessary so that each line has exactly length k. Spaces should be distributed as equally as possible, with the extra spaces, if any, distributed starting from the left.

If you can only fit one word on a line, then you should pad the right-hand side with spaces.

Each word is guaranteed not to be longer than k.

For example, given the list of words ["the", "quick", "brown", "fox", "jumps", "over", "the", "lazy", "dog"] and k = 16, you should return the following:

["the  quick brown", # 1 extra space on the left
"fox  jumps  over", # 2 extra spaces distributed evenly
"the   lazy   dog"] # 4 extra spaces distributed evenly"""
	lines = []
	iterator = iter(words) # I use an iterator separatelly in order to know when we get the last item
	w = next(iterator)
	while(True):
		nwords = []
		currlen = 0
		while(True):
			normalspaces = len(nwords) - 1 # e.g 3 words 2 spaces (a.b.c) where . is a space
			if currlen + len(w) + normalspaces + 1 > k: # + 1 since we assume we added that new word, we try to check if it fits in the line
				break
			currlen += len(w)
			nwords.append(w)
			w = next(iterator, None)
			if w is None:
				normalspaces = len(nwords) - 1 # Compute this on the last iteration as well
				break
		if normalspaces == -1: # This is not necessary (each word is guaranteed not to be longer than k), but I put it anyway otherwise it never stops when a word is longer than k
			return [] # Impossible to fit
		totalspaces = k - currlen - normalspaces # Number of additional spaces (discarding the one between two words) that can be on the line
		# Distributing spaces equally
		spacesperword = totalspaces // normalspaces if normalspaces > 0 else 0 # Does not matter its value when normalspaces <= 0
		# It's not always posible to have the same amount of spaces between two words
		oddspaces = totalspaces % normalspaces if normalspaces > 0 else 0 # Does not matter its value when normalspaces <= 0
		tmpstr = ""
		for tmpw in nwords:
			tmpstr += tmpw
			if normalspaces > 0: # This helps to avoid adding spaces at the end
				for i in range(spacesperword + 1): # +1 since we add the -must be there- space
					tmpstr += " "
				normalspaces -= 1
			if oddspaces > 0: # Odd spaces will be manifesting at the begining
				tmpstr += " "
				oddspaces -= 1
		lines.append(tmpstr.ljust(k, " ")) # We add the word and pad it with spaces (This is useful when we have only one word which means normalspaces == 0)
		if w is None:
			break # Job done
	return lines
for line in justify(["the", "quick", "brown", "fox", "jumps", "over", "the", "lazy", "dog"], 16):
	print(line.replace(" ", "."), len(line))