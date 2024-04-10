using Microsoft.EntityFrameworkCore;
using QuickLinker.API.Entities;

namespace QuickLinker.API.DbContexts
{
    public class QuickLinkerDbContext : DbContext
    {
        public QuickLinkerDbContext(DbContextOptions<QuickLinkerDbContext> options) : base(options)
        {
        }

        public DbSet<ShortenedURL> ShortenedURLs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // seed the database with dummy data
            modelBuilder.Entity<ShortenedURL>().HasData(
                new ShortenedURL("12345abcde", "www.google.com")
                {
                    ID = -1
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
