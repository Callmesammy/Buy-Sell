using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

/// <summary>
/// EF Core configuration for ProductView entity (for AI recommendations).
/// </summary>
public class ProductViewConfiguration : IEntityTypeConfiguration<ProductView>
{
    public void Configure(EntityTypeBuilder<ProductView> builder)
    {
        builder.HasKey(pv => pv.Id);

        builder.Property(pv => pv.ProductId)
            .IsRequired();

        builder.Property(pv => pv.UserId)
            .IsRequired();

        builder.Property(pv => pv.ViewedAt)
            .IsRequired();

        builder.Property(pv => pv.CreatedAt)
            .IsRequired();

        builder.Property(pv => pv.UpdatedAt)
            .IsRequired();

        builder.Property(pv => pv.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        // Indexes
        builder.HasIndex(pv => pv.ProductId)
            .HasDatabaseName("IX_ProductView_ProductId");

        builder.HasIndex(pv => pv.UserId)
            .HasDatabaseName("IX_ProductView_UserId");

        builder.HasIndex(pv => pv.ViewedAt)
            .HasDatabaseName("IX_ProductView_ViewedAt");

        builder.HasIndex(pv => new { pv.UserId, pv.ViewedAt })
            .HasDatabaseName("IX_ProductView_UserId_ViewedAt");

        // Foreign keys
        builder.HasOne(pv => pv.Product)
            .WithMany(p => p.ProductViews)
            .HasForeignKey(pv => pv.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pv => pv.User)
            .WithMany()
            .HasForeignKey(pv => pv.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.ToTable("ProductViews");
    }
}
