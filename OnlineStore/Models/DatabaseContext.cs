using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;

namespace OnlineStore
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Замените "ваша строка подключения" на фактическую строку подключения к вашей базе данных
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-KVMUOKD\\SQLEXPRESS;Initial Catalog=OnlineStoreDB;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Здесь вы можете добавить дополнительные настройки модели, если необходимо
        }
    }
}
