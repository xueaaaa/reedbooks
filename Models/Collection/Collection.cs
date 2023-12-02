using ReedBooks.Models.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace ReedBooks.Models.Collection
{
    /// <summary>
    /// A collection of uploaded books
    /// </summary>
    public class Collection : DatabaseObject, IEnumerable
    {
        private string _name;
        /// <summary>
        /// Name of collection
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value; 
                OnPropertyChanged(nameof(Name));
            }
        }

        private List<string> _linkedBooks;
        /// <summary>
        /// Books added to the collection (their identifiers)
        /// </summary>
        [NotMapped] public List<string> LinkedBooks
        {
            get => _linkedBooks;
            set
            {
                _linkedBooks = value;
                OnPropertyChanged(nameof(LinkedBooks));
            }
        }

        /// <summary>
        /// Books added to the collection converted to the Book class
        /// </summary>
        [NotMapped] public ObservableCollection<Book.Book> RepresentedAsBook
        {
            get
            {
                var books = new ObservableCollection<Book.Book>();

                foreach (var guid in LinkedBooks)
                    books.Add(App.ApplicationContext.Find<Book.Book>(Guid.Parse(guid)));

                return books;
            }
        }

        public IEnumerator GetEnumerator() => _linkedBooks.GetEnumerator();

        public Collection() { }
            
        public Collection(string name, List<Guid> books)
        {
            _linkedBooks = new List<string>();
            Name = name;
            foreach (var item in books) LinkedBooks.Add(item.ToString());

            Create();
        }
    }
}
