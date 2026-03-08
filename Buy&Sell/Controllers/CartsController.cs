using Application.Common;
using Application.DTOs.Cart;
using Application.Interfaces;
using Application.Validators.Cart;
using Buy_Sell.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Buy_Sell.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartsController : ControllerBase
{
    private readonly ICartService _cartService;
    private readonly AddToCartValidator _addToCartValidator;
    private readonly UpdateCartItemValidator _updateCartItemValidator;

    public CartsController(
        ICartService cartService,
        AddToCartValidator addToCartValidator,
        UpdateCartItemValidator updateCartItemValidator)
    {
        _cartService = cartService;
        _addToCartValidator = addToCartValidator;
        _updateCartItemValidator = updateCartItemValidator;
    }

    /// <summary>
    /// Get current user's cart
    /// </summary>
    [HttpGet("me")]
    public async Task<IActionResult> GetMyCart()
    {
        var buyerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
        var cart = await _cartService.GetMyCartAsync(buyerId);
        return Ok(new ApiResponse<CartResponse>
        {
            Success = true,
            Message = "Cart retrieved successfully",
            Data = cart
        });
    }

    /// <summary>
    /// Add item to cart
    /// </summary>
    [HttpPost("items")]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
    {
        var validationResult = await _addToCartValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Validation failed",
                Data = errors
            });
        }

        var buyerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
        var item = await _cartService.AddToCartAsync(buyerId, request.ProductId, request.Quantity);
        return Created(string.Empty, new ApiResponse<CartItemResponse>
        {
            Success = true,
            Message = "Item added to cart",
            Data = item
        });
    }

    /// <summary>
    /// Update cart item quantity
    /// </summary>
    [HttpPut("items/{cartItemId}")]
    public async Task<IActionResult> UpdateCartItem(
        Guid cartItemId,
        [FromBody] UpdateCartItemRequest request)
    {
        var validationResult = await _updateCartItemValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Validation failed",
                Data = errors
            });
        }

        var buyerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
        var item = await _cartService.UpdateCartItemAsync(buyerId, cartItemId, request.Quantity);
        return Ok(new ApiResponse<CartItemResponse>
        {
            Success = true,
            Message = "Item updated successfully",
            Data = item
        });
    }

    /// <summary>
    /// Remove item from cart
    /// </summary>
    [HttpDelete("items/{cartItemId}")]
    public async Task<IActionResult> RemoveFromCart(Guid cartItemId)
    {
        var buyerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
        await _cartService.RemoveFromCartAsync(buyerId, cartItemId);
        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Item removed from cart"
        });
    }

    /// <summary>
    /// Clear entire cart
    /// </summary>
    [HttpPost("clear")]
    public async Task<IActionResult> ClearCart()
    {
        var buyerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
        await _cartService.ClearCartAsync(buyerId);
        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Cart cleared successfully"
        });
    }
}
