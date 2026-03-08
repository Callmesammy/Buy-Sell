using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

/// <summary>
/// EF Core configuration for OrderItem entity.
/// </summary>
public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id);

        builder.Property(oi => oi.OrderId)
            .IsRequired();

        builder.Property(oi => oi.ProductId)
            .IsRequired();

        builder.Property(oi => oi.Quantity)
            .IsRequired();

        builder.Property(oi => oi.PriceAtPurchase)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(oi => oi.CreatedAt)
            .IsRequired();

        builder.Property(oi => oi.UpdatedAt)
            .IsRequired();

        builder.Property(oi => oi.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        // Indexes
        builder.HasIndex(oi => oi.OrderId)
            .HasDatabaseName("IX_OrderItem_OrderId");

        builder.HasIndex(oi => oi.ProductId)
            .HasDatabaseName("IX_OrderItem_ProductId");

        // Foreign keys
        builder.HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(oi => oi.Product)
            .WithMany()
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("OrderItems");
    }
}
