using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ReedBooks.Views.Controls
{
    public partial class IconButton : Button
    {
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(IconButton));

        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public IconButton()
        {
            InitializeComponent();
        }
    }
}
