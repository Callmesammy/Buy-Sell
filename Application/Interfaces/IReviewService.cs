using Application.DTOs.Review;

namespace Application.Interfaces;

public interface IReviewService
{
    /// <summary>
    /// Create a review for a product
    /// </summary>
    Task<ReviewResponse> CreateReviewAsync(Guid buyerId, CreateReviewRequest request);

    /// <summary>
    /// Get all reviews for a product
    /// </summary>
    Task<List<ReviewResponse>> GetProductReviewsAsync(Guid productId);

    /// <summary>
    /// Get current user's reviews
    /// </summary>
    Task<List<ReviewResponse>> GetMyReviewsAsync(Guid buyerId);

    /// <summary>
    /// Delete a review
    /// </summary>
    Task DeleteReviewAsync(Guid reviewId, Guid buyerId);

    /// <summary>
    /// Update a review
    /// </summary>
    Task<ReviewResponse> UpdateReviewAsync(Guid reviewId, Guid buyerId, CreateReviewRequest request);
}
