using ReedBooks.Models.Book;
using ReedBooks.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ReedBooks.Views
{
    public partial class ReadingDiaryWindow : Window
    {
        public ReadingDiaryWindow(Book book)
        {
            InitializeComponent();
            DataContext = new ReadingDiaryWindowViewModel(book);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((ReadingDiaryWindowViewModel)DataContext).DeleteQuoteCommand.Execute(((Button)sender).CommandParameter);
        }
    }
}
