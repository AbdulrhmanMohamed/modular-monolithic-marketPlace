namespace Modules.Shipping.Application.Handlers;

using MediatR;
using Modules.Shipping.Application.Commands;
using Modules.Shipping.Application.Results;
using Modules.Shipping.Domain.Interfaces;

public class ShipOrderHandler : IRequestHandler<ShipOrderCommand, ShipmentResult>
{
    private readonly IShipmentRepository _repository;

    public ShipOrderHandler(IShipmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<ShipmentResult> Handle(ShipOrderCommand request, CancellationToken cancellationToken)
    {
        var shipments = await _repository.GetByOrderIdAsync(request.OrderId);
        var shipment = shipments.FirstOrDefault();

        if (shipment is null)
            return ShipmentResult.Bad("Shipment not found");

        if (!ShipmentStateMachine.CanTransitionTo(shipment.Status, ShipmentStatus.Shipped))
            return ShipmentResult.Bad($"Cannot transition from {shipment.Status} to {ShipmentStatus.Shipped}. Valid transitions: {string.Join(", ", ShipmentStateMachine.GetValidTransitions(shipment.Status))}");

        shipment.Carrier = request.Carrier;
        shipment.TrackingNumber = request.TrackingNumber;
        shipment.Status = ShipmentStatus.Shipped;
        shipment.ShippedAt = DateTime.UtcNow;

        var updated = await _repository.UpdateAsync(shipment);

        var dto = new ShipmentDto(
            updated!.Id,
            updated.OrderId,
            updated.Status,
            updated.Carrier,
            updated.TrackingNumber,
            updated.ShippedAt,
            updated.DeliveredAt
        );

        return ShipmentResult.Ok(dto);
    }
}