using Microsoft.Win32;
using ReedBooks.Core;
using ReedBooks.Models.Book;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ReedBooks.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private Visibility _sidePanelVisibility;
        public Visibility SidePanelVisibility
        {
            get { return _sidePanelVisibility; }
            set
            {
                if (_sidePanelVisibility != value)
                {
                    _sidePanelVisibility = value;
                    OnPropertyChanged(nameof(SidePanelVisibility));
                }
            }
        }
        private int _selectedTab;
        public int SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                if (value >= 0) _selectedTab = value;
                OnPropertyChanged(nameof(SelectedTab));
            }
        }
        private ObservableCollection<Book> _loadedBooks;
        public ObservableCollection<Book> LoadedBooks
        {
            get { return _loadedBooks; }
            set
            {
                if (value != null) _loadedBooks = value;
                OnPropertyChanged(nameof(LoadedBooks));
            }
        }

        private ObservableCollection<Book> _searchedBooks;
        public ObservableCollection<Book> SearchedBooks
        {
            get { return _searchedBooks; }
            set
            {
                if (value != null) _searchedBooks = value;
                OnPropertyChanged(nameof(SearchedBooks));
            }
        }

        public ICommand ChangeSidePanelVisibilityCommand { get; }
        public ICommand SwitchToTabCommand { get; }
        public ICommand LoadFileCommand { get; }
        public ICommand DeleteBookCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand TEMP_ChangeThemeCommand { get; }

        public MainWindowViewModel()
        {
            ChangeSidePanelVisibilityCommand = new RelayCommand(obj => ChangeSidePanelVisibility());
            SwitchToTabCommand = new RelayCommand(obj => SwitchToTab(obj));
            LoadFileCommand = new RelayCommand(obj => LoadFile());
            DeleteBookCommand = new RelayCommand(obj => DeleteBook(obj));
            SearchCommand = new RelayCommand(obj => Search(obj));
            TEMP_ChangeThemeCommand = new RelayCommand(obj => TEMP_ChangeTheme());

            var books = Book.ReadAll();
            LoadedBooks = new ObservableCollection<Book>(books);
            SearchedBooks = LoadedBooks;
        }

        private void ChangeSidePanelVisibility()
        {
            switch (SidePanelVisibility)
            {
                case Visibility.Visible:
                    SidePanelVisibility = Visibility.Collapsed;
                    break;
                case Visibility.Collapsed:
                    SidePanelVisibility = Visibility.Visible;
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
                LoadedBooks.Add(book);
            }
        }

        public void DeleteBook(object param)
        {
            var guid = (Guid)param;
            var book = LoadedBooks.Where(b => b.Guid == guid).First();
            LoadedBooks.Remove(book);
            book.Delete();
        }

        public void Search(object param)
        {
            var namePart = (string)param;
            SearchedBooks = new ObservableCollection<Book>(LoadedBooks.Where(b => b.Name.ToLower().Contains(namePart.ToLower())).ToList());
        }

        public void TEMP_ChangeTheme()
        {
            var old = Application.Current.Resources.MergedDictionaries.Where(a => a.Source.OriginalString.EndsWith("theme.xaml")).First();
            Application.Current.Resources.MergedDictionaries.Remove(old);
            ResourceDictionary ne = new ResourceDictionary();
            ne.Source = old.Source.OriginalString.Contains("light") ? new Uri("Resources/Themes/dark.theme.xaml", UriKind.Relative) : new Uri("Resources/Themes/light.theme.xaml", UriKind.Relative);
            Application.Current.Resources.MergedDictionaries.Add(ne);
        }
    }
}
