namespace Application.DTOs.Store;

/// <summary>
/// Response DTO for store details.
/// </summary>
public class StoreResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public Guid SellerId { get; set; }
}
