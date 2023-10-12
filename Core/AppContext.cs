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

        public async Task<int> AddEntityAsync<T>(T enitity)
        {
            await AddAsync(enitity);
            return await SaveChangesAsync();
        }

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
