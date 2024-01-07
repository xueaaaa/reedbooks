using ReedBooks.Models.Shop;
using System.Windows;
using System.Windows.Controls;

namespace ReedBooks.Views.Controls
{
    public partial class ShopBookCard : Border
    {
        public static readonly DependencyProperty BookProperty =
            DependencyProperty.Register("Book", typeof(ParsedBook), typeof(ShopBookCard));

        public ParsedBook Book
        {
            get { return (ParsedBook)GetValue(BookProperty); }
            set { SetValue(BookProperty, value); }
        }

        public ShopBookCard()
        {
            InitializeComponent();
        }
    }
}
