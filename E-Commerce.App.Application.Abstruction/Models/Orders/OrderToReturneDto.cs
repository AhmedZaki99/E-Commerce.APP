using E_Commerce.App.Application.Abstruction.Models.Common;

namespace E_Commerce.App.Application.Abstruction.Models.Orders
{
    public class OrderToReturneDto
    {
        public int Id { get; set; }
        public required string BuyerEmail { get; set; }
        public  DateTime OrderDate { get; set; }
        public required string Status { get; set; }
        public required AddressDto ShippingAddress { get; set; }
        public int? DeliveryMethodId { get; set; }
        public string? DeliveryMethod { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public virtual required ICollection<OrderItemDto> OrderItems { get; set; }
    }
}
