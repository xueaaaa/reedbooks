using Microsoft.EntityFrameworkCore;
using ReedBooks.Models.Assessment;
using ReedBooks.Models.Book;
using ReedBooks.Models.Diary;
using System.Threading.Tasks;

namespace ReedBooks.Core
{
    public class AppContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Quote> Quotes { get; set; }

        /// <summary>
        /// Adds an item to the database and saves the changes
        /// </summary>
        /// <param name="enitity">Any entity for whose class there is a table in the database</param>
        /// <returns>The number of state entries written to the database</returns>
        public int AddEntity<T>(T enitity)
        {
            Add(enitity);
            return SaveChanges();
        }

        /// <summary>
        /// Asynchronously adds an item to the database and saves the changes
        /// </summary>
        /// <param name="enitity">Any entity for whose class there is a table in the database</param>
        /// <returns>The number of state entries written to the database</returns>
        public async Task<int> AddEntityAsync<T>(T enitity)
        {
            await AddAsync(enitity);
            return await SaveChangesAsync();
        }

        /// <summary>
        /// Updates an item in the database and saves the changes
        /// </summary>
        /// <param name="enitity">Any entity whose key matches any entry in the database</param>
        /// <returns>The number of state entries written to the database</returns>
        public int UpdateEntity<T>(T enitity)
        {
            Update(enitity);
            return SaveChanges();
        }

        /// <summary>
        /// Asynchronously updates an item in the database and saves the changes
        /// </summary>
        /// <param name="enitity">Any entity whose key matches any entry in the database</param>
        /// <returns>The number of state entries written to the database</returns>
        public async Task<int> UpdateEntityAsync<T>(T enitity)
        {
            Update(enitity);
            return await SaveChangesAsync();
        }

        /// <summary>
        /// Deletes an item from the database and saves the changes
        /// </summary>
        /// <param name="enitity">Any entity that matches any record in the database</param>
        /// <returns>The number of state entries written to the database</returns>
        public int RemoveEntity<T>(T enitity)
        {
            Remove(enitity);
            return SaveChanges();
        }

        /// <summary>
        /// Asynchronously deletes an item from the database and saves the changes
        /// </summary>
        /// <param name="enitity">Any entity that matches any record in the database</param>
        /// <returns>The number of state entries written to the database</returns>
        public async Task<int> RemoveEntityAsync<T>(T enitity)
        {
            Remove(enitity);
            return await SaveChangesAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("Data Source=reedbooks_data.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.BoundDiary)
                .WithOne()
                .HasForeignKey<ReadingDiary>(d => d.Guid);

            modelBuilder.Entity<ReadingDiary>()
                .HasOne(d => d.BookAssessment)
                .WithOne()
                .HasForeignKey<BookAssessment>(b => b.Guid);

            modelBuilder.Entity<ReadingDiary>()
                .HasOne(d => d.EmotionalAssessment)
                .WithOne()
                .HasForeignKey<EmotionalAssessment>(e => e.Guid);
        }
    }
}
