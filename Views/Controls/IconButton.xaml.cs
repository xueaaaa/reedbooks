using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace ReedBooks.Views.Controls
{
    public partial class IconButton : Button
    {
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(PackIconKind), typeof(IconButton));

        public PackIconKind Source
        {
            get => (PackIconKind)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public IconButton()
        {
            InitializeComponent();
        }
    }
}
