using Application.DTOs.Auth;

namespace Application.Interfaces;

/// <summary>
/// Service for authentication operations (register, login, token generation).
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a new buyer user.
    /// </summary>
    Task<AuthResponse> RegisterBuyerAsync(RegisterBuyerRequest request, CancellationToken ct = default);

    /// <summary>
    /// Registers a new seller user with a store.
    /// </summary>
    Task<AuthResponse> RegisterSellerAsync(RegisterSellerRequest request, CancellationToken ct = default);

    /// <summary>
    /// Authenticates a user and returns JWT token.
    /// </summary>
    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct = default);
}
