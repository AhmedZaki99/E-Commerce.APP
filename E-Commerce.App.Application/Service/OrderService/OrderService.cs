using AutoMapper;
using E_Commerce.App.Application.Abstruction.Models.Orders;
using E_Commerce.App.Application.Abstruction.Services.Basket;
using E_Commerce.App.Application.Abstruction.Services.Order;
using E_Commerce.App.Application.Exception;
using E_Commerce.App.Domain.Contract.Peresistence;
using E_Commerce.App.Domain.Entities.Order;
using E_Commerce.App.Domain.Entities.Product;
using E_Commerce.App.Domain.Specifications.Orders;

namespace E_Commerce.App.Application.Service.OrderService
{
    public class OrderService(IBasketService basketService, IUnitOfWork unitOfWork, IMapper mapper) : IOrderService
    {
        public async Task<OrderToReturneDto> CreateOrderAsync(string buyerEmail, OrderToCreateDto order)
        {
            //1. Get Basket from Baskets Repository
            var basket = await basketService.GetCustomerBasketAsync(order.BasketId);

            //2. Get Selected Items at Basket from Products Repository
            var orderItems = new List<OrderItem>();

            if (basket?.Items == null || !basket.Items.Any())
            {
                throw new BadRequestException("No items in basket");
            }
            else
            {
                var productRepo = unitOfWork.GetRepositieries<Product, int>();
                foreach (var item in basket.Items)
                {
                    var product = await productRepo.GetAsync(item.Id);

                    if (product != null)
                    {
                        var productItemOrdered = new ProductItemOrdered()
                        {
                            ProductId = product.Id,
                            ProductName = product.Name,
                            PictureUrl = product.PictureUrl ?? ""
                        };

                        var orderItem = new OrderItem()
                        {
                            Product = productItemOrdered,
                            Price = product.Price,
                            Quantity = item.Quantity
                        };

                        orderItems.Add(orderItem);
                    }
                }
            }

            //3. Calculate SubTotal
            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

            //4. Map Address
            var address = mapper.Map<Address>(order.ShippingAddress);

            var deliveryMethod = await unitOfWork.GetRepositieries<DeliveryMethod, int>().GetAsync(order.DeliveryMethodId);
            //5. Create Order 
            var orderToCreate = new Order()
            {
                BuyerEmail = buyerEmail,
                OrderItems = orderItems,
                ShippingAddress = address,
                Subtotal = subTotal,
                DeliveryMethod = deliveryMethod
            };
             await unitOfWork.GetRepositieries<Order, int>().AddAsync(orderToCreate);

            //6. Save to DB
            var created = await unitOfWork.SaveChangesAsync() > 0;

            if (!created) throw new BadRequestException("an error has occured during creating order");
            return mapper.Map<OrderToReturneDto>(orderToCreate);

        }
        public async Task<IEnumerable<OrderToReturneDto>> GetOrdersForUserAsync(string buyerEmail)
        {
            var orderSpec = new OrderSpecifications(buyerEmail);
            var order = await unitOfWork.GetRepositieries<Order, int>().GetAllSpecAsync(orderSpec);
            return mapper.Map<IEnumerable<OrderToReturneDto>>(order);
        }
        public async Task<OrderToReturneDto> GetOrderByIdAsync(int orderId, string buyerEmail)
        {
            var orderSpec = new OrderSpecifications(orderId, buyerEmail);
            var order = await unitOfWork.GetRepositieries<Order, int>().GetWithSpecAsync(orderSpec);
            if(order is null) throw new NotFoundException(nameof(buyerEmail), orderId);
            return mapper.Map<OrderToReturneDto>(order);
        }
        public async Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await unitOfWork.GetRepositieries<DeliveryMethod,int>().GetAllAsync();
            return mapper.Map<IEnumerable<DeliveryMethodDto>>(deliveryMethods);
        }
    }
}
