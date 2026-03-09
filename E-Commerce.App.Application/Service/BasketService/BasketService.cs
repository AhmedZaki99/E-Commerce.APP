using AutoMapper;
using E_Commerce.App.Application.Abstruction.Models.Basket;
using E_Commerce.App.Application.Abstruction.Services.Basket;
using E_Commerce.App.Application.Exception;
using E_Commerce.App.Domain.Contract.Infrastructre;
using E_Commerce.App.Domain.Entities.Basket;
using Microsoft.Extensions.Configuration;

namespace E_Commerce.App.Application.Service.BasketService
{
    public class BasketService(IBasketRepository basketRepository, IMapper mapper, IConfiguration configuration) : IBasketService
    {
        public async Task<CustomerBasketDto> GetCustomerBasketAsync(string basketId)
        {
            var basket = await basketRepository.GetAsync(basketId);

            if (basket is null) throw new NotFoundException(nameof(CustomerBasket), basketId);

            return mapper.Map<CustomerBasketDto>(basket);       
        }

        public async Task<CustomerBasketDto> UpdateCustomerBasketAsync(CustomerBasketDto basketDto)
        {
            var basket = mapper.Map<CustomerBasket>(basketDto);
            var timeToLive = TimeSpan.FromDays(double.Parse(configuration.GetSection("RedisSetting")["TimeToLiveInDays"]!));
            var updatedBasket = await basketRepository.UpdateAsync(basket, timeToLive);

            if (updatedBasket is null) throw new BadRequestException("Can't update, there is a problem with your this basket.");

            return basketDto;
        }  
        public async Task DeleteCustomerBasketAsync(string basketId)
        {
            var deleted = await basketRepository.DeleteAsync(basketId);
            if (!deleted) throw new BadRequestException("Unable to delete this basket.");
        }
    }
}