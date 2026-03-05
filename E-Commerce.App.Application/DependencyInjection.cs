using AutoMapper;
using E_Commerce.App.Application.Abstruction.Services;
using E_Commerce.App.Application.Abstruction.Services.Auth;
using E_Commerce.App.Application.Abstruction.Services.Basket;
using E_Commerce.App.Application.Abstruction.Services.Order;
using E_Commerce.App.Application.Mapping;
using E_Commerce.App.Application.Service;
using E_Commerce.App.Application.Service.Auth;
using E_Commerce.App.Application.Service.BasketService;
using E_Commerce.App.Application.Service.OrderService;
using E_Commerce.App.Domain.Contract.Infrastructre;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.App.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicatinServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));

            services.AddScoped(typeof(IBasketService), typeof(BasketService));
            services.AddScoped(typeof(Func<IBasketService>), (servicesProvider) =>
            {
                return ()=> servicesProvider.GetRequiredService<IBasketService>();
            }
            );

            services.AddScoped(typeof(IOrderService), typeof(OrderService));
            services.AddScoped(typeof(Func<IOrderService>), (servicesProvider) =>
            {
                return () => servicesProvider.GetRequiredService<IOrderService>();
            }
            );

            services.AddScoped(typeof(IAuthService), typeof(AuthService));
            services.AddScoped(typeof(Func<IAuthService>), serviceProvider =>
            {
                return () => serviceProvider.GetRequiredService<IAuthService>();
            });

            services.AddTransient(typeof(IEmailService), typeof(EmailService));

            return services;
        }

    }
}
