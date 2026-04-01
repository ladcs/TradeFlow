using TradeFlow.Orders.Enums;

namespace TradeFlow.Orders.models.Orders;

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string? Asset { get; set; }
    public OrderType Type { get; set; }
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}