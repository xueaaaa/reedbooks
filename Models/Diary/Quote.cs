using ReedBooks.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace ReedBooks.Models.Diary
{
    public class Quote : ObservableObject
    {
        private Guid _guid;
        [Key] public Guid Guid
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

        private Guid _diaryGuid;
        public Guid DiaryGuid
        {
            get => _diaryGuid;
            set
            {
                if (value != null)
                {
                    _diaryGuid = value;
                    OnPropertyChanged(nameof(DiaryGuid));
                }
            }
        }

        private string _data;
        public string Data
        {
            get => _data;
            set
            {
                _data = value;
                App.ApplicationContext.UpdateEntity(this);
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
                App.ApplicationContext.UpdateEntity(this);
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
                App.ApplicationContext.UpdateEntity(this);
                OnPropertyChanged(nameof(LocationInBook));
            }
        }

        public Quote(string data, Guid diaryGuid)
        {
            Guid = Guid.NewGuid();
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
