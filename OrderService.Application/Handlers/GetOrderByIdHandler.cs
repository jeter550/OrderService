using MediatR;
using OrderService.Application.DTOs;
using OrderService.Application.Interfaces;
using OrderService.Application.Queries;
using OrderService.Domain.Exceptions;
namespace OrderService.Application.Handlers;
public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
{
    private readonly IOrderRepository _orderRepo;

    public GetOrderByIdHandler(IOrderRepository orderRepo)
    {
        _orderRepo = orderRepo;
    }

    public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken ct)
    {
        var order = await _orderRepo.GetById(request.Id);

        if (order == null)
            throw new DomainException("Pedido não encontrado");

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