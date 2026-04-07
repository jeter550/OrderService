using OrderService.Domain.Enums;
using OrderService.Domain.Exceptions;

namespace OrderService.Domain.Entities;

public class Order
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public OrderStatus Status { get; private set; }
    public string Currency { get; private set; }
    public decimal Total { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items;

    private Order() { }

    public Order(Guid customerId, string currency)
    {
        if (customerId == Guid.Empty)
            throw new DomainException("CustomerId inválido");

        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainException("Currency inválida");

        Id = Guid.NewGuid();
        CustomerId = customerId;
        Currency = currency;
        Status = OrderStatus.Placed;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddItem(Guid productId, decimal unitPrice, int quantity)
    {
        var item = new OrderItem(productId, unitPrice, quantity);
        _items.Add(item);

        RecalculateTotal();
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Placed)
            throw new DomainException("Só é possível confirmar pedidos em status Placed");

        Status = OrderStatus.Confirmed;
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Canceled)
            return;

        if (Status != OrderStatus.Placed && Status != OrderStatus.Confirmed)
            throw new DomainException("Não é possível cancelar este pedido");

        Status = OrderStatus.Canceled;
    }

    private void RecalculateTotal()
    {
        Total = _items.Sum(x => x.Total);
    }
}