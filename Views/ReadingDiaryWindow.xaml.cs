using ReedBooks.Models.Book;
using ReedBooks.ViewModels;
using System.Windows;

namespace ReedBooks.Views
{
    public partial class ReadingDiaryWindow : Window
    {
        public ReadingDiaryWindow(Book book)
        {
            InitializeComponent();
            DataContext = new ReadingDiaryWindowViewModel(book);
        }
    }
}
