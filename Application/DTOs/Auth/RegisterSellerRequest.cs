namespace Application.DTOs.Auth;

/// <summary>
/// Request DTO for seller registration.
/// </summary>
public class RegisterSellerRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string StoreName { get; set; } = string.Empty;
    public string StoreDescription { get; set; } = string.Empty;
}
