using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
       builder.HasKey(c => c.Id);

       builder.Property(c => c.Name).
            HasMaxLength(100).
            IsRequired();

        builder.Property(c=>c.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.HasMany(o => o.Products)
           .WithOne()
           .HasForeignKey(oi => oi.CategoryId);
    }
}
