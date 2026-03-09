using E_Commerce.App.Domain.Common;

namespace E_Commerce.App.Domain.Entities.Order
{
    public class OrderItem : BaseEntity<int>
    {
        public required ProductItemOrdered Product { get; set; } 

        public decimal Price { get; set; } // the price of the product at the time of order, not the current price
        public int Quantity { get; set; }

    }
}
