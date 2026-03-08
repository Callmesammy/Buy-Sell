namespace Domain.Enums;

/// <summary>
/// Enum representing order statuses.
/// </summary>
public enum OrderStatus
{
    /// <summary>
    /// Order is pending payment.
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Payment has been confirmed.
    /// </summary>
    Paid = 1,

    /// <summary>
    /// Order is being processed.
    /// </summary>
    Processing = 2,

    /// <summary>
    /// Order has been shipped.
    /// </summary>
    Shipped = 3,

    /// <summary>
    /// Order has been delivered.
    /// </summary>
    Delivered = 4,

    /// <summary>
    /// Order has been cancelled.
    /// </summary>
    Cancelled = 5
}
