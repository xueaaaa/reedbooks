using ReedBooks.Models.Database;
using ReedBooks.Views.Controls;
using System;

namespace ReedBooks.Models.Book
{
    public class Position : DependentDatabaseObject
    {
        private NavigationItem _chapter;
        public NavigationItem Chapter
        {
            get => _chapter;
            set
            {
                _chapter = value;
                OnPropertyChanged(nameof(Chapter));
            }
        }

        private double _verticalOffset;
        public double VerticalOffset
        {
            get => _verticalOffset;
            set
            {
                _verticalOffset = value;
                OnPropertyChanged(nameof(VerticalOffset));
            }
        }

        public Position()
        {
            
        }

        public Position(Guid targetGuid, NavigationItem chapter, double verticalOffset) : this()
        {
            Guid = new Guid();
            TargetGuid = targetGuid;
            Chapter = chapter;
            VerticalOffset = verticalOffset;
        }
    }
}
