// QR and Cholesky.cpp: define el punto de entrada de la aplicación de consola.
//

#include "stdafx.h"
#include <iostream>
#include <string>
#include <stdio.h>
#include "Matrix.h"

using namespace std;
void print_matrix(Matrix<double>& m, bool transposed = false)
{
	for (int i = 0; i < m.rows; i++)
	{
		for (int j = 0; j < m.columns; j++)
		{
			printf("%.2f\t", ((transposed) ? m[j][i] : m[i][j]));
		}
		cout << endl;
	}
}
Matrix<double>& GetMatrix()
{
rowsagain:
	cout << "Introduzca el número de filas:" << endl;
	int f, c;
	cin >> f;
	if (f <= 0)
	{
		cout << "Error al introducir el número de filas" << endl;
		goto rowsagain;
	}
colsagain:
	cout << "Introduzca el número de columnas:" << endl;
	cin >> c;
	if (c <= 0)
	{
		cout << "Error al introducir el número de columnas" << endl;
		goto colsagain;
	}
	Matrix<double>* matrix = new Matrix<double>(f, c);
	cout << "Introduzca la matriz:" << endl;
	for (int i = 0; i < f; i++)
		for (int j = 0; j < c; j++)
		{
		mtrxvalagain: // Permite introducir el valor erroneo de nuevo
			try {
				cin >> (*matrix)[i][j];
			}
			catch (exception &e) {
				cout << "Valor incorrecto, introduzcalo de nuevo correctamente" << endl;
			}
		}
	return *matrix;
}


double norm(Matrix<double>& vect, int column = 0)
{
	double sum = 0;
	for (int i = 0; i < vect.rows; i++)
		sum += vect[i][column] * vect[i][column];
	return sqrt(sum);
}



Matrix<double>** householder(Matrix<double>& a)
{
	Matrix<double>* R = new Matrix<double>(a);
	Matrix<double>* Q = new Matrix<double>(a.rows, a.rows);
	for (int x = 0; x < a.rows; x++)
		for (int y = 0; y < a.rows; y++)
				(*Q)[x][y] = (x==y)?1.0:0.0;
	cout << endl << endl;
	print_matrix(*R);
	cout << endl << endl;
	print_matrix(*Q);
	Matrix<double>* gbg;
	for (int i = 0; i < a.columns; i++)
	{
		Matrix<double>* u = new Matrix<double>(a.rows - i, 1);
		for (int x = 0; x < a.rows; x++)
			(*u)[x][0] = (*R)[x + i][i];

		cout << "u" << endl;
		(*u)[0][0] -= norm(*u); // u = a - |a|e
		gbg = u;
		u = &((*u) / norm(*u)); // Direccion de u
		delete gbg;
		print_matrix(*u);
		Matrix<double>* ut = new Matrix<double>(1, a.rows - i);
		for (int x = 0; x < a.rows; x++) // Consiguiendo el transpuesto de u
			(*ut)[0][x] = (*u)[x][0];
		print_matrix(*u);
		print_matrix(*ut);
		cout << "after u" << endl;
		cout << "urows: " << u->rows << " ucols:" << u->columns << endl;
		cout << "utrows: " << ut->rows << " utcols:" << ut->columns << endl;

		Matrix<double>* tmp = &((*u)*(*ut)),
			*tmpd = &((*ut) * (*u));

		cout << "hexpsdadsadasd" << endl;
		print_matrix(*tmp);
		Matrix<double>* h = &((*tmp) / (*tmpd)[0][0]);
		delete tmp;
		delete tmpd;
		gbg = h;
		h = &((*h) * -2.0);
		delete gbg;
		for (int x = 0; x < a.rows - i; x++)
			(*h)[x][x] = 1.0 + (*h)[x][x]; // h se le resta a la identidad ()

		
		Matrix<double>* hexpanded = new Matrix<double>(a.rows, a.rows);
		for (int x = 0; x < i; x++)
			(*hexpanded)[x][x] = 1.0;
		for (int x = i; x < a.rows; x++)
			for (int y = i; y < a.rows; y++)
				(*hexpanded)[x][y] = (*h)[x - i][y - i]; // h insertada en la parte abajo derecha de la identidad
		delete h;
		gbg = Q;
		Q = &((*Q) * (*hexpanded));
		delete gbg;
		gbg = R;
		R = &((*hexpanded) * (*R));
		cout << "R:" << endl;
		print_matrix(*R);
		delete gbg;
		delete hexpanded;
	}
	Matrix<double>** setmtr = new Matrix<double>*[2];
	print_matrix(*Q);
	print_matrix(*R);
	setmtr[0] = Q;
	setmtr[1] = R;
	return setmtr;
}

Matrix<double>& cholesky(Matrix<double>& a)
{
	int n = a.columns;
	Matrix<double>* lower = new Matrix<double>(a.columns, a.columns); // Debe ser cuadrada
	for (int j = 0; j < n; j++)
	{
		double sum = 0;
		for (int i = 0; i < j; i++)
		{
			sum = 0;
			for (int k = 0; k < i; k++)
				sum += ((*lower)[k][i] * (*lower)[k][j]);
			(*lower)[i][j] = (a[i][j] - sum) / (*lower)[i][i];
		}
		sum = 0;
		for (int k = 0; k < j; k++)
		{
			sum += ((*lower)[k][j] * (*lower)[k][j]);
		}
		double tmp = a[j][j] - sum;
		if (tmp < 0) throw invalid_argument("");
		(*lower)[j][j] = sqrt(tmp);
	}
	return *lower;
}



int main() {
	while (true)
	{

		cout << "¿Que desea hacer?" << endl;
		string input;
		cin >> input;
		Matrix<double> a(3, 1);
		Matrix<double> b(3, 1);
		Matrix<double> r = a + b;
		if (input.compare("chol") == 0) // Calcular cholesky
		{
			Matrix<double>* mtr = &GetMatrix(); // Pedir matriz
			try {
				if (mtr->columns != mtr->rows) throw "";
				Matrix<double>* result = &cholesky(*mtr);
				cout << "Matriz L = " << endl;
				print_matrix(*result, true);
				cout << "Matriz U = " << endl;
				print_matrix(*result, false);
				delete result;
			}
			catch (exception)
			{
				cout << "No se puede calcular" << endl;
			}
			delete mtr; // Eliminar la matriz de la memoria
		}
		else if (input.compare("qr") == 0)
		{
			Matrix<double>* mtr = &GetMatrix(); // Pedir matriz
			try {
				Matrix<double>** qr = householder(*mtr);
				cout << "Matriz Q = " << endl;
				print_matrix(*(qr[0]));
				cout << "Matriz R = " << endl;
				cout << &qr[1] << endl;
				print_matrix(*(qr[1]));
				delete qr[0];
				delete qr[1];
				delete qr;
			}
			catch (exception)
			{
				cout << "No se puede calcular" << endl;
			}
			delete mtr; // Eliminar la matriz de la memoria
		}
		else if (input.compare("salir") == 0)
		{
			return 0;
		}
		else {
			cout << "Comando desconocido." << endl;
		}
	}
}

