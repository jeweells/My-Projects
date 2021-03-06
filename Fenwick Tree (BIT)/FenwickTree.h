// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: FenwickTree (Works with any commutable operator)
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Date: April 6, 2018
// :: Author: Abraham José Pacheco Becerra
// :: E-Mail: abraham.pacheco6319@gmail.com
// :: Description: This is a fenwick tree that not only performs sums, it can perform conjunctions, disjunctions,
// :: exclusive disjunctions and almost all (or all) operators with commutative property. Also it's possible to
// :: get acumulated values between ranges i..j
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: FenwickTree Functions
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: GetLength(): Returns the number of elements in the tree
// :: GetTreeOperation(): Returns the operator attached to the selected tree
// :: GetAccumulated(int f,int t): Returns the accumulated from f to t (with f and t in it)
// :: GetAccumulated(int idx): Returns the accumulated from 0 to idx (with 0 and idx in it)
// :: Update(int pos, int value): Updates the whole tree by setting value in the position pos
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: How to build new operators?
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Implement a struct with the operator () with two (only two) parameters
// :: The parameters must be int type as well as the returned value
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

using namespace std;

template <typename operation>
class FenwickTree {
private:
	operation apply_op;
	bool buildTree; // This works as an inner flag so that the tree can be build using Update(int,int)
	int* acum;
	int* realarray; // Careful! This is only a reference. If this is edited outside the class, the tree won't change. On the other hand, if the tree is edited, the reference will change.
	int length;
public:
	
	FenwickTree<operation>(int arr[], int length)
	{
		buildTree = false;
		this->length = length; // Setting arr length
		realarray = arr; // Saving arr's reference
		acum = new int[length]; // Creating the tree array
		BuildTree();
		/*for (int i = 0; i < length; i++) // Shows each acumulated array value
		{
			cout << "A[" << i + 1 << "] = " << acum[i] << endl;
		}*/
	}
	int GetLength(){ return length; }
	operation GetTreeOperation() { return apply_op; }

	// Gets the accumulated from f to t
	// The biggest range is [0, acum.length)
	int GetAccumulated(int f, int t)
	{
		if (f == t) // Only one value will lead to the realarray, no need to calculate anything
		{
			return realarray[f];
		}
		if (f > t) // Can't happen, values will be exchanged
		{
			int a = f;
			f = t;
			t = a;
		}
		if (f < 0 || t < 0) // The minimal position is 0
		{
			return -1;
		}
		if (t >= length) // We're flexible and we take it as the last one possible
		{
			t = length - 1;
		}
		t++; // Plus 1 so we can see the array as [1,acum.length]
		f++;
		int temp = (t&-t); // This shows how many cumulated values are in the position t
		int accumulated;
		// If there are more values in the cumulated array than between f and t in the real array, 
		// then we only get the value in the real array (not cumulated)
		if (t - temp < f)
		{
			t--; // Now we are getting the value before t
			accumulated = realarray[t]; // This is t due t was initially treated as [1,length]
		}
		else {
			accumulated = acum[t - 1]; // Now we get the accumulated value
			t -= temp; // Find the next t substracting the values previously obtained
		}
		// Now we're doing the same as before but inside a while
		while (t >= f)
		{
			temp = (t&-t); // This shows how many cumulated values are in the position t
			// If there are more values in the cumulated array than between f and t in the real array, 
			// then we only get the value in the real array (not cumulated)
			if (t - temp < f)
			{
				t--; // Now we are getting the value before t
				accumulated = apply_op(accumulated, realarray[t]);
			}
			else {
				accumulated = apply_op(accumulated, acum[t - 1]);
				t -= temp; // Find the next t substracting the values previously obtained
			}
		}

		return accumulated;
	}
	// Gets the acumulated in the position idx
	// The possition must be between [0, acum.length)
	int GetAccumulated(int idx)
	{
		return GetAccumulated(0, idx);
	}

	// Sets value in the arrays at the position pos
	void Update(int pos, int value)
	{
		realarray[pos] = value; // Updates the value in the real array (plane values)
		int elms;

		// If the function was called to build the tree, then you just jump one position
		// If so, pos should be 0 and value should be realarray[0]
		for (int i = pos + 1; i <= length; i+= ((buildTree)? 1 : elms)) 
		{
			elms = (i&-i); // Calculate the number of elements that are accumulated in the position i
			acum[i - 1] = realarray[i - 1]; // Sets the value in the position i

			// If there are more than 1 accumulated elements in the position i
			// Then we need to find a way to use the accumulated elements we already have before i
			if (elms > 1) 
			{
				for (int j = i-1; j > i - elms; j-= (j&-j)) 
				{
					// We start from i-1 because we already set the value of i
					// We need to substract to j the elements accumulated in the postion j
					// Example:
					// ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀
					// ▀ ▀ ▀ ▀
					// ▀ ▀     ▀ ▀
					// ▀   ▀   ▀   ▀
					// 1 2 3 4 5 6 7 8
					// We want to edit the elm 3
					// So we edit 3 and we need to edit the accumulated in 4 and 8
					// To edit 4, we edit 4 and get the accumulated of 3 and 2 (Also operate them with the accumulated in 4)
					// To edit 8, we edit 8 and get the accumulated of 7, 6 and 4 (Also operate them with the accumulated in 8)
					// We always edit the element i, then we get the accumulated of j (starting from j=i-1)
					// Then we substract to j the amount of elements in j in order to know what accumulated we need to get after
					// (Also operate them with the accumulated in i)
					acum[i - 1] = apply_op(acum[i-1], acum[j - 1]);
					
				}
			}
			
		}
	}
private:
	// This function builds the tree for the first time.
	// length, realarray, acum, treeoperation must be initialized
		void BuildTree()
		{
			// Builds the tree more efficiently
			buildTree = true;
			Update(0, realarray[0]);
			buildTree = false;
		}


};
