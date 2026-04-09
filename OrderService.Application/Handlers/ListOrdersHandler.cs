using MediatR;
using OrderService.Application.DTOs;
using OrderService.Application.Interfaces;
using OrderService.Application.Queries;
using OrderService.Domain.Entities;
using OrderService.Domain.Enums;
using OrderService.Domain.Exceptions;

namespace OrderService.Application.Handlers;

public class ListOrdersHandler : IRequestHandler<ListOrdersQuery, PagedResultDto<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;

    public ListOrdersHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<PagedResultDto<OrderDto>> Handle(ListOrdersQuery request, CancellationToken cancellationToken)
    {
        if (request.Page <= 0)
            throw new DomainException("Page deve ser maior que zero");

        if (request.PageSize <= 0)
            throw new DomainException("PageSize deve ser maior que zero");

        if (request.From.HasValue && request.To.HasValue && request.From > request.To)
            throw new DomainException("O intervalo de datas informado é inválido");

        OrderStatus? status = null;
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (!Enum.TryParse<OrderStatus>(request.Status, true, out var parsedStatus))
                throw new DomainException("Status inválido");

            status = parsedStatus;
        }

        var (orders, totalCount) = await _orderRepository.List(
            request.CustomerId,
            status,
            request.From,
            request.To,
            request.Page,
            request.PageSize,
            cancellationToken);

        return new PagedResultDto<OrderDto>
        {
            Items = orders.Select(MapOrder).ToList(),
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
        };
    }

    private static OrderDto MapOrder(Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            Status = order.Status.ToString(),
            Currency = order.Currency,
            Total = order.Total,
            CreatedAt = order.CreatedAt,
            Items = order.Items.Select(i => new OrderItemDto
            {
                ProductId = i.ProductId,
                UnitPrice = i.UnitPrice,
                Quantity = i.Quantity
            }).ToList()
        };
    }
}
