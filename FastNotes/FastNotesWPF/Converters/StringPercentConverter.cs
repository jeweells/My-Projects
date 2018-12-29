using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FastNotes.Converters
{
    public class StringPercentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.GetType() != typeof(double)) return value;
            return ((int)((double)value * 100.0)) + "%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.GetType() != typeof(string)) return value;
            double result;
            string str = (value as string).TrimEnd('%');
            return (double.TryParse(str, out result)) ? result : 0.0;
        }
    }
}
