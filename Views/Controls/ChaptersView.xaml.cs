using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ReedBooks.Views.Controls
{
    public partial class ChaptersView : ScrollViewer
    {
        public static readonly DependencyProperty NavigationProperty =
            DependencyProperty.Register("Navigation", typeof(List<NavigationItem>), typeof(ChaptersView));

        public List<NavigationItem> Navigation
        {
            get => (List<NavigationItem>)GetValue(NavigationProperty);
            set => SetValue(NavigationProperty, value);
        }

        public ChaptersView()
        {
            InitializeComponent();
        }
    }
}
