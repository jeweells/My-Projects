using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Controls
{
    /// <summary>
    /// Lógica de interacción para FontPicker.xaml
    /// </summary>
    public partial class FontPicker : Window
    {

        WindowViewModel BaseModel;
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.Process)]
        public static extern IntPtr GetActiveWindow();


        public Controls.Models.FontSelectedInfo SelectedInfo;

        public FontPicker()
        {
            var wih = new WindowInteropHelper(this);
            wih.Owner = GetActiveWindow();
            InitializeComponent();
            this.DataContext = BaseModel = new WindowViewModel(this);
            SelectedInfo = TryFindResource("FontSelectedInfo") as Controls.Models.FontSelectedInfo;

        }

        private void OnSelectClick(object sender, RoutedEventArgs e)
        {
            try
            {
                DialogResult = true;
            }
            catch { }
        }
        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            try
            {
                DialogResult = false;
            }
            catch { }
        }
    }
}
