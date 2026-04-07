using FluentAssertions;
using Moq;
using OrderService.Application.Commands;
using OrderService.Application.Interfaces;
using OrderService.Application.Handlers;
using OrderService.Domain.Entities;
using OrderService.Application.DTOs;

public class CreateOrderHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepo = new();
    private readonly Mock<IProductRepository> _productRepo = new();

    [Fact]
    public async Task Should_Create_Order_When_Valid()
    {
        // Arrange
        var productId = Guid.NewGuid();

        var product = new Product(productId, 100, 10);

        _productRepo
            .Setup(x => x.GetById(productId))
            .ReturnsAsync(product);

        var handler = new CreateOrderHandler(
            _orderRepo.Object,
            _productRepo.Object
        );

        var command = new CreateOrderCommand(
            Guid.NewGuid(),
            "BRL",
            new List<CreateOrderItemDto>
            {
                new() { ProductId = productId, Quantity = 2 }
            }
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();

        _orderRepo.Verify(x => x.Add(It.IsAny<Order>()), Times.Once);
        _orderRepo.Verify(x => x.Save(), Times.Once);
    }

    [Fact]
    public async Task Should_Throw_When_No_Items()
    {
        var handler = new CreateOrderHandler(
            _orderRepo.Object,
            _productRepo.Object
        );

        var command = new CreateOrderCommand(
            Guid.NewGuid(),
            "BRL",
            new List<CreateOrderItemDto>()
        );

        var act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Pedido deve ter itens");
    }

    [Fact]
    public async Task Should_Throw_When_Stock_Is_Insufficient()
    {
        var productId = Guid.NewGuid();

        var product = new Product(productId, 100, 1);

        _productRepo
            .Setup(x => x.GetById(productId))
            .ReturnsAsync(product);

        var handler = new CreateOrderHandler(
            _orderRepo.Object,
            _productRepo.Object
        );

        var command = new CreateOrderCommand(
            Guid.NewGuid(),
            "BRL",
            new List<CreateOrderItemDto>
            {
            new() { ProductId = productId, Quantity = 5 }
            }
        );

        var act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Estoque insuficiente");
    }
}