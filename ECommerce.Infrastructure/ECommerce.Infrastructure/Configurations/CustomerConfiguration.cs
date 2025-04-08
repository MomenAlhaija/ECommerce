using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(cust => cust.Id);

       
        builder.Property(cust => cust.Name)
               .IsRequired()
               .HasMaxLength(100);


        builder.Property(cust => cust.Email)
               .HasMaxLength(100);

        builder.HasIndex(cust => cust.Email)
               .IsUnique();

    }
}
