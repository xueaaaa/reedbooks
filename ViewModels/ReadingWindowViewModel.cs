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
        public Book ReadingBook { get; set; }

        private EpubBook _epubBook;
        public EpubBook EpubBook
        {
            get => _epubBook;
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
            ReadingBook = readingBook;
            _epubBook = ReadingBook.GetEpub();
            Navigation = BookContentLoader.LoadNavigation(EpubBook);

            MoveToAnotherDocumentCommand.Execute(_epubBook.ReadingOrder[0].FilePath);
        }

        public void MoveToAnotherDocument(object param)
        {
            var document = BookContentLoader.LoadChapterFromEpub(EpubBook, param.ToString());
            SelectedFlowDocument = document;
        }
    }
}
