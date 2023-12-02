using ReedBooks.Models.Assessment;
using ReedBooks.Models.Database;
using System;
using System.Collections.ObjectModel;

namespace ReedBooks.Models.Diary
{
    public class ReadingDiary : DependentDatabaseObject
    {
        private DateTime _beginReadingAt;
        public DateTime BeginReadingAt
        {
            get => _beginReadingAt;
            set
            {
                _beginReadingAt = value;
                App.ApplicationContext.UpdateEntity(this);
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
                Update();
                OnPropertyChanged(nameof(EndReadingAt));
            }
        }

        private DateTime _lastReadingAt;
        public DateTime LastReadingAt
        {
            get => _lastReadingAt;
            set
            {
                _lastReadingAt = value;
                Update();
                OnPropertyChanged(nameof(LastReadingAt));
            }
        }

        private EmotionalAssessment _emotionalAssessment;
        public EmotionalAssessment EmotionalAssessment
        {
            get => _emotionalAssessment;
            set
            {
                _emotionalAssessment = value;
                Update();
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
                Update();
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
                Update();
                OnPropertyChanged(nameof(PlotBriefRetelling));
            }
        }

        private bool _readingIsOver;
        public bool ReadingIsOver
        {
            get => _readingIsOver;
            set
            {
                _readingIsOver = value;
                Update();
                OnPropertyChanged(nameof(ReadingIsOver));
            }
        }

        private ObservableCollection<Quote> _quotes;
        public ObservableCollection<Quote> Quotes
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
            
        }

        public ReadingDiary(Guid bookGuid) : this()
        {
            TargetGuid = bookGuid;
            Create();
            _emotionalAssessment = new EmotionalAssessment(Guid);
            _bookAssessment = new BookAssessment(Guid);
            _quotes = new ObservableCollection<Quote>();
            _readingIsOver = false;
        }

        public void SetLastReadingDate()
        {
            LastReadingAt = DateTime.Now;
        }
    }
}
