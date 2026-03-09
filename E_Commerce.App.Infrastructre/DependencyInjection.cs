using E_Commerce.App.Domain.Contract.Infrastructre;
using E_Commerce.App.Infrastructre.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace E_Commerce.App.Infrastructre
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
              services.AddSingleton(typeof(IConnectionMultiplexer), (serviceProvider) =>
              {
                  var connectionString = configuration.GetConnectionString("Redis");
                  var connectionMultiplexerObj = ConnectionMultiplexer.Connect(connectionString!);
                  return connectionMultiplexerObj;
              });
            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            services.AddScoped(typeof(IFavouriteRepository), typeof(FavouriteRepository));

            return services;
        }
    }
}