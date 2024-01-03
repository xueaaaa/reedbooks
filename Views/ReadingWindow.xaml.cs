using ReedBooks.Models.Book;
using ReedBooks.ViewModels;
using System.Windows;
using System.Windows.Controls;
using TheArtOfDev.HtmlRenderer.Core.Entities;
using TheArtOfDev.HtmlRenderer.WPF;

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

        private void HtmlPanel_LinkClicked(object sender, RoutedEvenArgs<HtmlLinkClickedEventArgs> args)
        {
            ((ReadingWindowViewModel)DataContext).MoveToAnotherDocument(args.Data.Link);
            args.Handled = true;
        }

        //private void RenderCss(int fontSize = 16)
        //{
        //    string css = $"*{{color:black;font-size: {fontSize}px;}}";
        //    HtmlPanel.BaseStylesheet = css;
        //}
    }
}
