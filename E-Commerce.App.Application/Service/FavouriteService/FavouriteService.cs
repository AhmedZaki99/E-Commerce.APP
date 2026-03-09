using AutoMapper;
using E_Commerce.App.Application.Abstruction.Models.Favourite;
using E_Commerce.App.Application.Abstruction.Services.Favourite;
using E_Commerce.App.Application.Exception;
using E_Commerce.App.Domain.Contract.Infrastructre;
using E_Commerce.App.Domain.Entities.Favourite;
using Microsoft.Extensions.Configuration;

namespace E_Commerce.App.Application.Service.FavouriteService
{
    public class FavouriteService(IFavouriteRepository favouriteRepository, IMapper mapper, IConfiguration configuration) : IFavouriteService
    {
        public async Task<FavouriteItemDto> AddFavouriteAsync(string buyerEmail, FavouriteItemDto item)
        {
            var favourite = await favouriteRepository.GetAsync(buyerEmail);

            if (favourite is null)
                favourite = new Favourites { Id = buyerEmail };

            var items = favourite.Items.ToList();

            var exist = items.FirstOrDefault(x => x.ProductId == item.ProductId);

            if (exist is null)
                items.Add(mapper.Map<FavouriteItem>(item));

            favourite.Items = items;

            var ttl = TimeSpan.FromDays(double.Parse(configuration["RedisSetting:TimeToLiveInDays"]!));

            await favouriteRepository.UpdateAsync(favourite, ttl);

            return item;
        }

        public async Task<IEnumerable<FavouriteItemDto>> GetFavouriteAsync(string buyerEmail)
        {
            var favourite = await favouriteRepository.GetAsync(buyerEmail);

            if (favourite is null)
                return Enumerable.Empty<FavouriteItemDto>();

            return mapper.Map<IEnumerable<FavouriteItemDto>>(favourite.Items);
        }

        public async Task<IEnumerable<FavouriteItemDto>> GetFavouriteByCategoryAsync(string buyerEmail, string category)
        {
            var favourite = await favouriteRepository.GetAsync(buyerEmail);

            if (favourite is null)
                throw new NotFoundException(buyerEmail,category);

            var items = favourite.Items
                .Where(x => x.Category!.ToLower() == category.ToLower());

            return mapper.Map<IEnumerable<FavouriteItemDto>>(items);
        }

        public async Task<string> RemoveProductAsync(string buyerEmail, int productId)
        {
            var favourite = await favouriteRepository.GetAsync(buyerEmail);

            if (favourite is null)
                throw new NotFoundException(buyerEmail,productId);

            var items = favourite.Items.ToList();

            var product = items.FirstOrDefault(x => x.ProductId == productId);

            if (product is null)
                throw new NotFoundException(buyerEmail, productId);

            items.Remove(product);

            favourite.Items = items;

            var ttl = TimeSpan.FromDays(double.Parse(configuration["RedisSetting:TimeToLiveInDays"]!));

            await favouriteRepository.UpdateAsync(favourite, ttl);

            return "Product removed from favourite successfully";
        }
    }
}
