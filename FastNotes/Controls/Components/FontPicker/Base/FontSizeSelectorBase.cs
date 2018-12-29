using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Controls.Components.FontPicker
{
    public class FontSizeSelectorBase : ComboBox
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
            DependencyProperty.Register("SelectedFontFamily", typeof(string), typeof(FontSizeSelectorBase), new PropertyMetadata(null));




        public double SelectedFontSize
        {
            get { return (double)GetValue(SelectedFontSizeProperty); }
            set { SetValue(SelectedFontSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedFontSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedFontSizeProperty =
            DependencyProperty.Register("SelectedFontSize", typeof(double), typeof(FontSizeSelectorBase), new PropertyMetadata(14.0));




        #endregion


        #region Public Methods
        #endregion
    }
}
