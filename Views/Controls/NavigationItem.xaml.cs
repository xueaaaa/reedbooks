using System.Windows;
using System.Windows.Controls;

namespace ReedBooks.Views.Controls
{
    public partial class NavigationItem : TreeViewItem
    {
        public static readonly DependencyProperty LinkProperty =
            DependencyProperty.Register("Link", typeof(string), typeof(NavigationItem));

        public string Link
        {
            get => (string)GetValue(LinkProperty);
            set => SetValue(LinkProperty, value);
        }

        public NavigationItem BoundTo { get; set; }

        public NavigationItem()
        {
            InitializeComponent();
        }
    }
}
