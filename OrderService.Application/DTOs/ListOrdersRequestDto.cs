namespace OrderService.Application.DTOs;

public class ListOrdersRequestDto
{
    public Guid? CustomerId { get; set; }
    public string? Status { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
