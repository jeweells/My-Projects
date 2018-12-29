using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FontDialog
{
    /// <summary>
    /// Lógica de interacción para FontSizeSelector.xaml
    /// </summary>
    public partial class FontSizeSelector : FontSizeSelectorBase
    {
        #region Private Members
        #endregion

        public FontSizeSelector()
        {
            InitializeComponent();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            string validstr = "";
            string tmpText = Text;
            bool dotFound = false;

            foreach (var c in tmpText)
            {

                if (!dotFound && (c == '.' || c== ','))
                {
                    dotFound = true;
                    validstr += ".";
                }
                else if(char.IsNumber(c))
                {
                    validstr += c;
                }
            }
            if (validstr == "")
            {
                
                Text = "14";
                SelectedIndex = 5;
            }
            else
            {
                Text = validstr;
                int i = 0;
                foreach (TextBlock item in Items)
                {
                    if(item.Text == Text)
                    {
                        SelectedIndex = i;
                        break;
                    }
                    i++;
                }
            }
            
        }
    }
}
