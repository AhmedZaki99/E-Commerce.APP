using E_Commerce.App.Domain.Contract.Peresistence;
using E_Commerce.App.Domain.Contract.Peresistence.DbIntializer;
using E_Commerce.App.Infrastructre.presistent._Data;
using E_Commerce.App.Infrastructre.presistent._Data.Interceptor;
using E_Commerce.App.Infrastructre.presistent._Identity;
using E_Commerce.App.Infrastructre.presistent.Identity;
using E_Commerce.App.Infrastructre.presistent.Repositieries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.App.Infrastructre.presistent
{
    public static class DependencyInjection 
    {
        public static IServiceCollection AddPersistenceService(this IServiceCollection services,
                                                         IConfiguration configuration)
        {
            services.AddDbContext<StoreDbContext>(options => 
            
            options
            .UseLazyLoadingProxies()
            .UseSqlServer(configuration.GetConnectionString("StoreContext")));
        
            services.AddScoped< IStroreContextIntializer , StoreContextIntializer>();


            services.AddDbContext<StorIdentityDbContext>(options =>

            options
            .UseLazyLoadingProxies()
            .UseSqlServer(configuration.GetConnectionString("IdentityContext")));
            services.AddScoped<IStoreIdentityContextIntializer, StoreIdentityContextIntializer>();

            services.AddScoped(typeof(IUnitOfWork) , typeof(UnitOfWork));



            services.AddScoped(typeof(ISaveChangesInterceptor), typeof(CustomSaveChangesInterceptor));

            return services;
       }

    }
}
