using AutoMapper;
using E_Commerce.App.Application.Abstruction.Models.Product;
using E_Commerce.App.Domain.Entities.Product;
using Microsoft.Extensions.Configuration;

namespace E_Commerce.App.Application.Mapping
    {
        public class ProductPictureUrlResolver(IConfiguration configuration) : IValueResolver<Product, ProductToReturnDto, string?>
        {
            public string? Resolve(Product source, ProductToReturnDto destination, string? destMember, ResolutionContext context)
            {
                if (!string.IsNullOrEmpty(source.PictureUrl))
                    return $"{configuration["Urls:ApiBaseUrl"]}/{source.PictureUrl}";

                return string.Empty ;
            }

      
    }
    }
