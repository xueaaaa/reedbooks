using MaterialDesignThemes.Wpf;
using ReedBooks.Core;
using ReedBooks.Models.Book;
using ReedBooks.Models.Diary;
using ReedBooks.Views;
using ReedBooks.Views.Controls;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ReedBooks.ViewModels
{
    public class ReadingWindowViewModel : ObservableObject
    {
        public delegate void SelectedChapterChanged(double offset = 0);
        public event SelectedChapterChanged ChapterChanged;

        #region Properties
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
                if (value != null)
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

        private double _offsetReminder;
        public double OffsetReminder
        {
            get => _offsetReminder;
            set
            {
                _offsetReminder = value;
                OnPropertyChanged(nameof(OffsetReminder));
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

        private bool _previousEnabled;
        public bool PreviousEnabled
        {
            get => _previousEnabled;
            set
            {
                _previousEnabled = value;
                OnPropertyChanged(nameof(PreviousEnabled));
            }
        }

        private bool _nextEnabled = false;
        public bool NextEnabled
        {
            get => _nextEnabled;
            set
            {
                _nextEnabled = value;
                OnPropertyChanged(nameof(NextEnabled));
            }
        }

        private bool _returnEnabled;
        public bool ReturnEnabled
        {
            get => _returnEnabled;
            set
            {
                _returnEnabled = value;
                OnPropertyChanged(nameof(ReturnEnabled));
            }
        }

        private GridLength _chaptersViewLength = new GridLength(0.4, GridUnitType.Star);
        public GridLength ChaptersViewLength
        {
            get => _chaptersViewLength;
            set
            {
                _chaptersViewLength = value;
                OnPropertyChanged(nameof(ChaptersViewLength));
            }
        }

        private Visibility _splitterVisibility;
        public Visibility SplitterVisibility
        {
            get => _splitterVisibility;
            set
            {
                _splitterVisibility = value;
                OnPropertyChanged(nameof(SplitterVisibility));
            }
        }

        private bool _isClearMode;
        public bool IsClearMode
        {
            get => _isClearMode;
            set
            {
                _isClearMode = value;
                OnPropertyChanged(nameof(IsClearMode));
            }
        }

        private Visibility _concentrationRectanglesVisibility = Visibility.Collapsed;
        public Visibility ConcentrationRectanglesVisibility
        {
            get => _concentrationRectanglesVisibility;
            set
            {
                _concentrationRectanglesVisibility = value;
                OnPropertyChanged(nameof(ConcentrationRectanglesVisibility));
            }
        }

        private Visibility _concentrationHintsVisibility = Visibility.Collapsed;
        public Visibility ConcentrationHintsVisibility
        {
            get => _concentrationHintsVisibility;
            set
            {
                _concentrationHintsVisibility = value;
                OnPropertyChanged(nameof(ConcentrationHintsVisibility));
            }
        }

        private GridLength _topRectangleSize = new GridLength(0.1, GridUnitType.Star);
        public GridLength TopRectangleSize
        {
            get => _topRectangleSize;
            set
            {
                _topRectangleSize = value;
                OnPropertyChanged(nameof(TopRectangleSize));
            }
        }

        private bool _isConcentrationMode;
        public bool IsConcentrationMode
        {
            get => _isConcentrationMode;
            set
            {
                _isConcentrationMode = value;
                OnPropertyChanged(nameof(IsConcentrationMode));
            }
        }

        private double _currentChapterReadingProgress;
        public double CurrentChapterReadingProgress
        {
            get => _currentChapterReadingProgress;
            set
            {
                _currentChapterReadingProgress = value;
                OnPropertyChanged(nameof(CurrentChapterReadingProgress));
            }
        }

        private double _readingProgress;
        public double ReadingProgress
        {
            get => _readingProgress;
            set
            {
                _readingProgress = value;
                OnPropertyChanged(nameof(ReadingProgress));
            }
        }
        #endregion

        public ICommand MoveToAnotherDocumentCommand { get; }
        public ICommand MarkAsReadCommand { get; }
        public ICommand OpenReadingDiaryCommand { get; }
        public ICommand OnWindowClosingCommand { get; }
        public ICommand ReturnCommand { get; }
        public ICommand AddQuoteCommand { get; }
        public ICommand ClearModeCommand { get; }
        public ICommand ConcentrationModeCommand { get; }

        public ReadingWindowViewModel()
        {
            MoveToAnotherDocumentCommand = new RelayCommand(obj => MoveToAnotherDocument(obj));
            MarkAsReadCommand = new RelayCommand(obj => MarkAsRead(obj));
            OpenReadingDiaryCommand = new RelayCommand(obj => OpenReadingDiary());
            OnWindowClosingCommand = new RelayCommand(obj => OnWindowClosing());
            ReturnCommand = new RelayCommand(obj => Return());
            AddQuoteCommand = new RelayCommand(obj => AddQuote());
            ClearModeCommand = new RelayCommand(obj => ClearMode());
            ConcentrationModeCommand = new RelayCommand(obj => ConcentrationMode());
        }

        public ReadingWindowViewModel(Book readingBook) : this()
        {
            Book = readingBook;
            Navigation = Book.LoadNavigation();

            if (Book.LastReadingPosition != null) LoadLastPosition(Navigation);
            else MoveToAnotherDocument(Navigation.First());
        }

        private void LoadLastPosition(List<NavigationItem> collection)
        {
            foreach (var item in collection)
            {
                if (item.Link == Book.LastReadingPosition.Link)
                {
                    MoveToAnotherDocument(item);
                    return;
                }
                else if (item.ItemsSource != null) LoadLastPosition((List<NavigationItem>)item.ItemsSource);
            }
        }

        public void MoveToAnotherDocument(object param)
        {
            string finalLink = string.Empty;

            if (param is NavigationItem nav && nav != null)
            {
                CurrentNavigation = nav;
                FindAround(nav, Navigation);

                PreviousEnabled = PreviousNavigation != null;
                NextEnabled = NextNavigation != null;

                finalLink = nav.Link;

                CurrentChapterReadingProgress = 0;
                OffsetReminder = 0;
                ReturnEnabled = false;

                var document = Book.LoadChapter(finalLink);
                SelectedChapterHtml = document;

                ChapterChanged?.Invoke(OffsetReminder);

                double pos = LookFor(nav, Navigation);
                double len = GetCollectionLength(Navigation);
                ReadingProgress = (pos/len) * 100D;
            }
            else if (param is string link)
            {
                if (!link.StartsWith("http"))
                {
                    finalLink = string.Join(string.Empty, link.Take(link.LastIndexOf('l') + 1));
                    ReturnEnabled = true;
                    OffsetReminder = ScrollOffset;
                    var document = Book.LoadChapter(finalLink);
                    SelectedChapterHtml = document;
                    ChapterChanged?.Invoke();
                }
                else
                {
                    Process.Start(link);
                    return;
                }
            }

            QuoteLocation = CurrentNavigation.Header.ToString();
        }

        private int GetCollectionLength(List<NavigationItem> collection)
        {
            int count = 0;

            foreach (var item in collection)
            {
                if (item.ItemsSource != null) count += GetCollectionLength(item.ItemsSource as List<NavigationItem>);
                count++;
            }

            return count; 
        }

        private int LookFor(NavigationItem item, List<NavigationItem> collection, int position = 0)
        {
            foreach (var item1 in collection)
            {
                position++;

                if (item1 == item)
                {
                    return position; 
                }

                if (item1.ItemsSource != null)
                {
                    int positionInNestedCollection = LookFor(item, item1.ItemsSource as List<NavigationItem>, position + 1);

                    if (positionInNestedCollection != -1)
                    {
                        return positionInNestedCollection;
                    }

                    position += (item1.ItemsSource as List<NavigationItem>).Count;
                }
            }

            return -1;
        }

        private void FindAround(NavigationItem nav, List<NavigationItem> collection, List<NavigationItem> previousCollection = null)
        {
            NavigationItem previous = null;
            NavigationItem next = null;

            foreach (var item in collection)
            {
                if (nav == item)
                {
                    int index = collection.IndexOf(nav);

                    if (index - 1 > (-1))
                        previous = collection[index - 1];
                    else if (previousCollection != null)
                    {
                        NavigationItem previousItem = previousCollection.Where(c => (c.ItemsSource != null) && (c.ItemsSource as List<NavigationItem>).Contains(item)).First();
                        previous = previousCollection[previousCollection.IndexOf(previousItem)];
                    }

                    if (index + 1 < collection.Count && item.ItemsSource == null)
                        next = collection[index + 1];
                    else if (item.ItemsSource != null) next = ((List<NavigationItem>)item.ItemsSource)[0];

                    if (previous != null || next != null)
                    {
                        PreviousNavigation = previous;
                        NextNavigation = next;
                        return;
                    }
                }

                if (item.ItemsSource != null)
                    FindAround(nav, (List<NavigationItem>)item.ItemsSource, collection);
            }
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
            if (CurrentNavigation != null)
                Book.LastReadingPosition = new Position(Book.Guid, CurrentNavigation.Link, ScrollOffset);
        }

        public double OnWindowLoaded()
        {
            if (Book.LastReadingPosition != null) return Book.LastReadingPosition.Offset;
            return 0;
        }

        public void Return()
        {
            MoveToAnotherDocument(CurrentNavigation);
        }

        public async void AddQuote()
        {
            Quote toAdd = new Quote(QuoteData, Book.BoundDiary.Guid, QuoteAuthor, QuoteLocation);

            if (toAdd.Author != string.Empty && toAdd.Author != string.Empty)
            {
                Book.BoundDiary.Quotes.Add(toAdd);
                await toAdd.CreateAsync();
                QuoteData = string.Empty;
                QuoteAuthor = string.Empty;
                DialogHost.Close("ReadingDialog");
            }
            else
                new DialogWindow(Application.Current.Resources["dialog_error_title"].ToString(),
                    Application.Current.Resources["dialog_null_quote_content"].ToString(), Visibility.Hidden)
                    .ShowDialog();
        }

        public void ClearMode()
        {
            IsClearMode = !IsClearMode;

            if (IsClearMode)
            {
                ChaptersViewLength = new GridLength(0, GridUnitType.Star);
                SplitterVisibility = Visibility.Collapsed;
            }
            else
            {
                ChaptersViewLength = new GridLength(0.3, GridUnitType.Star);
                SplitterVisibility = Visibility.Visible;
            }
        }

        public void ConcentrationMode()
        {
            IsConcentrationMode = !IsConcentrationMode;

            if (IsConcentrationMode)
            {
                ConcentrationRectanglesVisibility = Visibility.Visible;
                if (Properties.Settings.Default.ShowInteractionHints) ConcentrationHintsVisibility = Visibility.Visible;
            }
            else 
            { 
                ConcentrationRectanglesVisibility = Visibility.Collapsed;
                ConcentrationHintsVisibility = Visibility.Collapsed;
            }
        }
    }
}
