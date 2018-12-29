using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FontDialog
{
    public class SystemFontListBase : UserControl
    {
        #region Private Members

        #endregion




        #region Public Members

        public string SelectedFontFamily
        {
            get { return (string)GetValue(SelectedFontFamilyProperty); }
            set { SetValue(SelectedFontFamilyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedFontFamily.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedFontFamilyProperty =
            DependencyProperty.Register("SelectedFontFamily", typeof(string), typeof(SystemFontListBase), new PropertyMetadata("Quesha"));

        #endregion

        #region Constructor
        public SystemFontListBase()
        {

        }
        #endregion


        #region Public Methods
        #endregion
    }
}
