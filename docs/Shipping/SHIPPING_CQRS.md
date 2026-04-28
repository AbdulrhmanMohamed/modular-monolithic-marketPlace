# Shipping CQRS Implementation

---

## Commands

```csharp
public record ShipOrderCommand(int OrderId, string Carrier, string TrackingNumber) : IRequest<ShipmentResult>;
```