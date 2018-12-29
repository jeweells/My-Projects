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

namespace FastNotes
{
    /// <summary>
    /// Lógica de interacción para FNToggleButton.xaml
    /// </summary>
    public partial class FNToggleButton : FNToggleButtonBase
    {
        public FNToggleButton()
        {
            InitializeComponent();
        
            Loaded += (a,b)=>
            {
                if (ToggleValue)
                {
                    Foreground = new SolidColorBrush(PressedForeColor);
                    Background = new SolidColorBrush(PressedColor);

                }
            };
        }
    }
}
