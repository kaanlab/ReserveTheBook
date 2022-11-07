using Microsoft.EntityFrameworkCore;
using ReserveTheBook.Database.Models;

namespace ReserveTheBook.Database
{
    public sealed class AppDbContext : DbContext
    {
        public AppDbContext()
            : base() { }

        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options) { }

        public DbSet<AuthorDb> Authors { get; set; }
        public DbSet<BookDb> Books { get; set; }
        public DbSet<ReservationDb> Reservations { get; set; }
    }
}
