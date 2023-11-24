using ReedBooks.Models.Book;
using ReedBooks.ViewModels;
using System.Windows;

namespace ReedBooks.Views
{
    public partial class ReadingWindow : Window
    {
        public ReadingWindow(Book readingBook)
        {
            InitializeComponent();

            DataContext = new ReadingWindowViewModel(readingBook);
            readingBook.SetStartReadingDate();
            readingBook.BoundDiary.SetLastReadingDate();
        }
    }
}
