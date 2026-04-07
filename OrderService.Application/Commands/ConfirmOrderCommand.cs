using MediatR;

namespace OrderService.Application.Commands
{
    public record ConfirmOrderCommand(Guid OrderId) : IRequest<Unit>;
}
