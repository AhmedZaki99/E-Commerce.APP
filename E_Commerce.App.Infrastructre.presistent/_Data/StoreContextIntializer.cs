using E_Commerce.App.Domain.Contract.Peresistence.DbIntializer;
using E_Commerce.App.Domain.Entities.Order;
using E_Commerce.App.Domain.Entities.Product;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace E_Commerce.App.Infrastructre.presistent._Data
{
    internal class StoreContextIntializer(StoreDbContext dbContext) : IStroreContextIntializer
    {
        public async Task SeedData(string ContenRootpath)
        {
            if (!dbContext.vendors.Any())
            {
                var path = Path.Combine(ContenRootpath, "Seeds", "Vendors.json");

                var VendorData = await File.ReadAllTextAsync(path);
                var Vendors = JsonSerializer.Deserialize<List<Vendor>>(VendorData);


                if (Vendors?.Count > 0)
                {

                    await dbContext.Set<Vendor>().AddRangeAsync(Vendors);
                    await dbContext.SaveChangesAsync();
                }
            }

            if (!dbContext.Categories.Any())
            {
                var path = Path.Combine(ContenRootpath, "Seeds", "Categories.json");

                var CategoriesData = await File.ReadAllTextAsync(path);
                var Categories = JsonSerializer.Deserialize<List<ProductCategory>>(CategoriesData);


                if (Categories?.Count > 0)
                {

                    await dbContext.Set<ProductCategory>().AddRangeAsync(Categories);
                    await dbContext.SaveChangesAsync();
                }
            }

            if (!dbContext.Products.Any())
            {
                var path = Path.Combine(ContenRootpath, "Seeds", "Products.json");

                var ProductsData = await File.ReadAllTextAsync(path);
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);


                if (Products?.Count > 0)
                {

                    await dbContext.Set<Product>().AddRangeAsync(Products);
                    await dbContext.SaveChangesAsync();
                }
            }


            if (!dbContext.DeliveryMethods.Any())
            {
                var path = Path.Combine(ContenRootpath, "Seeds", "delivery.json");

                var deliveryMethodsData = await File.ReadAllTextAsync(path);
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);


                if (deliveryMethods?.Count > 0)
                {

                    await dbContext.Set<DeliveryMethod>().AddRangeAsync(deliveryMethods);
                    await dbContext.SaveChangesAsync();
                }
            }

        }

        public async Task UpdateDateBase()
        {
            var PendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();

            if (PendingMigrations.Any())
                await dbContext.Database.MigrateAsync();
        }
    }
}
