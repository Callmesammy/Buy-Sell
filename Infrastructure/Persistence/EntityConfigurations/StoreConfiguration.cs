using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

/// <summary>
/// EF Core configuration for Store entity.
/// </summary>
public class StoreConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(s => s.Description)
            .HasMaxLength(1000);

        builder.Property(s => s.LogoUrl)
            .HasMaxLength(512);

        builder.Property(s => s.SellerId)
            .IsRequired();

        builder.Property(s => s.CreatedAt)
            .IsRequired();

        builder.Property(s => s.UpdatedAt)
            .IsRequired();

        builder.Property(s => s.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        // Indexes
        builder.HasIndex(s => s.SellerId)
            .IsUnique()
            .HasDatabaseName("IX_Store_SellerId");

        builder.HasIndex(s => s.Name)
            .HasDatabaseName("IX_Store_Name");

        // Foreign key
        builder.HasOne(s => s.Seller)
            .WithOne(u => u.Store)
            .HasForeignKey<Store>(s => s.SellerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Navigation property
        builder.HasMany(s => s.Products)
            .WithOne(p => p.Store)
            .HasForeignKey(p => p.StoreId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Stores");
    }
}
