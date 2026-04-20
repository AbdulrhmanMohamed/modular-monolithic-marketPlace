namespace Modules.Order.Application.Results;

public class OrderResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public OrderDto? Order { get; set; }
    public List<OrderDto>? Orders { get; set; }
}

public class OrderDto
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime? PlacedAt { get; set; }
    public List<OrderItemDto>? Items { get; set; }
    public PaymentDto? Payment { get; set; }
    public ShipmentDto? Shipment { get; set; }
}

public class OrderItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}

public class PaymentDto
{
    public string Status { get; set; } = string.Empty;
    public string? TransactionId { get; set; }
}

public class ShipmentDto
{
    public string Carrier { get; set; } = string.Empty;
    public string TrackingNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}