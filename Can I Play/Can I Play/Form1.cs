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
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		
		}
		UserConfig thisConfig;
		public Form1(UserConfig conf)
		{
			InitializeComponent();
			thisConfig = conf;
		}
		Thread t;
		Thread timerCount;
		const int timeout = 4000;
		const string ipaddress = "lan.leagueoflegends.com";
		bool textFull = false;
		ulong lostpackets = 0;
		ulong success = 0;
		ulong lag = 0;
		ulong maxping = 150; // ms
		double average = 0;
		ulong total = 0;
		ulong t_seconds = 0;
		ulong t_minutes = 0;
		byte lagTolerance = 0;
		ulong tries = 0;
		ulong pingLimit;

		string initialName = "Can I Play?";

		private void trackBar1_Scroll(object sender, EventArgs e)
		{
		}

		private void label1_Click(object sender, EventArgs e)
		{

		}

		private void label5_Click(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e) // Start
		{
			t = new Thread(new ParameterizedThreadStart(GetPing));
			timerCount = new Thread(new ParameterizedThreadStart(CountTime));
			lostpackets = 0;
			success = 0;
			lag = 0;
			average = 0;
			total = 0;
			lagTolerance = (byte) trackBar1.Value;
			
			try
			{
				maxping = ulong.Parse(maxping_box.Text);
			}
			catch
			{
				maxping = 150;
			}
			try
			{
				t_minutes = ulong.Parse(t_minutes_box.Text);
			}
			catch
			{
				t_minutes = 0;
			}
			t_seconds = 0;
			t.Start();
			timerCount.Start();
			tries++;
			Text = initialName + " - " + tries+ " intento(s)";
			tittle.Text = Text;
			start_btn.Enabled = false;
			stop_btn.Enabled = true;
		}

		private void button2_Click(object sender, EventArgs e) // Stop
		{
			try
			{
				t.Abort();
			}
			catch
			{

			}
			try
			{
				timerCount.Abort();
			}
			catch
			{
				
			}
			tries = 0;
			InsertText("System is paused.");
			Text = initialName;
			tittle.Text = Text;
			start_btn.Enabled = true;
			stop_btn.Enabled = false;
		}
		void StopWithoutTimerCount()
		{
			try
			{
				t.Abort();
			}
			catch
			{

			}
			tries = 0;
			InsertText("System is paused.");
			Text = initialName;
			start_btn.Enabled = true;
			stop_btn.Enabled = false;
		}
		private void InsertText(string txt)
		{
			// if is first time
			if(textFull)
			{
				infotab1.Text = infotab2.Text;
				infotab2.Text = infotab3.Text;
				infotab3.Text = txt;
			}
			else if (infotab1.Text == "System is paused." && infotab2.Text == "" && infotab3.Text == "")
			{
				infotab1.Text = txt;
			}
			else if (infotab2.Text == "" && infotab3.Text == "")
			{
				infotab1.ForeColor = Color.FromArgb(122, 122, 122);
				infotab2.Text = txt;
			}
			else if (infotab3.Text == "")
			{
				infotab2.ForeColor = Color.FromArgb(122, 122, 122);
				infotab1.ForeColor = Color.FromArgb(63,63,63);
				infotab3.Text = txt;
				textFull = true;
			}
		}
		private void UpdateInfo()
		{
			lost_number_label.Text = lostpackets+"";
			success_number_label.Text = success+"";
			lagged_number_label.Text = lag + "";
			average_number_label.Text = ((ulong)average)+"";
		}
		private void GetPing(object num)
		{
			
			Ping pingSender = new Ping();
			PingOptions options = new PingOptions();
			options.DontFragment = true;
			Semaphore _pool = new Semaphore(1, 1);
			PingReply reply;
			InsertText("System started.");
			while (true)
			{
				try
				{
					_pool.WaitOne();
					reply = pingSender.Send(ipaddress, timeout);
					long rrt = 0;
					if (reply.Status == IPStatus.Success)
					{
						rrt = reply.RoundtripTime;
						InsertText("Ping is " + rrt + " ms.");
						if ((ulong)rrt > maxping) // Means lag
						{
							lag++;
						}
						else
						{
							success++;
						}
						ulong noLost = success + lag;
						debugBox.Text += "\nPing: "+rrt;
						average = ((average * (noLost - 1) + (ulong)rrt) / noLost);
					}
					else
					{
						InsertText("Packet lost.");
						lostpackets++;
					}

					total++;
					UpdateInfo();
					Thread.Sleep(500);

					_pool.Release(1);
				}
				catch
				{
					return;
				}
			}
		}

		private void label6_Click(object sender, EventArgs e)
		{

		}

		private void label3_Click(object sender, EventArgs e)
		{

		}

		private void label9_Click(object sender, EventArgs e)
		{

		}

		private void label8_Click(object sender, EventArgs e)
		{

		}
		private void CountTime(object num)
		{
			Semaphore _pool = new Semaphore(1, 1);
			while (true)
			{
				_pool.WaitOne();
				if (t_seconds == 0)
				{
					t_seconds = 59;
					if (t_minutes == 0) // time is out
					{
						timeleft_label.Text = "0:00";
						try{ t.Abort(); } catch { }
						InsertText("Finished.");
						start_btn.Enabled = true;
						stop_btn.Enabled = false;
						double lagRelation = ((double)(lag + lostpackets) / total ) * 100;
						if (lagRelation > lagTolerance) // You cannot play
						{
							if (tryagain_check.Checked)
							{
								InsertText(lagRelation.ToString("0.#0") +"% of lag.");
								InsertText("Trying again.");
								showHistoryForm.AddHistory(new HistoryInfo((ulong) average, success, lostpackets, lag, maxping, lagRelation, lagTolerance));
								Thread.Sleep(3000);
								button1_Click(null, null);
							}
							else 
								MessageBox.Show(lagRelation.ToString("0.#0") + "% of lag. It's not recommendable to play.", "Bad conection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
							
						}
						else // You can play
						{
							MessageBox.Show(lagRelation.ToString("0.#0") + "% of lag. You can play with ease.", "Good conection", MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
						return;
					}
					else
					{
						t_minutes--;
					}
				}
				else
				{
					t_seconds--;
				}

				timeleft_label.Text = t_minutes	+ ":" + ((t_seconds < 10) ? "0" : "") + t_seconds;
				Thread.Sleep(1000);
				_pool.Release(1);
			}
		}

		private void trackBar1_ValueChanged(object sender, EventArgs e)
		{
			percent_label.Text = trackBar1.Value+"%";
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			t_minutes_box.Text = thisConfig.Minutes.ToString();
			maxping_box.Text = thisConfig.PingLimit.ToString();
			trackBar1.Value = thisConfig.LagTolerance;
			tryagain_check.Checked = thisConfig.TryUntilSuccess;
		}



		private void t_minutes_box_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (Char.IsDigit(e.KeyChar))
			{
				e.Handled = false;
			}
			else if (Char.IsControl(e.KeyChar))
			{
				e.Handled = false;
			}
			else if (Char.IsSeparator(e.KeyChar))
			{
				e.Handled = false;
			}
			else
			{
				e.Handled = true;
			}
		}

		private void label8_Click_1(object sender, EventArgs e)
		{

		}

		private void closeBtn(object sender, EventArgs e)
		{
			CloseInstructions();
		}
		private void minimizeBtn(object sender, EventArgs e)
		{
			this.WindowState = FormWindowState.Minimized;
		}

		private void panel1_Paint(object sender, PaintEventArgs e)
		{

		}
		public const int WM_NCLBUTTONDOWN = 0xA1;
		public const int HT_CAPTION = 0x2;

		[System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
		[System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
		public static extern bool ReleaseCapture();
		public void TopBar_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				ReleaseCapture();
				SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
			}
		}

		private void label10_Click(object sender, EventArgs e)
		{

		}

		private void maxping_box_TextChanged(object sender, EventArgs e)
		{

		}

		ShowHistoryForm showHistoryForm = new ShowHistoryForm();

		private void showHistory_btn_Click(object sender, EventArgs e)
		{
			showHistoryForm.Show();
		}

		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			CloseInstructions();
		}
		void CloseInstructions()
		{
			UserConfig t = new UserConfig(
				t_minutes_box.Text,
				maxping_box.Text,
				trackBar1.Value.ToString(),
				tryagain_check.Checked.ToString());
			Program.Exit(t);
		}
	}
}
