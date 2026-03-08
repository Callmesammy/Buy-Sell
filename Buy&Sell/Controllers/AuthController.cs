using Application.Common;
using Application.DTOs.Auth;
using Application.Interfaces;
using Application.Validators.Auth;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Buy_Sell.Controllers;

/// <summary>
/// Authentication controller for user registration and login.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly RegisterBuyerValidator _registerBuyerValidator;
    private readonly RegisterSellerValidator _registerSellerValidator;
    private readonly LoginValidator _loginValidator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IAuthService authService,
        RegisterBuyerValidator registerBuyerValidator,
        RegisterSellerValidator registerSellerValidator,
        LoginValidator loginValidator,
        ILogger<AuthController> logger)
    {
        _authService = authService;
        _registerBuyerValidator = registerBuyerValidator;
        _registerSellerValidator = registerSellerValidator;
        _loginValidator = loginValidator;
        _logger = logger;
    }

    /// <summary>
    /// Register a new buyer user.
    /// </summary>
    /// <param name="request">Buyer registration details</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>AuthResponse with JWT token</returns>
    /// <response code="200">Buyer registered successfully</response>
    /// <response code="400">Validation error</response>
    /// <response code="409">User already exists</response>
    [HttpPost("register-buyer")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterBuyer(
        [FromBody] RegisterBuyerRequest request,
        CancellationToken ct)
    {
        _logger.LogInformation("Register buyer request received for email: {Email}", request.Email);

        // Validate request
        var validationResult = await _registerBuyerValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());

            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Validation failed",
                Data = errors
            });
        }

        try
        {
            var response = await _authService.RegisterBuyerAsync(request, ct);
            _logger.LogInformation("Buyer registered successfully: {UserId}", response.UserId);

            return Ok(new ApiResponse<AuthResponse>
            {
                Success = true,
                Message = "Buyer registered successfully",
                Data = response
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering buyer");
            throw;
        }
    }

    /// <summary>
    /// Register a new seller user with a store.
    /// </summary>
    /// <param name="request">Seller registration details</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>AuthResponse with JWT token and StoreId</returns>
    /// <response code="200">Seller registered successfully</response>
    /// <response code="400">Validation error</response>
    /// <response code="409">User already exists</response>
    [HttpPost("register-seller")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterSeller(
        [FromBody] RegisterSellerRequest request,
        CancellationToken ct)
    {
        _logger.LogInformation("Register seller request received for email: {Email}", request.Email);

        // Validate request
        var validationResult = await _registerSellerValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());

            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Validation failed",
                Data = errors
            });
        }

        try
        {
            var response = await _authService.RegisterSellerAsync(request, ct);
            _logger.LogInformation("Seller registered successfully: {UserId}", response.UserId);

            return Ok(new ApiResponse<AuthResponse>
            {
                Success = true,
                Message = "Seller registered successfully",
                Data = response
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering seller");
            throw;
        }
    }

    /// <summary>
    /// Login user with email and password.
    /// </summary>
    /// <param name="request">Login credentials</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>AuthResponse with JWT token</returns>
    /// <response code="200">Login successful</response>
    /// <response code="400">Validation error</response>
    /// <response code="401">Invalid credentials</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken ct)
    {
        _logger.LogInformation("Login request received for email: {Email}", request.Email);

        // Validate request
        var validationResult = await _loginValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());

            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Validation failed",
                Data = errors
            });
        }

        try
        {
            var response = await _authService.LoginAsync(request, ct);
            _logger.LogInformation("User logged in successfully: {UserId}", response.UserId);

            return Ok(new ApiResponse<AuthResponse>
            {
                Success = true,
                Message = "Login successful",
                Data = response
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging in user");
            throw;
        }
    }
}
