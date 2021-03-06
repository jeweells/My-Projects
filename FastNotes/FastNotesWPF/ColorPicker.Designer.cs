﻿namespace FastNotes
{

	partial class ColorPicker
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
			this.colorPickedDisplayer = new FastNotes.DBPanel();
			this.cancelBtn = new System.Windows.Forms.Button();
			this.selectBtn = new System.Windows.Forms.Button();
			this.colorDisplayer = new FastNotes.DBPanel();
			this.colorDot = new System.Windows.Forms.PictureBox();
			this.hueSelector = new FastNotes.DBPanel();
			this.hueDot = new System.Windows.Forms.PictureBox();
			this.borderPanel = new FastNotes.DBPanel();
			this.blankPanel = new FastNotes.DBPanel();
			this.colorDisplayer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.colorDot)).BeginInit();
			this.hueSelector.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.hueDot)).BeginInit();
			this.borderPanel.SuspendLayout();
			this.blankPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// colorPickedDisplayer
			// 
			this.colorPickedDisplayer.Location = new System.Drawing.Point(0, 252);
			this.colorPickedDisplayer.Name = "colorPickedDisplayer";
			this.colorPickedDisplayer.Size = new System.Drawing.Size(398, 20);
			this.colorPickedDisplayer.TabIndex = 6;
			// 
			// cancelBtn
			// 
			this.cancelBtn.BackColor = System.Drawing.Color.White;
			this.cancelBtn.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
			this.cancelBtn.FlatAppearance.BorderSize = 0;
			this.cancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.cancelBtn.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cancelBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.cancelBtn.Location = new System.Drawing.Point(0, 272);
			this.cancelBtn.Name = "cancelBtn";
			this.cancelBtn.Size = new System.Drawing.Size(199, 31);
			this.cancelBtn.TabIndex = 1;
			this.cancelBtn.Text = "CANCEL";
			this.cancelBtn.UseVisualStyleBackColor = false;
			this.cancelBtn.Click += new System.EventHandler(this.button2_Click);
			// 
			// selectBtn
			// 
			this.selectBtn.BackColor = System.Drawing.Color.White;
			this.selectBtn.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
			this.selectBtn.FlatAppearance.BorderSize = 0;
			this.selectBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.selectBtn.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.selectBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.selectBtn.Location = new System.Drawing.Point(200, 272);
			this.selectBtn.Name = "selectBtn";
			this.selectBtn.Size = new System.Drawing.Size(199, 31);
			this.selectBtn.TabIndex = 0;
			this.selectBtn.Text = "SELECT";
			this.selectBtn.UseVisualStyleBackColor = false;
			this.selectBtn.Click += new System.EventHandler(this.button1_Click);
			// 
			// colorDisplayer
			// 
			this.colorDisplayer.Controls.Add(this.colorDot);
			this.colorDisplayer.Dock = System.Windows.Forms.DockStyle.Top;
			this.colorDisplayer.Location = new System.Drawing.Point(0, 0);
			this.colorDisplayer.Name = "colorDisplayer";
			this.colorDisplayer.Size = new System.Drawing.Size(398, 200);
			this.colorDisplayer.TabIndex = 5;
			this.colorDisplayer.Enter += new System.EventHandler(this.colorDisplayer_Enter);
			this.colorDisplayer.Leave += new System.EventHandler(this.colorDisplayer_Leave);
			this.colorDisplayer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.colorDisplayer_MouseDown);
			this.colorDisplayer.MouseEnter += new System.EventHandler(this.colorDisplayer_MouseEnter);
			this.colorDisplayer.MouseLeave += new System.EventHandler(this.colorDisplayer_MouseLeave);
			this.colorDisplayer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.colorDisplayer_MouseMove);
			this.colorDisplayer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.colorDisplayer_MouseUp);
			// 
			// colorDot
			// 
			this.colorDot.BackColor = System.Drawing.Color.Transparent;
			this.colorDot.BackgroundImage = global::FastNotes.Properties.Resources.PickerDot;
			this.colorDot.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.colorDot.Location = new System.Drawing.Point(212, 12);
			this.colorDot.Name = "colorDot";
			this.colorDot.Size = new System.Drawing.Size(17, 17);
			this.colorDot.TabIndex = 0;
			this.colorDot.TabStop = false;
			this.colorDot.MouseDown += new System.Windows.Forms.MouseEventHandler(this.colorDisplayer_MouseDown);
			this.colorDot.MouseEnter += new System.EventHandler(this.colorDisplayer_MouseEnter);
			this.colorDot.MouseMove += new System.Windows.Forms.MouseEventHandler(this.colorDisplayer_MouseMove);
			this.colorDot.MouseUp += new System.Windows.Forms.MouseEventHandler(this.colorDisplayer_MouseUp);
			// 
			// hueSelector
			// 
			this.hueSelector.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.hueSelector.Controls.Add(this.hueDot);
			this.hueSelector.Location = new System.Drawing.Point(15, 215);
			this.hueSelector.Name = "hueSelector";
			this.hueSelector.Size = new System.Drawing.Size(368, 22);
			this.hueSelector.TabIndex = 4;
			this.hueSelector.MouseDown += new System.Windows.Forms.MouseEventHandler(this.hueDot_MouseDown);
			this.hueSelector.MouseMove += new System.Windows.Forms.MouseEventHandler(this.hueSelector_MouseMove);
			this.hueSelector.MouseUp += new System.Windows.Forms.MouseEventHandler(this.hueDot_MouseUp);
			// 
			// hueDot
			// 
			this.hueDot.BackColor = System.Drawing.Color.Transparent;
			this.hueDot.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.hueDot.Cursor = System.Windows.Forms.Cursors.Default;
			this.hueDot.Image = global::FastNotes.Properties.Resources.grabDot;
			this.hueDot.Location = new System.Drawing.Point(0, 3);
			this.hueDot.Name = "hueDot";
			this.hueDot.Size = new System.Drawing.Size(12, 16);
			this.hueDot.TabIndex = 3;
			this.hueDot.TabStop = false;
			this.hueDot.MouseDown += new System.Windows.Forms.MouseEventHandler(this.hueDot_MouseDown);
			this.hueDot.MouseMove += new System.Windows.Forms.MouseEventHandler(this.hueSelector_MouseMove);
			this.hueDot.MouseUp += new System.Windows.Forms.MouseEventHandler(this.hueDot_MouseUp);
			// 
			// borderPanel
			// 
			this.borderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.borderPanel.Controls.Add(this.blankPanel);
			this.borderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.borderPanel.Location = new System.Drawing.Point(0, 0);
			this.borderPanel.Name = "borderPanel";
			this.borderPanel.Padding = new System.Windows.Forms.Padding(1);
			this.borderPanel.Size = new System.Drawing.Size(400, 305);
			this.borderPanel.TabIndex = 7;
			// 
			// blankPanel
			// 
			this.blankPanel.BackColor = System.Drawing.Color.White;
			this.blankPanel.Controls.Add(this.colorDisplayer);
			this.blankPanel.Controls.Add(this.colorPickedDisplayer);
			this.blankPanel.Controls.Add(this.hueSelector);
			this.blankPanel.Controls.Add(this.cancelBtn);
			this.blankPanel.Controls.Add(this.selectBtn);
			this.blankPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.blankPanel.Location = new System.Drawing.Point(1, 1);
			this.blankPanel.Name = "blankPanel";
			this.blankPanel.Size = new System.Drawing.Size(398, 303);
			this.blankPanel.TabIndex = 0;
			// 
			// ColorPicker
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.ClientSize = new System.Drawing.Size(400, 305);
			this.Controls.Add(this.borderPanel);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "ColorPicker";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ColorPicker";
			this.Load += new System.EventHandler(this.ColorPicker_Load);
			this.Shown += new System.EventHandler(this.ColorPicker_Shown);
			this.VisibleChanged += new System.EventHandler(this.ColorPicker_VisibleChanged);
			this.colorDisplayer.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.colorDot)).EndInit();
			this.hueSelector.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.hueDot)).EndInit();
			this.borderPanel.ResumeLayout(false);
			this.blankPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.PictureBox hueDot;
		private DBPanel colorDisplayer;
		private DBPanel hueSelector;
		private DBPanel colorPickedDisplayer;
		private System.Windows.Forms.Button cancelBtn;
		private System.Windows.Forms.Button selectBtn;
		private System.Windows.Forms.PictureBox colorDot;
		private DBPanel borderPanel;
		private DBPanel blankPanel;
	}
}