namespace Modules.Shipping.Application.Commands;

using MediatR;
using Modules.Shipping.Application.Results;

public record CreateShipmentCommand(
    int OrderId
) : IRequest<ShipmentResult>;

public record ShipOrderCommand(
    int OrderId,
    string Carrier,
    string TrackingNumber
) : IRequest<ShipmentResult>;

public record UpdateTrackingCommand(
    int ShipmentId,
    string Carrier,
    string TrackingNumber
) : IRequest<ShipmentResult>;

public record CancelShipmentCommand(
    int ShipmentId
) : IRequest<ShipmentResult>;