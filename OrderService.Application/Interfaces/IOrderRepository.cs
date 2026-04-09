using OrderService.Domain.Enums;
using OrderService.Domain.Entities;

namespace OrderService.Application.Interfaces;

public interface IOrderRepository
{
    Task Add(Order order);
    Task<Order?> GetById(Guid id);
    Task<(IReadOnlyCollection<Order> Orders, int TotalCount)> List(
        Guid? customerId,
        OrderStatus? status,
        DateTime? from,
        DateTime? to,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
    Task Save();
}
