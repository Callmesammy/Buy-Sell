namespace Application.DTOs.Auth;

/// <summary>
/// Request DTO for buyer registration.
/// </summary>
public class RegisterBuyerRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
