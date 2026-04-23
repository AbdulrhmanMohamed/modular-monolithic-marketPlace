namespace Modules.Shipping.Application.Handlers;

using MediatR;
using Modules.Shipping.Application.Commands;
using Modules.Shipping.Application.Results;
using Modules.Shipping.Domain.Entities;
using Modules.Shipping.Domain.Interfaces;

public class CreateShipmentHandler : IRequestHandler<CreateShipmentCommand, ShipmentResult>
{
    private readonly IShipmentRepository _repository;

    public CreateShipmentHandler(IShipmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<ShipmentResult> Handle(CreateShipmentCommand request, CancellationToken cancellationToken)
    {
        var shipment = new Shipment
        {
            OrderId = request.OrderId,
            Status = ShipmentStatus.Pending
        };

        var created = await _repository.CreateAsync(shipment);

        var dto = new ShipmentDto(
            created.Id,
            created.OrderId,
            created.Status,
            created.Carrier,
            created.TrackingNumber,
            created.ShippedAt,
            created.DeliveredAt
        );

        return ShipmentResult.Ok(dto);
    }
}