namespace OrderService.Application.DTOs;

public class CreateOrderDto
{
    public Guid CustomerId { get; set; }
    public string Currency { get; set; } = default!;
    public List<CreateOrderItemDto> Items { get; set; } = new();
}

public class CreateOrderItemDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}