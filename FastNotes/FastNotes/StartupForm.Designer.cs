using System;

namespace FastNotes
{
	partial class StartupForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartupForm));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.updateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DesktopsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newDesktopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteAllDesktopsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.importDesktopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportAllDesktopsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.runAtStartupContextMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 60000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "FastNotes";
            this.notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateToolStripMenuItem,
            this.DesktopsToolStripMenuItem,
            this.runAtStartupContextMenuStrip,
            this.exitToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(149, 92);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // updateToolStripMenuItem
            // 
            this.updateToolStripMenuItem.Name = "updateToolStripMenuItem";
            this.updateToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.updateToolStripMenuItem.Text = "Update";
            this.updateToolStripMenuItem.Click += new System.EventHandler(this.updateToolStripMenuItem_Click);
            // 
            // DesktopsToolStripMenuItem
            // 
            this.DesktopsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newDesktopToolStripMenuItem,
            this.deleteAllDesktopsToolStripMenuItem,
            this.toolStripSeparator1,
            this.importDesktopToolStripMenuItem,
            this.exportAllDesktopsToolStripMenuItem,
            this.toolStripSeparator2});
            this.DesktopsToolStripMenuItem.Name = "DesktopsToolStripMenuItem";
            this.DesktopsToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.DesktopsToolStripMenuItem.Text = "Desktops";
            // 
            // newDesktopToolStripMenuItem
            // 
            this.newDesktopToolStripMenuItem.Name = "newDesktopToolStripMenuItem";
            this.newDesktopToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.newDesktopToolStripMenuItem.Text = "New desktop";
            this.newDesktopToolStripMenuItem.Click += new System.EventHandler(this.newDesktopToolStripMenuItem_Click);
            // 
            // deleteAllDesktopsToolStripMenuItem
            // 
            this.deleteAllDesktopsToolStripMenuItem.Name = "deleteAllDesktopsToolStripMenuItem";
            this.deleteAllDesktopsToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.deleteAllDesktopsToolStripMenuItem.Text = "Delete all desktops";
            this.deleteAllDesktopsToolStripMenuItem.Click += new System.EventHandler(this.deleteAllDesktopsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(169, 6);
            // 
            // importDesktopToolStripMenuItem
            // 
            this.importDesktopToolStripMenuItem.Name = "importDesktopToolStripMenuItem";
            this.importDesktopToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.importDesktopToolStripMenuItem.Text = "Import desktops";
            this.importDesktopToolStripMenuItem.Click += new System.EventHandler(this.importDesktopToolStripMenuItem_Click);
            // 
            // exportAllDesktopsToolStripMenuItem
            // 
            this.exportAllDesktopsToolStripMenuItem.Name = "exportAllDesktopsToolStripMenuItem";
            this.exportAllDesktopsToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.exportAllDesktopsToolStripMenuItem.Text = "Export all desktops";
            this.exportAllDesktopsToolStripMenuItem.Click += new System.EventHandler(this.exportAllDesktopsToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(169, 6);
            // 
            // runAtStartupContextMenuStrip
            // 
            this.runAtStartupContextMenuStrip.Checked = true;
            this.runAtStartupContextMenuStrip.CheckState = System.Windows.Forms.CheckState.Checked;
            this.runAtStartupContextMenuStrip.Name = "runAtStartupContextMenuStrip";
            this.runAtStartupContextMenuStrip.Size = new System.Drawing.Size(148, 22);
            this.runAtStartupContextMenuStrip.Text = "Run at startup";
            this.runAtStartupContextMenuStrip.Click += new System.EventHandler(this.runAtStartupContextMenuStrip_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // StartupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(183)))), ((int)(((byte)(173)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(496, 473);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StartupForm";
            this.Opacity = 0D;
            this.Text = "FastNotes";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(183)))), ((int)(((byte)(173)))));
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.StartupForm_Activated);
            this.Deactivate += new System.EventHandler(this.StartupForm_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StartupForm_FormClosing);
            this.VisibleChanged += new System.EventHandler(this.StartupForm_VisibleChanged);
            this.Leave += new System.EventHandler(this.StartupForm_Leave);
            this.Resize += new System.EventHandler(this.StartupForm_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		private void openWithWindowsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		#endregion

		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.NotifyIcon notifyIcon1;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem runAtStartupContextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DesktopsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newDesktopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteAllDesktopsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem importDesktopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportAllDesktopsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem updateToolStripMenuItem;
    }
}