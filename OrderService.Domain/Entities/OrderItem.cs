using OrderService.Domain.Exceptions;

namespace OrderService.Domain.Entities;

public class OrderItem
{
    public Guid ProductId { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }

    public decimal Total => UnitPrice * Quantity;

    private OrderItem() { } // EF

    public OrderItem(Guid productId, decimal unitPrice, int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantidade deve ser maior que zero");

        ProductId = productId;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }
}