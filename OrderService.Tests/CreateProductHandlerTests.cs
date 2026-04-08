using FluentAssertions;
using Moq;
using OrderService.Application.Commands;
using OrderService.Application.Handlers;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;

public class CreateProductHandlerTests
{
    private readonly Mock<IProductRepository> _productRepo = new();

    [Fact]
    public async Task Should_Create_Product_When_Valid()
    {
        var handler = new CreateProductHandler(_productRepo.Object);

        var command = new CreateProductCommand(99.90m, 15);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().NotBeEmpty();

        _productRepo.Verify(x => x.Add(It.Is<Product>(p =>
            p.Id == result &&
            p.UnitPrice == 99.90m &&
            p.AvailableQuantity == 15)), Times.Once);
        _productRepo.Verify(x => x.Save(), Times.Once);
    }

    [Fact]
    public async Task Should_Throw_When_UnitPrice_Is_Invalid()
    {
        var handler = new CreateProductHandler(_productRepo.Object);

        var command = new CreateProductCommand(0, 10);

        var act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Preço unitário inválido");
    }

    [Fact]
    public async Task Should_Throw_When_AvailableQuantity_Is_Invalid()
    {
        var handler = new CreateProductHandler(_productRepo.Object);

        var command = new CreateProductCommand(10, -1);

        var act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Quantidade disponível inválida");
    }
}
