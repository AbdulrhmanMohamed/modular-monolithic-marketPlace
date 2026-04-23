namespace Modules.Shipping.Application.Handlers;

using MediatR;
using Modules.Shipping.Application.Commands;
using Modules.Shipping.Application.Results;
using Modules.Shipping.Domain.Interfaces;

public class UpdateTrackingHandler : IRequestHandler<UpdateTrackingCommand, ShipmentResult>
{
    private readonly IShipmentRepository _repository;

    public UpdateTrackingHandler(IShipmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<ShipmentResult> Handle(UpdateTrackingCommand request, CancellationToken cancellationToken)
    {
        var shipment = await _repository.GetByIdAsync(request.ShipmentId);

        if (shipment is null)
            return ShipmentResult.Bad("Shipment not found");

        shipment.Carrier = request.Carrier;
        shipment.TrackingNumber = request.TrackingNumber;

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