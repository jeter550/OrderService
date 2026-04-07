using MediatR;
using OrderService.Application.Commands;
using OrderService.Application.Interfaces;
using OrderService.Domain.Enums;
using OrderService.Domain.Exceptions;

namespace OrderService.Application.Handlers
{
    public class ConfirmOrderHandler : IRequestHandler<ConfirmOrderCommand, Unit>
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IProductRepository _productRepo;

        public ConfirmOrderHandler(
            IOrderRepository orderRepo,
            IProductRepository productRepo)
        {
            _orderRepo = orderRepo;
            _productRepo = productRepo;
        }

        public async Task<Unit> Handle(ConfirmOrderCommand request, CancellationToken ct)
        {
            var order = await _orderRepo.GetById(request.OrderId)
                ?? throw new DomainException("Pedido não encontrado");

            // IDEMPOTÊNCIA
            if (order.Status == OrderStatus.Confirmed)
                return Unit.Value;

            if (order.Status != OrderStatus.Placed)
                throw new DomainException("Pedido não pode ser confirmado");

            // BAIXA ESTOQUE
            foreach (var item in order.Items)
            {
                var product = await _productRepo.GetById(item.ProductId)
                    ?? throw new DomainException("Produto não encontrado");

                product.Reserve(item.Quantity);
            }

            order.Confirm();

            await _orderRepo.Save();

            return Unit.Value;
        }
    }
}