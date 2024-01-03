using ReedBooks.Models.Book;
using ReedBooks.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ReedBooks.Views
{
    public partial class ReadingWindow : Window
    {
        private int _currentFontSize = 16;

        public ReadingWindow(Book readingBook)
        {
            InitializeComponent();

            DataContext = new ReadingWindowViewModel(readingBook);
            readingBook.SetStartReadingDate();
            readingBook.SetLastReadingDate();

            ((ReadingWindowViewModel)DataContext).ChapterChanged += () =>
            {
                ScrollViewer.ScrollToHome();
                ChaptetsView.ViewSelectedItem = ((ReadingWindowViewModel)DataContext).CurrentNavigation;
            };
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

        private void IconButton_Click(object sender, RoutedEventArgs e)
        {
            ScrollViewer.ScrollToHome();
        }

        private void IconButton_Click_1(object sender, RoutedEventArgs e)
        {
            ScrollViewer.ScrollToEnd();
        }

        private void IconButton_Click_2(object sender, RoutedEventArgs e)
        {
            _currentFontSize += 2;
            RenderCss(_currentFontSize);
        }

        private void IconButton_Click_3(object sender, RoutedEventArgs e)
        {
            _currentFontSize -= 2;
            RenderCss(_currentFontSize);
        }

        private void RenderCss(int fontSize = 16)
        {
            string css = $"*{{color:{ColorConverter.ConvertFromString((string)Application.Current.Resources["text_color"].ToString())};font-size: {fontSize}px;}}";
            HtmlPanel.BaseStylesheet = css;
        }
    }
}
