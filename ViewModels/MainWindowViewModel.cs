using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using ReedBooks.Core;
using ReedBooks.Models.Book;
using ReedBooks.Models.Collection;
using ReedBooks.Models.Diary;
using ReedBooks.Properties;
using ReedBooks.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace ReedBooks.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        public delegate void BooksListChanged();

        private Visibility _sidePanelVisibility;
        public Visibility SidePanelVisibility
        {
            get => _sidePanelVisibility;
            set
            {
                if (_sidePanelVisibility != value)
                {
                    _sidePanelVisibility = value;
                    OnPropertyChanged(nameof(SidePanelVisibility));
                }
            }
        }
        private GridLength _sidePanelColumnLength;
        public GridLength SidePanelColumnLength
        {
            get => _sidePanelColumnLength;
            set
            {
                _sidePanelColumnLength = value;
                OnPropertyChanged(nameof(SidePanelColumnLength));
            }
        }

        private int _selectedTab;
        public int SelectedTab
        {
            get => _selectedTab;
            set
            {
                if (value >= 0) _selectedTab = value;
                OnPropertyChanged(nameof(SelectedTab));
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

        private ObservableCollection<Book> _searchedBooks;
        public ObservableCollection<Book> SearchedBooks
        {
            get => _searchedBooks;
            set
            {
                _searchedBooks = value;
                OnPropertyChanged(nameof(SearchedBooks));
            }
        }

        private ObservableCollection<Book> _currentBooks;
        public ObservableCollection<Book> CurrentBooks
        {
            get => _currentBooks;
            set
            {
                _currentBooks = value;
                OnPropertyChanged(nameof(CurrentBooks));
            }
        }

        private ObservableCollection<Book> _recentBooks;
        public ObservableCollection<Book> RecentBooks
        {
            get => _recentBooks;
            set
            {
                _recentBooks = value;
                OnPropertyChanged(nameof(RecentBooks));
            }
        }

        private ObservableCollection<Collection> _loadedCollections;
        public ObservableCollection<Collection> LoadedCollections
        {
            get => _loadedCollections;
            set
            {
                _loadedCollections = value;
                OnPropertyChanged(nameof(LoadedCollections));
            }
        }


        private string _newCollectionName;
        public string NewCollectionName
        {
            get => _newCollectionName;
            set
            {
                _newCollectionName = value;
                OnPropertyChanged(nameof(NewCollectionName));
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

        public ICommand HandleFileDropCommand { get; }
        public ICommand ChangeSidePanelVisibilityCommand { get; }
        public ICommand SwitchToTabCommand { get; }
        public ICommand LoadFileCommand { get; }
        public ICommand DeleteBookCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand ReadCommand { get; }
        public ICommand OpenReadingDiaryCommand { get; }
        public ICommand OpenSettingsCommand { get; }
        public ICommand SortByNameCommand { get; }
        public ICommand SortByNameDescendingCommand { get; }
        public ICommand SortByLastReadingDateCommand { get; }
        public ICommand SortByLastReadingDateDescendingCommand { get; }
        public ICommand CreateCollectionCommand { get; }
        public ICommand DeleteCollectionCommand { get; }

        public MainWindowViewModel()
        {
            HandleFileDropCommand = new RelayCommand(obj => HandleFileDrop(obj));
            ChangeSidePanelVisibilityCommand = new RelayCommand(obj => ChangeSidePanelVisibility());
            SwitchToTabCommand = new RelayCommand(obj => SwitchToTab(obj));
            LoadFileCommand = new RelayCommand(obj => LoadFile());
            DeleteBookCommand = new RelayCommand(obj => DeleteBook(obj));
            SearchCommand = new RelayCommand(obj => Search(obj));
            ReadCommand = new RelayCommand(obj => Read(obj));
            OpenReadingDiaryCommand = new RelayCommand(obj => OpenReadingDiary(obj));
            OpenSettingsCommand = new RelayCommand(obj => OpenSettings());
            SortByNameCommand = new RelayCommand(obj => SortByName());
            SortByNameDescendingCommand = new RelayCommand(obj => SortByNameDescending());
            SortByLastReadingDateCommand = new RelayCommand(obj => SortByLastReadingDate());
            SortByLastReadingDateDescendingCommand = new RelayCommand(obj => SortByLastReadingDateDescending());
            CreateCollectionCommand = new RelayCommand(obj => CreateCollection());
            DeleteCollectionCommand = new RelayCommand(obj => DeleteCollection(obj));

            SidePanelColumnLength = new GridLength(0.4, GridUnitType.Star);
            LoadedBooks = new ObservableCollection<Book>(Book.ReadAll());
            LoadedCollections = new ObservableCollection<Collection>(App.ApplicationContext.Collections);
            SearchedBooks = LoadedBooks;
            SelectedTab = Settings.Default.DefaultTab;

            CurrentBooks = new ObservableCollection<Book>(LoadedBooks
                .Where(b => b.BoundDiary.ReadingIsOver != true &&
                b.BoundDiary.LastReadingAt.Day >= DateTime.Now.Day - Settings.Default.CurrentCountedDays 
                && b.BoundDiary.LastReadingAt != DateTime.MinValue)
                .OrderByDescending(b => b.BoundDiary.LastReadingAt));

            RecentBooks = new ObservableCollection<Book>(LoadedBooks.Where(b => b.BoundDiary.LastReadingAt != DateTime.MinValue)
                .OrderByDescending(b => b.BoundDiary.LastReadingAt)
                .Take(Settings.Default.RecentBooksNumberDisplaying));

            SelectedCollectionBooks = new ObservableCollection<Book>();
        }

        public async void HandleFileDrop(object param)
        {
            var e = (DragEventArgs)param;

            if(e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string file in files)
                {
                    if(Path.GetExtension(file) != ".epub")
                    {
                        var dW = new DialogWindow(Application.Current.Resources["dialog_error_title"].ToString(),
                            Application.Current.Resources["dialog_not_epub_content"].ToString());
                        dW.ShowDialog();

                        return;
                    }

                    var book = await Book.Create(file);
                    LoadedBooks.Add(book);
                }
            }
        }

        public void ChangeSidePanelVisibility()
        {
            switch (SidePanelVisibility)
            {
                case Visibility.Visible:
                    SidePanelVisibility = Visibility.Hidden;
                    SidePanelColumnLength = new GridLength(0.05, GridUnitType.Star);
                    break;
                case Visibility.Hidden:
                    SidePanelVisibility = Visibility.Visible;
                    SidePanelColumnLength = new GridLength(0.4, GridUnitType.Star);
                    break;
                default:
                    SidePanelVisibility = Visibility.Visible;
                    break;
            }
        }

        public void SwitchToTab(object param)
        {
            SelectedTab = Convert.ToInt32(param);
        }

        public async void LoadFile()
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Epub-files (.epub)|*.epub";

            if (ofd.ShowDialog() == true)
            {
                var filePath = ofd.FileName;
                Book book = await Book.Create(filePath);
                LoadedBooks.Add(book);
            }
        }

        public void DeleteBook(object param)
        {
            var dialog = new DialogWindow(Application.Current.Resources["dialog_delete_book_title"].ToString(), 
                Application.Current.Resources["dialog_delete_book_content"].ToString());

            if (dialog.ShowDialog() == true)
            {
                var guid = (Guid)param;
                var book = LoadedBooks.Where(b => b.Guid == guid).First();
                LoadedBooks.Remove(book);
                book.Delete();
            }
        }

        public void Search(object param)
        {
            var namePart = (string)param;
            SearchedBooks = new ObservableCollection<Book>(LoadedBooks.Where(b => b.Name.ToLower().Contains(namePart.ToLower())).ToList());
        }

        public void Read(object param)
        {
            var selectedBook = (Book)param;
                    
            ReadingWindow rW = new ReadingWindow(selectedBook);
            rW.Show();
        }

        public void OpenReadingDiary(object param)
        {
            var selectedBook = (Book)param;
            ReadingDiaryWindow rDW = new ReadingDiaryWindow(selectedBook);
            rDW.Show();
        }

        public void OpenSettings()
        {
            SettingsWindow sW = new SettingsWindow();
            sW.ShowDialog();
        }

        public void SortByName()
        {
            SearchedBooks = new ObservableCollection<Book>(SearchedBooks.OrderBy(b => b.Name));
        }

        public void SortByNameDescending()
        {
            SearchedBooks = new ObservableCollection<Book>(SearchedBooks.OrderByDescending(b => b.Name));
        }

        public void SortByLastReadingDate()
        {
            SearchedBooks = new ObservableCollection<Book>(SearchedBooks.OrderByDescending(b => b.BoundDiary.LastReadingAt));
        }

        public void SortByLastReadingDateDescending()
        {
            SearchedBooks = new ObservableCollection<Book>(SearchedBooks.OrderBy(b => b.BoundDiary?.LastReadingAt));
        }

        public async void CreateCollection()
        {
            if(SelectedCollectionBooks.Count == 0)
            {
                var dW = new DialogWindow(Application.Current.Resources["dialog_error_title"].ToString(),
                    Application.Current.Resources["dialog_null_collection_content"].ToString());
                dW.ShowDialog();
                return;
            }

            var collection = await Collection.Create(NewCollectionName, SelectedCollectionBooks.Select(b => b.Guid).ToList());
            LoadedCollections.Add(collection);

            SelectedCollectionBooks = null;
            NewCollectionName = string.Empty;
            DialogHost.Close("Dialog");
        }

        public async void DeleteCollection(object param)
        {
            Collection toDelete = await App.ApplicationContext.FindAsync<Collection>(param);
            LoadedCollections.Remove(toDelete);
            await App.ApplicationContext.RemoveEntityAsync(toDelete);

            DialogHost.Close("Dialog");
        }
    }
}
