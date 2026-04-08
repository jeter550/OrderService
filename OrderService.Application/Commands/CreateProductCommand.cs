using MediatR;

namespace OrderService.Application.Commands;

public record CreateProductCommand(
    decimal UnitPrice,
    int AvailableQuantity
) : IRequest<Guid>;
