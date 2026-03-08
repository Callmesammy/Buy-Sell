using Application.Common;
using Application.DTOs.Review;
using Application.Interfaces;
using Application.Validators.Review;
using Buy_Sell.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Buy_Sell.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;
    private readonly CreateReviewValidator _createReviewValidator;

    public ReviewsController(IReviewService reviewService, CreateReviewValidator createReviewValidator)
    {
        _reviewService = reviewService;
        _createReviewValidator = createReviewValidator;
    }

    /// <summary>
    /// Create a review for a product (requires authentication)
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateReview([FromBody] CreateReviewRequest request)
    {
        var validationResult = await _createReviewValidator.ValidateAsync(request);
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
        var review = await _reviewService.CreateReviewAsync(buyerId, request);
        return Created(string.Empty, new ApiResponse<ReviewResponse>
        {
            Success = true,
            Message = "Review created successfully",
            Data = review
        });
    }

    /// <summary>
    /// Get all reviews for a product
    /// </summary>
    [HttpGet("product/{productId}")]
    public async Task<IActionResult> GetProductReviews(Guid productId)
    {
        var reviews = await _reviewService.GetProductReviewsAsync(productId);
        return Ok(new ApiResponse<List<ReviewResponse>>
        {
            Success = true,
            Message = "Product reviews retrieved successfully",
            Data = reviews
        });
    }

    /// <summary>
    /// Get current user's reviews (requires authentication)
    /// </summary>
    [HttpGet("my-reviews")]
    [Authorize]
    public async Task<IActionResult> GetMyReviews()
    {
        var buyerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
        var reviews = await _reviewService.GetMyReviewsAsync(buyerId);
        return Ok(new ApiResponse<List<ReviewResponse>>
        {
            Success = true,
            Message = "Your reviews retrieved successfully",
            Data = reviews
        });
    }

    /// <summary>
    /// Delete a review (requires authentication)
    /// </summary>
    [HttpDelete("{reviewId}")]
    [Authorize]
    public async Task<IActionResult> DeleteReview(Guid reviewId)
    {
        var buyerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
        await _reviewService.DeleteReviewAsync(reviewId, buyerId);
        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Review deleted successfully"
        });
    }

    /// <summary>
    /// Update a review (requires authentication)
    /// </summary>
    [HttpPut("{reviewId}")]
    [Authorize]
    public async Task<IActionResult> UpdateReview(
        Guid reviewId,
        [FromBody] CreateReviewRequest request)
    {
        var validationResult = await _createReviewValidator.ValidateAsync(request);
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
        var review = await _reviewService.UpdateReviewAsync(reviewId, buyerId, request);
        return Ok(new ApiResponse<ReviewResponse>
        {
            Success = true,
            Message = "Review updated successfully",
            Data = review
        });
    }
}
