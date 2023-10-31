using ReedBooks.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace ReedBooks.Models.Diary
{
    public class Quote : ObservableObject
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

        private string _data;
        public string Data
        {
            get => _data;
            set
            {
                _data = value;
                App.ApplicationContext.UpdateEnitity(this);
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
                App.ApplicationContext.UpdateEnitity(this);
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
                App.ApplicationContext.UpdateEnitity(this);
                OnPropertyChanged(nameof(LocationInBook));
            }
        }

        public Quote(string data)
        {
            Guid = Guid.NewGuid();
            Data = data;
        }

        public Quote(string data, string author) : this(data)
        {
            Author = author;
        }

        public Quote(string data, string author, string locationInBook) : this(data, author)
        {
            LocationInBook = locationInBook;
        }
    }
}
