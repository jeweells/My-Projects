﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Win32;

namespace FastNotes
{
	static class Program
	{
		public static LinkedList<Form> availableForms = new LinkedList<Form>();
		public static Form startupForm;
		/// <summary>
		/// Punto de entrada principal para la aplicación.
		/// </summary>
		/// 
		public static string logPath = Application.UserAppDataPath + "//log.fn";
		[STAThread]
		static void Main()
		{
            Process[] pname = Process.GetProcessesByName("FastNotes");
            if (pname.Length != 1)
                return;
            Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(true);
			AppDomain.CurrentDomain.UnhandledException += ExceptionLogWriter;
			startupForm = new StartupForm();
			Application.Run(startupForm);
		}
		static void ExceptionLogWriter(object sender, UnhandledExceptionEventArgs e)
		{

			Exception ex = (Exception)e.ExceptionObject;
			WriteExceptionToLog(ex);
		}
		public static void WriteExceptionToLog(Exception ex)
		{
			using (StreamWriter w = new StreamWriter(logPath, true))
			{
				w.WriteLine("".PadRight(30, '$'));
				w.WriteLine(DateTime.Now.ToLongDateString() + " => Message: " + ex.Message);
				w.WriteLine($"\tSource: {ex.Source}");
				w.WriteLine($"\tStackTrace: {ex.StackTrace}");
			}
		}

		public static Form CreateNote()
		{
			LinkedListNode<Form> lln = availableForms.AddLast(value: null);
			NoteForm f = new NoteForm(lln);
			lln.Value = f;
			return f;
		}

		public static void RunAtStartup(bool value)
		{
			try
			{
				RegistryKey rk = Registry.CurrentUser.OpenSubKey
					("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

				if (value)
					rk.SetValue(Application.ProductName, Application.ExecutablePath);
				else
					rk.DeleteValue(Application.ProductName, false);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
		}
	}
}
