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
using ColorManager;

namespace FastNotes
{
    public partial class NoteForm : Form
    {
        #region Private Members

        ColorPicker colorPicker;

        Bitmap pinBtnImage = global::FastNotes.Properties.Resources.fixedIcons_0006_pin;

        Bitmap pinnedBtnImage = global::FastNotes.Properties.Resources.fixedIcons_0005_pinned;
     
        /// <summary>
        /// Reference to the node in the group of notes declared in Program.availableForms
        /// </summary>
        LinkedListNode<Form> thisNode;
      
        /// <summary>
        /// Current data
        /// </summary>
        NoteData ndata = null;

        /// <summary>
        ///  Let outside classes know when this note hasn't saved its data
        /// </summary>
        bool changed = false;

        #endregion

        #region Protected Members

        #endregion

        #region Public Members

        /// <summary>
        /// Happens when the note is created just after it is painted randomly
        /// </summary>
        public event Action OnAfterRandomlyPainted;

        /// <summary>
        /// Happens when the data is created for the first time (when ndata is null and stops being null)
        /// </summary>
        public event Action OnAfterDataCreated;

        /// <summary>
        /// Tells how dark (or light) the colors of the note's text will be of the background
        /// </summary>
        public float TextInverseRelation { set; get; } = 0.3f;
        
        /// <summary>
        /// Tells how dark (or light) the colors of the icons on the top bar will be of the topbar
        /// </summary>
        public float IconInverseRelation { set; get; } = 0.3f;

        /// <summary>
        /// Tells how dark (or light) the colors of the topbar will be of the background
        /// </summary>
        public float TopBarInverseRelation { set; get; } = 0.1f;

        /// <summary>
        /// Returns whether this note is pinned or not
        /// </summary>
        public bool IsPinned
        {
            get { return TopMost; }
        }

        /// <summary>
        /// Leaves a mark that tells this note data hasn't been saved
        /// </summary>
        public bool Changed
        {
            get { return changed; }
            set
            {
                changed = value;
                if (changed)
                    StartupForm.DataSaved = false;
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
                bool dataCreated = false;
                if (ndata == null)
                {
                    ndata = new NoteData();
                    dataCreated = true;
                }
                ndata.mainColor = textBox.BackColor;
                ndata.noteSize = new Size(Width, Height);
                ndata.noteLocation = Location;
                ndata.rtbInfo = new NoteData.RichTextBox();
                ndata.rtbInfo.font = textBox.Font;
                ndata.rtbInfo.rtf = textBox.Rtf;
                ndata.rtbInfo.zoomFactor = textBox.ZoomFactor;
                ndata.pinned = IsPinned;
                if (dataCreated) OnAfterDataCreated?.Invoke(); // The data was just created
                return ndata;
            }
            set
            {
                ndata = value;
                if (value != null)
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
        
        #endregion

        #region Constructor

        public NoteForm(LinkedListNode<Form> thisNode)
        {
            this.thisNode = thisNode; // Save a reference of the node in the current notes list
            DoubleBuffered = true; // Helps flickering although I think here does nothing, it was solved when inheriting Panel class and doing it there
            InitializeComponent();
            try
            {
                textBox.Font = new Font(BuiltInFonts.Fonts.Quesha, 24f, FontStyle.Regular, GraphicsUnit.Pixel);

            }
            catch { }
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
                    OnAfterRandomlyPainted?.Invoke();
                }
            };
            
        }

        #endregion

        #region Private Methods
        
        /// <summary>
        /// Implements when it's wanted to change the color of a note
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNoteColorChangeClick(object sender, EventArgs e)
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

        /// <summary>
        /// Implements when a note is created
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCreateNoteBtnClick(object sender, EventArgs e)
        {
            var noteform = (NoteForm) Program.CreateNote();
            noteform.OnAfterRandomlyPainted += () =>
            {
                noteform.Width = this.Width;
                noteform.Height = this.Height;
                noteform.textBox.Font = this.textBox.Font;
                noteform.textBox.ZoomFactor = this.textBox.ZoomFactor;
                if (this.IsPinned) noteform.PinNote();
            };
            noteform.Show(Program.startupForm);

            StartupForm.DataSaved = false; // Let father know there are changes
        }

        /// <summary>
        /// Happens when touching the note topbar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTouchBarPressed(object sender, MouseEventArgs e)
		{
			TopBarActions.MouseDown(Handle, e); // This makes the top bar move the form
			Changed = true; // Data has changed
		}

        /// <summary>
        /// Clicking the erase note button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void OnEraseBtnClick(object sender, EventArgs e)
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

        /// <summary>
        /// Happens when the text of the note changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNoteContentChanged(object sender, EventArgs e)
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

        /// <summary>
        /// Happens when the pin button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPinBtnClick(object sender, EventArgs e)
        {
            if (IsPinned)
            {
                UnpinNote();
            }
            else
            {
                PinNote();
            }
            Changed = true;
        }

        /// <summary>
        /// Happens when the font button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFontBtnClick(object sender, EventArgs e)
        {
            fontPicker.Font = textBox.SelectionFont;
            if (DialogResult.OK == fontPicker.ShowDialog())
            {
                if (textBox.SelectionLength == 0)
                    textBox.Font = fontPicker.Font;
                else
                    textBox.SelectionFont = fontPicker.Font;
                Changed = true;
            }
        }

        /// <summary>
        /// Happens when someone wants to close this note
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNoteFormClosing(object sender, FormClosingEventArgs e)
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
        /// Removes the note as normally, it wont't ask if you really want to delete it
        /// </summary>
        void RemoveNoteNoDialog()
        {
            Program.availableForms.Remove(thisNode);
            StartupForm.DataSaved = false; // Let father know there are changes
            if (CloseWithTheLastNoteOpened && Program.availableForms.Count == 0)
            {
                Application.Exit();
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


        #endregion

        #region Protected Methods



        [DllImport("user32.dll")]
		protected static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);


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

        #endregion

        #region Public Methods
       
        /// <summary>
        /// Paints this note to a specific color
        /// </summary>
        /// <param name="color"></param>
		public void PaintNote(Color color)
		{
			// Textbox background color
			RGB textBoxColor = new RGB(color);
			
			// Make topbar background color
			HSL hslTopBarColor = new HSL(textBoxColor);
			hslTopBarColor.L += (hslTopBarColor.L + TopBarInverseRelation > 1) ? -TopBarInverseRelation : TopBarInverseRelation;
			RGB rgbTopBarColor = ColorManager.ColorManager.ChooseColorByContrastRatio(textBoxColor, TopBarInverseRelation);

            RGB rgbTextBoxTextColor = ColorManager.ColorManager.ChooseColorByContrastRatio(textBoxColor, TextInverseRelation);

			// Make topbar icon color
			RGB rgbTopbarTextColor = ColorManager.ColorManager.ChooseColorByContrastRatio(textBoxColor, IconInverseRelation);
			// Backgrund Colors
			BackColor = rgbTopBarColor.ToColor();
			topbar.BackColor = rgbTopBarColor.ToColor();
			textBox.BackColor = color;
			textBoxPanel.BackColor = color;

			// Top bar Text/Icons
			addNoteBtn.Image = new Bitmap(addNoteBtn.Image).ColorTint(rgbTopbarTextColor.ToColor());
			
			moreBtn.Image = new Bitmap(moreBtn.Image).ColorTint(rgbTopbarTextColor.ToColor());
			
			closeBtn.Image = new Bitmap(closeBtn.Image).ColorTint(rgbTopbarTextColor.ToColor());

			fontBtn.Image = new Bitmap(fontBtn.Image).ColorTint(rgbTopbarTextColor.ToColor());

			pinBtnImage = new Bitmap(pinBtnImage).ColorTint(rgbTopbarTextColor.ToColor());
			pinnedBtnImage = new Bitmap(pinnedBtnImage).ColorTint(rgbTopbarTextColor.ToColor());
			pinBtn.Image = (IsPinned) ? pinnedBtnImage : pinBtnImage;


			// TextBox  TEXT Color
			textBox.ForeColor = rgbTextBoxTextColor.ToColor();

		}

        /// <summary>
        /// Unpins the note and changes its button image
        /// </summary>
		public void UnpinNote()
		{
			TopMost = false;
			pinBtn.Image = pinBtnImage;
			Owner = StartupForm.Instance;
			Owner.BringToFront();
		}

        /// <summary>
        /// Pins the note and changes its button image
        /// </summary>
		public void PinNote()
		{
			TopMost = true;
			pinBtn.Image = pinnedBtnImage;
			Owner = null;
		}

        #endregion

    }
}
