using OrderService.Domain.Exceptions;

namespace OrderService.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int AvailableQuantity { get; private set; }

    private Product() { } // EF

    public Product(Guid id, decimal unitPrice, int availableQuantity)
    {
        Id = id;
        UnitPrice = unitPrice;
        AvailableQuantity = availableQuantity;
    }

    public void Reserve(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantidade inválida");

        if (AvailableQuantity < quantity)
            throw new DomainException("Estoque insuficiente");

        AvailableQuantity -= quantity;
    }

    public void Release(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantidade inválida");

        AvailableQuantity += quantity;
    }
}