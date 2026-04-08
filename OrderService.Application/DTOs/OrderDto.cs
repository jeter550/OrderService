namespace OrderService.Application.DTOs;

public class OrderDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string Status { get; set; } = default!;
    public string Currency { get; set; } = default!;
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<OrderItemDto> Items { get; set; } = new();
}

public class OrderItemDto
{
    public Guid ProductId { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
}