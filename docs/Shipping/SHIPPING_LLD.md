# Shipping Component - Low Level Design

---

## Use Cases

1. Create Shipment
2. Ship Order
3. Update Tracking
4. Cancel Shipment
5. Get Shipment by Order

---

## Class Diagram

```mermaid
classDiagram
    class IShipmentRepository {
        <<interface>>
        +CreateAsync(shipment) Task~Shipment~
        +UpdateAsync(shipment) Task~Shipment~
        +DeleteAsync(id) Task~bool~
        +GetByIdAsync(id) Task~Shipment?~
        +GetByOrderIdAsync(orderId) Task~Shipment?~
    }
    
    class ShipmentRepository {
        -ShippingDbContext _context
    }
    
    class Shipment {
        +int Id
        +int OrderId
        +string Status
        +string Carrier
        +string TrackingNumber
        +DateTime? ShippedAt
        +DateTime? DeliveredAt
    }
    
    class ShipmentStatus {
        <<enum>>
        Pending, Shipped, Delivered, Cancelled
    }
    
    IShipmentRepository <|.. ShipmentRepository
    ShipmentRepository --> Shipment
```

---

## Sequence: Create Shipment

```mermaid
sequenceDiagram
    participant User
    participant ShippingController
    participant Mediator
    participant CreateShipmentHandler
    participant ShipmentRepository
    
    User->>ShippingController: POST /shipping
    ShippingController->>Mediator: Send(CreateShipmentCommand)
    Mediator->>CreateShipmentHandler: Handle
    CreateShipmentHandler->>ShipmentRepository: CreateAsync(shipment)
    ShipmentRepository-->>CreateShipmentHandler: shipment
    CreateShipmentHandler-->>Mediator: ShipmentResult
    Mediator-->>ShippingController: ShipmentResult
    ShippingController-->>User: 201 Created
```

---

## Sequence: Ship Order

```mermaid
sequenceDiagram
    participant User
    participant ShippingController
    participant Mediator
    participant ShipOrderHandler
    participant ShipmentRepository
    
    User->>ShippingController: POST /shipping/ship
    ShippingController->>Mediator: Send(ShipOrderCommand)
    Mediator->>ShipOrderHandler: Handle
    ShipOrderHandler->>ShipmentRepository: UpdateAsync(shipment)
    ShipOrderHandler->>ShipOrderHandler: Set Status = Shipped
    ShipOrderHandler->>ShipOrderHandler: Set ShippedAt = DateTime.UtcNow
    ShipmentRepository-->>ShipOrderHandler: shipment
    ShipOrderHandler-->>Mediator: ShipmentResult
    Mediator-->>ShippingController: ShipmentResult
    ShippingController-->>User: 200 OK
```

---

## State Diagram

```mermaid
stateDiagram-v2
    [*] --> Pending
    Pending --> Shipped: Ship Order
    Shipped --> Delivered: Mark Delivered
    Pending --> Cancelled: Cancel
    Shipped --> Cancelled: Cancel
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
| **O** - Open/Closed | ✅ | Can extend with new shipment types |
| **L** - Liskov Substitution | ✅ | Repository interchangeable |
| **I** - Interface Segregation | ✅ | Focused interfaces |
| **D** - Dependency Inversion | ✅ | Depends on abstractions |

---

*Last updated: 2026-04-30*
