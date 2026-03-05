using AutoMapper;
using E_Commerce.App.Application.Abstruction.Models.Product;
using E_Commerce.App.Domain.Entities.Product;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.App.Application.Mapping
{
    public class VendorPictureUrlResloverInProduct(IConfiguration configuration) : IValueResolver<Product, ProductToReturnDto, string?>
    {
        public string? Resolve(Product source, ProductToReturnDto destination, string? destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.vendor!.PictureUrl))
                return null;
            return $"{configuration["URLs:ApiBaseUrl"]}/{source.vendor.PictureUrl}";

        }
    }
}
