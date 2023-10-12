using ReedBooks.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace ReedBooks.Models.Diary
{
    public class Quote : ObservableObject
    {
        [Key] public Guid Guid { get; set; }
        public string Data { get; set; }
        public string Author { get; set; }
        public string LocationInBook { get; set; }

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
