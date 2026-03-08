using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

/// <summary>
/// EF Core configuration for User entity.
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Role)
            .IsRequired();

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.UpdatedAt)
            .IsRequired();

        builder.Property(u => u.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        // Indexes
        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_User_Email");

        builder.HasIndex(u => u.Role)
            .HasDatabaseName("IX_User_Role");

        // Navigation properties
        builder.HasOne(u => u.Store)
            .WithOne(s => s.Seller)
            .HasForeignKey<Store>(s => s.SellerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(u => u.Cart)
            .WithOne(c => c.Buyer)
            .HasForeignKey<Cart>(c => c.BuyerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Users");
    }
}
