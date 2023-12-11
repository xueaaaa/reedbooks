using ReedBooks.Models.Assessment;
using System;
using System.Globalization;
using System.Windows.Data;

namespace ReedBooks.Converters
{
    public class EmoteToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Emote selected && parameter is Emote current)
            {
                if (selected == current) return true;
                else return false;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return true;
        }
    }
}
