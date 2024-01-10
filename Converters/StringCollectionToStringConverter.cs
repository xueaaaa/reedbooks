using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ReedBooks.Converters
{
    public class StringCollectionToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<string> strings) return string.Join(", ", strings);
            else if (value is string str) 
            {
                if(str == string.Empty) return Application.Current.Resources["m_not_stated"].ToString();
                return str; 
            }
            else if (value is int val)
            {
                if (val == 0) return Application.Current.Resources["m_not_stated"].ToString();
                return val.ToString();
            }
            return Application.Current.Resources["m_not_stated"].ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
