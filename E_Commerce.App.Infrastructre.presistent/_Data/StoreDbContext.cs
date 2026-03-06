using E_Commerce.App.Domain.Entities.Order;
using E_Commerce.App.Domain.Entities.Product;
using E_Commerce.App.Infrastructre.presistent.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Reflection;

namespace E_Commerce.App.Infrastructre.presistent._Data
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.ApplyConfigurationsFromAssembly(typeof(StoreDbContext).Assembly,
               type => type.GetCustomAttribute<DbContxtTypeAttribute>()?.DbContextType == typeof(StoreDbContext));
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Vendor> vendors { get; set; }
        public DbSet<ProductCategory> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }

    }
}
