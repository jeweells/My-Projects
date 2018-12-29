using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace FontDialog.Models
{
    public class FontSelectedInfo : INotifyPropertyChanged
    {
        #region Private Members
        private string _SelectedFontFamily = null;
        private double _SelectedFontSize;
        private bool _Bold;
        private bool _Underline;
        private bool _Italic;
        private bool _Strikethrough;
        private FontSelectedInfo startState;
        #endregion

        #region Public Members

        public event Action FontFeatureChanged = () => { };

        public string SelectedFontFamily
        {
            get { return _SelectedFontFamily; }
            set {
                _SelectedFontFamily = value;
                NotifyPropertyChanged(nameof(SelectedFontFamily));
                FontFeatureChanged();
            }
        }
        public double SelectedFontSize
        {
            get { return _SelectedFontSize; }
            set {
                _SelectedFontSize = value;
                NotifyPropertyChanged(nameof(SelectedFontSize));
                FontFeatureChanged();
            }
        }


        public bool Bold
        {
            get { return _Bold; }
            set { _Bold = value;
                NotifyPropertyChanged(nameof(Bold));
                NotifyPropertyChanged(nameof(SelectedFontWeight));
                FontFeatureChanged();
            }
        }

        public bool Italic
        {
            get { return _Italic; }
            set { _Italic = value;
                NotifyPropertyChanged(nameof(Italic));
                NotifyPropertyChanged(nameof(SelectedFontStyle));
                FontFeatureChanged();
            }
        }
        public bool Underline
        {
            get { return _Underline; }
            set { _Underline = value;
                NotifyPropertyChanged(nameof(Underline));
                NotifyPropertyChanged(nameof(SelectedTextDecorations));
                FontFeatureChanged();
            }
        }
        public bool Strikethrough
        {
            get { return _Strikethrough; }
            set { _Strikethrough = value;
                NotifyPropertyChanged(nameof(Strikethrough));
                NotifyPropertyChanged(nameof(SelectedTextDecorations));
                FontFeatureChanged();
            }
        }

        public FontStyle SelectedFontStyle
        {
            get {
                return Italic ? FontStyles.Italic : FontStyles.Normal;
            }
        }

        public FontWeight SelectedFontWeight
        {
            get
            {
                return Bold ? FontWeights.Bold : FontWeights.Normal;
            }
        }

        public TextDecorationCollection SelectedTextDecorations
        {
            get
            {
                var tds = new TextDecorationCollection();
                if(Underline) tds.Add(TextDecorations.Underline);
                if(Strikethrough) tds.Add(TextDecorations.Strikethrough);
                return tds;
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constructor
        public FontSelectedInfo()
        {
            SelectedFontSize = 14;
        }
        #endregion

        #region Private Methods
        void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Public Methods
        
        /// <summary>
        /// Applies the changes made in this object to textRange
        /// </summary>
        /// <param name="textRange"></param>
        public void ApplyOn(TextRange textRange)
        {
            if(SelectedFontFamily != null)
                textRange.ApplyPropertyValue(Control.FontFamilyProperty, new System.Windows.Media.FontFamily(SelectedFontFamily));
            textRange.ApplyPropertyValue(Control.FontSizeProperty, SelectedFontSize);
            textRange.ApplyPropertyValue(Control.FontWeightProperty, SelectedFontWeight);
            textRange.ApplyPropertyValue(Control.FontStyleProperty, SelectedFontStyle);
            textRange.ApplyPropertyValue(Inline.TextDecorationsProperty, SelectedTextDecorations);
        }



        public void ApplyOn(TextPointer contentStart, TextPointer contentEnd)
        {
            ApplyOn(new TextRange(contentStart, contentEnd));
        }

        public void InheritFrom(TextPointer contentStart, TextPointer contentEnd)
        {
            InheritFrom(new TextRange(contentStart, contentEnd));
        }

        /// <summary>
        /// Extracts the needed data from a textRange that is usually provided by containers such as text boxes
        /// </summary>
        /// <param name="textRange"></param>
        public void InheritFrom(TextRange textRange)
        {
            var fontFamilyProp = textRange.GetPropertyValue(Control.FontFamilyProperty);
            var fontSizeProp = textRange.GetPropertyValue(Control.FontSizeProperty);
            var fontWeightProp = textRange.GetPropertyValue(Control.FontWeightProperty);
            var fontStyleProp = textRange.GetPropertyValue(Control.FontStyleProperty);
            var fontTextDecorationsProp = textRange.GetPropertyValue(Inline.TextDecorationsProperty);

            try
            {
                if (fontFamilyProp != DependencyProperty.UnsetValue) SelectedFontFamily = (fontFamilyProp as System.Windows.Media.FontFamily).Source;
                if (fontSizeProp != DependencyProperty.UnsetValue) SelectedFontSize = (double)fontSizeProp;
                if (fontWeightProp != DependencyProperty.UnsetValue)
                {
                    var fontweight = (FontWeight)fontWeightProp;
                    Bold = fontweight.Equals(FontWeights.Bold);
                }
                if (fontStyleProp != DependencyProperty.UnsetValue) Italic = ((FontStyle)fontStyleProp).Equals(FontStyles.Italic);
                if (fontTextDecorationsProp != DependencyProperty.UnsetValue)
                {
                    var tmpDecorations = (TextDecorationCollection)fontTextDecorationsProp;
                    foreach (var td in tmpDecorations)
                    {
                        if (td.Location == TextDecorationLocation.Underline) Underline = true;
                        else if (td.Location == TextDecorationLocation.Strikethrough) Strikethrough = true;
                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            startState = new FontSelectedInfo();
            startState.SelectedFontFamily = SelectedFontFamily;
            startState.SelectedFontSize = SelectedFontSize;
            startState.Bold = Bold;
            startState.Underline = Underline;
            startState.Italic = Italic;
            startState.Strikethrough = Strikethrough;

        }

        /// <summary>
        /// Restores all the data to the state when the data was inherited
        /// </summary>
        public void Restore()
        {
            if(startState != null)
            {
                SelectedFontFamily = startState.SelectedFontFamily;
                SelectedFontSize = startState.SelectedFontSize;
                Bold = startState.Bold;
                Underline = startState.Underline;
                Italic = startState.Italic;
                Strikethrough = startState.Strikethrough;
            }
        }
        #endregion

    }
}
