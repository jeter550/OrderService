using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Persistence;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Product?> GetById(Guid id)
    {
        return await _context.Products.FindAsync(id);
    }
    public async Task Add(Product product)
    {
        await _context.Products.Add(product);
    }
    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }
}