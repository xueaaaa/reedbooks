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
            bool? val = (bool?)value;
            if (val == null)
                return Application.Current.Resources["hint_color"];
            else if (val == true)
                return Application.Current.Resources["danger_color"];
            else return Application.Current.Resources["success_color"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
