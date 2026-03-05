using E_Commerce.App.Domain.Contract.Peresistence.DbIntializer;
using E_Commerce.App.Domain.Entities.Identity;
using E_Commerce.App.Infrastructre.presistent.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.App.Infrastructre.presistent._Identity
{
    internal class StoreIdentityContextIntializer(StorIdentityDbContext dbcontext, UserManager<ApplicationsUser> userManager) : IStoreIdentityContextIntializer
    {

        public async Task UpdateDateBase()
        {

            var PendingMigrations = await dbcontext.Database.GetPendingMigrationsAsync();

            if (PendingMigrations.Any())
                await dbcontext.Database.MigrateAsync();
        }

        public async Task SeedData()
        {
            if (!userManager.Users.Any())
            {
                var user = new ApplicationsUser()
                {
                    DisableName = "Ahmed Adel",
                    UserName = "AhmedAdel",
                    Email = "ahmedadel2002259@gmail.com",
                    PhoneNumber = "01281524956"
                };

                var result = await userManager.CreateAsync(user, "P@ssw0rd");

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine(error.Description);
                    }
                }
            }
        }
    }
}
