using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controls.Models
{
    public class SelectedColorInfo : INotifyPropertyChanged
    {
        #region Private Members
        private ColorManager.HSV _SelectedColorHSV = new ColorManager.HSV(0,0,0);
        private ColorManager.sRGB _SelectedColorRgb = new ColorManager.sRGB(0, 0, 0);
        #endregion

        #region Public Members
        /// <summary>
        /// Happens when the color has changed
        /// </summary>
        public event Action<ColorManager.sRGB> ColorChanged;
        public ColorManager.HSV SelectedColorHSV
        {
            set
            {
                _SelectedColorHSV = value;
                _SelectedColorRgb = new ColorManager.sRGB(_SelectedColorHSV);
                ColorChanged?.Invoke(_SelectedColorRgb);
                NotifyPropertyChanged(nameof(SelectedColorHSV));
                NotifyPropertyChanged(nameof(SelectedColorRGB));
            }
            get
            {
                return _SelectedColorHSV;
            }
        }
        public ColorManager.sRGB SelectedColorRGB
        {
            set
            {
                _SelectedColorRgb = value;
                _SelectedColorHSV = new ColorManager.HSV(_SelectedColorRgb);
                ColorChanged?.Invoke(_SelectedColorRgb);
                NotifyPropertyChanged(nameof(SelectedColorHSV));
                NotifyPropertyChanged(nameof(SelectedColorRGB));
            }
            get
            {
                return _SelectedColorRgb;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructor


        #endregion

        #region Public Members
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public virtual void OnColorChanged()
        {
            ColorChanged?.Invoke(_SelectedColorRgb);
        }
        #endregion

    }
}
