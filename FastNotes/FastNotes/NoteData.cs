using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
}
