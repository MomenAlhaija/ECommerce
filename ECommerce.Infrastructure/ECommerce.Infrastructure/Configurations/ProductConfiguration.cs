using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(prod => prod.Id);

        builder.Property(prod => prod.Name)
                .IsRequired()
                .HasMaxLength(100);

        builder.HasOne<Category>()
         .WithMany(o => o.Products)
         .HasForeignKey(oi => oi.CategoryId);
        
    }
}
