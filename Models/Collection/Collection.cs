using ReedBooks.Core;
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
    public class Collection : ObservableObject, IEnumerable
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
        public List<string> LinkedBooks
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

        public Collection(string name)
        {
            Guid guid = Guid.NewGuid();
            _linkedBooks = new List<string>();
            Name = name;
        }

        /// <summary>
        /// Creates a collection, adds it to the database, and returns the created instance
        /// </summary>
        /// <param name="name">Name of collection</param>
        /// <param name="books">List of books guid</param>
        /// <returns>Collection</returns>
        public static async Task<Collection> Create(string name, List<Guid> books)
        {
            var collection = new Collection(name);

            foreach (var item in books) collection.LinkedBooks.Add(item.ToString());

            await App.ApplicationContext.AddEntityAsync(collection);

            return collection;
        }
    }
}
