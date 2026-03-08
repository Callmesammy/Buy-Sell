namespace Application.DTOs.Store;

/// <summary>
/// Response DTO for store statistics (rating and review count).
/// </summary>
public class StoreStatsResponse
{
    public Guid StoreId { get; set; }
    public string StoreName { get; set; } = string.Empty;
    public decimal AverageRating { get; set; }
    public int TotalReviews { get; set; }
    public int TotalProducts { get; set; }
}
