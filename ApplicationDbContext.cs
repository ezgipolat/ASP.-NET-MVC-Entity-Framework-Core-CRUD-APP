using Microsoft.EntityFrameworkCore;
using ServerInventoryApp.Models;

namespace ServerInventoryApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Server> Servers { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Kategorileri manuel olarak ekleyelim
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Web Sunucuları" },
                new Category { Id = 2, Name = "Veritabanı Sunucuları" },
                new Category { Id = 3, Name = "Uygulama Sunucuları" }
            );
        }
    }
}