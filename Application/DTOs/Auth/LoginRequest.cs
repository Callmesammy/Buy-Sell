namespace Application.DTOs.Auth;

/// <summary>
/// Request DTO for login.
/// </summary>
public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
