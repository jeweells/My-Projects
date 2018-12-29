using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FastNotes
{
	public class Config
	{
		public bool runAtStartup = true;
        public int selectedDesktop = 0;
        public List<Desktop> desktops;
        public string lastVersionChecked = Application.ProductVersion;
        public bool automaticallyHideOnBar = false;
        public bool askWhenClosingANote = true;
	}
}
