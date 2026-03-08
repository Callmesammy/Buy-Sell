using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.DTOs.Auth;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infastructure.Services;

/// <summary>
/// Service for handling user authentication operations.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly IConfiguration _configuration;

    public AuthService(
        IUserRepository userRepository,
        IStoreRepository storeRepository,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _storeRepository = storeRepository;
        _configuration = configuration;
    }

    /// <summary>
    /// Registers a new buyer user.
    /// </summary>
    public async Task<AuthResponse> RegisterBuyerAsync(RegisterBuyerRequest request, CancellationToken ct = default)
    {
        // Check if user already exists
        var existingUser = await _userRepository.GetByEmailAsync(request.Email, ct);
        if (existingUser != null)
            throw new ConflictException("User with this email already exists.");

        // Hash password
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // Create user entity
        var user = new User
        {
            Email = request.Email,
            PasswordHash = hashedPassword,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = UserRole.Buyer
        };

        // Add to repository and save
        await _userRepository.AddAsync(user, ct);
        await _userRepository.SaveChangesAsync(ct);

        // Generate JWT token
        var token = GenerateJwtToken(user);

        return new AuthResponse
        {
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role.ToString(),
            Token = token
        };
    }

    /// <summary>
    /// Registers a new seller user with a store.
    /// </summary>
    public async Task<AuthResponse> RegisterSellerAsync(RegisterSellerRequest request, CancellationToken ct = default)
    {
        // Check if user already exists
        var existingUser = await _userRepository.GetByEmailAsync(request.Email, ct);
        if (existingUser != null)
            throw new ConflictException("User with this email already exists.");

        // Hash password
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // Create user entity
        var user = new User
        {
            Email = request.Email,
            PasswordHash = hashedPassword,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = UserRole.Seller
        };

        // Create store entity
        var store = new Store
        {
            Name = request.StoreName,
            Description = request.StoreDescription ?? string.Empty,
            Seller = user,
            SellerId = user.Id
        };

        user.Store = store;

        // Add user (which will cascade add store)
        await _userRepository.AddAsync(user, ct);
        await _userRepository.SaveChangesAsync(ct);

        // Generate JWT token
        var token = GenerateJwtToken(user);

        return new AuthResponse
        {
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role.ToString(),
            Token = token,
            StoreId = store.Id
        };
    }

    /// <summary>
    /// Authenticates a user and returns JWT token.
    /// </summary>
    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        // Find user by email
        var user = await _userRepository.GetByEmailAsync(request.Email, ct);
        if (user == null)
            throw new UnauthorizedException("Invalid email or password.");

        // Verify password
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedException("Invalid email or password.");

        // Generate JWT token
        var token = GenerateJwtToken(user);

        return new AuthResponse
        {
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role.ToString(),
            Token = token,
            StoreId = user.Store?.Id
        };
    }

    /// <summary>
    /// Generates a JWT token for the given user.
    /// </summary>
    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var secret = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret not configured");
        var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"] ?? "60");

        var key = Encoding.UTF8.GetBytes(secret);
        var tokenHandler = new JwtSecurityTokenHandler();

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
