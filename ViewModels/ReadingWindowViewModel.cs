using MaterialDesignThemes.Wpf;
using ReedBooks.Core;
using ReedBooks.Models.Book;
using ReedBooks.Models.Diary;
using ReedBooks.Views;
using ReedBooks.Views.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TheArtOfDev.HtmlRenderer.WPF;

namespace ReedBooks.ViewModels
{
    public class ReadingWindowViewModel : ObservableObject
    {
        public delegate void SelectedChapterChanged();
        public event SelectedChapterChanged ChapterChanged;

        private Book _book;
        public Book Book
        {
            get => _book;
            set
            {
                _book = value;
                OnPropertyChanged(nameof(Book));
            }
        }

        private string _selectedChapterHtml;
        public string SelectedChapterHtml
        {
            get => _selectedChapterHtml;
            set
            {
                if(value != null)
                {
                    _selectedChapterHtml = value;
                    OnPropertyChanged(nameof(SelectedChapterHtml));
                }
            }
        }

        private double _scrollOffset;
        public double ScrollOffset
        {
            get => _scrollOffset;
            set
            {
                _scrollOffset = value;
                OnPropertyChanged(nameof(ScrollOffset));
            }
        }

        private List<NavigationItem> _navigation;
        public List<NavigationItem> Navigation
        {
            get => _navigation;
            set
            {
                _navigation = value;
                OnPropertyChanged(nameof(Navigation));
            }
        }

        private NavigationItem _previousNavigation;
        public NavigationItem PreviousNavigation
        {
            get => _previousNavigation;
            set
            {
                _previousNavigation = value;
                OnPropertyChanged(nameof(PreviousNavigation));
            }
        }

        private NavigationItem _currentNavigation;
        public NavigationItem CurrentNavigation
        {
            get => _currentNavigation;
            set
            {
                _currentNavigation = value;
                OnPropertyChanged(nameof(CurrentNavigation));
            }
        }

        private NavigationItem _nextNavigation;
        public NavigationItem NextNavigation
        {
            get => _nextNavigation;
            set
            {
                _nextNavigation = value;
                OnPropertyChanged(nameof(NextNavigation));
            }
        }

        private string _quoteData;
        public string QuoteData
        {
            get => _quoteData;
            set
            {
                _quoteData = value;
                OnPropertyChanged(nameof(QuoteData));
            }
        }

        private string _quoteAuthor;
        public string QuoteAuthor
        {
            get => _quoteAuthor;
            set
            {
                _quoteAuthor = value;
                OnPropertyChanged(nameof(QuoteAuthor));
            }
        }

        private string _quoteLocation;
        public string QuoteLocation
        {
            get => _quoteLocation;
            set
            {
                _quoteLocation = value;
                OnPropertyChanged(nameof(QuoteLocation));
            }
        }

        public ICommand MoveToAnotherDocumentCommand { get; }
        public ICommand MarkAsReadCommand { get; }
        public ICommand OpenReadingDiaryCommand { get; }
        public ICommand OnWindowClosingCommand { get; }
        public ICommand AddQuoteCommand { get; }

        public ReadingWindowViewModel()
        {
            MoveToAnotherDocumentCommand = new RelayCommand(obj => MoveToAnotherDocument(obj));
            MarkAsReadCommand = new RelayCommand(obj => MarkAsRead(obj));
            OpenReadingDiaryCommand = new RelayCommand(obj => OpenReadingDiary());
            OnWindowClosingCommand = new RelayCommand(obj => OnWindowClosing());
            AddQuoteCommand = new RelayCommand(obj => AddQuote());
        }

        public ReadingWindowViewModel(Book readingBook) : this()
        {
            HtmlPanel html = new HtmlPanel();

            Book = readingBook;
            Navigation = Book.LoadNavigation();

            if(Book.LastReadingPosition != null)
                MoveToAnotherDocument(Navigation.Where(n => n.Link == Book.LastReadingPosition.Link).First());
        }

        public void MoveToAnotherDocument(object param)
        {
            var nav = param as NavigationItem;

            CurrentNavigation = nav;
            foreach (var item in Navigation)
            {
                PreviousNavigation = LookFor(item, -1);
                if (PreviousNavigation != null) break;
            }
            foreach (var item in Navigation)
            {
                NextNavigation = LookFor(item, +1);
                if (NextNavigation != null) break;
            }

            if (nav != null)
            {
                var document = Book.LoadChapter(nav.Link);
                SelectedChapterHtml = document;
                ChapterChanged?.Invoke();
            }
        }

        private NavigationItem LookFor(NavigationItem nav, sbyte incrementValue = -1)
        {
            NavigationItem previous = null;
            if (nav.Header == CurrentNavigation.Header)
            {
                int index = Navigation.IndexOf(CurrentNavigation);
                if (index <= 0 && incrementValue < 0) return null;

                previous = Navigation[index + incrementValue];
            }

            if (nav.ItemsSource != null)
            {
                foreach (NavigationItem item in nav.ItemsSource)
                {
                    if (item.Header == CurrentNavigation.Header)
                    {
                        int index = Navigation.IndexOf(CurrentNavigation);
                        previous = Navigation[index + incrementValue];
                        break;
                    }

                    if (item.ItemsSource != null)
                    {
                        foreach (var item1 in item.ItemsSource)
                        {
                            previous = LookFor(nav, incrementValue);
                            if (previous != null) break;
                        }
                    }
                }
            }

            return previous;
        }

        public void MarkAsRead(object param)
        {
            DialogHost.Show(param, "ReadingDialog");
            Book.MarkAsRead();
        }

        public void OpenReadingDiary() 
        {
            ReadingDiaryWindow rDW = new ReadingDiaryWindow(Book);
            rDW.Show();
        }

        public void OnWindowClosing()
        {
            Book.LastReadingPosition = new Position(Book.Guid, CurrentNavigation.Link, ScrollOffset);
        }

        public double OnWindowLoaded()
        {
            if (Book.LastReadingPosition != null) return Book.LastReadingPosition.Offset;
            return 0;
        }

        public async void AddQuote()
        {
            Quote toAdd = new Quote(QuoteData, Book.BoundDiary.Guid, QuoteAuthor, QuoteLocation);

            if (toAdd.Author != string.Empty && toAdd.Author != string.Empty)
            {
                Book.BoundDiary.Quotes.Add(toAdd);
                await toAdd.CreateAsync();
                QuoteAuthor = string.Empty;
                QuoteLocation = string.Empty;
                DialogHost.Close("ReadingDialog");
            }
            else
                new DialogWindow(Application.Current.Resources["dialog_error_title"].ToString(),
                    Application.Current.Resources["dialog_null_quote_content"].ToString(), Visibility.Hidden)
                    .ShowDialog();
        }
    }
}
