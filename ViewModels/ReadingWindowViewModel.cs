using ReedBooks.Core;
using ReedBooks.Models.Book;
using ReedBooks.Views.Controls;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Windows.Input;
using VersOne.Epub;

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

        private FlowDocument _selectedFlowDocument;
        public FlowDocument SelectedFlowDocument
        {
            get => _selectedFlowDocument;
            set
            {
                if(value != null)
                {
                    _selectedFlowDocument = value;
                    OnPropertyChanged(nameof(SelectedFlowDocument));
                }
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

        public ICommand MoveToAnotherDocumentCommand { get; }

        public ReadingWindowViewModel()
        {
            MoveToAnotherDocumentCommand = new RelayCommand(obj => MoveToAnotherDocument(obj));
        }

        public ReadingWindowViewModel(Book readingBook) : this()
        {
            Book = readingBook;
            Navigation = Book.LoadNavigation();
        }

        public void MoveToAnotherDocument(object param)
        {
            var document = Book.LoadChapter(param.ToString());
            SelectedFlowDocument = document;
        }
    }
}
