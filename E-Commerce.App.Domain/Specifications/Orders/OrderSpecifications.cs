using E_Commerce.App.Domain.Entities.Order;

namespace E_Commerce.App.Domain.Specifications.Orders
{
    public class OrderSpecifications : BaseSpecifications<Order,int>
    {
        public OrderSpecifications(int orderId, string buyerEmail) : base(order => order.Id == orderId && order.BuyerEmail == buyerEmail)
        {
            AddIncludes();
        }
        public OrderSpecifications(string buyerEmail) : base(order => order.BuyerEmail == buyerEmail)
        {
            AddIncludes();
            AddOrderByDesc(order => order.OrderDate);
        }

        private protected override void AddIncludes()
        {
            base.AddIncludes();
            Includes.Add(order => order.OrderItems);
            Includes.Add(order => order.DeliveryMethod!);
        }
    }
}
