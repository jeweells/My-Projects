using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace FastNotes
{
	[Serializable]
	public class NoteData
	{
		public int noteId;
		public Color mainColor;
		[Serializable]
		public class RichTextBox
		{
			public string rtf;
			public float zoomFactor;
			public Font font;

		}
		public RichTextBox rtbInfo;
		public Point noteLocation;
		public Size noteSize;
		public bool pinned;
	}

    [Serializable]
    public class NoteData_1_0_1_4
    {
        public double TopPosition = 100;
        public double LeftPosition = 100;
        public System.Drawing.Color MainColor;
        public byte[] RtfData;
        public double ZoomFactor = 1;
        public double NoteWidth = 300;
        public double NoteHeight = 400;
        public bool IsPinned;
        public NoteData_1_0_1_4()
        {

        }
        public NoteData_1_0_1_4(NoteData olderData)
        {
            if (olderData == null) return;
            TopPosition = olderData.noteLocation == null? 100 : olderData.noteLocation.Y;
            LeftPosition = olderData.noteLocation == null? 100 : olderData.noteLocation.X;
            MainColor = olderData.mainColor;
            NoteWidth = olderData.noteSize == null? 300 : olderData.noteSize.Width;
            NoteHeight = olderData.noteSize == null? 400 : olderData.noteSize.Height;
            IsPinned = olderData.pinned;
            ZoomFactor = olderData.rtbInfo!= null? olderData.rtbInfo.zoomFactor : 1;
            var mems = new MemoryStream(ASCIIEncoding.Default.GetBytes(olderData.rtbInfo.rtf)); // Load the RTF text
            var document = new FlowDocument(); // Create this to apply styles to the RTF text
            var tr = new TextRange(document.ContentStart, document.ContentEnd);
            tr.Load(mems, System.Windows.DataFormats.Rtf);

            #region Applying new font values
            //try
            //{
            //    tr.ApplyPropertyValue(Control.FontFamilyProperty, new System.Windows.Media.FontFamily(olderData.rtbInfo.font.Name));
            //}
            //catch { }
            //try
            //{
            //       tr.ApplyPropertyValue(Control.FontSizeProperty, olderData.rtbInfo.font.Size * 96.0 / 72.0);
            //}
            //catch { }

            //try
            //{
            //    var weight = olderData.rtbInfo.font.Bold ? System.Windows.FontWeights.Bold : System.Windows.FontWeights.Normal;
            //    tr.ApplyPropertyValue(Control.FontWeightProperty, weight);
            //}
            //catch { }

            //try
            //{
            //    var style = olderData.rtbInfo.font.Italic? System.Windows.FontStyles.Italic : System.Windows.FontStyles.Normal;
            //    tr.ApplyPropertyValue(Control.FontStyleProperty, style);
            //}
            //catch { }

            //try
            //{
            //    var textDecorations = new System.Windows.TextDecorationCollection();
            //    if (olderData.rtbInfo.font.Underline) textDecorations.Add(System.Windows.TextDecorations.Underline);
            //    if (olderData.rtbInfo.font.Strikeout) textDecorations.Add(System.Windows.TextDecorations.Strikethrough);
            //    tr.ApplyPropertyValue(Inline.TextDecorationsProperty, textDecorations);
            //}
            //catch { }
            #endregion

            var memsOut = new MemoryStream();
            tr.Save(memsOut, System.Windows.DataFormats.Rtf);

            RtfData = memsOut.ToArray();

        }
    }
}
