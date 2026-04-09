using MediatR;
using OrderService.Application.DTOs;

namespace OrderService.Application.Queries;

public record ListOrdersQuery(
    Guid? CustomerId,
    string? Status,
    DateTime? From,
    DateTime? To,
    int Page,
    int PageSize) : IRequest<PagedResultDto<OrderDto>>;
