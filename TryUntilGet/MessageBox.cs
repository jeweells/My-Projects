﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TryUntilGet
{
	public partial class MessageBox : Form
	{
		public MessageBox()
		{
			InitializeComponent();
			MainForm.AlignCenter(button1, panel2);
			Activate();
			timer1.Start();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			Close();
		}
	}
}
