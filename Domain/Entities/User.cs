using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// Represents a user in the system (Buyer, Seller, or Admin).
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// User's email address (unique).
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Hashed password (BCrypt).
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// User's first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// User's last name.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// User's role in the system.
    /// </summary>
    public UserRole Role { get; set; }

    /// <summary>
    /// Navigation property: Store if user is a seller.
    /// </summary>
    public Store? Store { get; set; }

    /// <summary>
    /// Navigation property: Cart if user is a buyer.
    /// </summary>
    public Cart? Cart { get; set; }
}
