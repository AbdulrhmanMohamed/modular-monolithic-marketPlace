namespace Modules.Order.Application.Handlers;

using MediatR;
using Modules.Order.Application.Commands;
using Modules.Order.Application.Results;
using Modules.Order.Domain.Entities;
using Modules.Order.Domain.Services;

public class UpdateOrderStatusHandler(IOrderRepository _orderRepo) : IRequestHandler<UpdateOrderStatusCommand, OrderResult>
{
    public async Task<OrderResult> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepo.GetById(request.OrderId);
        if (order == null)
            return new() { Success = false, Error = "Order not found" };

        if (!OrderStateMachine.CanTransitionTo(order.Status, request.Status))
            return new() { Success = false, Error = $"Invalid status transition from {order.Status} to {request.Status}. Valid transitions: {string.Join(", ", OrderStateMachine.GetValidTransitions(order.Status))}" };

        await _orderRepo.UpdateStatus(request.OrderId, request.Status);

        var updated = await _orderRepo.GetById(request.OrderId);
        return new() { Success = true, Order = updated };
    }
}
