using ReedBooks.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace ReedBooks.Models.Assessment
{
    /// <summary>
    /// A class for evaluating a book based on the parameters
    /// </summary>
    public class BookAssessment : ObservableObject
    {
        private const ushort ASSESMENTS_TOTAL_NUMBER = 6;

        public delegate void AssessmentChangedEventHandler();
        /// <summary>
        /// Called upon any change in any grade
        /// </summary>
        public event AssessmentChangedEventHandler AssessmentChanged;

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

        private double _plotOriginality;
        [Range(1, 5)] public double PlotOriginality
        {
            get => _plotOriginality;
            set
            {
                _plotOriginality = value;
                App.ApplicationContext.UpdateEntity(this);
                AssessmentChanged();
                OnPropertyChanged(nameof(PlotOriginality));
            }
        }

        private double _characters;
        [Range(1, 5)] public double Characters
        {
            get => _characters;
            set
            {
                _characters = value;
                App.ApplicationContext.UpdateEntity(this);
                AssessmentChanged();
                OnPropertyChanged(nameof(Characters));
            }
        }

        private double _worldInsideBook;
        [Range(1, 5)] public double WorldInsideBook
        {
            get => _worldInsideBook;
            set
            {
                _worldInsideBook = value;
                App.ApplicationContext.UpdateEntity(this);
                AssessmentChanged();
                OnPropertyChanged(nameof(WorldInsideBook));
            }
        }

        private double _loveLine;
        [Range(1, 5)] public double LoveLine
        {
            get => _loveLine;
            set
            {
                _loveLine = value;
                App.ApplicationContext.UpdateEntity(this);
                AssessmentChanged();
                OnPropertyChanged(nameof(LoveLine));
            }
        }

        private double _humor;
        [Range(1, 5)] public double Humor
        {
            get => _humor;
            set
            {
                _humor = value;
                App.ApplicationContext.UpdateEntity(this);
                AssessmentChanged();
                OnPropertyChanged(nameof(Humor));
            }
        }

        private double _meaningFulness;
        [Range(1, 5)] public double MeaningFulness
        {
            get => _meaningFulness;
            set
            {
                _meaningFulness = value;
                App.ApplicationContext.UpdateEntity(this);
                AssessmentChanged();
                OnPropertyChanged(nameof(MeaningFulness));
            }
        }

        /// <summary>
        /// The arithmetic mean calculated on the basis of all grades assigned
        /// </summary>
        public double ArithmeticAverage
        {
            get => CalculateArithmeticAverage();
        }

        public BookAssessment()
        {
            Guid = Guid.NewGuid();
            AssessmentChanged += OnAssessmentChanged;
        }

        /// <summary>
        /// Calculates the book's overall grade based on 6 individual grades, rounded to hundredths
        /// </summary>
        /// <returns></returns>
        public double CalculateArithmeticAverage()
        {
            double rawAverage = (PlotOriginality 
                + Characters 
                + WorldInsideBook 
                + LoveLine 
                + Humor 
                + MeaningFulness) 
                / ASSESMENTS_TOTAL_NUMBER;

            return Math.Round(rawAverage,2);
        }

        private void OnAssessmentChanged()
        {
            OnPropertyChanged(nameof(ArithmeticAverage));
        }
    }
}
