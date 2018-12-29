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
    /// Lógica de interacción para SystemFontList.xaml
    /// </summary>
    public partial class SystemFontList : SystemFontListBase
    {
        #region PrivateMembers

        #endregion


        public SystemFontList()
        {
            InitializeComponent();
            TheComboBox.SelectionChanged += FontSelectedChanged;

            //var installedFontCollection = new System.Drawing.Text.InstalledFontCollection();

            //// Get the array of FontFamily objects.
            //var fontFamilies = installedFontCollection.Families;
            //foreach (var fFamily in fontFamilies)
            //{
            //    var c = new ListBoxItem() { Content = fFamily.Name, FontFamily = new FontFamily("Quesha") };
            //    Items.Add(c);
            //}
        }

        private void FontSelectedChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var ff = ((sender as ComboBox).SelectedItem as FontFamily);
            }
            catch
            {
                MessageBox.Show("An unexpected error has occurred");
            }
        }
    }
}
