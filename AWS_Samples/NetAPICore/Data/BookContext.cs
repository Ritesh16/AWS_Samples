using Microsoft.EntityFrameworkCore;
using NetAPICore.Entities;

namespace NetAPICore.Data
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options) : base(options)
        {

        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>().HasData(
                new Author { AuthorId = 1, Name = "Sanjay Gupta" },
                new Author { AuthorId = 2, Name = "Vince Kelly" },
                new Author { AuthorId = 3, Name = "Minda Harts" },
                new Author { AuthorId = 4, Name = "Susanne Tedrick" }
                );

            modelBuilder.Entity<Book>().HasData(
                new Book { BookId = 1, AuthorId = 1, Title = "Keep Sharp" },
                new Book { BookId = 2, AuthorId = 1, Title = "Healthy Habits" },
                new Book { BookId = 3, AuthorId = 2, Title = "Call of Duty" },
                new Book { BookId = 4, AuthorId = 3, Title = "Women of Color in Tech" },
                new Book { BookId = 5, AuthorId = 4, Title = "No books" }
                );

        }
    }
}
