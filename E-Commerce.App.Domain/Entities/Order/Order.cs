using E_Commerce.App.Domain.Common;

namespace E_Commerce.App.Domain.Entities.Order
{
    public class Order : BaseEntity<int>
    {
        public required string BuyerEmail { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public required Address ShippingAddress { get; set; }
        public int? DeliveryMethodId { get; set; }
        public virtual DeliveryMethod? DeliveryMethod { get; set; }
        public decimal Subtotal { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
        public decimal GetTotal() => Subtotal + DeliveryMethod!.Cost;
    } 
}
