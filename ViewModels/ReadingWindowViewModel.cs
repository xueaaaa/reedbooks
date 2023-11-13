using ReedBooks.Core;
using ReedBooks.Models.Book;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using VersOne.Epub;

namespace ReedBooks.ViewModels
{
    public class ReadingWindowViewModel : ObservableObject
    {
        private GridLength _chaptersListBoxLength;
        public GridLength ChaptersListBoxLength
        {
            get => _chaptersListBoxLength;
            set
            {
                if(value != null)
                {
                    _chaptersListBoxLength = value;
                    OnPropertyChanged(nameof(ChaptersListBoxLength));
                }
            }
        }
        public Book ReadingBook { get; set; }

        private EpubBook _epubBook;
        public EpubBook EpubBook
        {
            get => _epubBook;
        }

        private FlowDocument _loadedFlowDocument;
        public FlowDocument LoadedFlowDocument
        {
            get => _loadedFlowDocument;
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

        public ICommand ChangeSelectedFlowDocumentCommand { get; }

        public ReadingWindowViewModel()
        {
            ChangeSelectedFlowDocumentCommand = new RelayCommand(obj => ChangeSelectedFlowDocument(obj));

            ChaptersListBoxLength = new GridLength(0.4, GridUnitType.Star);
        }

        public ReadingWindowViewModel(Book readingBook) : base()
        {
            ReadingBook = readingBook;
            _epubBook = ReadingBook.GetEpub();
            _loadedFlowDocument = FlowDocumentContentLoader.LoadFromEpubBook(EpubBook);
        }

        public void ChangeSelectedFlowDocument(object param)
        {
            int index = (int)param;
            var flowDocument = new FlowDocument();
            flowDocument.Blocks.Add(LoadedFlowDocument.Blocks.ElementAt(index));
            SelectedFlowDocument = flowDocument;
        }
    }
}
