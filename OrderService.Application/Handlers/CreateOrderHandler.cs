using MediatR;
using OrderService.Application.Commands;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;
using OrderService.Domain.Exceptions;

namespace OrderService.Application.Handlers;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepo;
    private readonly IProductRepository _productRepo;

    public CreateOrderHandler(
        IOrderRepository orderRepo,
        IProductRepository productRepo)
    {
        _orderRepo = orderRepo;
        _productRepo = productRepo;
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken ct)
    {
        if (request.Items == null || !request.Items.Any())
            throw new DomainException("Pedido deve ter itens");

        var order = new Order(request.CustomerId, request.Currency);

        foreach (var item in request.Items)
        {
            if (item.Quantity <= 0)
                throw new DomainException("Quantidade inválida");

            var product = await _productRepo.GetById(item.ProductId);

            if (product == null)
                throw new DomainException($"Produto {item.ProductId} não existe");

            if (product.AvailableQuantity < item.Quantity)
                throw new DomainException("Estoque insuficiente");

            order.AddItem(product.Id, product.UnitPrice, item.Quantity);
        }

        await _orderRepo.Add(order);
        await _orderRepo.Save();

        return order.Id;
    }
}