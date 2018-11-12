using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FastNotes
{
    public partial class RenameDialog : Form
    {
        public RenameDialog()
        {
            InitializeComponent();
            Shown += (x, y) => newNameTextBox.SelectAll();
        }
        public class EventArgs
        {
            public bool Handled = false;
            public string Input;
        }
        public event Action<EventArgs> OnAccepting;

        public DialogResult ShowDialog(IWin32Window owner, out string newName)
        {
            DialogResult dr = ShowDialog(owner);
            newName = newNameTextBox.Text;
            return dr;
        }

        private void label1_Click(object sender, System.EventArgs e)
        {
            
        }

        public void acceptBtn_Click(object sender, System.EventArgs e)
        {
            EventArgs ea = new EventArgs();
            ea.Input = newNameTextBox.Text;
            OnAccepting?.Invoke(ea);
            if (!ea.Handled)
            {
                DialogResult = DialogResult.OK;
            }
        }

        public void cancelBtn_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void newNameTextBox_TextChanged(object sender, System.EventArgs e)
        {

        }

        private void newNameTextBox_Click(object sender, System.EventArgs e)
        {
        }
    }
}
