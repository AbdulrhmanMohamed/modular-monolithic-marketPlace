namespace Modules.Shipping.Domain.Services;

using Modules.Shipping.Domain.Entities;

public static class ShipmentStateMachine
{
    private static readonly Dictionary<ShipmentStatus, ShipmentStatus[]> _validTransitions = new()
    {
        { ShipmentStatus.Pending, new[] { ShipmentStatus.Shipped, ShipmentStatus.Cancelled } },
        { ShipmentStatus.Shipped, new[] { ShipmentStatus.Delivered, ShipmentStatus.Cancelled } },
        { ShipmentStatus.Delivered, Array.Empty<ShipmentStatus>() },
        { ShipmentStatus.Cancelled, Array.Empty<ShipmentStatus>() }
    };

    public static bool CanTransitionTo(ShipmentStatus currentStatus, ShipmentStatus newStatus)
    {
        if (!_validTransitions.ContainsKey(currentStatus))
            return false;

        var allowedTransitions = _validTransitions[currentStatus];
        return allowedTransitions.Contains(newStatus);
    }

    public static ShipmentStatus[] GetValidTransitions(ShipmentStatus currentStatus)
    {
        if (!_validTransitions.ContainsKey(currentStatus))
            return Array.Empty<ShipmentStatus>();

        return _validTransitions[currentStatus];
    }
}
