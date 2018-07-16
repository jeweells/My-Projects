# If nomemory = True then it starts again when fails, otherwise goes one step back
# def rsolve_n_queens(rand_int_generator_func, n, nomemory = True):
# 	array = [None] * n
# 	i = 0
# 	path = {}
# 	cur_path_keys = ["."] * (n+1) # Start our path keys
# 	rowset = range(n)
# 	rowsetnorows = set() # Same as rowset but rows where queens are are discarded
# 	while i < n:
# 		rowsleft = set(rowset)
# 		for j in range(i):
# 			x = i - j
# 			if array[j] + x in rowsleft:
# 				rowsleft.remove(array[j] + x)
# 			if array[j] - x in rowsleft:
# 				rowsleft.remove(array[j] - x)
# 		rowsleft = rowsleft - rowsetnorows # Erasing horizontal posibilities where queens have been placed
# 		if cur_path_keys[i] in path:
# 			rowsleft = rowsleft - path[cur_path_keys[i]] # Each element already visited
# 		setlen = len(rowsleft)
# 		if setlen == 0: # Solution no found
# 			if i == 0:
# 				return None # Impossible
# 			if nomemory is True:
# 				if i > 0:
# 					if cur_path_keys[i-1] not in path:
# 						path[cur_path_keys[i-1]] = set()
# 					path[cur_path_keys[i-1]].add(array[i-1])
# 				array = [None] * n
# 				rowsetnorows = set() # Restore all the rows deleted
# 				i = 0 # Go to the begining
# 				#print(path[cur_path_keys[i]])
# 			else:
# 				if i > 0:
# 					if cur_path_keys[i-1] not in path:
# 						path[cur_path_keys[i-1]] = set()
# 					path[cur_path_keys[i-1]].add(array[i-1])
# 					i -= 1 # Go 1 step back
# 					rowsetnorows.remove(array[i]) # Remove the number that indicated there was a queen on that row
# 					array[i] = None
# 				# print(str(array) + " pos: " + str(i))
# 			continue
# 		idx = 0
# 		target = rand_int_generator_func() % setlen # Selecting one random position from the set to place the queen
# 		for elm in rowsleft:
# 			if idx == target:
# 				array[i] = elm # New element
# 				rowsetnorows.add(elm) # Indicates there's a queen on this row (elm)
# 				cur_path_keys[i+1] = cur_path_keys[i] + "." + str(elm)
# 				break
# 			idx += 1
# 		i += 1
# 	return array # Solution found








def rsolve_n_queens(rand_int_generator_func, n, nomemory = True):
	import time
	array = [None] * n
	i = 0
	path = {}
	cur_path_keys = ["."] * (n+1) # Start our path keys
	rowset = range(n)
	rowsetnorows = set() # Same as rowset but rows where queens are are discarded
	diagonalup = [set() for _ in range(n)] # Stores where queens CAN'T be due to other queens that are placed diagonally
	# e.g from be (3,3) a queen. Then (4,2), (5,1) and so on are positions where any other queens can't be placed
	diagonaldown = [set() for _ in range(n)] # Stores where queens CAN'T be due to other queens that are placed diagonally
	# e.g from be (3,3) a queen. Then (4,4), (5,5) and so on are positions where any other queens can't be placed


	totaltime = 0
	erasingtime = 0
	targetingtime = 0
	starttt = time.time()
	while i < n:
		rowsleft = set(rowset)
		# for j in range(i):
		# 	x = i - j
		# 	if array[j] + x in rowsleft:
		# 		rowsleft.remove(array[j] + x)
		# 	if array[j] - x in rowsleft:
		# 		rowsleft.remove(array[j] - x)
		# print("diagonal up:", diagonalup)
		# print("diagonal down", diagonaldown)

# Erasing section
		startes = time.time()
		if i > 0:
			# for e in diagonalup[i-1]:
			# 	if e > 0:
			# 		diagonalup[i].add(e-1)
			# 	if e-1 in rowsleft:
			# 		rowsleft.remove(e-1)
			# for e in diagonaldown[i-1]:
			# 	if e + 1 < n:
			# 		diagonaldown[i].add(e+1)
			# 	if e+1 in rowsleft:
			# 		rowsleft.remove(e+1)
			diagonalup[i] |= set([e-1 for e in diagonalup[i-1] if e > 0])
			diagonaldown[i] |= set([e+1 for e in diagonaldown[i-1] if e + 1 < n])
			rowsleft -= diagonalup[i]
			rowsleft -= diagonaldown[i]
		rowsleft -= rowsetnorows # Erasing horizontal posibilities where queens have been placed
		if cur_path_keys[i] in path:
			rowsleft = rowsleft - path[cur_path_keys[i]] # Each element already visited
		endes = time.time()
		erasingtime += endes - startes
# End erasing section
		setlen = len(rowsleft)
		if setlen == 0: # Solution no found
			if i == 0:
				return None # Impossible
			if nomemory is True: # Reset
				if cur_path_keys[i-1] not in path:
					path[cur_path_keys[i-1]] = set()
				path[cur_path_keys[i-1]].add(array[i-1])
				array = [None] * n
				diagonaldown = [set() for _ in range(n)]
				diagonalup = [set() for _ in range(n)]
				rowsetnorows = set() # Restore all the rows deleted
				i = 0 # Go to the begining
				#print(path[cur_path_keys[i]])
			else: # Go one step backwards
				i -= 1 # Go 1 step back
				if cur_path_keys[i] not in path:
					path[cur_path_keys[i]] = set()
				path[cur_path_keys[i]].add(array[i])
				rowsetnorows.remove(array[i]) # Remove the number that indicated there was a queen on that row
				array[i] = None
				diagonaldown[i] = set()
				diagonaldown[i+1] = set()
				diagonalup[i] = set()
				diagonalup[i+1] = set()
				# print(str(array) + " pos: " + str(i))
			continue
		idx = 0
		starttgt = time.time()
		target = 0#rand_int_generator_func() % setlen # Selecting one random position from the set to place the queen
		for elm in rowsleft:
			if idx == target:
				array[i] = elm # New element
				diagonalup[i].add(elm)
				diagonaldown[i].add(elm)
				rowsetnorows.add(elm) # Indicates there's a queen on this row (elm)
				cur_path_keys[i+1] = cur_path_keys[i] + "." + str(elm)
				break
			idx += 1
		endtgt = time.time()
		targetingtime += endtgt - starttgt
		i += 1
	endtt = time.time()
	totaltime += endtt - starttt
	print("Total time:", totaltime)
	print("Erasing time:", erasingtime, " %", str(erasingtime/totaltime *100.0))
	print("Targeting time:", targetingtime, " %", str(targetingtime/totaltime*100.0))
	return array # Solution found




# 	array = [None] * n
# 	for i in range(n):
# 		rowsleft = set(range(n))
# 		for j in range(i):
# 			if array[j] in rowsleft:
# 				rowsleft.remove(array[j]) # Can't be a queen horizontally to that queen
# 			x = i - j
# 			if array[j] + x in rowsleft:
# 				rowsleft.remove(array[j] + x)
# 			if array[j] - x in rowsleft:
# 				rowsleft.remove(array[j] - x)
# 		setlen = len(rowsleft)
# 		if setlen == 0:
# 			return rsolve_n_queens_no_memory(rand_int_generator_func, n) # Solution no found, try again
# 		idx = 0
# 		target = rand_int_generator_func() % setlen # Selecting one random position from the set to place the queen
# 		for elm in rowsleft:
# 			if idx == target:
# 				array[i] = elm
# 				break
# 			idx += 1
# 	return array # Solution found


