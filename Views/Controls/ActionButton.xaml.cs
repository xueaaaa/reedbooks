using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ReedBooks.Views.Controls
{
    public partial class ActionButton : Button
    {
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(PackIconKind), typeof(ActionButton));
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ActionButton));
        public static readonly DependencyProperty ColorProperty = 
            DependencyProperty.Register("Color", typeof(Brush), typeof(ActionButton));

        public PackIconKind Source
        {
            get => (PackIconKind)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public Brush Color
        {
            get => (Brush)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public ActionButton()
        {
            InitializeComponent();
        }
    }
}
