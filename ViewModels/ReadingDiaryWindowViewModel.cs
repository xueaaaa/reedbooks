using ReedBooks.Core;
using ReedBooks.Models.Assessment;
using ReedBooks.Models.Book;
using ReedBooks.Models.Diary;
using System.Windows.Input;

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

        public ICommand SetBeginEmoteCommand { get; }
        public ICommand SetMiddleEmoteCommand { get; }
        public ICommand SetEndEmoteCommand { get; }

        public ReadingDiaryWindowViewModel()
        {
            
        }

        public ReadingDiaryWindowViewModel(Book book)
        {
            SetBeginEmoteCommand = new RelayCommand(obj => SetBeginEmote(obj));
            SetMiddleEmoteCommand = new RelayCommand(obj => SetMiddleEmote(obj));
            SetEndEmoteCommand = new RelayCommand(obj => SetEndEmote(obj));

            if (book.BoundDiary == null)
                book.BoundDiary = ReadingDiary.Create();
            Book = book;
        }

        public void SetBeginEmote(object param)
        {
            Book.BoundDiary.EmotionalAssessment.Start = (Emote)param;
        }

        public void SetMiddleEmote(object param)
        {
            Book.BoundDiary.EmotionalAssessment.Middle = (Emote)param;
        }

        public void SetEndEmote(object param)
        {
            Book.BoundDiary.EmotionalAssessment.End = (Emote)param;
        }
    }
}
