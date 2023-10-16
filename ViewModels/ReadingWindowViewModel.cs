using ReedBooks.Core;
using ReedBooks.Models.Book;
using System.Windows;

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

        public ReadingWindowViewModel()
        {
            ChaptersListBoxLength = new GridLength(0.3, GridUnitType.Star);
        }

        public ReadingWindowViewModel(Book readingBook) : base()
        {
            ReadingBook = readingBook;
        }
    }
}
