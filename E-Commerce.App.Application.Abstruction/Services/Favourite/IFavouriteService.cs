using E_Commerce.App.Application.Abstruction.Models.Favourite;

namespace E_Commerce.App.Application.Abstruction.Services.Favourite
{
    public interface IFavouriteService
    {
        Task<FavouriteItemDto> AddFavouriteAsync(string buyerEmail, FavouriteItemDto item);

        Task<IEnumerable<FavouriteItemDto>> GetFavouriteAsync(string buyerEmail);

        Task<IEnumerable<FavouriteItemDto>> GetFavouriteByCategoryAsync(string buyerEmail, string category);
        Task<string> RemoveProductAsync(string buyerEmail, int productId);
    }
}
