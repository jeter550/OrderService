using MediatR;
namespace OrderService.Application.Commands
{
    public record CancelOrderCommand(Guid OrderId) : IRequest<Unit>;
}

