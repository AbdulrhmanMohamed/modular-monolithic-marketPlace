namespace Modules.Shipping.Application.Handlers;

using MediatR;
using Modules.Shipping.Application.Commands;
using Modules.Shipping.Application.Results;
using Modules.Shipping.Domain.Interfaces;

public class CancelShipmentHandler : IRequestHandler<CancelShipmentCommand, ShipmentResult>
{
    private readonly IShipmentRepository _repository;

    public CancelShipmentHandler(IShipmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<ShipmentResult> Handle(CancelShipmentCommand request, CancellationToken cancellationToken)
    {
        var shipment = await _repository.GetByIdAsync(request.ShipmentId);

        if (shipment is null)
            return ShipmentResult.Bad("Shipment not found");

        if (!ShipmentStateMachine.CanTransitionTo(shipment.Status, ShipmentStatus.Cancelled))
            return ShipmentResult.Bad($"Cannot transition from {shipment.Status} to {ShipmentStatus.Cancelled}. Valid transitions: {string.Join(", ", ShipmentStateMachine.GetValidTransitions(shipment.Status))}");

        shipment.Status = ShipmentStatus.Cancelled;

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