using Microsoft.EntityFrameworkCore;
using ReedBooks.Models.Book;

namespace ReedBooks.Core
{
    public class AppContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("Data Source=reedbooks_data.db");
        }
    }
}
