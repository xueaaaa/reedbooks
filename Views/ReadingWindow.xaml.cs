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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((ReadingWindowViewModel)DataContext).OnWindowClosing();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ScrollViewer.ScrollToVerticalOffset(((ReadingWindowViewModel)DataContext).OnWindowLoaded());
        }
    }
}
