using ReedBooks.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace ReedBooks.Models.Assessment
{
    public class BookAssessment : ObservableObject
    {
        private const ushort ASSESMENTS_TOTAL_NUMBER = 6;

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

        private ushort _plotOriginality;
        [Range(1, 5)]
        public ushort PlotOriginality
        {
            get => _plotOriginality;
            set
            {
                _plotOriginality = value;
                OnPropertyChanged(nameof(PlotOriginality));
            }
        }

        private ushort _characters;
        [Range(1, 5)]
        public ushort Characters
        {
            get => _characters;
            set
            {
                _characters = value;
                OnPropertyChanged(nameof(Characters));
            }
        }

        private ushort _worldInsideBook;
        [Range(1, 5)]
        public ushort WorldInsideBook
        {
            get => _worldInsideBook;
            set
            {
                _worldInsideBook = value;
                OnPropertyChanged(nameof(WorldInsideBook));
            }
        }

        private ushort _loveLine;
        [Range(1, 5)]
        public ushort LoveLine
        {
            get => _loveLine;
            set
            {
                _loveLine = value;
                OnPropertyChanged(nameof(LoveLine));
            }
        }

        private ushort _humor;
        [Range(1, 5)]
        public ushort Humor
        {
            get => _humor;
            set
            {
                _humor = value;
                OnPropertyChanged(nameof(Humor));
            }
        }

        private ushort _meaningFulness;
        [Range(1, 5)]
        public ushort MeaningFulness
        {
            get => _meaningFulness;
            set
            {
                _meaningFulness = value;
                OnPropertyChanged(nameof(MeaningFulness));
            }
        }

        public BookAssessment() 
        {
            Guid = Guid.NewGuid();
        }

        /// <summary>
        /// Calculates the book's overall grade based on 6 individual grades, rounded to tenths
        /// </summary>
        /// <returns></returns>
        public float CalculateArithmeticAverage()
        {
            float rawAverage = (PlotOriginality 
                + Characters 
                + WorldInsideBook 
                + LoveLine 
                + Humor 
                + Meaningfulness) 
                / ASSESMENTS_TOTAL_NUMBER;

            return (float)Math.Round(rawAverage, 1);
        }
    }
}
