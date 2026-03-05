using E_Commerce.App.Application.Abstruction.Models.Orders;

namespace E_Commerce.App.Application.Abstruction.Services.Order
{
    public interface IOrderService
    {
        Task<OrderToReturneDto> CreateOrderAsync(string buyerEmail, OrderToCreateDto order);
        Task<IEnumerable<OrderToReturneDto>> GetOrdersForUserAsync(string buyerEmail);
        Task<OrderToReturneDto> GetOrderByIdAsync(int orderId, string buyerEmail);
         Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync();
    }
}
