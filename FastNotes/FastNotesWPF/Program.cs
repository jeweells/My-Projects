using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FastNotes
{
    class Program
    {
        public static LinkedList<Window> AvailableNotes = new LinkedList<Window>();
        public static MainWindow MainWindow;
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        /// 
        public static string logPath = Path.Combine(System.Windows.Forms.Application.UserAppDataPath, "log.fn");

        [STAThread]
        public static void Main()
        {
            Process[] pname = Process.GetProcessesByName("FastNotes");
            if (pname.Length != 1)
                return;
            AppDomain.CurrentDomain.UnhandledException += ExceptionLogWriter;
            App.Main();
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

        public static Window CreateNote()
        {
            LinkedListNode<Window> lln = AvailableNotes.AddFirst(value: null);
            NoteWindow note = new NoteWindow(lln);
            lln.Value = note;
            return note;
        }

        public static void RunAtStartup(bool value)
        {
            try
            {
                RegistryKey rk = Registry.CurrentUser.OpenSubKey
                    ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                if (value)
                    rk.SetValue(System.Windows.Forms.Application.ProductName, System.Windows.Forms.Application.ExecutablePath);
                else
                    rk.DeleteValue(System.Windows.Forms.Application.ProductName, false);
            }
            catch (Exception e)
            {
                Controls.MessageBox.Error(null, e.Message,
                    Controls.MessageBox.Ok);
            }
        }
    }
}
