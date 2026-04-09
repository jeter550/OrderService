using Microsoft.EntityFrameworkCore;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;
using OrderService.Domain.Enums;
using OrderService.Infrastructure.Persistence;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task Add(Order order)
    {
        await _context.Orders.AddAsync(order);
    }

    public async Task<Order?> GetById(Guid id)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<(IReadOnlyCollection<Order> Orders, int TotalCount)> List(
        Guid? customerId,
        OrderStatus? status,
        DateTime? from,
        DateTime? to,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Orders
            .Include(o => o.Items)
            .AsNoTracking()
            .AsQueryable();

        if (customerId.HasValue)
            query = query.Where(o => o.CustomerId == customerId.Value);

        if (status.HasValue)
            query = query.Where(o => o.Status == status.Value);

        if (from.HasValue)
            query = query.Where(o => o.CreatedAt >= from.Value);

        if (to.HasValue)
            query = query.Where(o => o.CreatedAt <= to.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        var orders = await query
            .OrderByDescending(o => o.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (orders, totalCount);
    }

    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }
}
