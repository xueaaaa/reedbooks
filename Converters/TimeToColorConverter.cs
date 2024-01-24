using System;
using System.Globalization;
using System.Windows.Data;

namespace ReedBooks.Converters
{
    public class TimeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is int val)
            {
                if (val >= Properties.Settings.Default.TimeGoal) return App.Current.Resources["success_color"];
                return App.Current.Resources["accent_color"];
            }

            return App.Current.Resources["accent_color"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
