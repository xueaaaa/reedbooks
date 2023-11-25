using System.Windows;
using System.Windows.Media;

namespace ReedBooks.Controls
{
    public class ButtonWithImage : DependencyObject
    {
        public static readonly DependencyProperty ImageSource =
            DependencyProperty.RegisterAttached("ImageSource", typeof(ImageSource), typeof(ButtonWithImage));

        public static void SetImageSource(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(ImageSource, value);
        }

        public static ImageSource GetImageSource(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(ImageSource);
        }
    }
}
