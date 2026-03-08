using Application.DTOs.Cart;
using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;

namespace Infastructure.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public CartService(ICartRepository cartRepository, IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }

    public async Task<CartResponse> GetMyCartAsync(Guid buyerId)
    {
        var cart = await _cartRepository.GetByBuyerIdAsync(buyerId);

        if (cart == null)
        {
            // Create new cart if doesn't exist
            cart = new Cart { BuyerId = buyerId };
            await _cartRepository.AddAsync(cart);
            await _cartRepository.SaveChangesAsync();
        }

        return MapToResponse(cart);
    }

    public async Task<CartItemResponse> AddToCartAsync(Guid buyerId, Guid productId, int quantity)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
            throw new NotFoundException($"Product {productId} not found");

        if (product.Stock < quantity)
            throw new ConflictException($"Insufficient stock. Available: {product.Stock}");

        var cart = await _cartRepository.GetByBuyerIdAsync(buyerId);
        if (cart == null)
        {
            cart = new Cart { BuyerId = buyerId };
            await _cartRepository.AddAsync(cart);
            await _cartRepository.SaveChangesAsync();
        }

        // Check if product already in cart
        var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
        
        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            var newItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = productId,
                Quantity = quantity
            };
            cart.CartItems.Add(newItem);
        }

        await _cartRepository.SaveChangesAsync();
        
        var cartItem = cart.CartItems.First(ci => ci.ProductId == productId);
        return MapCartItemToResponse(cartItem, product);
    }

    public async Task<CartItemResponse> UpdateCartItemAsync(Guid buyerId, Guid cartItemId, int quantity)
    {
        var cart = await _cartRepository.GetByBuyerIdAsync(buyerId);
        if (cart == null)
            throw new NotFoundException("Cart not found");

        var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
        if (cartItem == null)
            throw new NotFoundException("Item not found in cart");

        var product = await _productRepository.GetByIdAsync(cartItem.ProductId);
        if (product == null)
            throw new NotFoundException("Product not found");

        if (product.Stock < quantity)
            throw new ConflictException($"Insufficient stock. Available: {product.Stock}");

        cartItem.Quantity = quantity;
        await _cartRepository.SaveChangesAsync();

        return MapCartItemToResponse(cartItem, product);
    }

    public async Task RemoveFromCartAsync(Guid buyerId, Guid cartItemId)
    {
        var cart = await _cartRepository.GetByBuyerIdAsync(buyerId);
        if (cart == null)
            throw new NotFoundException("Cart not found");

        var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
        if (cartItem == null)
            throw new NotFoundException("Item not found in cart");

        cart.CartItems.Remove(cartItem);
        await _cartRepository.SaveChangesAsync();
    }

    public async Task ClearCartAsync(Guid buyerId)
    {
        var cart = await _cartRepository.GetByBuyerIdAsync(buyerId);
        if (cart == null)
            throw new NotFoundException("Cart not found");

        cart.CartItems.Clear();
        await _cartRepository.SaveChangesAsync();
    }

    private CartResponse MapToResponse(Cart cart)
    {
        return new CartResponse
        {
            Id = cart.Id,
            Items = cart.CartItems
                .Where(ci => !ci.IsDeleted)
                .Select(ci => new CartItemResponse
                {
                    Id = ci.Id,
                    ProductId = ci.ProductId,
                    ProductTitle = ci.Product?.Title ?? "Unknown",
                    ProductPrice = ci.Product?.Price ?? 0,
                    Quantity = ci.Quantity,
                    TotalPrice = (ci.Product?.Price ?? 0) * ci.Quantity,
                    AddedAt = ci.CreatedAt
                })
                .ToList(),
            ItemCount = cart.CartItems.Count(ci => !ci.IsDeleted),
            TotalPrice = cart.CartItems
                .Where(ci => !ci.IsDeleted)
                .Sum(ci => (ci.Product?.Price ?? 0) * ci.Quantity)
        };
    }

    private CartItemResponse MapCartItemToResponse(CartItem cartItem, Product product)
    {
        return new CartItemResponse
        {
            Id = cartItem.Id,
            ProductId = cartItem.ProductId,
            ProductTitle = product.Title,
            ProductPrice = product.Price,
            Quantity = cartItem.Quantity,
            TotalPrice = product.Price * cartItem.Quantity,
            AddedAt = cartItem.CreatedAt
        };
    }
}
