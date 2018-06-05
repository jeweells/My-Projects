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

namespace Can_I_Play
{
	public partial class ShowHistoryForm : Form
	{
		public ShowHistoryForm()
		{
			InitializeComponent();
			historyRecount = new List<HistoryInfo>();
			rows = new List<TableLayoutPanel>();
			Thread x = new Thread(Nose);
			x.Start();
			
		}

		void Nose(object a)
		{
			bbb:
			HistoryInfo x = new HistoryInfo(10, 11, 12, 13, 14, 15.4, 20);
			AddHistory(x);
			Thread.Sleep(2000);
			goto bbb;
		}

		public List<HistoryInfo> historyRecount;
		List<TableLayoutPanel> rows;

		public void AddHistory(HistoryInfo hInfo)
		{
			historyRecount.Add(hInfo);
			rows.Add(new System.Windows.Forms.TableLayoutPanel());
			rows[rows.Count - 1].SuspendLayout();
			rows[rows.Count - 1].ColumnCount = 8;
			rows[rows.Count - 1].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			rows[rows.Count - 1].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 66F));
			rows[rows.Count-1].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 73F));
			rows[rows.Count-1].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
			rows[rows.Count-1].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 61F));
			rows[rows.Count-1].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
			rows[rows.Count-1].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
			rows[rows.Count-1].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
			Label temp = new Label();
			temp.Text = historyRecount.Count.ToString();
			rows[rows.Count-1].Controls.Add(temp, 0, 0);
			rows[rows.Count-1].Controls.Add(historyRecount[historyRecount.Count - 1].LagRelation, 1, 0);
			rows[rows.Count-1].Controls.Add(historyRecount[historyRecount.Count - 1].Average, 2, 0);
			rows[rows.Count-1].Controls.Add(historyRecount[historyRecount.Count - 1].Success, 3, 0);
			rows[rows.Count-1].Controls.Add(historyRecount[historyRecount.Count - 1].Lost, 4, 0);
			rows[rows.Count-1].Controls.Add(historyRecount[historyRecount.Count - 1].Lagged, 5, 0);
			rows[rows.Count-1].Controls.Add(historyRecount[historyRecount.Count - 1].PingLimit, 6, 0);
			rows[rows.Count-1].Controls.Add(historyRecount[historyRecount.Count - 1].Time, 7, 0);
			rows[rows.Count-1].Font = new System.Drawing.Font("Myriad Pro", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			rows[rows.Count-1].ForeColor = System.Drawing.Color.White;
			rows[rows.Count-1].Location = new System.Drawing.Point(14, 47);
			rows[rows.Count-1].Name = "gRow+"+(rows.Count - 1);
			rows[rows.Count-1].RowCount = 1;
			rows[rows.Count-1].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			rows[rows.Count-1].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			rows[rows.Count-1].Size = new System.Drawing.Size(589, 21);
			rows[rows.Count - 1].TabIndex = 33;
			TableLayoutPanel tmp = rows[rows.Count - 1];
			try {
				rowsContent.Controls.Add(tmp);
			}
			catch(ArgumentException)
			{
				MessageBox.Show("Argument Exception in AddHistory()");
			}
			rows[rows.Count - 1].ResumeLayout(false);
			rowsContent.ResumeLayout(false);
			
		}


		private void vScrollBar1_ValueChanged(object sender, EventArgs e)
		{
			contentInfo.Location = new Point(contentInfo.Location.X, -vScrollBar1.Value);
		}

		private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
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

		private void label17_Click(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
			Hide();
		}
	}
}
