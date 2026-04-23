namespace Modules.Shipping.Application.Handlers;

using MediatR;
using Modules.Shipping.Application.Queries;
using Modules.Shipping.Application.Results;
using Modules.Shipping.Domain.Interfaces;

public class GetShipmentByOrderHandler : IRequestHandler<GetShipmentByOrderQuery, ShipmentResult>
{
    private readonly IShipmentRepository _repository;

    public GetShipmentByOrderHandler(IShipmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<ShipmentResult> Handle(GetShipmentByOrderQuery request, CancellationToken cancellationToken)
    {
        var shipments = await _repository.GetByOrderIdAsync(request.OrderId);
        var shipment = shipments.FirstOrDefault();

        if (shipment is null)
            return ShipmentResult.Bad("Shipment not found");

        var dto = new ShipmentDto(
            shipment.Id,
            shipment.OrderId,
            shipment.Status,
            shipment.Carrier,
            shipment.TrackingNumber,
            shipment.ShippedAt,
            shipment.DeliveredAt
        );

        return ShipmentResult.Ok(dto);
    }
}