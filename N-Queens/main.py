import lincongr
import ran0
import ran1
import randomcsharp
import schrage
import chisquaretest
import time
import nqueens

def solvequeens(randgenerator):
	while True:
		testmode = input("""\tElija el tipo de test:\n
	1: Resolver con algoritmo las vegas fallo y reinicia
	2: Resolver con algoritmo las vegas fallo y un paso atrás
	3: Volver
	""")
		if testmode == "3":
			return
		modes = {
			"1": True, # Usando Las Vegas, con reinicio si falla
			"2": False} # Usando Las Vegas, se devuelve 1 paso atras si falla
		if testmode not in modes:
			print("Opción incorrecta")
			continue
		n = input("Introduzca N:")
		start = time.time()
		solu = nqueens.rsolve_n_queens(randgenerator.take_next_int, int(n), modes[testmode])
		end = time.time()
		cur_time = ( end - start ) * 1000
		print("\tDuración {0} milisegundos. {1}".format(int(cur_time), "No se pudo resolver" if solu is None else "Resuelto."))
		if solu is not None:
			pointsol = []
			print("\tSolución: ")
			for i in range(int(n)):
				line = "\t\t"
				for j in range(int(n)):
					if solu[j] == i:
						line += "##"
						pointsol.append((i+1,j+1))
					elif (i%2 == 0 and j%2 == 0) or (i%2 != 0 and j%2 != 0):
							line += "██"
					else:
							line += "  "
				print(line)
			print("Forma en puntos [1..."+str(n)+"]: " + str(pointsol))
		return


def dotest(randgenerator):
	while True:
		testmode = input("""\tElija el tipo de test:\n
	1: Resolver con algoritmo las vegas fallo y reinicia
	2: Resolver con algoritmo las vegas fallo y un paso atrás
	3: Volver
	""")
		if testmode == "3":
			return
		modes = {
			"1": True, # Usando Las Vegas, con reinicio si falla
			"2": False} # Usando Las Vegas, se devuelve 1 paso atras si falla
		if testmode not in modes:
			print("Opción incorrecta")
			continue 
		for ntest in [4,8,10,20,50,100,500]: # Testing cases for N using the best random generator
			maxtime = mintime = avgtime = None
			cum = 0
			#solved = False
			#while solved is False:
			for t in range(20): # 20 cases for each test
				print("Para n = {0} caso {1}".format(ntest, t+1))
				start = time.time()
				solu = nqueens.rsolve_n_queens(randgenerator.take_next_int, ntest, modes[testmode])
				end = time.time()
				cur_time = ( end - start ) * 1000
				if maxtime is None:
					maxtime = mintime = avgtime = cur_time
				else:
					if maxtime < cur_time:
						maxtime = cur_time
					if mintime > cur_time:
						mintime = cur_time
					cum += cur_time
				print("\tDuración {0} milisegundos. {1}".format(int(cur_time), "No se pudo resolver" if solu is None else "Resuelto."))
				if solu is not None:
					print("\tSolución: ")
					for i in range(ntest):
						line = "\t\t"
						for j in range(ntest):
							if solu[j] == i:
								line += "Q"
							elif (i%2 == 0 and j%2 == 0) or (i%2 != 0 and j%2 != 0):
									line += "█"
							else:
									line += " "

						print(line)
			avgtime = cum / 20.0
			print("Terminado n = {0}. Tmin = {1}, Tmax = {2}, Tavg = {3} (milisegundos)".format(ntest, mintime, maxtime, avgtime))
		return


def main():
	# def is_prime(n):
	# 	ndivs = 0
	# 	for i in range(2, math.ceil(n/2)):
	# 		if n % i == 0:
	# 			ndivs += 1
	# 		if ndivs > 1:
	# 			return False
	# 	return True

	# r1 = lincongr.RandomLCG(mode = 0)
	# for i in range(50):
	# 	print(r1.take_next())

	# r0 = ran0.Ran0(1)
	# for i in range(10):
	# 	print(r0.take_next())

	# r1 = schrage.RandomSCH(seed = 10000, mode =3)
	# for i in range(50):
	# 	print(r1.take_next())

	# r1 = ran1.Ran1(1)
	# for i in range(10):
	# 	print(r1.take_next())

	# Random generators
	rcsharp = randomcsharp.RandomCSharp(int(time.time())) # From C#
	rran0 = ran0.Ran0(int(time.time())) # ran0 
	rran1 = ran1.Ran1(int(time.time())) # ran1
	rranlincongr0 = lincongr.RandomLCG(seed = int(time.time()), mode = 0) # Linear congruence set 1
	rranlincongr1 = lincongr.RandomLCG(seed = int(time.time()), mode = 1) # Linear congruence set 2
	rranlincongr2 = lincongr.RandomLCG(seed = int(time.time()), mode = 2) # Linear congruence set 3
	rranschrage0 = schrage.RandomSCH(seed = int(time.time()), mode = 0) # Schrage set 1
	rranschrage1 = schrage.RandomSCH(seed = int(time.time()), mode = 1) # Schrage set 2
	rranschrage2 = schrage.RandomSCH(seed = int(time.time()), mode = 2) # Schrage set 3

	# Storing all generators to be iterated
	randgenerators = {
		"C#": rcsharp, 
		"ran0": rran0, 
		"ran1": rran1,
		"Linear congruence set 1": rranlincongr0,
		"Linear congruence set 2": rranlincongr1,
		"Linear congruence set 3": rranlincongr2, 
		"Schrage set 1": rranschrage0,
		"Schrage set 2": rranschrage1,
		"Schrage set 3": rranschrage2
		}
	r = int(3e1)
	n = int(10*r) # Using N = 10r
	twosqrtr = 2 * (r ** (1/2))
	minkey, minval, rdistance, mindifference = None, None, 1, r
	for key, generator in randgenerators.items():
		valuetest = chisquaretest.Test(generator.take_next_int, n, r)
		difference = abs(valuetest - r)
		tmpdistance = difference / r # Zero means it's r, 1 means too far from r
		print("Probando "+key+" random generator")
		print("\t{0}% cerca de r = {1}".format((1.0-tmpdistance)*100.0, r))
		print("\tTest {0}. {1} < {2} = 2sqrt({3})".format("aprobado" if twosqrtr > difference else "reprobado",
		difference, twosqrtr, r))
		if difference < twosqrtr and (minval is None or rdistance > tmpdistance):
			minkey, minval, rdistance, mindifference = key, valuetest, tmpdistance, difference

	if(mindifference < twosqrtr):
		print("{0} elegido como el mejor con {1}% de cercanía a r".format(minkey, (1.0-rdistance)*100.0))
	else:
		print("Ningún generador pasó el test")
		return
	option = ""
	options = {
	"1": dotest,
	"2": solvequeens
	}
	while True:
		option = input("""Elija una opción:\n
	1: Hacer test para N = 4,8,10,20,50,100,500
	2: Introducir N para resolver las N-Reinas
	3: Salir
	""")
		if option == "3":
			return
		if option not in options:
			print("Opción incorrecta")
			continue
		options[option](randgenerators[minkey]) # Execute option

if __name__ == '__main__':
	main()