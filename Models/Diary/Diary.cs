using ReedBooks.Core;
using ReedBooks.Models.Assessment;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ReedBooks.Models.Diary
{
    public class ReadingDiary : ObservableObject
    {
        private Guid _guid;
        [Key]
        public Guid Guid
        {
            get => _guid;
            set
            {
                if (value != null)
                {
                    _guid = value;
                    OnPropertyChanged(nameof(Guid));
                }
            }
        }

        private DateTime _beginReadingAt;
        public DateTime BeginReadingAt
        {
            get => _beginReadingAt;
            set
            {
                _beginReadingAt = value;
                App.ApplicationContext.UpdateEnitity(this);
                OnPropertyChanged(nameof(BeginReadingAt));
            }
        }

        private DateTime _endReadingAt;
        public DateTime EndReadingAt
        {
            get => _endReadingAt;
            set
            {
                _endReadingAt = value;
                App.ApplicationContext.UpdateEnitity(this);
                OnPropertyChanged(nameof(EndReadingAt));
            }
        }

        private EmotionalAssessment _emotionalAssessment;
        public EmotionalAssessment EmotionalAssessment
        {
            get => _emotionalAssessment;
            set
            {
                _emotionalAssessment = value;
                App.ApplicationContext.UpdateEnitity(this);
                OnPropertyChanged(nameof(EmotionalAssessment));
            }
        }

        private BookAssessment _bookAssessment;
        public BookAssessment BookAssessment
        {
            get => _bookAssessment;
            set
            {
                _bookAssessment = value;
                App.ApplicationContext.UpdateEnitity(this);
                OnPropertyChanged(nameof(BookAssessment));
            }
        }

        private string _plotBriefRetelling;
        public string PlotBriefRetelling
        {
            get => _plotBriefRetelling;
            set
            {
                _plotBriefRetelling = value;
                App.ApplicationContext.UpdateEnitity(this);
                OnPropertyChanged(nameof(PlotBriefRetelling));
            }
        }

        private ObservableCollection<Quote> _quotes;
        [NotMapped] public ObservableCollection<Quote> Quotes
        {
            get => _quotes;
            set
            {
                _quotes = value;
                OnPropertyChanged(nameof(Quotes));
            }
        }

        public ReadingDiary()
        {
            Quotes = new ObservableCollection<Quote>(App.ApplicationContext.Quotes.Where(q => q.DiaryGuid == this.Guid));
        }

        public static ReadingDiary Create()
        {
            var diary = new ReadingDiary();
            diary._guid = Guid.NewGuid();
            diary._emotionalAssessment = new EmotionalAssessment();
            diary._bookAssessment = new BookAssessment();
            diary._quotes = new ObservableCollection<Quote>();

            return diary;
        }
    }
}
