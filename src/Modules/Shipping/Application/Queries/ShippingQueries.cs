namespace Modules.Shipping.Application.Queries;

using MediatR;
using Modules.Shipping.Application.Results;

public record GetShipmentByOrderQuery(
    int OrderId
) : IRequest<ShipmentResult>;

public record GetShipmentByIdQuery(
    int ShipmentId
) : IRequest<ShipmentResult>;