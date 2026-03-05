using AutoMapper;
using E_Commerce.App.Application.Abstruction.Models.Basket;
using E_Commerce.App.Application.Abstruction.Models.Product;
using E_Commerce.App.Domain.Entities.Basket;
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
        }
    }
}
