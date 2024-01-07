using System.Windows;
using System.Windows.Controls;

namespace ReedBooks.Views.Controls
{
    public partial class ShopControl : ItemsControl
    {
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(ShopControl));

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public ShopControl()
        {
            InitializeComponent();
        }
    }
}
