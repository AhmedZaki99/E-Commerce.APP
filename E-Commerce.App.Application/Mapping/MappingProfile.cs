using AutoMapper;
using E_Commerce.App.Application.Abstruction.Models.Basket;
using E_Commerce.App.Application.Abstruction.Models.Common;
using E_Commerce.App.Application.Abstruction.Models.Orders;
using E_Commerce.App.Application.Abstruction.Models.Product;
using E_Commerce.App.Domain.Entities.Basket;
using E_Commerce.App.Domain.Entities.Order;
using E_Commerce.App.Domain.Entities.Product;

namespace E_Commerce.App.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(dest => dest.vendor, opt => opt.MapFrom(src => src.vendor!.Name))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category!.Name))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<ProductPictureUrlResolver>())
                .ForMember(dest => dest.VendorPictureUrl, opt => opt.MapFrom<VendorPictureUrlResloverInProduct>());
           
            
            CreateMap<Vendor, VendorDto>()
                .ForMember(dest=> dest.PictureUrl,opt=>opt.MapFrom<VendorPictureUrlResolver>());
           
            CreateMap<ProductCategory, CategoryDto>()
                .ForMember(dest=> dest.PictureUrl, opt => opt.MapFrom<CategoeryPicturteUrlResolver>());
            
            CreateMap<BasketItem, BasketItemDto>().ReverseMap();
            CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();
            CreateMap<Order, OrderToReturneDto>().ForMember(dest => dest.DeliveryMethod, opt => opt.MapFrom(src => src.DeliveryMethod!.ShortName));
            
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.ProductId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.VendorName, opt => opt.MapFrom(src => src.Product.Vendor))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<OrderItemPictureUrlResolver>());
            
            CreateMap<Address, AddressDto>().ReverseMap();
            
            CreateMap<DeliveryMethod, DeliveryMethodDto>();
        }
    }
}
