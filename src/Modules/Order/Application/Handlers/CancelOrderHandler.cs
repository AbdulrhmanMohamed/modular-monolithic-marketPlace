namespace Modules.Order.Application.Handlers;

using MediatR;
using Modules.Order.Application.Commands;
using Modules.Order.Application.Results;
using Modules.Order.Domain.Entities;
using Modules.Order.Domain.Interfaces;

public class CancelOrderHandler(IOrderRepository _orderRepo) : IRequestHandler<CancelOrderCommand, OrderResult>
{
    public async Task<OrderResult> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepo.GetById(request.OrderId);
        if (order == null)
            return new() { Success = false, Error = "Order not found" };

        if (order.Status == OrderStatus.Shipped || order.Status == OrderStatus.Delivered)
            return new() { Success = false, Error = "Cannot cancel shipped or delivered orders" };

        if (!OrderStateMachine.CanTransitionTo(order.Status, OrderStatus.Cancelled))
            return new() { Success = false, Error = $"Cannot transition from {order.Status} to {OrderStatus.Cancelled}" };

        await _orderRepo.UpdateStatus(request.OrderId, OrderStatus.Cancelled);

        var updated = await _orderRepo.GetById(request.OrderId);
        return new() { Success = true, Order = updated };
    }
}
}