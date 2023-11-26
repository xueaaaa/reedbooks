﻿using Microsoft.Win32;
using ReedBooks.Core;
using ReedBooks.Models.Book;
using ReedBooks.Models.Diary;
using ReedBooks.Properties;
using ReedBooks.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
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

        public MainWindowViewModel()
        {
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

            SidePanelColumnLength = new GridLength(0.4, GridUnitType.Star);
            LoadedBooks = new ObservableCollection<Book>(Book.ReadAll());
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
        }

        private void ChangeSidePanelVisibility()
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

        private void SwitchToTab(object param)
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
                book.BoundDiary = ReadingDiary.Create();
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
    }
}
