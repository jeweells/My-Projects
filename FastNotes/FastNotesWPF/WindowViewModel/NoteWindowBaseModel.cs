using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FastNotes
{
    public class NoteWindowBaseModel : INotifyPropertyChanged
    {
        #region Private Members

        Visibility _PinVisibility = Visibility.Visible;
        System.Windows.Media.Brush _MainBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 153, 255));
        System.Windows.Media.Brush _DarkMainBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 102, 255));
        System.Windows.Media.Brush _DarkTextBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
        System.Windows.Media.Brush _TextBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
        System.Windows.Media.Brush _InverseTextBrushAZero = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 0, 0, 0));
        double _ZoomFactor = 1;

        #endregion


        #region Public Members

        public event Action ZoomFactorChanged;

        public double ZoomFactor
        {
            get { return _ZoomFactor; }
            set
            {
                if (value < 0.25 || value > 9) return;
                _ZoomFactor = value;
                OnPropertyChanged(nameof(ZoomFactor));
                ZoomFactorChanged?.Invoke();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Changes the pin visibility.
        /// If this is visible, automatically the unpin visibility will be collapsed.
        /// If this is collapsed, automatically the unpin visibility will be visible.
        /// </summary>
        public Visibility PinVisibility {
            set
            {
                _PinVisibility = value;
                OnPropertyChanged(nameof(PinVisibility));
                OnPropertyChanged(nameof(UnpinVisibility));
            }
            get
            {
                return _PinVisibility;
            }
        }

        /// <summary>
        /// Changes the unpin visibility.
        /// If this is visible, automatically the pin visibility will be collapsed.
        /// If this is collapsed, automatically the pin visibility will be visible.
        /// </summary>
        public Visibility UnpinVisibility
        {
            set
            {
                _PinVisibility = value ^ Visibility.Collapsed;
                OnPropertyChanged(nameof(PinVisibility));
                OnPropertyChanged(nameof(UnpinVisibility));
            }
            get
            {
                return _PinVisibility ^ Visibility.Collapsed;
            }
        }

        private void OnPropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }


        /// <summary>
        /// Color which the rest of the colors are based
        /// </summary>
        public System.Windows.Media.Color MainColor
        {
            get
            {
                return (_MainBrush as System.Windows.Media.SolidColorBrush).Color;
            }

            set
            {
                _MainBrush = new System.Windows.Media.SolidColorBrush(value);
                OnPropertyChanged(nameof(MainBrush));
                OnPropertyChanged(nameof(MainColor));
            }
        }


        /// <summary>
        /// Color which the rest of the colors are based
        /// </summary>
        public System.Windows.Media.Brush MainBrush
        {
            get
            {
                return _MainBrush;
            }
            set
            {
                _MainBrush = value;
                OnPropertyChanged(nameof(MainBrush));
                OnPropertyChanged(nameof(MainColor));
            }

        }

        /// <summary>
        /// This color is based on MainColor, a little bit more dark than MainColor
        /// </summary>
        public System.Windows.Media.Brush DarkMainBrush
        {
            get
            {
                return _DarkMainBrush;
            }
            set
            {
                _DarkMainBrush = value;
                OnPropertyChanged(nameof(DarkMainBrush));
                OnPropertyChanged(nameof(DarkMainColor));
            }
        }


        /// <summary>
        /// This color is based on MainColor, a little bit more dark than MainColor
        /// </summary>
        public System.Windows.Media.Color DarkMainColor
        {
            get
            {
                return (_DarkMainBrush as System.Windows.Media.SolidColorBrush).Color;
            }

            set
            {
                _DarkMainBrush = new System.Windows.Media.SolidColorBrush(value);
                OnPropertyChanged(nameof(DarkMainBrush));
                OnPropertyChanged(nameof(DarkMainColor));
            }
        }

        /// <summary>
        /// Color that can be noticed when MainColor is the background
        /// </summary>
        public System.Windows.Media.Brush TextBrush
        {
            get
            {
                return _TextBrush;
            }
            set
            {
                _TextBrush = value;
                try
                {
                    var brushColor = ((System.Windows.Media.SolidColorBrush)value).Color;
                    _InverseTextBrushAZero = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(0, (byte)(255-brushColor.R), (byte)(255-brushColor.G), (byte) (255-brushColor.B)));
                }
                catch { }
                OnPropertyChanged(nameof(TextColor));
                OnPropertyChanged(nameof(TextBrush));
                OnPropertyChanged(nameof(InverseTextBrushAZero));
            }

        }


        /// <summary>
        /// Color that can be noticed when MainColor is the background (with alpha 0)
        /// </summary>
        public System.Windows.Media.Brush InverseTextBrushAZero
        {
            get
            {
                return _InverseTextBrushAZero;
            }

        }



        /// <summary>
        /// Color that can be noticed when MainColor is the background
        /// </summary>
        public System.Windows.Media.Color TextColor
        {
            get
            {
                return (_TextBrush as System.Windows.Media.SolidColorBrush).Color;
            }

            set
            {
                _TextBrush = new System.Windows.Media.SolidColorBrush(value);
                OnPropertyChanged(nameof(TextColor));
                OnPropertyChanged(nameof(TextBrush));
            }

        }


        /// <summary>
        /// Color that can be noticed when DarkMainColor is the background
        /// </summary>
        public System.Windows.Media.Brush DarkTextBrush
        {
            get
            {
                return _DarkTextBrush;
            }
            set
            {
                _DarkTextBrush = value;
                OnPropertyChanged(nameof(DarkTextBrush));
                OnPropertyChanged(nameof(DarkTextColor));
            }

        }


        /// <summary>
        /// Color that can be noticed when DarkMainColor is the background
        /// </summary>
        public System.Windows.Media.Color DarkTextColor
        {
            get
            {
                return (_DarkTextBrush as System.Windows.Media.SolidColorBrush).Color;
            }

            set
            {
                _DarkTextBrush = new System.Windows.Media.SolidColorBrush(value);
                OnPropertyChanged(nameof(DarkTextColor));
                OnPropertyChanged(nameof(DarkTextBrush));
            }

        }
        #endregion


        #region Constructor

        public NoteWindowBaseModel()
        {
            
        }

        #endregion
    }
}
