using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

/// <summary>
/// EF Core configuration for Cart entity.
/// </summary>
public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.BuyerId)
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .IsRequired();

        builder.Property(c => c.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        // Indexes
        builder.HasIndex(c => c.BuyerId)
            .IsUnique()
            .HasDatabaseName("IX_Cart_BuyerId");

        // Foreign key
        builder.HasOne(c => c.Buyer)
            .WithOne(u => u.Cart)
            .HasForeignKey<Cart>(c => c.BuyerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Navigation property
        builder.HasMany(c => c.CartItems)
            .WithOne(ci => ci.Cart)
            .HasForeignKey(ci => ci.CartId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Carts");
    }
}
