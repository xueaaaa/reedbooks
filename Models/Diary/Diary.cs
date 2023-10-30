using ReedBooks.Core;
using ReedBooks.Models.Assessment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
                OnPropertyChanged(nameof(PlotBriefRetelling));
            }
        }

        private List<Quote> _quotes;
        public List<Quote> Quotes
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
            Guid = Guid.NewGuid();
            EmotionalAssessment = new EmotionalAssessment();
            BookAssessment = new BookAssessment();
            Quotes = new List<Quote>();
        }
    }
}
