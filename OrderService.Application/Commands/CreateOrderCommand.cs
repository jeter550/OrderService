using MediatR;
using OrderService.Application.DTOs;

namespace OrderService.Application.Commands
{
    public record CreateOrderCommand(
        Guid CustomerId,
        string Currency,
        List<CreateOrderItemDto> Items
    ) : IRequest<Guid>;
}