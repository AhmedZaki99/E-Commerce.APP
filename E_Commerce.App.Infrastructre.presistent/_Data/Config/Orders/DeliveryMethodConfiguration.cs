using E_Commerce.App.Domain.Entities.Order;
using E_Commerce.App.Infrastructre.presistent._Data.Config.BaseConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.App.Infrastructre.presistent._Data.Config.Orders
{
    public class DeliveryMethodConfiguration : BaseEntityConfiguration<DeliveryMethod, int>
    {
        public override void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            base.Configure(builder);

               builder.Property(D => D.Cost).HasColumnType("decimal(8,2)");
        }
    }
}
