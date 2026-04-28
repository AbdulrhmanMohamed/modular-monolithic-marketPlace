# Inventory CQRS Implementation

---

## Commands (Write)

```csharp
public record DecrementStockCommand(int ProductId, int Quantity) : IRequest<InventoryResult>;
public record IncrementStockCommand(int ProductId, int Quantity) : IRequest<InventoryResult>;
public record ReserveStockCommand(int ProductId, int Quantity) : IRequest<InventoryResult>;
public record ReleaseStockCommand(int ProductId, int Quantity) : IRequest<InventoryResult>;
```

## Queries (Read)

```csharp
public record GetStockQuery(int ProductId) : IRequest<InventoryResult>;
```