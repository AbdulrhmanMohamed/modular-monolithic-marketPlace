namespace Modules.Order.Domain.Services;

using Modules.Order.Domain.Entities;

public static class OrderStateMachine
{
    private static readonly Dictionary<OrderStatus, OrderStatus[]> _validTransitions = new()
    {
        { OrderStatus.Pending, new[] { OrderStatus.Processing, OrderStatus.Cancelled } },
        { OrderStatus.Processing, new[] { OrderStatus.Confirmed, OrderStatus.Cancelled } },
        { OrderStatus.Confirmed, new[] { OrderStatus.Shipped } },
        { OrderStatus.Shipped, new[] { OrderStatus.Delivered } },
        { OrderStatus.Delivered, Array.Empty<OrderStatus>() },
        { OrderStatus.Cancelled, Array.Empty<OrderStatus>() }
    };

    public static bool CanTransitionTo(OrderStatus currentStatus, OrderStatus newStatus)
    {
        if (!_validTransitions.ContainsKey(currentStatus))
            return false;

        var allowedTransitions = _validTransitions[currentStatus];
        return allowedTransitions.Contains(newStatus);
    }

    public static OrderStatus[] GetValidTransitions(OrderStatus currentStatus)
    {
        if (!_validTransitions.ContainsKey(currentStatus))
            return Array.Empty<OrderStatus>();

        return _validTransitions[currentStatus];
    }
}
