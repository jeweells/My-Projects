/// <summary>
/// Finds the lowest missing number in the array, starting from start_from (inclusive) in O(n)
/// WARNING: This function modifies the array
/// </summary>
/// <returns>starting from start_from (inclusive) to start_from + arr.Length (inclusive)</returns>
static int FindLowestMissing(int[] arr, int start_from = 0)
{
	int length = arr.Length;
	bool isTheFirst = true;
	for(int i = length - 1; i >= 0; i--)
	{
		if(arr[i] == start_from)
		{
			isTheFirst = false;
			continue;
		}
		if (arr[i] < start_from || arr[i] >= start_from + length)
			arr[i] = start_from; // Sets numbers out of the possible returned value range as the minimal number to be returned
	}
	if (isTheFirst) return start_from; // if the first element wasn't found, it means this is the missing number
	int helper = length + start_from;
	for(int i = 0; i < length; i++)
	{
		if(arr[i] >= helper) // Check if it's out of the range
		{
			if (arr[arr[i] - helper] < helper) // Access the position indicated in its value restoring that number by putting it into the range and shiftening it to fit in 0 to length
				arr[arr[i] - helper] += length; // Take out of the range the number if it's not out of the range
		}
		else
		{
			if (arr[arr[i] - start_from] < helper) // Just shift the value in the array to fit in 0 to length and access it
				arr[arr[i] - start_from] += length; // Take out of the range the number if it's not out of the range
		}
	}
	for (int i = 0; i < length; i++)
		if (arr[i] < helper) // If there's one number that's still in the range 0 to length, that's the minimum
			return i + start_from;
	return length + start_from; // There are no numbers in the range 0 to length so the minimum is the next number that follows the biggest 
}