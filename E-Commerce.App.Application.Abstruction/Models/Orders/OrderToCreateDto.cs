using E_Commerce.App.Application.Abstruction.Models.Common;

namespace E_Commerce.App.Application.Abstruction.Models.Orders
{
    public class OrderToCreateDto
    {
        public required string BasketId { get; set; } 
        public int DeliveryMethodId { get; set; }
        public required AddressDto ShippingAddress { get; set; }
    }
}
