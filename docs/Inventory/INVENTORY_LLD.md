# Inventory Component - Low Level Design

---

## Use Cases

1. Decrement Stock
2. Increment Stock
3. Reserve Stock
4. Release Stock
5. Get Stock

---

## Class Diagram

```mermaid
classDiagram
    class IInventoryRepository {
        <<interface>>
        +GetByProductIdAsync(productId) Task~Inventory?~
        +UpdateAsync(inventory) Task~Inventory~
        +CreateAsync(inventory) Task~Inventory~
    }
    
    class InventoryRepository {
        -InventoryDbContext _context
    }
    
    class Inventory {
        +int Id
        +int ProductId
        +int Quantity
        +int ReservedQuantity
    }
    
    IInventoryRepository <|.. InventoryRepository
    InventoryRepository --> Inventory
```

---

## Sequence: Decrement Stock

```mermaid
sequenceDiagram
    participant User
    participant InventoryController
    participant Mediator
    participant DecrementStockHandler
    participant InventoryRepository
    
    User->>InventoryController: POST /inventory/decrement
    InventoryController->>Mediator: Send(DecrementStockCommand)
    Mediator->>DecrementStockHandler: Handle
    DecrementStockHandler->>InventoryRepository: GetByProductIdAsync(productId)
    InventoryRepository-->>DecrementStockHandler: inventory
    DecrementStockHandler->>DecrementStockHandler: Check AvailableQuantity >= Quantity
    DecrementStockHandler->>InventoryRepository: UpdateAsync(inventory)
    InventoryRepository-->>DecrementStockHandler: updated
    DecrementStockHandler-->>Mediator: InventoryResult
    Mediator-->>InventoryController: InventoryResult
    InventoryController-->>User: 200 OK
```

---

## Sequence: Reserve Stock

```mermaid
sequenceDiagram
    participant OrderService
    participant Mediator
    participant ReserveStockHandler
    participant InventoryRepository
    
    OrderService->>Mediator: Send(ReserveStockCommand)
    Mediator->>ReserveStockHandler: Handle
    ReserveStockHandler->>InventoryRepository: GetByProductIdAsync(productId)
    ReserveStockHandler->>ReserveStockHandler: Check AvailableQuantity >= Quantity
    ReserveStockHandler->>InventoryRepository: UpdateAsync(inventory)
    ReserveStockHandler-->>Mediator: InventoryResult
```

---

## Design Patterns ✅

- CQRS (Commands/Queries)
- Repository
- Service Layer

---

## SOLID ✅

| Principle | Status | Description |
|-----------|--------|-------------|
| **S** - Single Responsibility | ✅ | Each handler has one job |
| **O** - Open/Closed | ✅ | Can extend with new operations |
| **L** - Liskov Substitution | ✅ | Repository interchangeable |
| **I** - Interface Segregation | ✅ | Focused interfaces |
| **D** - Dependency Inversion | ✅ | Depends on abstractions |

---

*Last updated: 2026-04-30*
