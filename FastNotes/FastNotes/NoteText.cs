using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FastNotes
{
	public class NoteText : System.Windows.Forms.RichTextBox
	{
		public NoteText() : base()
		{
			try
			{
				Font = new Font(StartupForm.Instance.novaSquare, 12);
			}
			catch
			{

			}
			LinkClicked += NoteText_LinkClicked;
			KeyDown += NoteText_KeyDown;
			KeyPress += NoteText_KeyPress;
		}

		private void NoteText_KeyPress(object sender, KeyPressEventArgs e)
		{

		}
	
		private void NoteText_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.Control)
			{
				if (e.KeyCode == Keys.B)
				{
					BoldSelection();
				}
				else if (e.KeyCode == Keys.U)
				{
					UnderlineSelection();
				}
				else if (e.KeyCode == Keys.S)
				{
					StrikeoutSelection();
				}
				else if (e.KeyCode == Keys.N)
				{
					ItalicSelection();
				}
				else if (e.KeyCode == Keys.Add)
				{
					ZoomInSelection();
				}
				else if (e.KeyCode == Keys.Subtract)
				{
					ZoomOutSelection();
				}
			}
		}
		void ZoomSelectionAux(int size)
		{
			CHARFORMAT fmt = new CHARFORMAT();
			fmt.cbSize = Marshal.SizeOf(fmt);
			fmt.dwMask = CFM_SIZE;
			fmt.yHeight = Convert.ToInt32(size);
			SetCharFormatMessage(ref fmt);
		}
		public void ZoomOutSelection()
		{
			if (SelectionFont != null)
			{
				ZoomSelectionAux(Convert.ToInt32((SelectionFont.Size - 1f) * 20));
				//SelectionFont = new Font(SelectionFont.FontFamily, SelectionFont.Size - 1f, SelectionFont.Style);
			}
			else
			{
				DrawingControl.SuspendDrawing(this);
				int start = SelectionStart;
				int length = SelectionLength;
				Select(start, 1);
				float size = SelectionFont.Size;
				Select(start, length);
				ZoomSelectionAux(Convert.ToInt32((size - 1f) * 20));
				DrawingControl.ResumeDrawing(this);
			}
		}

		public void ZoomInSelection()
		{
			if (SelectionFont != null)
			{
				ZoomSelectionAux(Convert.ToInt32((SelectionFont.Size + 1f) * 20));
				//SelectionFont = new Font(SelectionFont.FontFamily, SelectionFont.Size - 1f, SelectionFont.Style);
			}
			else
			{
				DrawingControl.SuspendDrawing(this);
				int start = SelectionStart;
				int length = SelectionLength;
				Select(start, 1);
				float size = SelectionFont.Size;
				Select(start, length);
				ZoomSelectionAux(Convert.ToInt32((size + 1f) * 20));
				DrawingControl.ResumeDrawing(this);
			}
		}

		public void ItalicSelection()
		{
			FontStyle fs;
			if (SelectionFont != null)
			{
				fs = SelectionFont.Style ^ FontStyle.Italic;
				//	SelectionFont = new Font(SelectionFont.FontFamily, SelectionFont.Size, fs);
			}
			else
			{
				DrawingControl.SuspendDrawing(this);
				int start = SelectionStart;
				int length = SelectionLength;
				Select(start, 1);
				fs = SelectionFont.Style ^ FontStyle.Italic;
				Select(start, length);
				DrawingControl.ResumeDrawing(this);
			}
			ApplyStyle(CFM_ITALIC, fs.HasFlag(FontStyle.Italic));
		}

		public void StrikeoutSelection()
		{
			FontStyle fs;
			if (SelectionFont != null)
			{
				fs = SelectionFont.Style ^ FontStyle.Strikeout;
				//	SelectionFont = new Font(SelectionFont.FontFamily, SelectionFont.Size, fs);
			}
			else
			{
				DrawingControl.SuspendDrawing(this);
				int start = SelectionStart;
				int length = SelectionLength;
				Select(start, 1);
				fs = SelectionFont.Style ^ FontStyle.Strikeout;
				Select(start, length);
				DrawingControl.ResumeDrawing(this);
			}
			ApplyStyle(CFM_STRIKE, fs.HasFlag(FontStyle.Strikeout));
		}

		public void BoldSelection()
		{
			FontStyle fs;
			if (SelectionFont != null)
			{
				fs = SelectionFont.Style ^ FontStyle.Bold;
				//	SelectionFont = new Font(SelectionFont.FontFamily, SelectionFont.Size, fs);
			}
			else
			{
				DrawingControl.SuspendDrawing(this);
				int start = SelectionStart;
				int length = SelectionLength;
				Select(start, 1);
				fs = SelectionFont.Style ^ FontStyle.Bold;
				Select(start, length);
				DrawingControl.ResumeDrawing(this);
			}
			ApplyStyle(CFM_BOLD, fs.HasFlag(FontStyle.Bold));
		}
		public void UnderlineSelection()
		{

			FontStyle fs;
			if (SelectionFont != null)
			{
				fs = SelectionFont.Style ^ FontStyle.Underline;
				//	SelectionFont = new Font(SelectionFont.FontFamily, SelectionFont.Size, fs);
			}
			else
			{
				DrawingControl.SuspendDrawing(this);
				int start = SelectionStart;
				int length = SelectionLength;
				Select(start, 1);
				fs = SelectionFont.Style ^ FontStyle.Underline;
				Select(start, length);
				DrawingControl.ResumeDrawing(this);
			}
			ApplyStyle(CFM_UNDERLINE, fs.HasFlag(FontStyle.Underline));
		}

		private void NoteText_LinkClicked(object sender, System.Windows.Forms.LinkClickedEventArgs e)
		{
			
			System.Diagnostics.Process.Start(e.LinkText);
		}



		private const int EM_SETCHARFORMAT = 1092;

		private const UInt32 CFM_BOLD = 0x00000001;
		private const UInt32 CFM_ITALIC = 0x00000002;
		private const UInt32 CFM_UNDERLINE = 0x00000004;
		private const UInt32 CFM_STRIKE = 0x00000008;
		private const UInt32 CFM_FACE = 0x20000000;
		private const UInt32 CFM_SIZE = 0x80000000;
		private const int SCF_SELECTION = 1;

		[StructLayout(LayoutKind.Sequential)]
		private struct CHARFORMAT
		{
			public int cbSize;
			public uint dwMask;
			public uint dwEffects;
			public int yHeight;
			public int yOffset;
			public int crTextColor;
			public byte bCharSet;
			public byte bPitchAndFamily;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
			public char[] szFaceName;

			// CHARFORMAT2 from here onwards.
			public short wWeight;
			public short sSpacing;
			public int crBackColor;
			public int LCID;
			public uint dwReserved;
			public short sStyle;
			public short wKerning;
			public byte bUnderlineType;
			public byte bAnimation;
			public byte bRevAuthor;
		}

		[DllImport("user32", CharSet = CharSet.Auto)]
		private static extern int SendMessage(HandleRef hWnd,
				int msg, int wParam, ref CHARFORMAT lp);

		private void SetCharFormatMessage(ref CHARFORMAT fmt)
		{
			SendMessage(new HandleRef(this, Handle),
				  EM_SETCHARFORMAT, SCF_SELECTION, ref fmt);
		}

		private void ApplyStyle(uint style, bool on)
		{

			CHARFORMAT fmt = new CHARFORMAT();
			fmt.cbSize = Marshal.SizeOf(fmt);
			fmt.dwMask = style;
			if (on)
				fmt.dwEffects = style;
			SetCharFormatMessage(ref fmt);
		}

		
	}
}
