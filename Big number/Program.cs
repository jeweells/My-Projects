// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Homework 5
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Date: May 15, 2018
// :: Author: Abraham José Pacheco Becerra
// :: E-Mail: abraham.pacheco6319@gmail.com
// :: Description: BigNumber class stores a number of X digits. It can sum, subtract 
// :: and multiply. Multiplicating is below O(N^2), returns the value of multiplying two numbers
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Restrictions in Main function
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Digits must be positive, having between 1 and 5000 digits (inclusive)
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Compilation
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: It has been successfully proved in Visual Studio Community 2017 15.2
// :: Also https://repl.it/@jeweells/TAP-Homework-5
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Input
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Number1 Number2
// :: Example:
// :: 59934 884949434
// :: Output: 53038559377356
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Explanation
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Each multipication can be splitted in many parts of the following way:
// :: if we have 59934 884949434, it's the same if we say:
// :: (0*10^5 + 59934) * (8849*10^5 + 49434)
// :: =(0*8849)10^10 + (59934*8849 + 49434*0)10^5 + 59934*49434
// :: we want to get this into that form so
// :: We use the longest number to decide how many digits will have each part splitted.
// :: In this case the longest number has 9 digits, we divide this number by 2, which gives us
// :: 4 after truncating it, but in order to have two parts of that number, we must sum 1, 
// :: this number is found in general cases in adding 1 if 9%2 is greater than 0
// :: or just adding 9%2. Therefore we find that if we want to split this number in two parts
// :: each parts must contain at max 5 digits. Also, the other number will be splitted in this
// :: same amount of digits. We will have for the first number: 8849 and 49434, for the second
// :: number: 0 and 59934. We have now the numbers we needed, and now we repeat the process
// :: for each multiplication (except the ones multiplicating by 10^x)
// :: This means we repeat this for 0*8849, 59934*8849, 59934*49434
// :: Splitting 59934 in two parts: 059 and 934
// :: Splitting 8849 in two parts: 008 and 849
// :: Then we use the formula: (008*059)*10^6 + (008*934+059*849)*10^3 + 934*849 
// :: Doing the same procedure for the numbers.
// :: We stop splitting when the longest number has less than 4 digits.
// :: And we operate as usual, with the same formula.
// :: Joining all the results and summing will give the result desired, and this is below O(N^2)
// :: which is what a normal multiplication would be
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::


using System;

namespace Homework5
{
	class BigNumber
	{
		string number;
		public BigNumber(string number)
		{
			if (number.Length > 0)
			{
				if (number[0] == '-' || Char.IsNumber(number[0]))
				{
					for (int i = 1; i < number.Length; i++)
					{
						if (!Char.IsNumber(number[i])) throw new FormatException("Wrong format for the big number.");
					}
					this.number = number.TrimStart(new[] { '0', '-' });
					if (String.Empty == this.number) this.number = "0";

					if (this.number != "0" && number[0] == '-')
						this.number = '-' + this.number;
				}
				else { throw new FormatException("Wrong format for the big number."); }
			}
			else this.number = "0";
		}
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		static public BigNumber operator +(BigNumber a, BigNumber b)
		{
			return Sum(a, b);
		}
		static public BigNumber operator -(BigNumber a, BigNumber b)
		{
			return Subtraction(a, b);
		}
		static public implicit operator string(BigNumber number)
		{
			return number.number;
		}
		static public implicit operator BigNumber(int number)
		{
			return new BigNumber(number.ToString());
		}
		static public implicit operator BigNumber(uint number)
		{
			return new BigNumber(number.ToString());
		}
		static public implicit operator BigNumber(ulong number)
		{
			return new BigNumber(number.ToString());
		}
		static public implicit operator BigNumber(long number)
		{
			return new BigNumber(number.ToString());
		}
		static public implicit operator BigNumber(byte number)
		{
			return new BigNumber(number.ToString());
		}
		static public BigNumber operator -(BigNumber n)
		{
			return Negate(n);
		}
		static public BigNumber Negate(BigNumber n)
		{
			string a = n.number;
			if (a[0] == '-') a = a.TrimStart('-');
			else a = "-" + a;
			return new BigNumber(a);
		}
		static char CharSum(char a, char b, char pCarry, out char carry)
		{
			int an = Int32.Parse(a + "");
			int bn = Int32.Parse(b + "");
			int pCarryn = Int32.Parse(pCarry + "");
			int anbn = an + bn + pCarryn;
			carry = (anbn / 10).ToString()[0];
			return (anbn % 10).ToString()[0];
		}
		public static BigNumber Sum(BigNumber a, BigNumber b)
		{
			if((a.number[0] == '-' && b.number[0] == '-') || // - plus -  or + plus +
				(a.number[0] != '-' && b.number[0] != '-')) // both cases are a sum
			{
				string result = "";
				string atr = a.number.TrimStart('-');
				string btr = b.number.TrimStart('-');
				string shorteststr = (atr.Length <= btr.Length) ? atr : btr;
				string largeststr = (atr.Length > btr.Length) ? atr : btr;
				char carry = '0';
				bool negativeResult = a.number[0] == '-';
				for (int i = 0; i < shorteststr.Length; i++)
				{
					result = CharSum(
						shorteststr[shorteststr.Length - 1 - i],
						largeststr[largeststr.Length - 1 - i],
						carry,
						out carry) + result;
				}
				// Adding the rest of the numbers
				for (int i = shorteststr.Length; i < largeststr.Length; i++)
				{
					result = CharSum(largeststr[largeststr.Length - 1 - i], '0', carry, out carry) + result;
				}
				if (carry != '0') result = carry + result;
				if (negativeResult) result = '-' + result;
				return new BigNumber(result);
			}
			else // It's a subtraction
			{
				if (a.number[0] == '-') return Subtraction(b, -a);
				else return Subtraction(a, -b);
			}
		}
		static char CharSubtraction(char a, char b, char pCarry, out char carry)
		{ 
			int an = Int32.Parse(a + "");
			int bn = Int32.Parse(b + "");
			int pCarryn = Int32.Parse(pCarry + "");
			if ((an -= pCarryn) < bn)
			{
				carry = '1';
				an += 10;
			}
			else carry = '0';
			int anbn = an - bn;
			return anbn.ToString()[0];
		}
		public static BigNumber Subtraction(BigNumber a, BigNumber b)
		{
			if ((a.number[0] == '-' && b.number[0] != '-') || // - minus +  or + minus -
				(a.number[0] != '-' && b.number[0] == '-')) // both cases are a sum
			{
				return Sum(a, -b);
			}
			else // It's a subtraction ( - minus - or + minus + )
			{
				string atr = a.number.TrimStart('-');
				string btr = b.number.TrimStart('-');
				bool negativeResult;
				char carry = '0';
				string result = "";
				if (new BigNumber(atr) > new BigNumber(btr)) // |a| > |b|
				{
					negativeResult = a.number[0] == '-';
					for (int i = 0; i < btr.Length; i++) // b is the shortest
					{
						result = CharSubtraction(
							atr[atr.Length - 1 - i], 
							btr[btr.Length - 1 - i], 
							carry, 
							out carry) + result;
					}
					for (int i = btr.Length; i < atr.Length; i++)
					{
						result = CharSubtraction(
							atr[atr.Length - 1 - i],
							'0',
							carry,
							out carry) + result;
					}
				}
				else // |b| > |a|
				{
					negativeResult = b.number[0] != '-';
					for (int i = 0; i < atr.Length; i++) // b is the shortest
					{
						result = CharSubtraction(
							btr[btr.Length - 1 - i],
							atr[atr.Length - 1 - i],
							carry,
							out carry) + result;
					}
					for (int i = atr.Length; i < btr.Length; i++)
					{
						result = CharSubtraction(
							btr[btr.Length - 1 - i],
							'0',
							carry,
							out carry) + result;
					}
				}
				if (negativeResult) result = '-' + result;
				return new BigNumber(result);
			}
		}
		public static bool operator ==(BigNumber a, BigNumber b)
		{
			return Compare(a, b) == 0;
		}
		public static bool operator !=(BigNumber a, BigNumber b)
		{
			return Compare(a, b) != 0;
		}
		public static bool operator >(BigNumber a, BigNumber b)
		{
			return Compare(a, b) == -1;
		}
		public static bool operator <(BigNumber a, BigNumber b)
		{
			return Compare(a, b) == 1;
		}
		public static bool operator >=(BigNumber a, BigNumber b)
		{
			return a == b || a > b;
		}
		public static bool operator <=(BigNumber a, BigNumber b)
		{
			return a == b || b > a;
		}
		// Return value: 
		// 0, if equals
		// 1, if b > a
		// -1, if a > b 
		public static int Compare(BigNumber a, BigNumber b)
		{
			if (a.number[0] == '-' && b.number[0] != '-') return 1; // b > a
			else if (a.number[0] != '-' && b.number[0] == '-') return -1; // a > b
			else if (a.number.Length > b.number.Length) return -1; // a > b
			else if (b.number.Length > a.number.Length) return 1; // b > a
			else // Same length
			{
				string astr = a.number.TrimStart('-');
				string bstr = b.number.TrimStart('-');
				for (int i = 0; i < astr.Length; i++)
				{
					int aint = Int32.Parse(astr[i]+"");
					int bint = Int32.Parse(bstr[i]+"");
					if (aint > bint) return -1; // a > b
					else if (bint > aint) return 1; // b > a
				}
			}
			return 0; // Both are the same
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		static BigNumber SmallMultiplication(BigNumber a, BigNumber b)
		{
			BigNumber[] aparts = SplitIn(a, 2, 2);
			BigNumber[] bparts = SplitIn(b, 2, 2);
			int w = Int32.Parse(aparts[0].number);// ww00 from a
			int x = Int32.Parse(aparts[1].number);// 00xx from a
			int y = Int32.Parse(bparts[0].number);// yy00 from b
			int z = Int32.Parse(bparts[1].number);// 00zz from b
			int wy = w * y, 
				wz = w * z, 
				xy = x * y, 
				xz = x * z;
			int result = 10000 * wy + 100 * (wz+xy) + xz;
			return result;
		}
		/// <summary>
		/// Returns the number n multiplied by 10^exp
		/// </summary>
		/// <param name="n"></param>
		/// <param name="exp"></param>
		/// <returns></returns>
		static BigNumber NTimes10To(BigNumber n, int exp)
		{
			string result = n.number;
			for (int i = 0; i < exp; i++) result += "0";
			return new BigNumber(result);
		}
		/// <summary>
		/// Splits the number n in ndigits digits, and forces the array to be length length
		/// </summary>
		/// <param name="n"></param>
		/// <param name="ndigits"></param>
		/// <param name="length">Length of the array</param>
		/// <returns>If the number can be splitted in more parts than length, the right length parts are returned</returns>
		static BigNumber[] SplitIn(BigNumber n, int ndigits, int length)
		{
			BigNumber[] rarray = new BigNumber[length];
			int arrpos = length - 1;
			string tmp = "";
			for (int i = n.number.Length - 1; i >= 0; i--)
			{
				tmp = n.number[i] + tmp;
				if (tmp.Length == ndigits)
				{
					rarray[arrpos] = new BigNumber(tmp);
					arrpos--;
					tmp = "";
					if (arrpos < 0) break;
				}
			}
			while (arrpos >= 0) { rarray[arrpos] = new BigNumber(tmp); tmp = ""; arrpos--; }
			return rarray;
		}
		/// <summary>
		/// Splits the number n in numbers of ndigits digits.
		/// </summary>
		/// <param name="n"></param>
		/// <param name="ndigits"></param>
		/// <returns></returns>
		static BigNumber[] SplitIn(BigNumber n, int ndigits)
		{
			int nnums = n.number.Length / ndigits +
				((n.number.Length % ndigits > 0) ? 1 : 0);
			BigNumber[] rarray = new BigNumber[nnums];
			int arrpos = nnums - 1;
			string tmp = "";
			for (int i = n.number.Length-1; i >= 0; i--)
			{
				tmp = n.number[i] + tmp;
				if(tmp.Length == ndigits)
				{
					rarray[arrpos] = new BigNumber(tmp);
					arrpos--;
					tmp = "";
				}
			}
			if (arrpos == 0) rarray[arrpos] = new BigNumber(tmp);
			return rarray;
		}
		// This functions handles the signs while multiplyaux operates
		static public BigNumber Multiply(BigNumber a, BigNumber b)
		{
			string astr = a.number.TrimStart('-');
			string bstr = b.number.TrimStart('-');
			BigNumber aunsigned = new BigNumber(astr);
			BigNumber bunsigned = new BigNumber(bstr);
			int asign, bsign;
			if (a.number[0] == '-') asign = -1; else asign = 1;
			if (b.number[0] == '-') bsign = -1; else bsign = 1;
			return (asign * bsign < 0) ?
				Negate(MultiplyAux(aunsigned, bunsigned)) :
				MultiplyAux(aunsigned, bunsigned);
		}
		// This function must work without signs
		static BigNumber MultiplyAux(BigNumber a, BigNumber b)
		{
			if (a.number == "0" || b.number == "0") return new BigNumber("0");
			if (a.number == "1") return new BigNumber(b.number);
			else if (b.number == "1") return new BigNumber(a.number);
			if(a.number.Length <= 4 && b.number.Length <= 4)
			{
				return SmallMultiplication(a, b);
			}
			else
			{
				int exp = (a.number.Length > b.number.Length) ?
					a.number.Length / 2 + a.number.Length % 2
					:
					b.number.Length / 2 + b.number.Length % 2;
				BigNumber[] aparts = SplitIn(a, exp, 2);
				BigNumber[] bparts = SplitIn(b, exp, 2);
				// For a example :
				//  12345678
				// x87654321
				// aparts[0] = 1234, aparts[1] = 5678
				// bparts[0] = 8765, bparts[1] = 4321
				// exp = 4, (digits divided by 2)
				// then:
				// ( 1234*10e4 + 5678 ) *
				// ( 8765*10e4 + 4321 )
				// = 1234*8765*10e8 
				// + (1234*4321 + 8765*5678)*10e4
				// + 5678*4321
				// This was the base case, recursively would split the digits until it reaches
				// multiplications of 4 digits by 4 digits or less
				return NTimes10To(MultiplyAux(aparts[0], bparts[0]), 2*exp) 
					+ NTimes10To(MultiplyAux(aparts[0], bparts[1]) + MultiplyAux(aparts[1], bparts[0]), exp)
					+ MultiplyAux(aparts[1], bparts[1]);
			}
		}
		static public BigNumber operator *(BigNumber a, BigNumber b)
		{
			return Multiply(a, b);
		}
	}
	class Program
	{
		static void Main(string[] args)
		{
			string line = Console.ReadLine();
			string[] operands = line.Split(' ');
			if(operands.Length != 2 || // Different than 2 operands
				operands[0].Length > 5000 || // More than 5000 digits
				operands[1].Length > 5000 || // More than 5000 digits
				operands[0][0] == '-' || // Negative
				operands[1][0] == '-') // Negative
			{
				Console.WriteLine("Error in the input format");
			}
			else
			{
				try
				{
					Console.WriteLine(new BigNumber(operands[0]) * new BigNumber(operands[1]));
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
			}
		}
	}
}
