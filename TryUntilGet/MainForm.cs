using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tools.WebChecker;
using System.Threading;
using System.IO;

namespace TryUntilGet
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			Align();
			Data.Initialize();
			label4.Text = Data.LastAttempts.ToString();
			attempts = Data.LastAttempts;
			textBox1.Text = Data.LastUrl.ToString();
			textBox2.Text = Data.LastTimeout.ToString();
		}
		

		public void Align()
		{
			int defaultLabelWidth = 50;
			int defaultTextBoxWidth = 180;
			textBox2.Width = textBox1.Width = defaultTextBoxWidth;
			int totalobjwidth = defaultLabelWidth + defaultTextBoxWidth;
			label1.Location = new Point((panel4.Width - totalobjwidth) / 2, (panel4.Height - label1.Height) / 2);
			textBox1.Location = new Point((panel4.Width - totalobjwidth) / 2 + defaultLabelWidth, (panel4.Height - textBox1.Height) / 2);
			label2.Location = new Point((panel5.Width - totalobjwidth) / 2, (panel5.Height - label2.Height) / 2);
			textBox2.Location = new Point((panel5.Width - totalobjwidth) / 2 + defaultLabelWidth, (panel5.Height - textBox2.Height) / 2);
			AlignCenter(button1, panel6);
		}
		public static void AlignCenter(Control element, Control container)
		{
			element.Location = new Point((container.Width - element.Width) / 2, (container.Height - element.Height) / 2);
		}

		private void label1_Click(object sender, EventArgs e)
		{

		}
		ulong attempts = 0;
		delegate void SetTextDel(Control o, string text);
		public void SetText(Control o, string text)
		{
			if (o.InvokeRequired)
			{
				SetTextDel s = new SetTextDel(SetText);
				Invoke(s, new object[] { o, text });
			}
			else
			{
				o.Text = text;
			}
		}
		string currentDataFile;
		string currentLogFile;
		public ulong Attempts { get { return attempts; }
			set
			{
				attempts = value;
				SetText(label4, attempts.ToString());
				notifyIcon1.Text = attempts+ " attempts so far";
				try
				{
					using (StreamWriter wr = new StreamWriter(currentDataFile, false))
					{
						wr.WriteLine("LASTATTEMPTS=" + attempts);
						wr.WriteLine("LASTTIMEOUT=" + textBox2.Text);
					}
				}
				catch { }
				
			}
		}
		bool stopped = true;
		string start = "Start";
		string stop = "Stop";
		Thread t;

		void PressStartBtn()
		{
			button1.Text = stop;
			AlignCenter(button1, panel6);
			if (!textBox1.Text.StartsWith("http://")) textBox1.Text = "http://" + textBox1.Text;
			try
			{
				using (StreamWriter wr = new StreamWriter(Data.Files.Config, false))
				{
					wr.WriteLine("LASTURL=" + textBox1.Text);
					wr.WriteLine("AUTOSTART=1");
				}
			}
			catch { }
			Data.Initialize();
			label4.Text = Data.LastAttempts.ToString();
			Attempts = Data.LastAttempts;
			currentDataFile = Data.Files.DataFrom(textBox1.Text);
			currentLogFile = Data.Files.LogFrom(textBox1.Text);
			textBox1.Enabled = false;
			textBox2.Enabled = false;
			stopped = false;
			t = new Thread(TryUntilGet);
			t.Start();
		}

		void PressStopBtn()
		{
			try
			{
				using (StreamWriter wr = new StreamWriter(Data.Files.Config, false))
				{
					wr.WriteLine("LASTURL=" + textBox1.Text);
					wr.WriteLine("AUTOSTART=0");
				}
			}
			catch { }
			button1.Text = start;
			AlignCenter(button1, panel6);
			textBox1.Enabled = true;
			textBox2.Enabled = true;
			stopped = true;
			try
			{
				t.Abort();
			}
			catch { }
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if(stopped)
			{
				PressStartBtn();
			}
			else
			{
				PressStopBtn();
			}
		}


		void TryUntilGet()
		{
			int timeout;
			try
			{
				timeout = Int32.Parse(textBox2.Text)*1000;
			}
			catch(FormatException) { timeout = 60000; }
			catch(OverflowException) { timeout = Int32.MaxValue; }
			System.Net.HttpWebResponse wr;
			do
			{
				wr = WebChecker.Try(textBox1.Text, timeout);
				try
				{
					bool append = ((new FileInfo(currentLogFile)).Length >= Data.Files.MaxLogSize) ? false : true;
					using (StreamWriter wrs = new StreamWriter(currentLogFile, append))
					{
						if (wr == null)
							wrs.WriteLine("FAILURE ON {0:G}", DateTime.Now);
						else
							wrs.WriteLine("SUCCESS ON {0:G}", DateTime.Now);
					}
				}
				catch { }
				Attempts++;
			} while (wr == null);
			Attempts = 0;
			Invoke(new SuccessDel(Success));
		}
		delegate void SuccessDel();
		void Success()
		{
			MessageBox mb = new MessageBox();
			mb.ShowDialog(this);
			Process.Start(textBox1.Text);
			PressStopBtn();
		}

		private void panel3_Paint(object sender, PaintEventArgs e)
		{

		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
		}

		private void textBox2_TextChanged(object sender, EventArgs e)
		{
		}



		private void textBox1_Click(object sender, EventArgs e)
		{
			((TextBox)sender).SelectAll();
		}

		public void ToggleVisible()
		{
			if (Visible)
			{
				Hide();
			}
			else
			{
				Show();
				Activate();
			}
		}

		public void CloseApplication()
		{
			try
			{
				Application.Exit();
			}
			catch { }
		}


		private void button2_Click(object sender, EventArgs e)
		{
			ToggleVisible();
		}

		private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			ToggleVisible();
		}

		private void notifyIcon1_MouseUp(object sender, MouseEventArgs e)
		{
			
		}

		private void notifyIcon1_BalloonTipShown(object sender, EventArgs e)
		{
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CloseApplication();
		}

		private void notifyIcon1_Click(object sender, EventArgs e)
		{
			ToggleVisible();
		}

		private void MainForm_Leave(object sender, EventArgs e)
		{
			Hide();
		}

		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
			TryUntilGet();
		}

		private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{

		}

		private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			Success();
		}

		private void textBox2_KeyDown(object sender, KeyEventArgs e)
		{
		}

		private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true;
		}

		private void panel7_Paint(object sender, PaintEventArgs e)
		{

		}
		private void MainForm_Load(object sender, EventArgs e)
		{
			

		}
		Point wMargin = new Point(10, 10);

		void SetWindowLocation()
		{
			if (!Visible) return;
			switch (Taskbar.Position)
			{
				case TaskbarPosition.Unknown:
					Location = new Point(Screen.PrimaryScreen.Bounds.Width - Width - wMargin.X, Screen.PrimaryScreen.Bounds.Height - Height - wMargin.Y);
					break;
				case TaskbarPosition.Left:
					Location = new Point(Taskbar.CurrentBounds.Width + wMargin.X, Screen.PrimaryScreen.Bounds.Height - Height - wMargin.Y);
					break;
				case TaskbarPosition.Top:
					Location = new Point(Screen.PrimaryScreen.Bounds.Width - Width - wMargin.X, Taskbar.CurrentBounds.Height + wMargin.Y);
					break;
				case TaskbarPosition.Right:
					Location = new Point(Screen.PrimaryScreen.Bounds.Width - Width - wMargin.X - Taskbar.CurrentBounds.Width, Screen.PrimaryScreen.Bounds.Height - Height - wMargin.Y);
					break;
				case TaskbarPosition.Bottom:
					Location = new Point(Screen.PrimaryScreen.Bounds.Width - Width - wMargin.X, Screen.PrimaryScreen.Bounds.Height - Height - wMargin.Y - Taskbar.CurrentBounds.Height);
					break;
				default:
					Location = new Point(Screen.PrimaryScreen.Bounds.Width - Width - wMargin.X, Screen.PrimaryScreen.Bounds.Height - Height - wMargin.Y);
					break;
			}	
		}

		private void MainForm_VisibleChanged(object sender, EventArgs e)
		{
			SetWindowLocation();
		}

		private void MainForm_Shown(object sender, EventArgs e)
		{
			if (Data.AutoStart) { Update(); PressStartBtn(); }
		}
	}
}
