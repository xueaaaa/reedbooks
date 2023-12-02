using ReedBooks.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ReedBooks.Models.Database
{
    public abstract class DatabaseObject : ObservableObject
    {
        private Guid _guid;
        /// <summary>
        /// The unique identifier of an object in the database. Created only through the Create() method
        /// </summary>
        [Key] public Guid Guid
        {
            get => _guid;
            set
            {
                _guid = value;
                OnPropertyChanged(nameof(Guid));
            }
        }

        /// <summary>
        /// Assigns a new unique identifier and creates a new record in the database
        /// </summary>
        public virtual int Create()
        {
            Guid = Guid.NewGuid();

            return App.ApplicationContext.AddEntity(this);
        }

        public virtual Task<int> CreateAsync()
        {
            Guid = Guid.NewGuid();

            return App.ApplicationContext.AddEntityAsync(this);
        }

        /// <summary>
        /// Updates the database record associated with the current object key. 
        /// To update any information, the current value of the object property must be different from the one specified in the database
        /// </summary>
        public virtual int Update()
        {
            return App.ApplicationContext.UpdateEntity(this);
        }

        /// <summary>
        /// Deletes the database record associated with the current object key
        /// </summary>
        public virtual int Remove()
        {
            return App.ApplicationContext.RemoveEntity(this);
        }

        public virtual async Task<int> RemoveAsync()
        {
            return await App.ApplicationContext.RemoveEntityAsync(this);
        }
    }
}
