namespace Modules.Shipping.Application.Results;

public record ShipmentResult(
    bool Success,
    ShipmentDto? Shipment,
    string? Error
)
{
    public static ShipmentResult Ok(ShipmentDto shipment) => new(true, shipment, null);
    public static ShipmentResult Bad(string error) => new(false, null, error);
};

public record ShipmentDto(
    int Id,
    int OrderId,
    ShipmentStatus Status,
    string? Carrier,
    string? TrackingNumber,
    DateTime? ShippedAt,
    DateTime? DeliveredAt
);