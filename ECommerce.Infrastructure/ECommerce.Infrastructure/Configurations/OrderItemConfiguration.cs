using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(orderItem => orderItem.Id);

        builder.HasOne<Product>()
               .WithMany()
               .HasForeignKey(orderItem => orderItem.ProductId);

        builder.Property(orderItem => orderItem.Price)
               .IsRequired();

        builder.Property(orderItem => orderItem.Quantity)
                .IsRequired();

        builder.HasOne<Order>()
          .WithMany(o => o.OrderItems)
          .HasForeignKey(oi => oi.OrderId); 

    }
}
