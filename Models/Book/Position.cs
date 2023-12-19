using ReedBooks.Models.Database;
using System;

namespace ReedBooks.Models.Book
{
    public class Position : DependentDatabaseObject
    {
        private string _link;
        public string Link
        {
            get => _link;
            set
            {
                _link = value;
                OnPropertyChanged(nameof(Link));
            }
        }

        private double _offset;
        public double Offset
        {
            get => _offset;
            set
            {
                _offset = value;
                OnPropertyChanged(nameof(Offset));
            }
        }

        public Position() { }

        public Position(Guid bookGuid, string link, double offset)
        {
            TargetGuid = bookGuid;
            Link = link;
            Offset = offset;

            Create();
        }
    }
}
