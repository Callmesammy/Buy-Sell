namespace Application.DTOs.Review;

public class ReviewResponse
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string BuyerName { get; set; } = null!;
    public int Rating { get; set; }
    public string Comment { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
