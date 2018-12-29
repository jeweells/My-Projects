namespace FastNotes
{
	partial class NoteForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NoteForm));
            this.topbar = new FastNotes.DBPanel();
            this.pinBtn = new System.Windows.Forms.Button();
            this.fontBtn = new System.Windows.Forms.Button();
            this.addNoteBtn = new System.Windows.Forms.Button();
            this.moreBtn = new System.Windows.Forms.Button();
            this.closeBtn = new System.Windows.Forms.Button();
            this.panel2 = new FastNotes.DBPanel();
            this.textBoxPanel = new FastNotes.DBPanel();
            this.textBox = new FastNotes.NoteText();
            this.rightClicNoteText = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAllCtrlAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.boldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.italicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.strikeoutCtrlSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.underlineCtrlUToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomInCtrlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomOutCtrlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.alignLeftCtrlJToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alignCenterCtrlWToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alignRightCtrlRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fontPicker = new System.Windows.Forms.FontDialog();
            this.topbar.SuspendLayout();
            this.panel2.SuspendLayout();
            this.textBoxPanel.SuspendLayout();
            this.rightClicNoteText.SuspendLayout();
            this.SuspendLayout();
            // 
            // topbar
            // 
            this.topbar.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.topbar.Controls.Add(this.pinBtn);
            this.topbar.Controls.Add(this.fontBtn);
            this.topbar.Controls.Add(this.addNoteBtn);
            this.topbar.Controls.Add(this.moreBtn);
            this.topbar.Controls.Add(this.closeBtn);
            this.topbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.topbar.Location = new System.Drawing.Point(4, 4);
            this.topbar.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.topbar.Name = "topbar";
            this.topbar.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.topbar.Size = new System.Drawing.Size(230, 35);
            this.topbar.TabIndex = 1;
            this.topbar.TabStop = true;
            this.topbar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnTouchBarPressed);
            // 
            // pinBtn
            // 
            this.pinBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.pinBtn.FlatAppearance.BorderSize = 0;
            this.pinBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pinBtn.Image = global::FastNotes.Properties.Resources.fixedIcons_0006_pin;
            this.pinBtn.Location = new System.Drawing.Point(118, 0);
            this.pinBtn.Name = "pinBtn";
            this.pinBtn.Size = new System.Drawing.Size(28, 31);
            this.pinBtn.TabIndex = 6;
            this.pinBtn.TabStop = false;
            this.pinBtn.UseVisualStyleBackColor = true;
            this.pinBtn.Click += new System.EventHandler(this.OnPinBtnClick);
            // 
            // fontBtn
            // 
            this.fontBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.fontBtn.FlatAppearance.BorderSize = 0;
            this.fontBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fontBtn.Image = global::FastNotes.Properties.Resources.fixedIcons_0004_font;
            this.fontBtn.Location = new System.Drawing.Point(146, 0);
            this.fontBtn.Name = "fontBtn";
            this.fontBtn.Size = new System.Drawing.Size(28, 31);
            this.fontBtn.TabIndex = 5;
            this.fontBtn.TabStop = false;
            this.fontBtn.UseVisualStyleBackColor = true;
            this.fontBtn.Click += new System.EventHandler(this.OnFontBtnClick);
            // 
            // addNoteBtn
            // 
            this.addNoteBtn.Dock = System.Windows.Forms.DockStyle.Left;
            this.addNoteBtn.FlatAppearance.BorderSize = 0;
            this.addNoteBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addNoteBtn.Image = global::FastNotes.Properties.Resources.fixedIcons_0007_add;
            this.addNoteBtn.Location = new System.Drawing.Point(0, 0);
            this.addNoteBtn.Name = "addNoteBtn";
            this.addNoteBtn.Size = new System.Drawing.Size(29, 31);
            this.addNoteBtn.TabIndex = 2;
            this.addNoteBtn.TabStop = false;
            this.addNoteBtn.UseVisualStyleBackColor = true;
            this.addNoteBtn.Click += new System.EventHandler(this.OnCreateNoteBtnClick);
            // 
            // moreBtn
            // 
            this.moreBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.moreBtn.FlatAppearance.BorderSize = 0;
            this.moreBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.moreBtn.Image = global::FastNotes.Properties.Resources.fixedIcons_0000_colorPicker2;
            this.moreBtn.Location = new System.Drawing.Point(174, 0);
            this.moreBtn.Name = "moreBtn";
            this.moreBtn.Size = new System.Drawing.Size(28, 31);
            this.moreBtn.TabIndex = 3;
            this.moreBtn.TabStop = false;
            this.moreBtn.UseVisualStyleBackColor = true;
            this.moreBtn.Click += new System.EventHandler(this.OnNoteColorChangeClick);
            // 
            // closeBtn
            // 
            this.closeBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.closeBtn.FlatAppearance.BorderSize = 0;
            this.closeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeBtn.Image = global::FastNotes.Properties.Resources.fixedIcons_0002_trash;
            this.closeBtn.Location = new System.Drawing.Point(202, 0);
            this.closeBtn.Margin = new System.Windows.Forms.Padding(0);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(28, 31);
            this.closeBtn.TabIndex = 4;
            this.closeBtn.TabStop = false;
            this.closeBtn.UseVisualStyleBackColor = true;
            this.closeBtn.Click += new System.EventHandler(this.OnEraseBtnClick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.textBoxPanel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(4, 39);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(230, 232);
            this.panel2.TabIndex = 2;
            // 
            // textBoxPanel
            // 
            this.textBoxPanel.Controls.Add(this.textBox);
            this.textBoxPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPanel.Location = new System.Drawing.Point(0, 0);
            this.textBoxPanel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.textBoxPanel.Name = "textBoxPanel";
            this.textBoxPanel.Padding = new System.Windows.Forms.Padding(5);
            this.textBoxPanel.Size = new System.Drawing.Size(230, 232);
            this.textBoxPanel.TabIndex = 1;
            this.textBoxPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // textBox
            // 
            this.textBox.AcceptsTab = true;
            this.textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox.ContextMenuStrip = this.rightClicNoteText;
            this.textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox.EnableAutoDragDrop = true;
            this.textBox.Font = new System.Drawing.Font("Lane - Narrow", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox.Location = new System.Drawing.Point(5, 5);
            this.textBox.Name = "textBox";
            this.textBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.textBox.Size = new System.Drawing.Size(220, 222);
            this.textBox.TabIndex = 1;
            this.textBox.Text = "";
            this.textBox.TextChanged += new System.EventHandler(this.OnNoteContentChanged);
            // 
            // rightClicNoteText
            // 
            this.rightClicNoteText.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.selectAllCtrlAToolStripMenuItem,
            this.toolStripSeparator1,
            this.boldToolStripMenuItem,
            this.italicToolStripMenuItem,
            this.strikeoutCtrlSToolStripMenuItem,
            this.underlineCtrlUToolStripMenuItem,
            this.zoomInCtrlToolStripMenuItem,
            this.zoomOutCtrlToolStripMenuItem,
            this.toolStripSeparator2,
            this.alignLeftCtrlJToolStripMenuItem,
            this.alignCenterCtrlWToolStripMenuItem,
            this.alignRightCtrlRToolStripMenuItem});
            this.rightClicNoteText.Name = "rightClicNoteText";
            this.rightClicNoteText.Size = new System.Drawing.Size(196, 368);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.cutToolStripMenuItem.Text = "Cut (Ctrl + X)";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.copyToolStripMenuItem.Text = "Copy (Ctrl + C)";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.pasteToolStripMenuItem.Text = "Paste (Ctrl + V)";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.deleteToolStripMenuItem.Text = "Delete (Supr)";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.undoToolStripMenuItem.Text = "Undo (Ctrl + Z)";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.redoToolStripMenuItem.Text = "Redo (Ctrl + Y)";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            // 
            // selectAllCtrlAToolStripMenuItem
            // 
            this.selectAllCtrlAToolStripMenuItem.Name = "selectAllCtrlAToolStripMenuItem";
            this.selectAllCtrlAToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.selectAllCtrlAToolStripMenuItem.Text = "Select All (Ctrl + A)";
            this.selectAllCtrlAToolStripMenuItem.Click += new System.EventHandler(this.selectAllCtrlAToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(192, 6);
            // 
            // boldToolStripMenuItem
            // 
            this.boldToolStripMenuItem.Name = "boldToolStripMenuItem";
            this.boldToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.boldToolStripMenuItem.Text = "Bold (Ctrl + B)";
            this.boldToolStripMenuItem.Click += new System.EventHandler(this.boldToolStripMenuItem_Click);
            // 
            // italicToolStripMenuItem
            // 
            this.italicToolStripMenuItem.Name = "italicToolStripMenuItem";
            this.italicToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.italicToolStripMenuItem.Text = "Italic (Ctrl + N)";
            this.italicToolStripMenuItem.Click += new System.EventHandler(this.italicToolStripMenuItem_Click);
            // 
            // strikeoutCtrlSToolStripMenuItem
            // 
            this.strikeoutCtrlSToolStripMenuItem.Name = "strikeoutCtrlSToolStripMenuItem";
            this.strikeoutCtrlSToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.strikeoutCtrlSToolStripMenuItem.Text = "Strikeout (Ctrl + S)";
            this.strikeoutCtrlSToolStripMenuItem.Click += new System.EventHandler(this.strikeoutCtrlSToolStripMenuItem_Click);
            // 
            // underlineCtrlUToolStripMenuItem
            // 
            this.underlineCtrlUToolStripMenuItem.Name = "underlineCtrlUToolStripMenuItem";
            this.underlineCtrlUToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.underlineCtrlUToolStripMenuItem.Text = "Underline (Ctrl + U)";
            this.underlineCtrlUToolStripMenuItem.Click += new System.EventHandler(this.underlineCtrlUToolStripMenuItem_Click);
            // 
            // zoomInCtrlToolStripMenuItem
            // 
            this.zoomInCtrlToolStripMenuItem.Name = "zoomInCtrlToolStripMenuItem";
            this.zoomInCtrlToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.zoomInCtrlToolStripMenuItem.Text = "Zoom in (Ctrl + +)";
            this.zoomInCtrlToolStripMenuItem.Click += new System.EventHandler(this.zoomInCtrlToolStripMenuItem_Click);
            // 
            // zoomOutCtrlToolStripMenuItem
            // 
            this.zoomOutCtrlToolStripMenuItem.Name = "zoomOutCtrlToolStripMenuItem";
            this.zoomOutCtrlToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.zoomOutCtrlToolStripMenuItem.Text = "Zoom out (Ctrl + -)";
            this.zoomOutCtrlToolStripMenuItem.Click += new System.EventHandler(this.zoomOutCtrlToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(192, 6);
            // 
            // alignLeftCtrlJToolStripMenuItem
            // 
            this.alignLeftCtrlJToolStripMenuItem.Name = "alignLeftCtrlJToolStripMenuItem";
            this.alignLeftCtrlJToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.alignLeftCtrlJToolStripMenuItem.Text = "Align Left (Ctrl + J)";
            this.alignLeftCtrlJToolStripMenuItem.Click += new System.EventHandler(this.alignLeftCtrlJToolStripMenuItem_Click);
            // 
            // alignCenterCtrlWToolStripMenuItem
            // 
            this.alignCenterCtrlWToolStripMenuItem.Name = "alignCenterCtrlWToolStripMenuItem";
            this.alignCenterCtrlWToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.alignCenterCtrlWToolStripMenuItem.Text = "Align Center (Ctrl + W)";
            this.alignCenterCtrlWToolStripMenuItem.Click += new System.EventHandler(this.alignCenterCtrlWToolStripMenuItem_Click);
            // 
            // alignRightCtrlRToolStripMenuItem
            // 
            this.alignRightCtrlRToolStripMenuItem.Name = "alignRightCtrlRToolStripMenuItem";
            this.alignRightCtrlRToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.alignRightCtrlRToolStripMenuItem.Text = "Align Right (Ctrl + R)";
            this.alignRightCtrlRToolStripMenuItem.Click += new System.EventHandler(this.alignRightCtrlRToolStripMenuItem_Click);
            // 
            // NoteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Maroon;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(238, 275);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.topbar);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NoteForm";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Note";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnNoteFormClosing);
            this.Shown += new System.EventHandler(this.NoteForm_Shown);
            this.topbar.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.textBoxPanel.ResumeLayout(false);
            this.rightClicNoteText.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Button addNoteBtn;
		private System.Windows.Forms.Button moreBtn;
		private System.Windows.Forms.Button closeBtn;
		private NoteText textBox;
		private System.Windows.Forms.Button pinBtn;
		private System.Windows.Forms.Button fontBtn;
		private System.Windows.Forms.FontDialog fontPicker;
		private DBPanel topbar;
		private DBPanel panel2;
		private DBPanel textBoxPanel;
		private System.Windows.Forms.ContextMenuStrip rightClicNoteText;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem boldToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem italicToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem strikeoutCtrlSToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem underlineCtrlUToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem zoomInCtrlToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem zoomOutCtrlToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem selectAllCtrlAToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem alignLeftCtrlJToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem alignCenterCtrlWToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem alignRightCtrlRToolStripMenuItem;
	}
}

