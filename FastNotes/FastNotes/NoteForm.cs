using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accord.Imaging;
namespace FastNotes
{
    public partial class NoteForm : Form
    {

        Bitmap pinBtnImage = global::FastNotes.Properties.Resources.fixedIcons_0006_pin;
        Bitmap pinnedBtnImage = global::FastNotes.Properties.Resources.fixedIcons_0005_pinned;

        LinkedListNode<Form> thisNode; // Reference to the node in the group of notes declared in Program.availableForms
        NoteData ndata = null; // Current data
        bool changed = false; // Let outside classes know when this note hasn't saved its data

        /// <summary>
        /// When closing all notes if there are no notes and this is true, the application will be requested to exit
        /// </summary>
        public bool CloseWithTheLastNoteOpened { get; set; } = true;
        /// <summary>
        /// When this variable is set to true, the confirmation to delete the note will be ignored and assumed as "yes"
        /// </summary>
        public bool CloseDialog { get; set; } = true;
        /// <summary>
        /// Do not perform any operations about data when this note is closing
        /// </summary>
        public bool ForgetDataWhenClosing { get; set; } = false;

        /// <summary>
        /// Overriding this does nothing I believe
        /// </summary>
        protected override CreateParams CreateParams
		{
			get
			{
				var cp = base.CreateParams;
				cp.ExStyle |= 0x02000000;    // Turn on WS_EX_COMPOSITED
				return cp;
			}
		}

        public NoteForm(LinkedListNode<Form> thisNode)
        {
            this.thisNode = thisNode; // Save a reference of the node in the current notes list
            DoubleBuffered = true; // Helps flickering although I think here does nothing, it was solved when inheriting Panel class and doing it there
            InitializeComponent();

            // Make buttons' borders transparent
            addNoteBtn.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            moreBtn.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            closeBtn.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            fontBtn.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            pinBtn.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            Load += (x, y) =>{
                if (ndata == null)
                { // Painting randomly the note (Only when it hasn't loaded the data which means it has already been painted)
                    Random r = new Random(DateTime.Now.Millisecond);
                    int rgb = Math.Abs(r.Next(0, 0x1000000)); // Random in [0, 256)
                    PaintNote(Color.FromArgb((rgb & 0xFF0000) >> 16, (rgb & 0xFF00) >> 8, rgb & 0xFF)); // Create and paint our note randomly
                }
            };
        }
        /// <summary>
        /// Leaves a mark that tells this note data hasn't been saved
        /// </summary>
		public bool Changed { get { return changed;  }
			set {
				changed = value;
				if (changed)
					StartupForm.dataSaved = false;
			}
		}

        /// <summary>
        /// The note data including position, text, color, etc.
        /// If not initialized, it intializes itself when calling it
        /// </summary>
		public NoteData Data
		{
			get
			{
				if (!changed && ndata != null) return ndata;
				if (ndata == null) ndata = new NoteData();
				ndata.mainColor = textBox.BackColor;
				ndata.noteSize = new Size(Width, Height);
				ndata.noteLocation = Location;
				ndata.rtbInfo = new NoteData.RichTextBox();
				ndata.rtbInfo.font = textBox.Font;
				ndata.rtbInfo.rtf = textBox.Rtf;
				ndata.rtbInfo.zoomFactor = textBox.ZoomFactor;
				ndata.pinned = IsPinned;
				return ndata;
			}
			set
			{
				ndata = value;
				if(value != null)
				{
					Location = value.noteLocation;
					Width = value.noteSize.Width;
					Height = value.noteSize.Height;
					textBox.Rtf = value.rtbInfo.rtf;
					//textBox.Font = value.rtbInfo.font;
					textBox.ZoomFactor = value.rtbInfo.zoomFactor;
					if (ndata.pinned) PinNote();
					PaintNote(value.mainColor);
					changed = false;
				}
			}
		}

        /// <summary>
        /// Happens when touching the note topbar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void panel1_MouseDown(object sender, MouseEventArgs e)
		{
			TopBarActions.MouseDown(Handle, e); // This makes the top bar move the form
			Changed = true; // Data has changed
		}

        /// <summary>
        /// Clicking the erase note button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void button1_Click(object sender, EventArgs e)
		{
            Close();

			//if (textBox.Text == "" || DialogResult.Yes == MessageBox.Show("Are you sure you want to delete this note?", "Deleting note", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
			//{
			//	Program.availableForms.Remove(thisNode);
			//	StartupForm.dataSaved = false; // Let father know there are changes
			//	if (Program.availableForms.Count == 0)
			//	{
			//		Application.Exit();
			//	}
			//	else
			//	{
			//		Close();
			//	}
			//}
		}

		protected struct SCROLLINFO
		{
			public uint cbSize;
			public uint fMask;
			public int nMin;
			public int nMax;
			public uint nPage;
			public int nPos;
			public int nTrackPos;
		}

		[DllImport("user32.dll")]
		protected static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
		[DllImport("user32.dll")]
		protected static extern bool GetScrollInfo(IntPtr hwnd, int fnBar, ref SCROLLINFO lpsi);
		

        /// <summary>
        /// Makes the note resizable
        /// </summary>
        /// <param name="m"></param>
		protected override void WndProc(ref Message m)
		{
			int grab = 16;
			base.WndProc(ref m);
			if (m.Msg == 0x84)
			{  // Trap WM_NCHITTEST
				var pos = this.PointToClient(new Point(m.LParam.ToInt32()));
				if (pos.X >= this.ClientSize.Width - grab && pos.Y >= this.ClientSize.Height - grab)
					m.Result = new IntPtr(17);  // HT_BOTTOMRIGHT
				else if (pos.X <= grab && pos.Y <= grab)
					m.Result = new IntPtr(13); // HT_TOPLEFT
				else if (pos.X <= grab && pos.Y >= this.ClientSize.Height - grab)
					m.Result = new IntPtr(16); // HT_BOTTOMLEFT
				else if (pos.X >= this.ClientSize.Width - grab && pos.Y <= grab)
					m.Result = new IntPtr(14); // HT_TOPRIGHT
				else if (pos.Y <= grab)
					m.Result = new IntPtr(12); // HT_TOP
				else if (pos.X <= grab)
					m.Result = new IntPtr(10); // HT_LEFT
				else if (pos.X >= this.ClientSize.Width - grab)
					m.Result = new IntPtr(11); // HT_RIGHT
				else if (pos.Y >= this.ClientSize.Height - grab)
					m.Result = new IntPtr(15); // HT_BOTTOM
			}
		}

        /// <summary>
        /// Implements when a note is created
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void button3_Click(object sender, EventArgs e)
		{
			Program.CreateNote().Show(Program.startupForm);
			StartupForm.dataSaved = false; // Let father know there are changes
		}
		ColorPicker colorPicker;
        /// <summary>
        /// Implements when it's wanted to change the color of a note
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void button2_Click(object sender, EventArgs e)
		{
			if (colorPicker == null)
			{
				Font f = new Font(DefaultFont, FontStyle.Bold);
				colorPicker = new ColorPicker(textBox.BackColor, f);
			}
			else
			{
				colorPicker.SetStartColor(textBox.BackColor);
			}
			if (colorPicker.ShowDialog() == DialogResult.OK)
			{
				PaintNote(colorPicker.currentColorPicked);
				Changed = true;
			}
		}


		float RelativeIuminanceHelper(float component)
		{

			return (component <= 0.03928)? component / 12.92f :
				(float)Math.Pow((component + 0.055)/1.055, 2.4f);
		}
		float RelativeIuminance(ref RGB color)
		{
			float Rg = RelativeIuminanceHelper(color.Red/255f),
				Gg = RelativeIuminanceHelper(color.Green/255f),
				Bg = RelativeIuminanceHelper(color.Blue/255f);
			return 0.2126f * Rg + 0.7152f * Gg + 0.0722f * Bg;
		}

		float ContrastRatio(ref RGB color1, ref RGB color2)
		{
			float r1 = RelativeIuminance(ref color1);
			float r2 = RelativeIuminance(ref color2);

			//MessageBox.Show($"Rel of ({color1.ToString()}) is {r1.ToString()} \nRel of {color2.ToString()} is {r2}");
			return (Math.Max(r1, r2) + 0.05f) / (Math.Min(r1, r2) + 0.05f); 
		}

		RGB ChooseBetterContrastByLuminance(ref RGB backColor, float lumRatio)
		{
			HSL hslBackColor = HSL.FromRGB(backColor);
			float defaultLum = hslBackColor.Luminance;
			hslBackColor.Luminance += lumRatio;
			if (hslBackColor.Luminance > 1) hslBackColor.Luminance = 1;

			RGB tmpColor1 = hslBackColor.ToRGB();

			hslBackColor.Luminance = defaultLum - lumRatio;
			if (hslBackColor.Luminance < 0) hslBackColor.Luminance = 0;

			RGB tmpColor2 = hslBackColor.ToRGB();

			RGB tmp =  (ContrastRatio(ref tmpColor1, ref backColor) > ContrastRatio(ref backColor, ref tmpColor2)) ?
				tmpColor1 :
				tmpColor2;
			//MessageBox.Show($"BackColor: ({backColor.Red},{backColor.Green},{backColor.Blue})\n" +
			//	$"Color1: ({tmpColor1.Red}, {tmpColor1.Green},{tmpColor1.Blue}) ratio -> {ContrastRatio(ref tmpColor1, ref backColor).ToString()}" +
			//	$"Color2: ({tmpColor2.Red}, {tmpColor2.Green},{tmpColor2.Blue}) ratio -> {ContrastRatio(ref backColor, ref tmpColor2).ToString()}");
			return tmp;
		}

		public bool IsPinned
		{
			get { return TopMost; }
		}

        /// <summary>
        /// Paints this note to a specific color
        /// </summary>
        /// <param name="color"></param>
		public void PaintNote(Color color)
		{
			float percent = 0.1f;
			float inversion = 0.4f;
			float textInversion = 0.7f;
			// Textbox background color
			RGB textBoxColor = new RGB(color);
			
			// Make topbar background color
			HSL hslTopBarColor = HSL.FromRGB(textBoxColor);
			hslTopBarColor.Luminance += (hslTopBarColor.Luminance + percent > 1) ? -percent : percent;
			RGB rgbTopBarColor = hslTopBarColor.ToRGB();

			// Make text color
			HSL hslTextBoxTextColor = HSL.FromRGB(textBoxColor);

			RGB rgbTextBoxTextColor = ChooseBetterContrastByLuminance(ref textBoxColor, textInversion);

			// Make topbar icon color
			RGB rgbTopbarTextColor = ChooseBetterContrastByLuminance(ref rgbTopBarColor, inversion);
			// Backgrund Colors
			BackColor = rgbTopBarColor.Color;
			topbar.BackColor = rgbTopBarColor.Color;
			textBox.BackColor = color;
			textBoxPanel.BackColor = color;

			// Top bar Text/Icons
			addNoteBtn.Image = new Bitmap(addNoteBtn.Image).ColorTint(rgbTopbarTextColor.Color);
			
			moreBtn.Image = new Bitmap(moreBtn.Image).ColorTint(rgbTopbarTextColor.Color);
			
			closeBtn.Image = new Bitmap(closeBtn.Image).ColorTint(rgbTopbarTextColor.Color);

			fontBtn.Image = new Bitmap(fontBtn.Image).ColorTint(rgbTopbarTextColor.Color);

			pinBtnImage = new Bitmap(pinBtnImage).ColorTint(rgbTopbarTextColor.Color);
			pinnedBtnImage = new Bitmap(pinnedBtnImage).ColorTint(rgbTopbarTextColor.Color);
			pinBtn.Image = (IsPinned) ? pinnedBtnImage : pinBtnImage;


			// TextBox  TEXT Color
			textBox.ForeColor = rgbTextBoxTextColor.Color;

		}


		private void NoteForm_Shown(object sender, EventArgs e)
		{
            //if (ndata == null)
            //{
            //    Random r = new Random(DateTime.Now.Millisecond);
            //    int rgb = Math.Abs(r.Next(0, 0x1000000));
            //    PaintNote(Color.FromArgb((rgb & 0xFF0000) >> 16, (rgb & 0xFF00) >> 8, rgb & 0xFF)); // Create and paint our note randomly
            //}
        }

		private void textBox_TextChanged(object sender, EventArgs e)
		{
			
		}

		private void panel1_Paint(object sender, PaintEventArgs e)
		{

		}

		private void textBox_TextChanged_1(object sender, EventArgs e)
		{
			Changed = true;
		}

		private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
		{

		}

		private void button1_Click_1(object sender, EventArgs e)
		{

		}

		private void button2_Click_1(object sender, EventArgs e)
		{
		}

		public void UnpinNote()
		{
			TopMost = false;
			pinBtn.Image = pinBtnImage;
			Owner = StartupForm.Instance;
			Owner.BringToFront();
		}
		public void PinNote()
		{
			TopMost = true;
			pinBtn.Image = pinnedBtnImage;
			Owner = null;
		}
		private void pinBtn_Click(object sender, EventArgs e)
		{
			if(IsPinned)
			{
				UnpinNote();
			}
			else
			{
				PinNote();
			}
			Changed = true;
		}

		private void fontBtn_Click(object sender, EventArgs e)
		{
			fontPicker.Font = textBox.SelectionFont;
			if(DialogResult.OK == fontPicker.ShowDialog())
			{
				if (textBox.SelectionLength == 0)
					textBox.Font = fontPicker.Font;
				else
					textBox.SelectionFont = fontPicker.Font;
				Changed = true;
			}
		}
        #region RightClickFunctions
        private void boldToolStripMenuItem_Click(object sender, EventArgs e)
		{
			textBox.BoldSelection();
		}

		private void copyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			textBox.Copy();
		}

		private void cutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			textBox.Cut();
		}

		private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			textBox.Paste();
		}

		private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SendKeys.Send("{DELETE}");
		}

		private void undoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SendKeys.Send("^Z");
		}

		private void redoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SendKeys.Send("^Y");
		}

		private void italicToolStripMenuItem_Click(object sender, EventArgs e)
		{
			textBox.ItalicSelection();
		}

		private void strikeoutCtrlSToolStripMenuItem_Click(object sender, EventArgs e)
		{
			textBox.StrikeoutSelection();
		}

		private void underlineCtrlUToolStripMenuItem_Click(object sender, EventArgs e)
		{
			textBox.UnderlineSelection();
		}

		private void zoomInCtrlToolStripMenuItem_Click(object sender, EventArgs e)
		{
			textBox.ZoomInSelection();
		}

		private void zoomOutCtrlToolStripMenuItem_Click(object sender, EventArgs e)
		{
			textBox.ZoomOutSelection();
		}

		private void selectAllCtrlAToolStripMenuItem_Click(object sender, EventArgs e)
		{
			textBox.SelectAll();
		}

		private void alignLeftCtrlJToolStripMenuItem_Click(object sender, EventArgs e)
		{
			textBox.SelectionAlignment = HorizontalAlignment.Left;
		}

		private void alignCenterCtrlWToolStripMenuItem_Click(object sender, EventArgs e)
		{
			textBox.SelectionAlignment = HorizontalAlignment.Center;
		}

		private void alignRightCtrlRToolStripMenuItem_Click(object sender, EventArgs e)
		{
			textBox.SelectionAlignment = HorizontalAlignment.Right;
		}
        #endregion

        private void NoteForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ForgetDataWhenClosing) return;
            switch (e.CloseReason)
            {
                case CloseReason.None:
                    // Idk
                    throw new NotSupportedException("Close reason: None");
                case CloseReason.WindowsShutDown:
                    // Close without notifying 
                    break;
                case CloseReason.MdiFormClosing:
                    // Never should happen
                    throw new NotSupportedException("Close reason: MdiFormClosing");
                case CloseReason.UserClosing:
                    // Ask confirmation
                    if (!CloseDialog || textBox.Text == "" || DialogResult.Yes == MessageBox.Show("Are you sure you want to delete this note?", "Deleting note", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        RemoveNoteNoDialog();
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                    break;
                case CloseReason.TaskManagerClosing:
                    // Close without notifying
                    break;
                case CloseReason.FormOwnerClosing:
                    // Close without notifying
                    break;
                case CloseReason.ApplicationExitCall:
                    // Close without notifying
                    break;
            }
        }

        /// <summary>
        /// Removes the note as normally but it doesn't ask if you really want to delete it
        /// </summary>
        void RemoveNoteNoDialog()
        {
            Program.availableForms.Remove(thisNode);
            StartupForm.dataSaved = false; // Let father know there are changes
            if (CloseWithTheLastNoteOpened && Program.availableForms.Count == 0)
            {
                Application.Exit();
            }
        }
    }
}
