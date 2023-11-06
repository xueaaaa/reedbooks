using ReedBooks.Models.Assessment;
using System;
using System.Globalization;
using System.Windows.Data;

namespace ReedBooks.Core
{
    public class EnumToPackIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Emote emote)
            {
                switch (emote)
                {
                    case Emote.Sadness:
                        return "EmoticonCryOutline";
                    case Emote.Amazement:
                        return "EmoticonFrownOutline";
                    case Emote.Indifference:
                        return "EmoticonNeutralOutline";
                    case Emote.Interesting:
                        return "FaceManShimmerOutline";
                    case Emote.Happyness:
                        return "EmoticonExcitedOutline";
                    default:
                        return null;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
