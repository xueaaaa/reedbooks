using ReedBooks.Models.Book;
using System.Windows;
using System.Windows.Controls;

namespace ReedBooks.Views.Controls
{
    public partial class BookCard : Border
    {
        public static readonly DependencyProperty BookProperty =
            DependencyProperty.Register("Book", typeof(Book), typeof(BookCard));

        public Book Book
        {
            get => (Book)GetValue(BookProperty);
            set => SetValue(BookProperty, value);
        }

        public BookCard()
        {
            InitializeComponent();
        }
    }
}
