class DailyCoding9{
	public static int LargestSum(int[] arr)
	{
		int index = 0;
		int length = arr.Length;
		int maxin = Max(arr);
		if(maxin <= 0) return maxin; // All negatives, only one element is the largest sum
		// Imagine a pointer, which starts at a in
		// [a, b, c, d, _, a, ...] (it's repeated)
		// This patern is repeated is iteration which gives us to this conclusion:
		// Taking these elements
		// [a,,c,,*] (a+c)
		// [a,,,d,_,*] (a+d)
		// [,b,,d,_,*] (b+d)
		// If we know which is the biggest
		// we will start at * on the next iteration depending on the case and * will be the new a 
		int max_sum = 0;
		while(index < length)
		{
			int a = arr[index];
			int b = (index+1 < length && arr[index+1] > 0)? arr[index+1] : 0;
			int c = (index+2 < length && arr[index+2] > 0)? arr[index+2] : 0;
			int d = (index+3 < length && arr[index+3] > 0)? arr[index+3] : 0;
			int cur_max = Max(new[]{ a+c, a+d, b+d});
			if(cur_max == a+c) index += 4; // Go to _
			else index += 5; // Go to second a
			max_sum += cur_max;
		}
		return max_sum;
	}

	static int Max(int[] arr) // Max in an array
	{
		int e = arr[0];
		for(int i = 1, endi = arr.Length; i < endi; i++)
		{
			if(arr[i] > e) e = arr[i];	
		}
		return e;
	}
}
