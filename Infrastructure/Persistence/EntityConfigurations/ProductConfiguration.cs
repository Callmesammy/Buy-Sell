using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

/// <summary>
/// EF Core configuration for Product entity.
/// </summary>
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(4000);

        builder.Property(p => p.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.Stock)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(p => p.ImageUrls)
            .HasMaxLength(2000);

        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(p => p.CategoryId)
            .IsRequired();

        builder.Property(p => p.StoreId)
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .IsRequired();

        builder.Property(p => p.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        // Indexes
        builder.HasIndex(p => p.Title)
            .HasDatabaseName("IX_Product_Title");

        builder.HasIndex(p => p.CategoryId)
            .HasDatabaseName("IX_Product_CategoryId");

        builder.HasIndex(p => p.StoreId)
            .HasDatabaseName("IX_Product_StoreId");

        builder.HasIndex(p => p.IsActive)
            .HasDatabaseName("IX_Product_IsActive");

        // Foreign keys
        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Store)
            .WithMany(s => s.Products)
            .HasForeignKey(p => p.StoreId)
            .OnDelete(DeleteBehavior.Cascade);

        // Navigation properties
        builder.HasMany(p => p.Reviews)
            .WithOne(r => r.Product)
            .HasForeignKey(r => r.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.ProductViews)
            .WithOne(pv => pv.Product)
            .HasForeignKey(pv => pv.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Products");
    }
}
