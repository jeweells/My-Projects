using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TryUntilGet
{
	static class Data
	{
		public static class Directories
		{
			public static string Root = System.Environment.CurrentDirectory;
			public static string Log = Root + @"\Logs";
			public static string Config = Root + @"\Config";
			public static void Initialize()
			{
				Directory.CreateDirectory(Log);
				Directory.CreateDirectory(Config);
			}
			public static string TransformUrl(string url)
			{
				string result = Log + "\\";
				foreach(char c in url)
				{
					if (char.IsLetter(c)) result += c;
				}
				return result;
			}
			/// <summary>
			/// Builds the url path, if it was already created, then this works to get the path of an url
			/// </summary>
			/// <param name="url"></param>
			/// <returns></returns>
			public static string BuildUrlPath(string url)
			{
				string dir = TransformUrl(url);
				Directory.CreateDirectory(dir);
				string logfile = dir + @"\" + Files.LogName;
				string datafile = dir + @"\" + Files.DataName;
				if (!File.Exists(logfile)) File.Create(logfile);
				if (!File.Exists(datafile))
				{
					using (StreamWriter wr = new StreamWriter(datafile))
					{
						wr.WriteLine("LASTTIMEOUT=60");
						wr.WriteLine("LASTATTEMPTS=0");
					}
				}
				return dir;
			}
		}
		public static class Files
		{
			public static string Config = Directories.Config + @"\config";
			public static string LogName = "log.tug";
			public static string DataName = "data.tug";
			/// <summary>
			/// Max bytes of the log file
			/// </summary>
			public static long MaxLogSize = 10000;
			public static void Initialize()
			{
				if (!File.Exists(Config))
				{
					using (StreamWriter wr = new StreamWriter(Config))
					{
						wr.WriteLine("LASTURL=");
						wr.WriteLine("AUTOSTART=0");
					}
				}
			}
			public static string DataFrom(string url)
			{
				return Directories.TransformUrl(url) + "\\" + DataName;
			}
			public static string LogFrom(string url)
			{
				return Directories.TransformUrl(url) + "\\" + LogName;
			}

		}
		public static string LastUrl;
		public static ulong LastAttempts = 0;
		public static int LastTimeout = 60;
		public static bool AutoStart = false;
		public static void Initialize()
		{
			Directories.Initialize();
			Files.Initialize();
			using (StreamReader rd = new StreamReader(Files.Config))
			{
				while(!rd.EndOfStream)
				{
					string[] tmp = rd.ReadLine().Split(new char[] { '=' }, 2);
					if (tmp.Length == 2)
					{
						if (tmp[0] == "LASTURL")
						{
							LastUrl = tmp[1];
						}
						else if (tmp[0] == "AUTOSTART")
						{
							if (tmp[1] == "1")
								AutoStart = true;
							else
								AutoStart = false;
						}
					}
				}
			}
			if(LastUrl.Length > 0)
			{
				string path = Directories.BuildUrlPath(LastUrl);
				using (StreamReader rd = new StreamReader(path + @"\" + Files.DataName))
				{
					while (!rd.EndOfStream)
					{
						string[] tmp = rd.ReadLine().Split(new char[] { '=' }, 2);
						if (tmp.Length == 2)
						{
							if (tmp[0] == "LASTTIMEOUT")
							{
								try
								{
									LastTimeout = Int32.Parse(tmp[1]);
								}
								catch { LastTimeout = 60; }
							}
							else if (tmp[0] == "LASTATTEMPTS")
							{
								try
								{
									LastAttempts = UInt64.Parse(tmp[1]);
								}
								catch { LastAttempts = 0; }
							}
						}
					}
				}
			}

		}
	}
}
