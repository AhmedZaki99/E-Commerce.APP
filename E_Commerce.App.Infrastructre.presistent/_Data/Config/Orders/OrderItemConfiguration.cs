using E_Commerce.App.Domain.Entities.Order;
using E_Commerce.App.Infrastructre.presistent._Data.Config.BaseConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.App.Infrastructre.presistent._Data.Config.Orders
{
    public class OrderItemConfiguration : BaseEntityConfiguration<OrderItem, int>
    {
        public override void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            base.Configure(builder);

               builder.OwnsOne(I => I.Product, P => P.WithOwner());

               builder.Property(I => I.Price).HasColumnType("decimal(8,2)");
        }
    }
}
