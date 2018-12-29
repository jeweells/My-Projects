using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace FastNotes.Converters
{
    public class OpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value.GetType() == typeof(SolidColorBrush))
            {
                SolidColorBrush targetBrush;
                double targetOpacity = double.Parse((string)parameter);
                SolidColorBrush valueBrush = (SolidColorBrush)value;
                Color color = Color.FromArgb((byte)(targetOpacity * 255.0), valueBrush.Color.R, valueBrush.Color.G, valueBrush.Color.B);
                targetBrush = new SolidColorBrush(color);
                return targetBrush;

            }
            else if(value.GetType() == typeof(Color))
            {
                double targetOpacity = double.Parse((string)parameter);
                Color valueColor = (Color)value;
                Color color = Color.FromArgb((byte)(targetOpacity * 255.0), valueColor.R, valueColor.G, valueColor.B);
                return color;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
