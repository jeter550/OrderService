using MediatR;
using OrderService.Application.Commands;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;
using OrderService.Domain.Exceptions;

namespace OrderService.Application.Handlers;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _productRepo;

    public CreateProductHandler(IProductRepository productRepo)
    {
        _productRepo = productRepo;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken ct)
    {
        if (request.UnitPrice <= 0)
            throw new DomainException("Preço unitário inválido");

        if (request.AvailableQuantity < 0)
            throw new DomainException("Quantidade disponível inválida");

        var product = new Product(Guid.NewGuid(), request.UnitPrice, request.AvailableQuantity);

        await _productRepo.Add(product);
        await _productRepo.Save();

        return product.Id;
    }
}
