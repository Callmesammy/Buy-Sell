using Application.DTOs.Cart;

namespace Application.Interfaces;

public interface ICartService
{
    /// <summary>
    /// Get or create cart for current buyer
    /// </summary>
    Task<CartResponse> GetMyCartAsync(Guid buyerId);

    /// <summary>
    /// Add item to cart
    /// </summary>
    Task<CartItemResponse> AddToCartAsync(Guid buyerId, Guid productId, int quantity);

    /// <summary>
    /// Update quantity of item in cart
    /// </summary>
    Task<CartItemResponse> UpdateCartItemAsync(Guid buyerId, Guid cartItemId, int quantity);

    /// <summary>
    /// Remove item from cart
    /// </summary>
    Task RemoveFromCartAsync(Guid buyerId, Guid cartItemId);

    /// <summary>
    /// Clear entire cart
    /// </summary>
    Task ClearCartAsync(Guid buyerId);
}
