using Application.DTOs.Review;
using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;

namespace Infastructure.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;

    public ReviewService(
        IReviewRepository reviewRepository,
        IProductRepository productRepository,
        IUserRepository userRepository)
    {
        _reviewRepository = reviewRepository;
        _productRepository = productRepository;
        _userRepository = userRepository;
    }

    public async Task<ReviewResponse> CreateReviewAsync(Guid buyerId, CreateReviewRequest request)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product == null)
            throw new NotFoundException($"Product {request.ProductId} not found");

        var buyer = await _userRepository.GetByIdAsync(buyerId);
        if (buyer == null)
            throw new NotFoundException("Buyer not found");

        // Check if review already exists
        var existingReview = await _reviewRepository.GetReviewAsync(request.ProductId, buyerId);
        if (existingReview != null)
            throw new ConflictException("You have already reviewed this product");

        var review = new Review
        {
            ProductId = request.ProductId,
            BuyerId = buyerId,
            Rating = request.Rating,
            Comment = request.Comment
        };

        await _reviewRepository.AddAsync(review);
        await _reviewRepository.SaveChangesAsync();

        return MapToResponse(review, buyer);
    }

    public async Task<List<ReviewResponse>> GetProductReviewsAsync(Guid productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
            throw new NotFoundException($"Product {productId} not found");

        var reviews = await _reviewRepository.GetByProductIdAsync(productId);
        
        var reviewResponses = new List<ReviewResponse>();
        foreach (var review in reviews)
        {
            var buyer = await _userRepository.GetByIdAsync(review.BuyerId);
            reviewResponses.Add(MapToResponse(review, buyer));
        }

        return reviewResponses;
    }

    public async Task<List<ReviewResponse>> GetMyReviewsAsync(Guid buyerId)
    {
        var buyer = await _userRepository.GetByIdAsync(buyerId);
        if (buyer == null)
            throw new NotFoundException("Buyer not found");

        var reviews = await _reviewRepository.GetByBuyerIdAsync(buyerId);
        return reviews.Select(r => MapToResponse(r, buyer)).ToList();
    }

    public async Task DeleteReviewAsync(Guid reviewId, Guid buyerId)
    {
        var review = await _reviewRepository.GetByIdAsync(reviewId);
        if (review == null)
            throw new NotFoundException($"Review {reviewId} not found");

        if (review.BuyerId != buyerId)
            throw new UnauthorizedException("You don't have permission to delete this review");

        review.IsDeleted = true;
        await _reviewRepository.SaveChangesAsync();
    }

    public async Task<ReviewResponse> UpdateReviewAsync(Guid reviewId, Guid buyerId, CreateReviewRequest request)
    {
        var review = await _reviewRepository.GetByIdAsync(reviewId);
        if (review == null)
            throw new NotFoundException($"Review {reviewId} not found");

        if (review.BuyerId != buyerId)
            throw new UnauthorizedException("You don't have permission to update this review");

        review.Rating = request.Rating;
        review.Comment = request.Comment;
        
        await _reviewRepository.SaveChangesAsync();

        var buyer = await _userRepository.GetByIdAsync(buyerId);
        return MapToResponse(review, buyer!);
    }

    private ReviewResponse MapToResponse(Review review, User? buyer)
    {
        return new ReviewResponse
        {
            Id = review.Id,
            ProductId = review.ProductId,
            BuyerName = buyer != null ? $"{buyer.FirstName} {buyer.LastName}" : "Anonymous",
            Rating = review.Rating,
            Comment = review.Comment,
            CreatedAt = review.CreatedAt,
            UpdatedAt = review.UpdatedAt
        };
    }
}
