using OrderService.Domain.Entities;

namespace OrderService.Application.Interfaces;

public interface IOrderRepository
{
    Task Add(Order order);
    Task<Order?> GetById(Guid id);
    Task Save();
}