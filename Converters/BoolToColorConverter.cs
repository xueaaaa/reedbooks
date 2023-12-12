using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ReedBooks.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool val) return val ? Application.Current.Resources["text_color"] : Application.Current.Resources["hint_color"];
            return Application.Current.Resources["accent_color"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
