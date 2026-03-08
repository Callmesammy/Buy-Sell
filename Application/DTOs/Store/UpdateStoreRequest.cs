namespace Application.DTOs.Store;

/// <summary>
/// Request DTO for updating store information.
/// </summary>
public class UpdateStoreRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
