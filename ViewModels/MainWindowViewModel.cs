﻿using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using ReedBooks.Core;
using ReedBooks.Models.Book;
using ReedBooks.Models.Collection;
using ReedBooks.Models.Shop;
using ReedBooks.Properties;
using ReedBooks.ViewModels.Helpers;
using ReedBooks.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ReedBooks.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        #region Properties
        private DownloadManagerViewModel _downloadManager;
        public DownloadManagerViewModel DownloadManager
        {
            get => _downloadManager;
            set
            {
                _downloadManager = value;
                OnPropertyChanged(nameof(DownloadManager));
            }
        }

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

        private bool _hideReadingNow;
        public bool HideReadingNow
        {
            get => _hideReadingNow;
            set
            {
                _hideReadingNow = value;
                OnPropertyChanged(nameof(HideReadingNow));
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

        private ObservableCollection<ParsedBook> _shopSearchedBooks;
        public ObservableCollection<ParsedBook> ShopSearchedBooks
        {
            get => _shopSearchedBooks;
            set
            {
                _shopSearchedBooks = value;
                OnPropertyChanged(nameof(ShopSearchedBooks));
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        private bool _isNull = true;
        public bool IsNull
        {
            get => _isNull;
            set
            {
                _isNull = value;
                OnPropertyChanged(nameof(IsNull));
            }
        }

        private int _goal;
        public int Goal
        {
            get => _goal;
            set
            {
                _goal = value;
                OnPropertyChanged(nameof(Goal));
            }
        }

        public TimeGoalController TimeGoal
        {
            get => App.TimeGoalController;
        }

        public Visibility DebugVisibility
        {
            get
            {
                #if DEBUG
                return Visibility.Visible;
                #else
                return Visibility.Collapsed;
                #endif
            }
        }

        public bool IsInternetConnected
        {
            get => App.IsInternetConnected;
        }

        public bool IsInternetNotConnected
        {
            get => !IsInternetConnected;
        }

        public int BlurRadius
        {
            get => IsInternetConnected ? -1 : 10;
        }
        #endregion

        #region Commands
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
        public ICommand SortOnlyUnreadCommand { get; }
        public ICommand ResetSortCommand { get; }
        public ICommand CreateCollectionCommand { get; }
        public ICommand EditCollectionCommand { get; }
        public ICommand DeleteCollectionCommand { get; }
        public ICommand ShareCommand { get; }
        public ICommand ShopSearchCommand { get; }
        public ICommand HideBookCommand { get; }
        public ICommand ShowBookCommand { get; }
        public ICommand TemporaryShowBookCommand { get; }
        #endregion

        public MainWindowViewModel()
        {
            if (Settings.Default.Password != string.Empty)
            {
                bool? res = new AuthorizationWindow().ShowDialog();
                if (res == null) App.Restart();
                else if (res == false) Application.Current.Shutdown();
            }

            DownloadManager = new DownloadManagerViewModel();
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
            SortOnlyUnreadCommand = new RelayCommand(obj => SortOnlyUnread());
            ResetSortCommand = new RelayCommand(obj => ResetSort());
            CreateCollectionCommand = new RelayCommand(obj => CreateCollection());
            EditCollectionCommand = new RelayCommand(obj => EditCollection(obj));
            DeleteCollectionCommand = new RelayCommand(obj => DeleteCollection(obj));
            ShareCommand = new RelayCommand(obj => Share(obj));
            ShopSearchCommand = new RelayCommand(obj => ShopSearch(obj));
            HideBookCommand = new RelayCommand(obj => HideBook(obj));
            ShowBookCommand = new RelayCommand(obj => ShowBook(obj));
            TemporaryShowBookCommand = new RelayCommand(obj => TemporaryShowBook(obj));

            SidePanelColumnLength = new GridLength(0.3, GridUnitType.Star);
            HideReadingNow = Settings.Default.HideReadingNow;
            LoadedBooks = new ObservableCollection<Book>(Book.ReadAll().Result);
            LoadedCollections = new ObservableCollection<Collection>(App.ApplicationContext.Collections);
            SearchedBooks = new ObservableCollection<Book>(LoadedBooks.Where(b => b.IsTempHidden != true));
            SelectedTab = Settings.Default.DefaultTab;
            Goal = Settings.Default.TimeGoal;

            UpdateRecentAndCurrent();

            SelectedCollectionBooks = new ObservableCollection<Book>();

            DownloadManager.DownloadCompleted += (book) =>
            {
                LoadedBooks.Add(book);
            };

            Settings.Default.PropertyChanged += (o, e) =>
            {
                HideReadingNow = Settings.Default.HideReadingNow;
                Goal = Settings.Default.TimeGoal;

                UpdateRecentAndCurrent();
            };
        }

        private void UpdateRecentAndCurrent()
        {
            CurrentBooks = new ObservableCollection<Book>(LoadedBooks
                .Where(b => b.BoundDiary.ReadingIsOver != true &&
                b.BoundDiary.LastReadingAt.Day >= DateTime.Now.Day - Settings.Default.CurrentCountedDays
                && b.BoundDiary.LastReadingAt != DateTime.MinValue)
                .OrderByDescending(b => b.BoundDiary.LastReadingAt));

            RecentBooks = new ObservableCollection<Book>(LoadedBooks.Where(b => b.BoundDiary.LastReadingAt != DateTime.MinValue)
                .OrderByDescending(b => b.BoundDiary.LastReadingAt)
                .Take(Settings.Default.RecentBooksNumberDisplaying));
        }

        public void HandleFileDrop(object param)
        {
            var e = (DragEventArgs)param;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string file in files)
                {
                    if (Path.GetExtension(file) != ".epub")
                    {
                        var dW = new DialogWindow(Application.Current.Resources["dialog_error_title"].ToString(),
                            Application.Current.Resources["dialog_not_epub_content"].ToString(), Visibility.Hidden);
                        dW.ShowDialog();

                        return;
                    }

                    var book = new Book(file);
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
                    SidePanelColumnLength = new GridLength(0.02, GridUnitType.Star);
                    break;
                case Visibility.Hidden:
                    SidePanelVisibility = Visibility.Visible;
                    SidePanelColumnLength = new GridLength(0.3, GridUnitType.Star);
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

        public void LoadFile()
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Epub-files (*.epub)|*.epub";

            if (ofd.ShowDialog() == true)
            {
                var filePath = ofd.FileName;

                Book book = new Book(filePath);
                LoadedBooks.Add(book);
            }
        }

        public async void DeleteBook(object param)
        {
            var dialog = new DialogWindow(Application.Current.Resources["dialog_delete_book_title"].ToString(), 
                Application.Current.Resources["dialog_delete_book_content"].ToString());

            if (dialog.ShowDialog() == true)
            {
                var guid = (Guid)param;
                var book = LoadedBooks.Where(b => b.Guid == guid).First();
                LoadedBooks.Remove(book);
                var containsThisBook = LoadedCollections.Where(c => c.RepresentedAsBook.Contains(book)).ToList();

                await book.RemoveAsync();
            }
        }

        public void Search(object param)
        {
            var namePart = (string)param;

            if (namePart.Length < 3) return;
            SearchedBooks = new ObservableCollection<Book>(LoadedBooks.Where(b => b.IsTempHidden != true && b.Name.ToLower().Contains(namePart.ToLower())).ToList());
        }

        public void Read(object param)
        {
            var selectedBook = (Book)param;
                    
            ReadingWindow rW = new ReadingWindow(selectedBook);
            rW.ShowDialog();
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

        public void SortOnlyUnread()
        {
            SearchedBooks = new ObservableCollection<Book>(SearchedBooks.Where(b => b.BoundDiary?.ReadingIsOver == false));
        }

        public void ResetSort()
        {
            SearchedBooks = new ObservableCollection<Book>(LoadedBooks.Where(b => b.IsTempHidden != true));
        }

        public void CreateCollection()
        {
            if(SelectedCollectionBooks.Count == 0 || CollectionName == string.Empty)
            {
                var dW = new DialogWindow(Application.Current.Resources["dialog_error_title"].ToString(),
                    Application.Current.Resources["dialog_null_collection_content"].ToString(), Visibility.Hidden);
                dW.ShowDialog();
                return;
            }

            var collection = new Collection(CollectionName, SelectedCollectionBooks.Select(b => b.Guid).ToList());
            LoadedCollections.Add(collection);

            SelectedCollectionBooks = new ObservableCollection<Book>();
            CollectionName = string.Empty;
            DialogHost.Close("MainWindowDialog");
        }

        public void EditCollection(object param)
        {
            var eCW = new EditCollectionWindow((Collection)param, LoadedBooks);
            eCW.ShowDialog();
        }

        public async void DeleteCollection(object param)
        {
            Collection toDelete = await App.ApplicationContext.FindAsync<Collection>(param);
            LoadedCollections.Remove(toDelete);
            await toDelete.RemoveAsync();

            if(DialogHost.IsDialogOpen("MainWindowDialog")) 
                DialogHost.Close("MainWindowDialog");
        }

        public void Share(object param)
        {
            var book = (Book)param;
            string path = book.Share();

            var dialog = new DialogWindow(Application.Current.Resources["dialog_share_file_title"].ToString(),
                Application.Current.Resources["dialog_share_file_content"].ToString(), Visibility.Visible);

            if (dialog.ShowDialog() == true) Process.Start("explorer.exe", Path.GetDirectoryName(path));
        }

        public async void ShopSearch(object param)
        {
            IsNull = false;
            ShopSearchedBooks = new ObservableCollection<ParsedBook>();
            IsLoading = true;
            var searched = await new Parser().Parse(param.ToString());
            IsLoading = false;
            if (searched != null) ShopSearchedBooks = new ObservableCollection<ParsedBook>(searched.OrderByDescending(b => b.RatedUsersNumber).ThenBy(b => b.DownloadEnabled == true));
            else IsNull = true;
        }

        public void HideBook(object param)
        {
            Book book = (Book)param;
            book.IsHidden = true;
            book.IsTempHidden = true;
            OnPropertyChanged(nameof(LoadedBooks));
        }

        public void ShowBook(object param)
        {
            Book book = (Book)param;
            book.IsHidden = false; 
            book.IsTempHidden = false;
            OnPropertyChanged(nameof(LoadedBooks));
        }

        public void TemporaryShowBook(object param)
        {
            Book book = (Book)param;

            if(Settings.Default.Password != string.Empty)
            {
                bool? result = new AuthorizationWindow().ShowDialog();
                if (result.HasValue && result.Value != true) return;
            }

            book.IsTempHidden = false;
            OnPropertyChanged(nameof(LoadedBooks));
        }
    }
}
