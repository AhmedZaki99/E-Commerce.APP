using E_Commerce.App.Application.Abstruction.Services.Auth;
using E_Commerce.App.Application.Abstruction.Services.Basket;
using E_Commerce.App.Application.Abstruction.Services.Favourite;
using E_Commerce.App.Application.Abstruction.Services.Order;
using E_Commerce.App.Application.Abstruction.Services.Product;

namespace E_Commerce.App.Application.Abstruction.Services
{
    public interface IServiceManager
    {
        public IproductServices ProductService {  get; }
        public IBasketService BasketService { get; }
        public IAuthService AuthService { get;  }
        public IOrderService OrderService { get; }
        public IFavouriteService FavouriteService { get; }
    }
}
