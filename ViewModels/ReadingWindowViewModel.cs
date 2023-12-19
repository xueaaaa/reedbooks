using MaterialDesignThemes.Wpf;
using ReedBooks.Core;
using ReedBooks.Models.Book;
using ReedBooks.Views;
using ReedBooks.Views.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace ReedBooks.ViewModels
{
    public class ReadingWindowViewModel : ObservableObject
    {
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

        public ICommand MoveToAnotherDocumentCommand { get; }
        public ICommand MarkAsReadCommand { get; }
        public ICommand OpenReadingDiaryCommand { get; }
        public ICommand OnWindowClosingCommand { get; }

        public ReadingWindowViewModel()
        {
            MoveToAnotherDocumentCommand = new RelayCommand(obj => MoveToAnotherDocument(obj));
            MarkAsReadCommand = new RelayCommand(obj => MarkAsRead(obj));
            OpenReadingDiaryCommand = new RelayCommand(obj => OpenReadingDiary());
            OnWindowClosingCommand = new RelayCommand(obj => OnWindowClosing());
        }

        public ReadingWindowViewModel(Book readingBook) : this()
        {
            Book = readingBook;
            Navigation = Book.LoadNavigation();

            if(Book.LastReadingPosition != null)
                MoveToAnotherDocument(Navigation.Where(n => n.Link == Book.LastReadingPosition.Link).First());
        }

        public void MoveToAnotherDocument(object param)
        {
            var nav = param as NavigationItem;
            CurrentNavigation = nav;
            var document = Book.LoadChapter(nav.Link);
            SelectedChapterHtml = document;
        }

        public void MarkAsRead(object param)
        {
            DialogHost.Show(param);
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
    }
}
