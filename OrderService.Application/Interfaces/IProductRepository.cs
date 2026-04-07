using OrderService.Domain.Entities;

namespace OrderService.Application.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetById(Guid id);
    //TODO: ESTENDER DEPOIS
    //Task<IEnumerable<Order>> GetPaged(int page);
}