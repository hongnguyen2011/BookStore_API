using BookStore_API.Migrations;
using BookStore_API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStore_API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {

        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Book> BooksList { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Book>().HasData(new Book
            {
                Id = 1,
                Name = "Spring Roll",
                Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                Image = "https://redmangoimages.blob.core.windows.net/redmango/spring roll.jpg",
                Price = 7.99,
                Category = "Appetizer",
                SpecialTag = ""
            }, new Book
            {
                Id = 2,
                Name = "Idli",
                Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                Image = "https://redmangoimages.blob.core.windows.net/redmango/idli.jpg",
                Price = 8.99,
                Category = "Appetizer",
                SpecialTag = ""
            }, new Book
            {
                Id = 3,
                Name = "Panu Puri",
                Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                Image = "https://redmangoimages.blob.core.windows.net/redmango/panipuri.jpg",
                Price = 8.99,
                Category = "Appetizer",
                SpecialTag = "Best Seller"
            }, new Book
            {
                Id = 4,
                Name = "Hakka Noodles",
                Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                Image = "https://redmangoimages.blob.core.windows.net/redmango/hakka noodles.jpg",
                Price = 10.99,
                Category = "Entrée",
                SpecialTag = ""
            }, new Book
            {
                Id = 5,
                Name = "Sweet Rolls",
                Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                Image = "https://redmangoimages.blob.core.windows.net/redmango/sweet rolls.jpg",
                Price = 3.99,
                Category = "Dessert",
                SpecialTag = "Top Rated"
            });
        }
    }
}
