namespace Application.DTOs.Order;

public class CreateOrderRequest
{
    /// <summary>
    /// If not provided, order will be created from the user's cart
    /// </summary>
    public List<OrderItemInput>? Items { get; set; }
}

public class OrderItemInput
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
