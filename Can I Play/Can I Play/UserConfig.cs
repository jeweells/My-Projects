using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.NetworkInformation;
using System.Globalization;

namespace Can_I_Play
{
	public class UserConfig
	{
		ulong minutes;
		ulong pinglimit;
		byte lagtolerance;
		bool tryuntilsuccess;
		public ulong Minutes { get { return minutes; } set { minutes = (value < 0)? 3 : value; } }
		public ulong PingLimit { get { return pinglimit; } set { pinglimit = (value < 0) ? 150 : value; } }
		public byte LagTolerance { get { return lagtolerance; } set { lagtolerance = (value > 100)? (byte)5 : ((value < 0)? (byte) 5 : value); } }
		public bool TryUntilSuccess { get { return tryuntilsuccess; } set { tryuntilsuccess = value; } }

		public UserConfig()
		{
			Minutes = 3;
			PingLimit = 150;
			LagTolerance = 5;
			TryUntilSuccess = true;
		}
		public UserConfig(ArrayList arr)
		{
			string [,] temp = Splitter(arr);
			bool[] found = new bool[4];
			ulong temp1;
			byte temp2;
			for (int i = 0; i < arr.Count; i++)
			{
				if (!found[0] && temp[i, 0].ToLower() == "minutes")
				{
					Minutes = ((ulong.TryParse(temp[i, 1], out temp1)) ? temp1 : 3);
					found[0] = true;
				}
				if(!found[1] && temp[i, 0].ToLower() == "ping limit")
				{
					PingLimit = (ulong.TryParse(temp[i, 1], out temp1)) ? temp1 : 150;
					found[1] = true;
				}
				if (!found[2] && temp[i, 0].ToLower() == "lag tolerance")
				{
					LagTolerance = (byte.TryParse(temp[i, 1], out temp2)) ? temp2 : (byte)5;
					found[2] = true;
				}
				if (!found[3] && temp[i, 0].ToLower() == "try until success")
				{
					string str = temp[i, 1].ToLower();
					while(str.Length > 5 && str[0] == ' ') 
					{
						str = str.Substring(1);
					}
					while(str.Length > 5 && str[str.Length-1] == ' ')
					{
						str = str.Substring(0, str.Length-1);
					}

					TryUntilSuccess = (str == "false") ? false : true;
					found[3] = true;
				}
			}
			if (!found[0]) Minutes = 3;
			if (!found[1]) PingLimit = 150;
			if (!found[2]) LagTolerance = 5;
			if (!found[3]) TryUntilSuccess = true;
		}
		string [,] Splitter(ArrayList arr)
		{
			string[,] temp = new string[arr.Count,2];
			for(int j = 0; j < arr.Count; j++)
			{
				string x = (string) arr[j];
				string izq = "", der = "";
				bool found = false;
				for (int i = 0; i < x.Length; i++)
				{
					if (!found)
					{
						if (x[i] != ':')
							izq += x[i];
						else
							found = true;
					}
					else
						der += x[i];
				}
				temp[j, 0] = izq;
				temp[j, 1] = der;
			}
			return temp;
		}
		public UserConfig(ulong minutes, ulong pinglimit, byte lagtolerance, bool tryuntilsuccess)
		{
			Minutes = minutes;
			PingLimit = pinglimit;
			LagTolerance = lagtolerance;
			TryUntilSuccess = tryuntilsuccess;
		}
		public UserConfig(string minutes, string pinglimit, string lagtolerance, string tryuntilsuccess)
		{
			ulong temp1;
			byte temp2;
			Minutes = (ulong.TryParse(minutes, out temp1)) ? temp1 : 3;
			PingLimit = (ulong.TryParse(pinglimit, out temp1)) ? temp1 : 150;
			LagTolerance = (byte.TryParse(lagtolerance, out temp2)) ? temp2 : (byte) 5;
			TryUntilSuccess = (tryuntilsuccess.ToLower() == "false") ? false : true;
		}
	}
}
