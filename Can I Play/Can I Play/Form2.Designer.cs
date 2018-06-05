namespace Can_I_Play
{
	partial class ShowHistoryForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.TopBar = new System.Windows.Forms.Panel();
			this.tittle = new System.Windows.Forms.Label();
			this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.rowsContent = new System.Windows.Forms.FlowLayoutPanel();
			this.row1 = new System.Windows.Forms.TableLayoutPanel();
			this.try_num_row1 = new System.Windows.Forms.Label();
			this.plag_num_row1 = new System.Windows.Forms.Label();
			this.success_num_row1 = new System.Windows.Forms.Label();
			this.lost_num_row1 = new System.Windows.Forms.Label();
			this.lagged_num_row1 = new System.Windows.Forms.Label();
			this.time_num_row1 = new System.Windows.Forms.Label();
			this.avg_num_row1 = new System.Windows.Forms.Label();
			this.pinglim_num_row1 = new System.Windows.Forms.Label();
			this.contentInfo = new System.Windows.Forms.FlowLayoutPanel();
			this.TopBar.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.rowsContent.SuspendLayout();
			this.row1.SuspendLayout();
			this.contentInfo.SuspendLayout();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(98)))), ((int)(((byte)(180)))));
			this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.button1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(98)))), ((int)(((byte)(180)))));
			this.button1.FlatAppearance.BorderSize = 0;
			this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(98)))), ((int)(((byte)(180)))));
			this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(125)))));
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Font = new System.Drawing.Font("Myriad Pro", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(238)))), ((int)(((byte)(255)))));
			this.button1.Location = new System.Drawing.Point(610, 0);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(35, 38);
			this.button1.TabIndex = 29;
			this.button1.Text = "✖";
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// TopBar
			// 
			this.TopBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(98)))), ((int)(((byte)(180)))));
			this.TopBar.Controls.Add(this.button1);
			this.TopBar.Controls.Add(this.tittle);
			this.TopBar.Dock = System.Windows.Forms.DockStyle.Top;
			this.TopBar.Location = new System.Drawing.Point(0, 0);
			this.TopBar.Name = "TopBar";
			this.TopBar.Size = new System.Drawing.Size(645, 38);
			this.TopBar.TabIndex = 29;
			this.TopBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TopBar_MouseDown);
			// 
			// tittle
			// 
			this.tittle.BackColor = System.Drawing.Color.Transparent;
			this.tittle.Font = new System.Drawing.Font("Myriad Pro", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tittle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(238)))), ((int)(((byte)(255)))));
			this.tittle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.tittle.Location = new System.Drawing.Point(14, 10);
			this.tittle.Name = "tittle";
			this.tittle.Size = new System.Drawing.Size(292, 18);
			this.tittle.TabIndex = 26;
			this.tittle.Text = "History";
			this.tittle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.tittle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TopBar_MouseDown);
			// 
			// vScrollBar1
			// 
			this.vScrollBar1.Location = new System.Drawing.Point(617, 66);
			this.vScrollBar1.Name = "vScrollBar1";
			this.vScrollBar1.Size = new System.Drawing.Size(19, 130);
			this.vScrollBar1.TabIndex = 30;
			this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
			this.vScrollBar1.ValueChanged += new System.EventHandler(this.vScrollBar1_ValueChanged);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 8;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 66F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 73F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 61F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.label4, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.label5, 4, 0);
			this.tableLayoutPanel1.Controls.Add(this.label6, 5, 0);
			this.tableLayoutPanel1.Controls.Add(this.label8, 7, 0);
			this.tableLayoutPanel1.Controls.Add(this.label3, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.label7, 6, 0);
			this.tableLayoutPanel1.Font = new System.Drawing.Font("Myriad Pro", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tableLayoutPanel1.ForeColor = System.Drawing.Color.White;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(14, 47);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(589, 21);
			this.tableLayoutPanel1.TabIndex = 33;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(23, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Try";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(53, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(51, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "% of lag";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(192, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(50, 16);
			this.label4.TabIndex = 3;
			this.label4.Text = "Success";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(257, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(30, 16);
			this.label5.TabIndex = 4;
			this.label5.Text = "Lost";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(318, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(48, 16);
			this.label6.TabIndex = 5;
			this.label6.Text = "Lagged";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(467, 0);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(35, 16);
			this.label8.TabIndex = 7;
			this.label8.Text = "Time";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(119, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(53, 16);
			this.label3.TabIndex = 2;
			this.label3.Text = "Average";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(382, 0);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(62, 16);
			this.label7.TabIndex = 6;
			this.label7.Text = "Ping Limit";
			// 
			// rowsContent
			// 
			this.rowsContent.AutoSize = true;
			this.rowsContent.Controls.Add(this.row1);
			this.rowsContent.FlowDirection = System.Windows.Forms.FlowDirection.BottomUp;
			this.rowsContent.Location = new System.Drawing.Point(3, 3);
			this.rowsContent.Name = "rowsContent";
			this.rowsContent.Size = new System.Drawing.Size(595, 27);
			this.rowsContent.TabIndex = 35;
			// 
			// row1
			// 
			this.row1.ColumnCount = 8;
			this.row1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.row1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 66F));
			this.row1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 73F));
			this.row1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
			this.row1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 61F));
			this.row1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
			this.row1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
			this.row1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
			this.row1.Controls.Add(this.try_num_row1, 0, 0);
			this.row1.Controls.Add(this.plag_num_row1, 1, 0);
			this.row1.Controls.Add(this.success_num_row1, 3, 0);
			this.row1.Controls.Add(this.lost_num_row1, 4, 0);
			this.row1.Controls.Add(this.lagged_num_row1, 5, 0);
			this.row1.Controls.Add(this.time_num_row1, 7, 0);
			this.row1.Controls.Add(this.avg_num_row1, 2, 0);
			this.row1.Controls.Add(this.pinglim_num_row1, 6, 0);
			this.row1.Font = new System.Drawing.Font("Myriad Pro", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.row1.ForeColor = System.Drawing.Color.White;
			this.row1.Location = new System.Drawing.Point(3, 3);
			this.row1.Name = "row1";
			this.row1.RowCount = 1;
			this.row1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.row1.Size = new System.Drawing.Size(589, 21);
			this.row1.TabIndex = 35;
			// 
			// try_num_row1
			// 
			this.try_num_row1.AutoSize = true;
			this.try_num_row1.Location = new System.Drawing.Point(3, 0);
			this.try_num_row1.Name = "try_num_row1";
			this.try_num_row1.Size = new System.Drawing.Size(23, 16);
			this.try_num_row1.TabIndex = 0;
			this.try_num_row1.Text = "Try";
			// 
			// plag_num_row1
			// 
			this.plag_num_row1.AutoSize = true;
			this.plag_num_row1.Location = new System.Drawing.Point(53, 0);
			this.plag_num_row1.Name = "plag_num_row1";
			this.plag_num_row1.Size = new System.Drawing.Size(51, 16);
			this.plag_num_row1.TabIndex = 1;
			this.plag_num_row1.Text = "% of lag";
			// 
			// success_num_row1
			// 
			this.success_num_row1.AutoSize = true;
			this.success_num_row1.Location = new System.Drawing.Point(192, 0);
			this.success_num_row1.Name = "success_num_row1";
			this.success_num_row1.Size = new System.Drawing.Size(50, 16);
			this.success_num_row1.TabIndex = 3;
			this.success_num_row1.Text = "Success";
			// 
			// lost_num_row1
			// 
			this.lost_num_row1.AutoSize = true;
			this.lost_num_row1.Location = new System.Drawing.Point(257, 0);
			this.lost_num_row1.Name = "lost_num_row1";
			this.lost_num_row1.Size = new System.Drawing.Size(30, 16);
			this.lost_num_row1.TabIndex = 4;
			this.lost_num_row1.Text = "Lost";
			// 
			// lagged_num_row1
			// 
			this.lagged_num_row1.AutoSize = true;
			this.lagged_num_row1.Location = new System.Drawing.Point(318, 0);
			this.lagged_num_row1.Name = "lagged_num_row1";
			this.lagged_num_row1.Size = new System.Drawing.Size(48, 16);
			this.lagged_num_row1.TabIndex = 5;
			this.lagged_num_row1.Text = "Lagged";
			// 
			// time_num_row1
			// 
			this.time_num_row1.AutoSize = true;
			this.time_num_row1.Location = new System.Drawing.Point(467, 0);
			this.time_num_row1.Name = "time_num_row1";
			this.time_num_row1.Size = new System.Drawing.Size(35, 16);
			this.time_num_row1.TabIndex = 7;
			this.time_num_row1.Text = "Time";
			// 
			// avg_num_row1
			// 
			this.avg_num_row1.AutoSize = true;
			this.avg_num_row1.Location = new System.Drawing.Point(119, 0);
			this.avg_num_row1.Name = "avg_num_row1";
			this.avg_num_row1.Size = new System.Drawing.Size(53, 16);
			this.avg_num_row1.TabIndex = 2;
			this.avg_num_row1.Text = "Average";
			// 
			// pinglim_num_row1
			// 
			this.pinglim_num_row1.AutoSize = true;
			this.pinglim_num_row1.Location = new System.Drawing.Point(382, 0);
			this.pinglim_num_row1.Name = "pinglim_num_row1";
			this.pinglim_num_row1.Size = new System.Drawing.Size(62, 16);
			this.pinglim_num_row1.TabIndex = 6;
			this.pinglim_num_row1.Text = "Ping Limit";
			// 
			// contentInfo
			// 
			this.contentInfo.Controls.Add(this.rowsContent);
			this.contentInfo.Location = new System.Drawing.Point(8, 66);
			this.contentInfo.Name = "contentInfo";
			this.contentInfo.Size = new System.Drawing.Size(600, 130);
			this.contentInfo.TabIndex = 36;
			// 
			// ShowHistoryForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(32)))), ((int)(((byte)(41)))));
			this.ClientSize = new System.Drawing.Size(645, 225);
			this.Controls.Add(this.contentInfo);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.vScrollBar1);
			this.Controls.Add(this.TopBar);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.Name = "ShowHistoryForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "History";
			this.TopBar.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.rowsContent.ResumeLayout(false);
			this.row1.ResumeLayout(false);
			this.row1.PerformLayout();
			this.contentInfo.ResumeLayout(false);
			this.contentInfo.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Panel TopBar;
		private System.Windows.Forms.Label tittle;
		private System.Windows.Forms.VScrollBar vScrollBar1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.FlowLayoutPanel rowsContent;
		private System.Windows.Forms.TableLayoutPanel row1;
		private System.Windows.Forms.Label try_num_row1;
		private System.Windows.Forms.Label plag_num_row1;
		private System.Windows.Forms.Label success_num_row1;
		private System.Windows.Forms.Label lost_num_row1;
		private System.Windows.Forms.Label lagged_num_row1;
		private System.Windows.Forms.Label time_num_row1;
		private System.Windows.Forms.Label avg_num_row1;
		private System.Windows.Forms.Label pinglim_num_row1;
		private System.Windows.Forms.FlowLayoutPanel contentInfo;
	}
}