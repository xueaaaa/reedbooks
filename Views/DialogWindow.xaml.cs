﻿using ReedBooks.ViewModels;
using System.Windows;

namespace ReedBooks.Views
{
    public partial class DialogWindow : Window
    {
        public DialogWindow(string title, string content)
        {
            InitializeComponent();
            DataContext = new DialogWindowViewModel(title, content);
        }
    }
}
