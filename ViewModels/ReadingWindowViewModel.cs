using ReedBooks.Core;
using ReedBooks.Models.Book;

namespace ReedBooks.ViewModels
{
    public class ReadingWindowViewModel : ObservableObject
    {
        public Book ReadingBook { get; set; }

        public ReadingWindowViewModel()
        {
        }

        public ReadingWindowViewModel(Book readingBook) 
        {
            ReadingBook = readingBook;
        }
    }
}
