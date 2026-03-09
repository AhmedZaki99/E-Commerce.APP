using AutoMapper;
using E_Commerce.App.Application.Abstruction.Models.Favourite;
using E_Commerce.App.Domain.Entities.Favourite;
using Microsoft.Extensions.Configuration;

namespace E_Commerce.App.Application.Mapping
{
    public class FavouritePictureUrlResolver(IConfiguration configuration) : IValueResolver<FavouriteItem, FavouriteItemDto, string?>
    {
        public string? Resolve(FavouriteItem source, FavouriteItemDto destination, string? destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
                return $"{configuration["Urls:ApiBaseUrl"]}/{source.PictureUrl}";

            return string.Empty;
        }
    }       
}
