# Order Component - Low Level Design

---

## Use Cases

1. Place Order
2. Get Order by ID
3. Get User Orders
4. Cancel Order
5. Update Status

---

## Class Diagram

```mermaid
classDiagram
    class IOrderService {
        +PlaceOrder(command) Task~OrderResult~
        +GetById(id) Task~OrderResult~
        +Cancel(id) Task~bool~
    }
    
    class OrderService {
        -IOrderRepository _repo
        -ICartService _cartService
        -IInventoryService _inventoryService
    }
    
    class Order {
        +int Id
        +int UserId
        +string Status
        +decimal TotalAmount
    }
    
    class OrderStatus {
        <<enum>>
        Pending, Processing, Confirmed, Shipped, Delivered, Cancelled
    }
    
    IOrderService <|.. OrderService
```

---

## Sequence: Place Order

```mermaid
sequenceDiagram
    participant User
    participant OrderController
    participant OrderService
    participant CartService
    participant InventoryService
    
    User->>OrderController: POST /orders
    OrderController->>OrderService: PlaceOrder(command)
    OrderService->>CartService: GetCart(cartId)
    OrderService->>InventoryService: DecrementStock(items)
    OrderService->>OrderService: CreateOrder()
    OrderService-->>OrderController: OrderResult
    OrderController-->>User: 201 Created
```

---

## State Diagram

```mermaid
stateDiagram-v2
    [*] --> Pending
    Pending --> Processing: Payment
    Processing --> Confirmed: Success
    Processing --> Cancelled: Failed
    Confirmed --> Shipped
    Shipped --> Delivered
```

---

## Design Patterns

- CQRS
- Transaction Script
- Domain Events

---

## SOLID ✅