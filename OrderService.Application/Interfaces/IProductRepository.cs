using OrderService.Domain.Entities;

namespace OrderService.Application.Interfaces;

public interface IProductRepository
{
    Task Add(Product product);
    Task<Product?> GetById(Guid id);
    Task Save();
    //TODO: ESTENDER DEPOIS
    //Task<IEnumerable<Order>> GetPaged(int page);
}
