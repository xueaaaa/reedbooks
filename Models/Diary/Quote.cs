using ReedBooks.Models.Database;
using System;

namespace ReedBooks.Models.Diary
{
    public class Quote : DependentDatabaseObject
    {
        private string _data;
        public string Data
        {
            get => _data;
            set
            {
                _data = value;
                Update();
                OnPropertyChanged(nameof(Data));
            }
        }

        private string _author;
        public string Author
        {
            get => _author;
            set
            {
                _author = value;
                Update();
                OnPropertyChanged(nameof(Author));
            }
        }

        private string _locationInBook;
        public string LocationInBook
        {
            get => _locationInBook;
            set
            {
                _locationInBook = value;
                Update();
                OnPropertyChanged(nameof(LocationInBook));
            }
        }

        public Quote()
        {
            Guid = Guid.NewGuid();
        }

        public Quote(string data, Guid diaryGuid) : this()
        {
            TargetGuid = diaryGuid;
            _data = data;
        }

        public Quote(string data, Guid diaryGuid, string author) : this(data, diaryGuid)
        {
            _author = author;
        }

        public Quote(string data, Guid diaryGuid, string author, string locationInBook) : this(data, diaryGuid, author)
        {
            _locationInBook = locationInBook;
        }
    }
}
