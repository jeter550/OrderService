using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Entities;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Currency)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(o => o.Total)
            .HasColumnType("decimal(18,2)");

        builder.Property(o => o.Status)
            .IsRequired();

        builder.HasMany(typeof(OrderItem), "_items")
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}