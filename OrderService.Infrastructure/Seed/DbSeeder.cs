using OrderService.Domain.Entities;
using OrderService.Domain.Enums;
using OrderService.Infrastructure.Persistence;

namespace OrderService.Infrastructure.Seek;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        //Primeira carga apenas para testar a integração
        if (context.Products.Any() || context.Orders.Any())
            return;

        var products = CreateProducts();
        var orders = CreateOrders(products);

        context.Products.AddRange(products);
        context.Orders.AddRange(orders);

        await context.SaveChangesAsync();
    }

    private static List<Product> CreateProducts()
    {
        return
        [
            new Product(Guid.Parse("11111111-1111-1111-1111-111111111111"), 19.90m, 10000),
            new Product(Guid.Parse("22222222-2222-2222-2222-222222222222"), 24.50m, 10000),
            new Product(Guid.Parse("33333333-3333-3333-3333-333333333333"), 39.90m, 10000),
            new Product(Guid.Parse("44444444-4444-4444-4444-444444444444"), 49.90m, 10000),
            new Product(Guid.Parse("55555555-5555-5555-5555-555555555555"), 59.90m, 10000),
            new Product(Guid.Parse("66666666-6666-6666-6666-666666666666"), 79.90m, 10000),
            new Product(Guid.Parse("77777777-7777-7777-7777-777777777777"), 99.90m, 10000),
            new Product(Guid.Parse("88888888-8888-8888-8888-888888888888"), 129.90m, 10000),
            new Product(Guid.Parse("99999999-9999-9999-9999-999999999999"), 159.90m, 10000),
            new Product(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), 199.90m, 10000)
        ];
    }

    private static List<Order> CreateOrders(List<Product> products)
    {
        var orders = new List<Order>(capacity: 500);

        for (var orderIndex = 0; orderIndex < 500; orderIndex++)
        {
            var product = products[orderIndex % products.Count];
            var quantity = (orderIndex / products.Count) + 1;

            var order = new Order(CreateCustomerId(orderIndex), "BRL");
            order.AddItem(product.Id, product.UnitPrice, quantity);

            ApplyStatus(order, product, quantity, orderIndex);

            orders.Add(order);
        }

        return orders;
    }

    private static void ApplyStatus(Order order, Product product, int quantity, int orderIndex)
    {
        var statusBucket = orderIndex % 10;

        var targetStatus = statusBucket switch
        {
            <= 5 => OrderStatus.Confirmed,
            <= 7 => OrderStatus.Canceled,
            8 => OrderStatus.Placed,
            _ => OrderStatus.Canceled
        };

        if (targetStatus == OrderStatus.Confirmed)
        {
            product.Reserve(quantity);
            order.Confirm();
            return;
        }

        if (targetStatus == OrderStatus.Canceled && statusBucket <= 7)
        {
            product.Reserve(quantity);
            order.Confirm();
            product.Release(quantity);
        }

        if (targetStatus == OrderStatus.Canceled)
            order.Cancel();
    }

    private static Guid CreateCustomerId(int orderIndex)
    {
        var suffix = (orderIndex + 1).ToString("D12");
        return Guid.Parse($"00000000-0000-0000-0000-{suffix}");
    }
}
