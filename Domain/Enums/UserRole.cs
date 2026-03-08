namespace Domain.Enums;

/// <summary>
/// Enum representing user roles in the system.
/// </summary>
public enum UserRole
{
    /// <summary>
    /// Buyer role - can purchase products.
    /// </summary>
    Buyer = 0,

    /// <summary>
    /// Seller role - can create and manage products.
    /// </summary>
    Seller = 1,

    /// <summary>
    /// Admin role - can manage the platform.
    /// </summary>
    Admin = 2
}
