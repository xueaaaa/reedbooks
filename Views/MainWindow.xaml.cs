﻿using ReedBooks.Models.Book;
using ReedBooks.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ReedBooks.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((MainWindowViewModel)DataContext).SearchCommand.Execute(((TextBox)sender).Text);
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((MainWindowViewModel)DataContext).ReadCommand.Execute(((ListView)sender).SelectedItem);
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            ((MainWindowViewModel)DataContext).HandleFileDropCommand.Execute(e);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((MainWindowViewModel)DataContext).SelectedCollectionBooks = new ObservableCollection<Book>();

            foreach (var item in ((ListBox)sender).SelectedItems)
            {
                ((MainWindowViewModel)DataContext).SelectedCollectionBooks.Add((Book)item);
            }
        }

        private void IconButton_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)DataContext).DeleteCollectionCommand.Execute(((Button)sender).CommandParameter);
        }
    }
}
