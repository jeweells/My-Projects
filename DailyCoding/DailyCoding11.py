# Implement an autocomplete system. That is, given a query string s and a set of all possible query strings, return all strings in the set that have s as a prefix.

# For example, given the query string de and the set of strings [dog, deer, deal], return [deer, deal].

# Hint: Try preprocessing the dictionary into a more efficient data structure to speed up queries.
# Preprocesses the array to a dictionary
def prep_array(array):
	mdict = {}
	for i in range(0, len(array)):
		tmpdict = mdict # Each word add each letter as a key
		for j in range(0, len(array[i])):
			if array[i][j] not in tmpdict:
				tmpdict[array[i][j]] = {} # Where each letter has a dictionary as a value
			tmpdict = tmpdict[array[i][j]] # And goes deeper each letter to add more letters from the word
		tmpdict["word"] = array[i] # Once the word has been written we add the key "word" which means going this far gives you a word (you can go even deeper to get more words)
	return mdict

# Use prep_array(array) first to get the dictionary
def auto_complete(word, dicty):
	if dicty is None: # No dictionary, no words
		return
	assert type(dicty) is dict, "auto_complete must receive a dictionary"
	tmpdicty = dicty
	for i in word: # Each letter of the word
		if i in tmpdicty: # If it's on the dictionary, we go deeper
			tmpdicty = tmpdicty[i] # Set the dictionary that contains that letter
		else:
			return # No words that starts with word's value
	def print_words(dicty): # DFS in order to return the words
		if dicty is not None:
			for key in dicty:
				if key == "word":
					yield dicty[key]
				else:
					for x in print_words(dicty[key]):
						yield x
	for x in print_words(tmpdicty):
		yield x