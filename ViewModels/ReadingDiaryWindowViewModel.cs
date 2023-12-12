using MaterialDesignThemes.Wpf;
using ReedBooks.Core;
using ReedBooks.Models.Assessment;
using ReedBooks.Models.Book;
using ReedBooks.Models.Diary;
using ReedBooks.Views;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
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

        private ObservableCollection<string> _genres;
        public ObservableCollection<string> Genres
        {
            get => _genres;
            set
            {
                _genres = value;
                OnPropertyChanged(nameof(Genres));
            }
        }

        public ICommand SetBeginEmoteCommand { get; }
        public ICommand SetMiddleEmoteCommand { get; }
        public ICommand SetEndEmoteCommand { get; }
        public ICommand AddQuoteCommand { get; }
        public ICommand DeleteQuoteCommand { get; }

        public ReadingDiaryWindowViewModel()
        {
            SetBeginEmoteCommand = new RelayCommand(obj => SetBeginEmote(obj));
            SetMiddleEmoteCommand = new RelayCommand(obj => SetMiddleEmote(obj));
            SetEndEmoteCommand = new RelayCommand(obj => SetEndEmote(obj));
            AddQuoteCommand = new RelayCommand(obj => AddQuote());
            DeleteQuoteCommand = new RelayCommand(obj => DeleteQuote(obj));

            QuoteData = string.Empty;
            QuoteAuthor = string.Empty;
            QuoteLocation = string.Empty;
        }

        public ReadingDiaryWindowViewModel(Book book) : this()
        {
            Book = book;

            Genres = new ObservableCollection<string>();
            var localeDictionary = Application.Current.Resources.MergedDictionaries.Where(d => d.Source.OriginalString.Contains("lang")).First();

            foreach (var key in localeDictionary.Keys)
            {
                if(key.ToString().StartsWith("genre"))
                {
                    Genres.Add(Application.Current.Resources[key].ToString());
                }
            }

            Genres = new ObservableCollection<string>(Genres.OrderBy(g => g));
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

        public async void AddQuote()
        {
            Quote toAdd = new Quote(QuoteData, Book.BoundDiary.Guid, QuoteAuthor, QuoteLocation);

            if (toAdd.Author != string.Empty && toAdd.Author != string.Empty)
            {
                Book.BoundDiary.Quotes.Add(toAdd);
                await toAdd.CreateAsync();
                QuoteData = string.Empty;
                QuoteAuthor = string.Empty;
                QuoteLocation = string.Empty;
                DialogHost.Close("ReadingDiaryDialog");
            }
            else
                new DialogWindow(Application.Current.Resources["dialog_error_title"].ToString(), 
                    Application.Current.Resources["dialog_null_quote_content"].ToString(), Visibility.Hidden)
                    .ShowDialog();
        }

        public async void DeleteQuote(object param)
        {
            Quote toDelete = await App.ApplicationContext.FindAsync<Quote>(param);
            Book.BoundDiary.Quotes.Remove(toDelete);
            await App.ApplicationContext.RemoveEntityAsync(toDelete);
            DialogHost.Close("ReadingDiaryDialog");
        }
    }
}
