namespace Application.DTOs.Order;

public class OrderItemResponse
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductTitle { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal PriceAtPurchase { get; set; }
    public decimal SubTotal { get; set; }
}
