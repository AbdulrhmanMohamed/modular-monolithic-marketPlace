# Order CQRS Implementation

---

## Commands

```csharp
public record PlaceOrderCommand(int UserId, int? AddressId) : IRequest<OrderResult>;
public record CancelOrderCommand(int OrderId) : IRequest<OrderResult>;
public record UpdateOrderStatusCommand(int OrderId, string Status) : IRequest<OrderResult>;
```

## Queries

```csharp
public record GetOrderByIdQuery(int OrderId) : IRequest<OrderResult>;
public record GetOrderByNumberQuery(string OrderNumber) : IRequest<OrderResult>;
public record GetUserOrdersQuery(int UserId) : IRequest<OrderResult>;
```