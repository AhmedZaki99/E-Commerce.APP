using E_Commerce.App.Domain.Contract.Infrastructre;
using E_Commerce.App.Domain.Entities.Favourite;
using StackExchange.Redis;
using System.Text.Json;

namespace E_Commerce.App.Infrastructre.Repositories
{
    public class FavouriteRepository(IConnectionMultiplexer redis) : IFavouriteRepository
    {
        private readonly IDatabase _database = redis.GetDatabase();

        public async Task<Favourites?> GetAsync(string id)
        {
            var data = await _database.StringGetAsync(id);

            return data.IsNullOrEmpty
                ? null
                : JsonSerializer.Deserialize<Favourites>(data!);
        }

        public async Task<Favourites?> UpdateAsync(Favourites favourite, TimeSpan timeToLive)
        {
            var value = JsonSerializer.Serialize(favourite);

            var created = await _database.StringSetAsync(favourite.Id, value, timeToLive);

            if (created) return favourite;

            return null;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _database.KeyDeleteAsync(id);
        }
    }
}
