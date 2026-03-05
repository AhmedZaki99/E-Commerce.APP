using E_Commerce.App.Domain.Entities.Order;
using E_Commerce.App.Infrastructre.presistent._Data.Config.BaseConfig;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.App.Infrastructre.presistent._Data.Config.Orders
{
    public class OrderConfiguration : BaseEntityConfiguration<Domain.Entities.Order.Order, int>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Domain.Entities.Order.Order> builder)
        {
            base.Configure(builder);

            builder.OwnsOne(O => O.ShippingAddress, S => S.WithOwner());

            builder.Property(O => O.Status).HasConversion
                                           (
                                              (OStatus) => OStatus.ToString(),
                                              (OStatus) => (OrderStatus)Enum.Parse(typeof(OrderStatus), OStatus)
                                           );
            builder.Property(O => O.Subtotal).HasColumnType("decimal(8,2)");

            builder.HasOne(O => O.DeliveryMethod)
                   .WithMany()
                   .HasForeignKey(O => O.DeliveryMethodId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(O => O.OrderItems)
                   .WithOne()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
