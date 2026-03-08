using Application.Common;
using Application.DTOs.Store;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Buy_Sell.Controllers;

/// <summary>
/// Stores controller for managing seller stores.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class StoresController : ControllerBase
{
    private readonly IStoreService _storeService;
    private readonly ILogger<StoresController> _logger;

    public StoresController(
        IStoreService storeService,
        ILogger<StoresController> logger)
    {
        _storeService = storeService;
        _logger = logger;
    }

    /// <summary>
    /// Get store details by ID.
    /// </summary>
    /// <param name="id">Store ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Store details</returns>
    /// <response code="200">Store retrieved successfully</response>
    /// <response code="404">Store not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<StoreResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStoreById(
        [FromRoute] Guid id,
        CancellationToken ct = default)
    {
        _logger.LogInformation("Get store request - StoreId: {StoreId}", id);

        try
        {
            var result = await _storeService.GetStoreByIdAsync(id, ct);

            return Ok(new ApiResponse<StoreResponse>
            {
                Success = true,
                Message = "Store retrieved successfully",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving store");
            throw;
        }
    }

    /// <summary>
    /// Get current user's store (seller only).
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Current user's store</returns>
    /// <response code="200">Store retrieved successfully</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Store not found</response>
    [Authorize]
    [HttpGet("me/store")]
    [ProducesResponseType(typeof(ApiResponse<StoreResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyStore(CancellationToken ct = default)
    {
        _logger.LogInformation("Get current user's store request");

        try
        {
            var sellerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User ID not found in claims"));
            var result = await _storeService.GetStoreBySellerIdAsync(sellerId, ct);

            return Ok(new ApiResponse<StoreResponse>
            {
                Success = true,
                Message = "Store retrieved successfully",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving current user's store");
            throw;
        }
    }

    /// <summary>
    /// Get store statistics (ratings, reviews, products).
    /// </summary>
    /// <param name="id">Store ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Store statistics</returns>
    /// <response code="200">Store stats retrieved successfully</response>
    /// <response code="404">Store not found</response>
    [HttpGet("{id}/stats")]
    [ProducesResponseType(typeof(ApiResponse<StoreStatsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStoreStats(
        [FromRoute] Guid id,
        CancellationToken ct = default)
    {
        _logger.LogInformation("Get store stats request - StoreId: {StoreId}", id);

        try
        {
            var result = await _storeService.GetStoreStatsAsync(id, ct);

            return Ok(new ApiResponse<StoreStatsResponse>
            {
                Success = true,
                Message = "Store statistics retrieved successfully",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving store stats");
            throw;
        }
    }

    /// <summary>
    /// Update store information (seller only).
    /// </summary>
    /// <param name="id">Store ID</param>
    /// <param name="request">Updated store details</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Updated store</returns>
    /// <response code="200">Store updated successfully</response>
    /// <response code="400">Validation error</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Store not found</response>
    [Authorize]
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<StoreResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStore(
        [FromRoute] Guid id,
        [FromBody] UpdateStoreRequest request,
        CancellationToken ct = default)
    {
        _logger.LogInformation("Update store request - StoreId: {StoreId}", id);

        // Validate input
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Store name is required",
                Data = null
            });
        }

        try
        {
            var sellerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User ID not found in claims"));
            var result = await _storeService.UpdateStoreAsync(id, sellerId, request, ct);

            _logger.LogInformation("Store updated successfully: {StoreId}", id);

            return Ok(new ApiResponse<StoreResponse>
            {
                Success = true,
                Message = "Store updated successfully",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating store");
            throw;
        }
    }

    /// <summary>
    /// Get products for a store by store ID.
    /// </summary>
    /// <param name="storeId">Store ID</param>
    /// <param name="page">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Paginated products</returns>
    /// <response code="200">Products retrieved successfully</response>
    /// <response code="404">Store not found</response>
    [HttpGet("{storeId}/products")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<object>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStoreProducts(
        [FromRoute] Guid storeId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        _logger.LogInformation("Get store products request - StoreId: {StoreId}", storeId);

        // This would typically call a ProductService method with storeId
        // For now, returning a placeholder response
        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Store products retrieved successfully",
            Data = new { storeId, page, pageSize, products = new List<object>() }
        });
    }
}
