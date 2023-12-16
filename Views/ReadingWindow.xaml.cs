using ReedBooks.Models.Book;
using ReedBooks.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ReedBooks.Views
{
    public partial class ReadingWindow : Window
    {
        public ReadingWindow(Book readingBook)
        {
            InitializeComponent();

            DataContext = new ReadingWindowViewModel(readingBook);
            readingBook.SetStartReadingDate();
            readingBook.SetLastReadingDate();
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ((ReadingWindowViewModel)DataContext).ScrollOffset = e.VerticalOffset;
        }
    }
}
