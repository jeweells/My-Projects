namespace Can_I_Play
{
	partial class Form1
	{
		/// <summary>
		/// Variable del diseñador necesaria.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Limpiar los recursos que se estén usando.
		/// </summary>
		/// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Código generado por el Diseñador de Windows Forms

		/// <summary>
		/// Método necesario para admitir el Diseñador. No se puede modificar
		/// el contenido de este método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.start_btn = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.stop_btn = new System.Windows.Forms.Button();
			this.infotab3 = new System.Windows.Forms.Label();
			this.trackBar1 = new System.Windows.Forms.TrackBar();
			this.percent_label = new System.Windows.Forms.Label();
			this.t_minutes_box = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.infotab2 = new System.Windows.Forms.Label();
			this.infotab1 = new System.Windows.Forms.Label();
			this.label88 = new System.Windows.Forms.Label();
			this.success_number_label = new System.Windows.Forms.Label();
			this.lost_number_label = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.lagged_number_label = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.average_number_label = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.timeleft_label = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.tryagain_check = new System.Windows.Forms.CheckBox();
			this.maxping_box = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.tittle = new System.Windows.Forms.Label();
			this.TopBar = new System.Windows.Forms.Panel();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.showHistory_btn = new System.Windows.Forms.Button();
			this.debugBox = new System.Windows.Forms.RichTextBox();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
			this.TopBar.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// start_btn
			// 
			this.start_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(98)))), ((int)(((byte)(180)))));
			this.start_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.start_btn.Cursor = System.Windows.Forms.Cursors.Hand;
			this.start_btn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(98)))), ((int)(((byte)(180)))));
			this.start_btn.FlatAppearance.BorderSize = 0;
			this.start_btn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(98)))), ((int)(((byte)(180)))));
			this.start_btn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(125)))));
			this.start_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.start_btn.Font = new System.Drawing.Font("Myriad Pro Light", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.start_btn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(238)))), ((int)(((byte)(255)))));
			this.start_btn.Location = new System.Drawing.Point(94, 123);
			this.start_btn.Name = "start_btn";
			this.start_btn.Size = new System.Drawing.Size(79, 27);
			this.start_btn.TabIndex = 0;
			this.start_btn.Text = "Start";
			this.start_btn.UseVisualStyleBackColor = false;
			this.start_btn.Click += new System.EventHandler(this.button1_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Myriad Pro Light", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(25, 60);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 15);
			this.label1.TabIndex = 1;
			this.label1.Text = "Time:";
			this.label1.Click += new System.EventHandler(this.label1_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Myriad Pro Light", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.Color.White;
			this.label2.Location = new System.Drawing.Point(26, 89);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(79, 15);
			this.label2.TabIndex = 2;
			this.label2.Text = "Lag tolerance:";
			// 
			// stop_btn
			// 
			this.stop_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(98)))), ((int)(((byte)(180)))));
			this.stop_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.stop_btn.Cursor = System.Windows.Forms.Cursors.Hand;
			this.stop_btn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.stop_btn.Enabled = false;
			this.stop_btn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(98)))), ((int)(((byte)(180)))));
			this.stop_btn.FlatAppearance.BorderSize = 0;
			this.stop_btn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(98)))), ((int)(((byte)(180)))));
			this.stop_btn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(125)))));
			this.stop_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.stop_btn.Font = new System.Drawing.Font("Myriad Pro Light", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.stop_btn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(238)))), ((int)(((byte)(255)))));
			this.stop_btn.Location = new System.Drawing.Point(179, 123);
			this.stop_btn.Name = "stop_btn";
			this.stop_btn.Size = new System.Drawing.Size(79, 27);
			this.stop_btn.TabIndex = 3;
			this.stop_btn.Text = "Stop";
			this.stop_btn.UseVisualStyleBackColor = false;
			this.stop_btn.Click += new System.EventHandler(this.button2_Click);
			// 
			// infotab3
			// 
			this.infotab3.Font = new System.Drawing.Font("Myriad Pro Light", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.infotab3.ForeColor = System.Drawing.Color.White;
			this.infotab3.Location = new System.Drawing.Point(12, 202);
			this.infotab3.Name = "infotab3";
			this.infotab3.Size = new System.Drawing.Size(329, 23);
			this.infotab3.TabIndex = 4;
			this.infotab3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// trackBar1
			// 
			this.trackBar1.AutoSize = false;
			this.trackBar1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.trackBar1.Location = new System.Drawing.Point(102, 87);
			this.trackBar1.Maximum = 100;
			this.trackBar1.Name = "trackBar1";
			this.trackBar1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.trackBar1.Size = new System.Drawing.Size(192, 25);
			this.trackBar1.TabIndex = 5;
			this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
			this.trackBar1.Value = 5;
			this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
			this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
			// 
			// percent_label
			// 
			this.percent_label.Font = new System.Drawing.Font("Myriad Pro", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.percent_label.ForeColor = System.Drawing.Color.White;
			this.percent_label.Location = new System.Drawing.Point(282, 89);
			this.percent_label.Name = "percent_label";
			this.percent_label.Size = new System.Drawing.Size(44, 13);
			this.percent_label.TabIndex = 6;
			this.percent_label.Text = "5%";
			this.percent_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// t_minutes_box
			// 
			this.t_minutes_box.Font = new System.Drawing.Font("Myriad Pro", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.t_minutes_box.Location = new System.Drawing.Point(60, 57);
			this.t_minutes_box.MaxLength = 5;
			this.t_minutes_box.Name = "t_minutes_box";
			this.t_minutes_box.Size = new System.Drawing.Size(40, 22);
			this.t_minutes_box.TabIndex = 7;
			this.t_minutes_box.Text = "3";
			this.t_minutes_box.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.t_minutes_box_KeyPress);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Myriad Pro", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.ForeColor = System.Drawing.Color.White;
			this.label5.Location = new System.Drawing.Point(103, 60);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(49, 14);
			this.label5.TabIndex = 8;
			this.label5.Text = "minutes";
			this.label5.Click += new System.EventHandler(this.label5_Click);
			// 
			// infotab2
			// 
			this.infotab2.Font = new System.Drawing.Font("Myriad Pro Light", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.infotab2.ForeColor = System.Drawing.Color.White;
			this.infotab2.Location = new System.Drawing.Point(12, 179);
			this.infotab2.Name = "infotab2";
			this.infotab2.Size = new System.Drawing.Size(329, 23);
			this.infotab2.TabIndex = 9;
			this.infotab2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// infotab1
			// 
			this.infotab1.Font = new System.Drawing.Font("Myriad Pro Light", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.infotab1.ForeColor = System.Drawing.Color.White;
			this.infotab1.Location = new System.Drawing.Point(12, 156);
			this.infotab1.Name = "infotab1";
			this.infotab1.Size = new System.Drawing.Size(329, 23);
			this.infotab1.TabIndex = 10;
			this.infotab1.Text = "System is paused.";
			this.infotab1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label88
			// 
			this.label88.Font = new System.Drawing.Font("Myriad Pro Light", 9.749999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label88.ForeColor = System.Drawing.Color.White;
			this.label88.Location = new System.Drawing.Point(25, 226);
			this.label88.Name = "label88";
			this.label88.Size = new System.Drawing.Size(55, 23);
			this.label88.TabIndex = 11;
			this.label88.Text = "Success:";
			this.label88.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// success_number_label
			// 
			this.success_number_label.Font = new System.Drawing.Font("Myriad Pro", 10F);
			this.success_number_label.ForeColor = System.Drawing.Color.White;
			this.success_number_label.Location = new System.Drawing.Point(78, 226);
			this.success_number_label.Name = "success_number_label";
			this.success_number_label.Size = new System.Drawing.Size(55, 23);
			this.success_number_label.TabIndex = 12;
			this.success_number_label.Text = "0";
			this.success_number_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lost_number_label
			// 
			this.lost_number_label.Font = new System.Drawing.Font("Myriad Pro", 10F);
			this.lost_number_label.ForeColor = System.Drawing.Color.White;
			this.lost_number_label.Location = new System.Drawing.Point(78, 249);
			this.lost_number_label.Name = "lost_number_label";
			this.lost_number_label.Size = new System.Drawing.Size(55, 23);
			this.lost_number_label.TabIndex = 14;
			this.lost_number_label.Text = "0";
			this.lost_number_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Myriad Pro Light", 9.749999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.ForeColor = System.Drawing.Color.White;
			this.label7.Location = new System.Drawing.Point(25, 249);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(47, 23);
			this.label7.TabIndex = 13;
			this.label7.Text = "Lost:";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lagged_number_label
			// 
			this.lagged_number_label.Font = new System.Drawing.Font("Myriad Pro", 10F);
			this.lagged_number_label.ForeColor = System.Drawing.Color.White;
			this.lagged_number_label.Location = new System.Drawing.Point(78, 272);
			this.lagged_number_label.Name = "lagged_number_label";
			this.lagged_number_label.Size = new System.Drawing.Size(49, 23);
			this.lagged_number_label.TabIndex = 16;
			this.lagged_number_label.Text = "0";
			this.lagged_number_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lagged_number_label.Click += new System.EventHandler(this.label3_Click);
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Myriad Pro Light", 9.749999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.ForeColor = System.Drawing.Color.White;
			this.label6.Location = new System.Drawing.Point(25, 272);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(55, 23);
			this.label6.TabIndex = 15;
			this.label6.Text = "Lagged:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label6.Click += new System.EventHandler(this.label6_Click);
			// 
			// average_number_label
			// 
			this.average_number_label.BackColor = System.Drawing.Color.Transparent;
			this.average_number_label.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.average_number_label.Font = new System.Drawing.Font("Myriad Pro", 10F);
			this.average_number_label.ForeColor = System.Drawing.Color.White;
			this.average_number_label.Location = new System.Drawing.Point(244, 226);
			this.average_number_label.Name = "average_number_label";
			this.average_number_label.Size = new System.Drawing.Size(55, 23);
			this.average_number_label.TabIndex = 18;
			this.average_number_label.Text = "0";
			this.average_number_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.average_number_label.Click += new System.EventHandler(this.label8_Click);
			// 
			// label9
			// 
			this.label9.BackColor = System.Drawing.Color.Transparent;
			this.label9.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.label9.Font = new System.Drawing.Font("Myriad Pro Light", 9.749999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.ForeColor = System.Drawing.Color.White;
			this.label9.Location = new System.Drawing.Point(181, 226);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(63, 23);
			this.label9.TabIndex = 17;
			this.label9.Text = "Average:";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label9.Click += new System.EventHandler(this.label9_Click);
			// 
			// timeleft_label
			// 
			this.timeleft_label.BackColor = System.Drawing.Color.Transparent;
			this.timeleft_label.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.timeleft_label.Font = new System.Drawing.Font("Myriad Pro", 10F);
			this.timeleft_label.ForeColor = System.Drawing.Color.White;
			this.timeleft_label.Location = new System.Drawing.Point(244, 249);
			this.timeleft_label.Name = "timeleft_label";
			this.timeleft_label.Size = new System.Drawing.Size(107, 23);
			this.timeleft_label.TabIndex = 20;
			this.timeleft_label.Text = "0:00";
			this.timeleft_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label11
			// 
			this.label11.BackColor = System.Drawing.Color.Transparent;
			this.label11.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.label11.Font = new System.Drawing.Font("Myriad Pro Light", 9.749999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label11.ForeColor = System.Drawing.Color.White;
			this.label11.Location = new System.Drawing.Point(181, 249);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(63, 23);
			this.label11.TabIndex = 19;
			this.label11.Text = "Time left:";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tryagain_check
			// 
			this.tryagain_check.AutoSize = true;
			this.tryagain_check.Checked = true;
			this.tryagain_check.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tryagain_check.Font = new System.Drawing.Font("Myriad Pro Light", 9.749999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tryagain_check.ForeColor = System.Drawing.Color.White;
			this.tryagain_check.Location = new System.Drawing.Point(184, 278);
			this.tryagain_check.Name = "tryagain_check";
			this.tryagain_check.Size = new System.Drawing.Size(117, 20);
			this.tryagain_check.TabIndex = 21;
			this.tryagain_check.Text = "Try until success";
			this.tryagain_check.UseVisualStyleBackColor = true;
			// 
			// maxping_box
			// 
			this.maxping_box.Font = new System.Drawing.Font("Myriad Pro", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.maxping_box.Location = new System.Drawing.Point(237, 58);
			this.maxping_box.Name = "maxping_box";
			this.maxping_box.Size = new System.Drawing.Size(65, 22);
			this.maxping_box.TabIndex = 22;
			this.maxping_box.Text = "150";
			this.maxping_box.TextChanged += new System.EventHandler(this.maxping_box_TextChanged);
			this.maxping_box.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.t_minutes_box_KeyPress);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Myriad Pro Light", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.ForeColor = System.Drawing.Color.White;
			this.label3.Location = new System.Drawing.Point(180, 61);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(59, 15);
			this.label3.TabIndex = 23;
			this.label3.Text = "Ping limit:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Myriad Pro", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.ForeColor = System.Drawing.Color.White;
			this.label4.Location = new System.Drawing.Point(304, 61);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(22, 14);
			this.label4.TabIndex = 24;
			this.label4.Text = "ms";
			// 
			// tittle
			// 
			this.tittle.BackColor = System.Drawing.Color.Transparent;
			this.tittle.Font = new System.Drawing.Font("Myriad Pro", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tittle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(238)))), ((int)(((byte)(255)))));
			this.tittle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.tittle.Location = new System.Drawing.Point(37, 10);
			this.tittle.Name = "tittle";
			this.tittle.Size = new System.Drawing.Size(292, 18);
			this.tittle.TabIndex = 26;
			this.tittle.Text = "Can I Play?";
			this.tittle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.tittle.Click += new System.EventHandler(this.label8_Click_1);
			this.tittle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TopBar_MouseDown);
			// 
			// TopBar
			// 
			this.TopBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(98)))), ((int)(((byte)(180)))));
			this.TopBar.Controls.Add(this.button2);
			this.TopBar.Controls.Add(this.button1);
			this.TopBar.Controls.Add(this.tittle);
			this.TopBar.Controls.Add(this.pictureBox1);
			this.TopBar.Dock = System.Windows.Forms.DockStyle.Top;
			this.TopBar.Location = new System.Drawing.Point(0, 0);
			this.TopBar.Name = "TopBar";
			this.TopBar.Size = new System.Drawing.Size(353, 38);
			this.TopBar.TabIndex = 28;
			this.TopBar.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
			this.TopBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TopBar_MouseDown);
			// 
			// button2
			// 
			this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(98)))), ((int)(((byte)(180)))));
			this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
			this.button2.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(98)))), ((int)(((byte)(180)))));
			this.button2.FlatAppearance.BorderSize = 0;
			this.button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(98)))), ((int)(((byte)(180)))));
			this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(125)))));
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button2.Font = new System.Drawing.Font("Myriad Pro", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(238)))), ((int)(((byte)(255)))));
			this.button2.Location = new System.Drawing.Point(284, 0);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(35, 38);
			this.button2.TabIndex = 30;
			this.button2.Text = "🗕";
			this.button2.UseVisualStyleBackColor = false;
			this.button2.Click += new System.EventHandler(this.minimizeBtn);
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
			this.button1.Location = new System.Drawing.Point(319, 0);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(35, 38);
			this.button1.TabIndex = 29;
			this.button1.Text = "✖";
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new System.EventHandler(this.closeBtn);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(12, 10);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(40, 50);
			this.pictureBox1.TabIndex = 27;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TopBar_MouseDown);
			// 
			// showHistory_btn
			// 
			this.showHistory_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(98)))), ((int)(((byte)(180)))));
			this.showHistory_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.showHistory_btn.Cursor = System.Windows.Forms.Cursors.Hand;
			this.showHistory_btn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(98)))), ((int)(((byte)(180)))));
			this.showHistory_btn.FlatAppearance.BorderSize = 0;
			this.showHistory_btn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(98)))), ((int)(((byte)(180)))));
			this.showHistory_btn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(125)))));
			this.showHistory_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.showHistory_btn.Font = new System.Drawing.Font("Myriad Pro Light", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.showHistory_btn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(238)))), ((int)(((byte)(255)))));
			this.showHistory_btn.Location = new System.Drawing.Point(123, 321);
			this.showHistory_btn.Name = "showHistory_btn";
			this.showHistory_btn.Size = new System.Drawing.Size(108, 27);
			this.showHistory_btn.TabIndex = 29;
			this.showHistory_btn.Text = "Show history";
			this.showHistory_btn.UseVisualStyleBackColor = false;
			this.showHistory_btn.Click += new System.EventHandler(this.showHistory_btn_Click);
			// 
			// debugBox
			// 
			this.debugBox.Location = new System.Drawing.Point(70, 344);
			this.debugBox.Name = "debugBox";
			this.debugBox.Size = new System.Drawing.Size(202, 90);
			this.debugBox.TabIndex = 30;
			this.debugBox.Text = "";
			// 
			// Form1
			// 
			this.AcceptButton = this.start_btn;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(32)))), ((int)(((byte)(41)))));
			this.CancelButton = this.stop_btn;
			this.ClientSize = new System.Drawing.Size(353, 319);
			this.Controls.Add(this.debugBox);
			this.Controls.Add(this.showHistory_btn);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.maxping_box);
			this.Controls.Add(this.tryagain_check);
			this.Controls.Add(this.timeleft_label);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.average_number_label);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.lagged_number_label);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.lost_number_label);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.success_number_label);
			this.Controls.Add(this.label88);
			this.Controls.Add(this.infotab1);
			this.Controls.Add(this.infotab2);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.t_minutes_box);
			this.Controls.Add(this.trackBar1);
			this.Controls.Add(this.infotab3);
			this.Controls.Add(this.stop_btn);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.start_btn);
			this.Controls.Add(this.percent_label);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.TopBar);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Can I Play?";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
			this.TopBar.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button start_btn;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button stop_btn;
		private System.Windows.Forms.Label infotab3;
		private System.Windows.Forms.Label percent_label;
		private System.Windows.Forms.TextBox t_minutes_box;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TrackBar trackBar1;
		private System.Windows.Forms.Label infotab2;
		private System.Windows.Forms.Label infotab1;
		private System.Windows.Forms.Label label88;
		private System.Windows.Forms.Label success_number_label;
		private System.Windows.Forms.Label lost_number_label;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label lagged_number_label;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label average_number_label;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label timeleft_label;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.CheckBox tryagain_check;
		private System.Windows.Forms.TextBox maxping_box;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label tittle;
		private System.Windows.Forms.Panel TopBar;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button showHistory_btn;
		private System.Windows.Forms.RichTextBox debugBox;
		private System.Windows.Forms.Button button2;
	}
}

