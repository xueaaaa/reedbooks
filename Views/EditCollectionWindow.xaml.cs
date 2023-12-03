using ReedBooks.Models.Book;
using ReedBooks.Models.Collection;
using ReedBooks.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ReedBooks.Views
{
    public partial class EditCollectionWindow : Window
    {
        public EditCollectionWindow(Collection collection, ObservableCollection<Book> loadedBooks)
        {
            InitializeComponent();
            DataContext = new EditCollectionViewModel(collection, loadedBooks);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((EditCollectionViewModel)DataContext).SelectedCollectionBooks = new ObservableCollection<Book>();

            foreach (var item in ((ListBox)sender).SelectedItems)
            {
                ((EditCollectionViewModel)DataContext).SelectedCollectionBooks.Add((Book)item);
            }
        }
    }
}
