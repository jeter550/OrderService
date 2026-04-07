using System.Data;
using MediatR;
using OrderService.Application.Commands;
using OrderService.Application.Interfaces;
using OrderService.Domain.Enums;
using OrderService.Domain.Exceptions;

namespace OrderService.Application.Handlers
{
    public class CancelOrderHandler : IRequestHandler<CancelOrderCommand, Unit>
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IProductRepository _productRepo;

        public CancelOrderHandler(
            IOrderRepository orderRepo,
            IProductRepository productRepo)
        {
            _orderRepo = orderRepo;
            _productRepo = productRepo;
        }
        
        public async Task<Unit> Handle(CancelOrderCommand request, CancellationToken ct)
        {
            var order = await _orderRepo.GetById(request.OrderId)
                ?? throw new DomainException("Pedido não encontrado");

            // IDEMPOTÊNCIA
            if (order.Status == OrderStatus.Canceled)
                return Unit.Value;

            // SE ESTAVA CONFIRMADO → DEVOLVE ESTOQUE
            if (order.Status == OrderStatus.Confirmed)
            {
                foreach (var item in order.Items)
                {
                    var product = await _productRepo.GetById(item.ProductId)
                        ?? throw new DomainException("Produto não encontrado");

                    product.Release(item.Quantity);
                }
            }

            order.Cancel();

            await _orderRepo.Save();

            return Unit.Value;
        }


    }
}