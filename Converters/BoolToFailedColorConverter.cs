using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ReedBooks.Converters
{
    public class BoolToFailedColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool val) return val ? Application.Current.Resources["danger_color"] : Application.Current.Resources["hint_color"];
            return Application.Current.Resources["hint_color"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
