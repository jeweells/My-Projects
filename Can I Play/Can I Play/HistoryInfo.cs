using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.NetworkInformation;
using System.Globalization;

namespace Can_I_Play
{
	public class HistoryInfo
	{
		// Variables
		System.Windows.Forms.Label average;
		System.Windows.Forms.Label success;
		System.Windows.Forms.Label lost;
		System.Windows.Forms.Label lagged;
		System.Windows.Forms.Label pinglim;
		System.Windows.Forms.Label lagRelation;
		System.Windows.Forms.Label lagTolerance;
		System.Windows.Forms.Label time;

		// Getters and Setters
		public Label Average { get { return average; } set { average = value; } }
		public Label Success { get { return success; } set { success = value; } }
		public Label Lost { get { return lost; } set { lost = value; } }
		public Label Lagged { get { return lagged; } set { lagged = value; } }
		public Label PingLimit { get { return pinglim; } set { pinglim = value; } }
		public Label LagRelation { get { return lagRelation; } set { lagRelation = value; } }
		public Label LagTolerance { get { return lagTolerance; } set { lagTolerance = value; } }
		public Label Time { get { return time; } set { time = value; } }

		public HistoryInfo(ulong avg, ulong scss, ulong lst, ulong lagd, ulong plim, double lagRel, byte lagTol)
		{
			Average = new Label();
			Success = new Label();
			Lost = new Label();
			Lagged = new Label();
			PingLimit = new Label();
			LagRelation = new Label();
			LagTolerance = new Label();
			time = new Label();

			Average.AutoSize = true;
			Average.Location = new System.Drawing.Point(3, 0);
			Average.Size = new System.Drawing.Size(23, 16);
			Average.Text = avg.ToString();

			Success.AutoSize = true;
			Success.Location = new System.Drawing.Point(3, 0);
			Success.Size = new System.Drawing.Size(23, 16);
			Success.Text = scss.ToString();

			Lost.AutoSize = true;
			Lost.Location = new System.Drawing.Point(3, 0);
			Lost.Size = new System.Drawing.Size(23, 16);
			Lost.Text = lst.ToString();

			Lagged.AutoSize = true;
			Lagged.Location = new System.Drawing.Point(3, 0);
			Lagged.Size = new System.Drawing.Size(23, 16);
			Lagged.Text = lagd.ToString();

			PingLimit.AutoSize = true;
			PingLimit.Location = new System.Drawing.Point(3, 0);
			PingLimit.Size = new System.Drawing.Size(23, 16);
			PingLimit.Text = plim.ToString();

			LagRelation.AutoSize = true;
			LagRelation.Location = new System.Drawing.Point(3, 0);
			LagRelation.Size = new System.Drawing.Size(23, 16);
			LagRelation.Text = lagRel.ToString();

			LagTolerance.AutoSize = true;
			LagTolerance.Location = new System.Drawing.Point(3, 0);
			LagTolerance.Size = new System.Drawing.Size(23, 16);
			LagTolerance.Text = lagTol.ToString();

			time.AutoSize = true;
			time.Location = new System.Drawing.Point(3, 0);
			time.Size = new System.Drawing.Size(23, 16);
			time.Text = DateTime.Now.ToString();

		}
	}
}
