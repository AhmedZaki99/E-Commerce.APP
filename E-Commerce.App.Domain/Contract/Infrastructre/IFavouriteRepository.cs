using E_Commerce.App.Domain.Entities.Favourite;

namespace E_Commerce.App.Domain.Contract.Infrastructre
{
    public interface IFavouriteRepository
    {
        Task<Favourites?> GetAsync(string id);

        Task<Favourites?> UpdateAsync(Favourites favourite, TimeSpan timeToLive);

        Task<bool> DeleteAsync(string id);
    }
}
