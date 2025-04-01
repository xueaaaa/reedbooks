using System.Windows;
using System.Windows.Controls;

namespace ReedBooks.Views.Controls
{
    public partial class ShopControl : ItemsControl
    {
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(ShopControl));
        public static readonly DependencyProperty IsNullProperty =
            DependencyProperty.Register("IsNull", typeof(bool), typeof(ShopControl));

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public bool IsNull
        {
            get { return (bool)GetValue(IsNullProperty); }
            set { SetValue(IsNullProperty, value); }
        }

        public bool IsInternetConnected => App.IsInternetConnected;
        
        public ShopControl()
        {
            InitializeComponent();
        }
    }
}
