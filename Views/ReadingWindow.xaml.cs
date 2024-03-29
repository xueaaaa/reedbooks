﻿using ReedBooks.Models.Book;
using ReedBooks.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

            ((ReadingWindowViewModel)DataContext).ChapterChanged += (offset) =>
            {
                if (offset == 0) ScrollViewer.ScrollToHome();
                else ScrollViewer.ScrollToVerticalOffset(offset);
                ChaptetsView.ViewSelectedItem = ((ReadingWindowViewModel)DataContext).CurrentNavigation;
            };
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var ctx = ((ReadingWindowViewModel)DataContext);
            ctx.ScrollOffset = e.VerticalOffset;

            if (ScrollViewer.ScrollableHeight == 0) ctx.CurrentChapterReadingProgress = 100;
            else ctx.CurrentChapterReadingProgress = (e.VerticalOffset / (ScrollViewer.ScrollableHeight / 100D));
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

        private void Rectangle_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset + (-e.Delta / 30));
        }

        private void Rectangle_MouseWheel_1(object sender, MouseWheelEventArgs e)
        {
            var vm = (ReadingWindowViewModel)DataContext;
            double val = vm.TopRectangleSize.Value;

            if(e.Delta < 0)
            {
                val += val / 30;
            } 
            else
            {
                val -= val / 30;
            }

            var newGridLength = new GridLength(val, GridUnitType.Star);
            vm.TopRectangleSize = newGridLength;
        }
    }
}
