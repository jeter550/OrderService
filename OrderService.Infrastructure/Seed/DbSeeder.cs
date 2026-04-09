using OrderService.Domain.Entities;
using OrderService.Infrastructure.Persistence;
namespace OrderService.Infrastructure.Seek;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (context.Products.Any())
            return;

        var products = new List<Product>
        {
            new Product(Guid.Parse("11111111-1111-1111-1111-111111111111"), 100, 50),
            new Product(Guid.Parse("22222222-2222-2222-2222-222222222222"), 200, 30),
            new Product(Guid.Parse("33333333-3333-3333-3333-333333333333"), 50, 100)
        };

        context.Products.AddRange(products);

        await context.SaveChangesAsync();
    }
}