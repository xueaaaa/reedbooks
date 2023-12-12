using ReedBooks.Core;
using ReedBooks.Models.Book;
using ReedBooks.Models.Collection;
using ReedBooks.Views;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ReedBooks.ViewModels
{
    public class EditCollectionViewModel : ObservableObject
    {
        private Collection _collection;
        public Collection Collection
        {
            get => _collection;
            set
            {
                _collection = value;
                OnPropertyChanged(nameof(Collection));
            }
        }

        private string _collectionName;
        public string CollectionName
        {
            get => _collectionName;
            set
            {
                _collectionName = value;
                OnPropertyChanged(nameof(CollectionName));
            }
        }

        private ObservableCollection<Book> _loadedBooks;
        public ObservableCollection<Book> LoadedBooks
        {
            get => _loadedBooks;
            set
            {
                _loadedBooks = value;
                OnPropertyChanged(nameof(LoadedBooks));
            }
        }

        private ObservableCollection<Book> _selectedCollectionBooks;
        public ObservableCollection<Book> SelectedCollectionBooks
        {
            get => _selectedCollectionBooks;
            set
            {
                _selectedCollectionBooks = value;
                OnPropertyChanged(nameof(SelectedCollectionBooks));
            }
        }

        public ICommand EditCollectionCommand { get; }

        public EditCollectionViewModel()
        {
            EditCollectionCommand = new RelayCommand(obj => EditCollection());
        }

        public EditCollectionViewModel(Collection collection, ObservableCollection<Book> loadedBooks) : this()
        {
            Collection = collection;
            LoadedBooks = loadedBooks;
            CollectionName = collection.Name;
        }

        public async void EditCollection()
        {
            if (SelectedCollectionBooks.Count == 0 || CollectionName == string.Empty)
            {
                var eDW = new DialogWindow(Application.Current.Resources["dialog_error_title"].ToString(),
                    Application.Current.Resources["dialog_null_collection_content"].ToString());
                eDW.ShowDialog();
                return;
            }

            Collection.Name = CollectionName;
            Collection.LinkedBooks = SelectedCollectionBooks.Select(c => c.Guid.ToString()).ToList();
            await Collection.UpdateAsync();

            var dW = new DialogWindow(Application.Current.Resources["dialog_success_title"].ToString(),
                    Application.Current.Resources["dialog_success_content"].ToString(), Visibility.Hidden);
                dW.ShowDialog();
        }
    }
}
