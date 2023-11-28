using ReedBooks.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ReedBooks.Models.Collection
{
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

        private List<Guid> _linkedBooks;
        public List<Guid> LinkedBooks
        {
            get => _linkedBooks;
        }

        public Guid this[Guid bookGuid]
        {
            get => _linkedBooks.Where(b => b == bookGuid).FirstOrDefault();
            set
            {
                var book = _linkedBooks.Find(b => b == bookGuid);
                var intId = _linkedBooks.IndexOf(book);
                _linkedBooks[intId] = value;
            }
        }

        public IEnumerator GetEnumerator() => _linkedBooks.GetEnumerator();

        public Collection()
        {
            _linkedBooks = new List<Guid>();
        }
    }
}
