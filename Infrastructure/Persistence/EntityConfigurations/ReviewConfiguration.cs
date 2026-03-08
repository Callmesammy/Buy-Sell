using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

/// <summary>
/// EF Core configuration for Review entity.
/// </summary>
public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.ProductId)
            .IsRequired();

        builder.Property(r => r.BuyerId)
            .IsRequired();

        builder.Property(r => r.Rating)
            .IsRequired();

        builder.Property(r => r.Comment)
            .HasMaxLength(2000);

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.Property(r => r.UpdatedAt)
            .IsRequired();

        builder.Property(r => r.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        // Indexes
        builder.HasIndex(r => r.ProductId)
            .HasDatabaseName("IX_Review_ProductId");

        builder.HasIndex(r => r.BuyerId)
            .HasDatabaseName("IX_Review_BuyerId");

        builder.HasIndex(r => new { r.ProductId, r.BuyerId })
            .IsUnique()
            .HasDatabaseName("IX_Review_ProductId_BuyerId");

        // Foreign keys
        builder.HasOne(r => r.Product)
            .WithMany(p => p.Reviews)
            .HasForeignKey(r => r.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Buyer)
            .WithMany()
            .HasForeignKey(r => r.BuyerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("Reviews");
    }
}
