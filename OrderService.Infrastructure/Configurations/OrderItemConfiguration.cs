using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Entities;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey("ProductId", "UnitPrice", "Quantity");

        builder.Property(x => x.UnitPrice)
            .HasColumnType("decimal(18,2)");
    }
}