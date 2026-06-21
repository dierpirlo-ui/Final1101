using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ReadCity.Data.Models;

namespace ReadCity.Data.Context
{
    /// <summary>
    /// Контекст базы данных для работы с SQL Server.
    /// Содержит все сущности и настройки связей между таблицами.
    /// </summary>
    public class ReadCityDbContext : DbContext
    {
        /// <summary>
        /// Таблица ролей пользователей.
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        /// <summary>
        /// Таблица производителей товаров.
        /// </summary>
        public DbSet<Manufacturer> Manufacturers { get; set; }

        /// <summary>
        /// Таблица категорий товаров.
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Таблица статусов заказов.
        /// </summary>
        public DbSet<OrderStatus> OrderStatuses { get; set; }

        /// <summary>
        /// Таблица пользователей системы.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Таблица товаров в каталоге.
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Таблица заказов.
        /// </summary>
        public DbSet<OrderFinal> OrderFinals { get; set; }

        /// <summary>
        /// Таблица позиций в заказе.
        /// </summary>
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = GetConnectionString();
            optionsBuilder.UseSqlServer(connectionString);
        }

        private string GetConnectionString()
        {
            try
            {
                var basePath = AppDomain.CurrentDomain.BaseDirectory;

                var configuration = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");

                if (string.IsNullOrEmpty(connectionString))
                    throw new Exception("Строка подключения не найдена");
                return connectionString;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка чтения appsettings.json: {ex.Message}");
                return "Server=KIRTIM\\mssql;Database=ispp4103;Trusted_Connection=True;TrustServerCertificate=True;";
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<Manufacturer>().ToTable("Manufacturer");
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<OrderStatus>().ToTable("OrderStatus");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<OrderFinal>().ToTable("OrderFinal");
            modelBuilder.Entity<OrderItem>().ToTable("OrderItem");

            modelBuilder.Entity<Role>()
                .HasKey(r => r.RoleId);

            modelBuilder.Entity<Manufacturer>()
                .HasKey(m => m.ManufacturerId);

            modelBuilder.Entity<Category>()
                .HasKey(c => c.CategoryId);

            modelBuilder.Entity<OrderStatus>()
                .HasKey(os => os.StatusId);

            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<Product>()
                .HasKey(p => p.ProductId);

            modelBuilder.Entity<OrderFinal>()
                .HasKey(o => o.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasKey(oi => oi.OrderItemId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);

            modelBuilder.Entity<OrderFinal>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<OrderFinal>()
                .HasOne(o => o.Status)
                .WithMany()
                .HasForeignKey(o => o.StatusId);
        }
    }
}
