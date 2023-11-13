using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ReedBooks.Views.Controls
{
    public partial class ButtonWithImage : Button
    {
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(ButtonWithImage));
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ButtonWithImage));

        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public ButtonWithImage()
        {
            InitializeComponent();
        }
    }
}
