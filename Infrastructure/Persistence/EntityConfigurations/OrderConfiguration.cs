using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

/// <summary>
/// EF Core configuration for Order entity.
/// </summary>
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.BuyerId)
            .IsRequired();

        builder.Property(o => o.Status)
            .IsRequired()
            .HasDefaultValue(Domain.Enums.OrderStatus.Pending);

        builder.Property(o => o.TotalAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(o => o.StripePaymentIntentId)
            .HasMaxLength(256);

        builder.Property(o => o.CreatedAt)
            .IsRequired();

        builder.Property(o => o.UpdatedAt)
            .IsRequired();

        builder.Property(o => o.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        // Indexes
        builder.HasIndex(o => o.BuyerId)
            .HasDatabaseName("IX_Order_BuyerId");

        builder.HasIndex(o => o.Status)
            .HasDatabaseName("IX_Order_Status");

        builder.HasIndex(o => o.StripePaymentIntentId)
            .HasDatabaseName("IX_Order_StripePaymentIntentId");

        builder.HasIndex(o => o.CreatedAt)
            .HasDatabaseName("IX_Order_CreatedAt");

        // Foreign key
        builder.HasOne(o => o.Buyer)
            .WithMany()
            .HasForeignKey(o => o.BuyerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Navigation property
        builder.HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Orders");
    }
}
