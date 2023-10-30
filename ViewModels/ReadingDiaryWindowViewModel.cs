using ReedBooks.Core;
using ReedBooks.Models.Book;
using ReedBooks.Models.Diary;

namespace ReedBooks.ViewModels
{
    public class ReadingDiaryWindowViewModel : ObservableObject
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

        public ReadingDiaryWindowViewModel()
        {

        }

        public ReadingDiaryWindowViewModel(Book book)
        {
            if (book.BoundDiary == null)
                book.BoundDiary = new ReadingDiary();
            Book = book;
        }
    }
}
