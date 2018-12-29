using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using ColorManager;

namespace FastNotes.Converters
{
    public class WhiteBlackColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;


            SolidColorBrush background = (SolidColorBrush) value;


            sRGB back = new sRGB(background.Color.R / 255f, background.Color.G / 255f, background.Color.B / 255f);
            sRGB newBack = back.ChooseColorByContrastRatio(7f);


            //sRGB black = new sRGB(0, 0, 0);
            //sRGB white = new sRGB(new HSL(0, 0, .5f));
            //SolidColorBrush newBack =
            //    ColorManager.ColorManager.ContrastRatio(black, back) > ColorManager.ColorManager.ContrastRatio(white, back) ?
            //    new SolidColorBrush(Color.FromRgb(0, 0, 0)) :
            //    new SolidColorBrush(Color.FromRgb((byte) (white.R * 255f), (byte)(white.G * 255f), (byte)(white.B * 255f)));
            return new SolidColorBrush(Color.FromRgb((byte)(newBack.R * 255f), (byte)(newBack.G * 255f), (byte)(newBack.B * 255f)));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value; // Imposibiru :(
        }
    }
}
