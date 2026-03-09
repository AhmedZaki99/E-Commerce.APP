using AutoMapper;
using E_Commerce.App.Application.Abstruction.Services;
using E_Commerce.App.Application.Abstruction.Services.Auth;
using E_Commerce.App.Application.Abstruction.Services.Basket;
using E_Commerce.App.Application.Abstruction.Services.Favourite;
using E_Commerce.App.Application.Abstruction.Services.Order;
using E_Commerce.App.Application.Abstruction.Services.Product;
using E_Commerce.App.Application.Service.ProductServicies;
using E_Commerce.App.Domain.Contract.Peresistence;

namespace E_Commerce.App.Application.Service
{
    internal class ServiceManager : IServiceManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly Lazy<IproductServices> _productService;
        private readonly Lazy<IBasketService> _basketService;
        private readonly Lazy<IAuthService> _authService;
        private readonly Lazy<IOrderService> _orderService;
        private readonly Lazy<IFavouriteService> _favouriteService;
        public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper, Func<IFavouriteService> favouriteServiceFactory, Func<IOrderService> orderServiceFactory , Func<IBasketService> basketServiceFactory, Func<IAuthService> AuthServiceFactory)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;

            _productService = new Lazy<IproductServices>(() => new ProductService(_unitOfWork, _mapper));
            _basketService = new Lazy<IBasketService>(basketServiceFactory, LazyThreadSafetyMode.ExecutionAndPublication);
            _authService = new Lazy<IAuthService>(AuthServiceFactory, LazyThreadSafetyMode.ExecutionAndPublication);
            _orderService = new Lazy<IOrderService>(orderServiceFactory, LazyThreadSafetyMode.ExecutionAndPublication);
            _favouriteService = new Lazy<IFavouriteService>(favouriteServiceFactory);
        }

        public IproductServices ProductService => _productService.Value;

        public IBasketService BasketService => _basketService.Value;

        public IAuthService AuthService => _authService.Value;

        public IOrderService OrderService => _orderService.Value;
        public IFavouriteService FavouriteService => _favouriteService.Value;

    }
}