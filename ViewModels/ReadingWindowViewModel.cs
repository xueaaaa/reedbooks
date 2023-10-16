using ReedBooks.Core;
using ReedBooks.Models.Book;
using System.Windows;
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

        public ReadingWindowViewModel()
        {
            ChaptersListBoxLength = new GridLength(0.3, GridUnitType.Star);
        }

        public ReadingWindowViewModel(Book readingBook) : base()
        {
            ReadingBook = readingBook;
            _epubBook = ReadingBook.GetEpub();
        }
    }
}
