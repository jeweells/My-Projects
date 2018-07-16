#Given an integer k and a string s, find the length of the longest substring that contains at most k distinct characters.

#For example, given s = "abcba" and k = 2, the longest substring with k distinct characters is "bcb".
def findlongstr(word, ndiffchar):
	bestdiff = ""
	for i in range(0, len(word)):# Take each letter
		setdiff = set() # Initialize a set to know how many different letters we're taking
		for j in range(i, len(word)):#From that letter to the end
			if word[j] not in setdiff:# A new letter found
				setdiff.add(word[j]) # Add the letter
				if len(setdiff) > ndiffchar:
					break # Overpassed the max number of differences
		if len(setdiff) <= ndiffchar:# The whole word should be returned
			if len(bestdiff) < j+1 - i: # The new best must be longer in order to replace the old one
					bestdiff = word[i:j+1] # j+1 since j+1 isn't taken, but j is
		else: # A break occurred
			if len(bestdiff) < j - i: # from j to j due to j+1 would take the new different letter that broke the loop
					bestdiff = word[i:j]
	return bestdiff

print(findlongstr("abcba", 2))