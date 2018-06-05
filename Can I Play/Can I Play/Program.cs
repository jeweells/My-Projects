using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace Can_I_Play
{
	static class Program
	{
		static string CONFIG_PATH_NOFILE = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
		static string CONFIG_FILE = "config.cip";
		static string CONFIG_FOLDER = "Can I Play";
		static string CONFIG_PATH = CONFIG_PATH_NOFILE + "\\" + CONFIG_FOLDER + "\\"+ CONFIG_FILE;
		
		/// <summary>
		/// Punto de entrada principal para la aplicación.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			if (File.Exists(CONFIG_PATH))
			{
				StreamReader objReader = new StreamReader(CONFIG_PATH);
				string sLine = "";
				ArrayList arrText = new ArrayList();
				while (sLine != null)
				{
					sLine = objReader.ReadLine();
					if (sLine != null)
						arrText.Add(sLine);
				}
				objReader.Close();
				UserConfig userConf = new UserConfig(arrText);

				Application.Run(new Form1(userConf));
			}
			else
			{
				Application.Run(new Form1());
			}
		}


		public static void Exit(UserConfig conf)
		{
			// Saving Configuration
			try
			{
				if (!File.Exists(CONFIG_PATH))
			{
				Directory.CreateDirectory(CONFIG_PATH_NOFILE + "\\" + CONFIG_FOLDER);
			}
				using (StreamWriter wr = new StreamWriter(CONFIG_PATH))
				{
					wr.WriteLine("Minutes: " + conf.Minutes);
					wr.WriteLine("Ping Limit: " + conf.PingLimit);
					wr.WriteLine("Lag Tolerance: " + conf.LagTolerance);
					wr.WriteLine("Try Until Success: " + conf.TryUntilSuccess.ToString());
					wr.Close();
				}
				
				System.Environment.Exit(0);

			}
			catch(Exception e)
			{
				MessageBox.Show(e.InnerException.ToString());
			}
		} 
	}
}
