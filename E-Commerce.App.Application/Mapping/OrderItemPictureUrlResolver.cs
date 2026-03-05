using AutoMapper;
using E_Commerce.App.Application.Abstruction.Models.Orders;
using E_Commerce.App.Domain.Entities.Order;
using Microsoft.Extensions.Configuration;

namespace E_Commerce.App.Application.Mapping
    {
        public class OrderItemPictureUrlResolver(IConfiguration configuration) : IValueResolver<OrderItem, OrderItemDto, string>
        {
            public string Resolve(OrderItem source, OrderItemDto destination, string? destMember, ResolutionContext context)
            {
                if (!string.IsNullOrEmpty(source.Product.PictureUrl))
                    return $"{configuration["Urls:ApiBaseUrl"]}/{source.Product.PictureUrl}";
                    
                return string.Empty ;
            }
        }
    }
