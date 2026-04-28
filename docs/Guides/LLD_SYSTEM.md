# Low Level Design - Complete System

---

## Table of Contents

1. [System Overview](#1-system-overview)
2. [Module Communication Map](#2-module-communication-map)
3. [Cross-Module Sequences](#3-cross-module-sequences)
4. [Shared Components](#4-shared-components)
5. [Data Flow Architecture](#5-data-flow-architecture)
6. [Event Communication](#6-event-communication)
7. [Integration Points](#7-integration-points)

---

## 1. System Overview

### 1.1 Complete Module Map

```mermaid
graph TB
    subgraph "Client Layer"
        Web[Web Client]
        Mobile[Mobile App]
        Admin[Admin Panel]
    end
    
    subgraph "API Layer"
        AuthAPI[Auth Controller]
        ProductAPI[Product Controller]
        CartAPI[Cart Controller]
        OrderAPI[Order Controller]
        CategoryAPI[Category Controller]
        PaymentAPI[Payment Controller]
        ShippingAPI[Shipping Controller]
    end
    
    subgraph "Application Layer"
        AuthApp[Auth Service]
        ProductApp[Product Service]
        CartApp[Cart Service]
        OrderApp[Order Service]
        CategoryApp[Category Service]
        PaymentApp[Payment Service]
        ShippingApp[Shipping Service]
    end
    
    subgraph "Infrastructure Layer"
        Db[(Database)]
        TokenSvc[Token Service]
        Gateway[Payment Gateway]
        Carrier[Carrier API]
        Notif[Notification Service]
    end
    
    Web --> AuthAPI
    Web --> ProductAPI
    Web --> CartAPI
    Web --> OrderAPI
    
    Admin --> ProductAPI
    Admin --> CategoryAPI
    Admin --> OrderAPI
    
    Mobile --> AuthAPI
    Mobile --> ProductAPI
    Mobile --> CartAPI
    Mobile --> OrderAPI
    
    AuthAPI --> AuthApp
    ProductAPI --> ProductApp
    CartAPI --> CartApp
    OrderAPI --> OrderApp
    CategoryAPI --> CategoryApp
    PaymentAPI --> PaymentApp
    ShippingAPI --> ShippingApp
    
    AuthApp --> TokenSvc
    ProductApp --> Db
    CartApp --> Db
    OrderApp --> Db
    CategoryApp --> Db
    PaymentApp --> Db
    PaymentApp --> Gateway
    ShippingApp --> Db
    ShippingApp --> Carrier
    ShippingApp --> Notif
```

---

## 2. Module Communication Map

### 2.1 Module Dependencies

```mermaid
graph LR
    Auth -->|User data| Products
    Auth -->|User data| Cart
    Auth -->|User data| Order
    Auth -->|User data| Category
    
    Products -->|Product data| Cart
    Products -->|Product data| Order
    Products -->|Category link| Category
    
    Cart -->|Cart data| Order
    
    Order -->|Order data| Payment
    Order -->|Order data| Shipping
    Order -->|Inventory| Products
```

### 2.2 Dependency Table

| Module | Depends On | Provides To |
|--------|------------|-------------|
| **Auth** | None | Products, Cart, Order, Category |
| **Products** | None | Cart, Order, Category |
| **Category** | None | Products |
| **Cart** | Auth, Products | Order |
| **Order** | Auth, Cart, Products, Category | Payment, Shipping |
| **Payment** | Order | None |
| **Shipping** | Order, Auth | None |

### 2.3 Service Communication Matrix

| Consumer | Auth | Products | Category | Cart | Order | Payment | Shipping |
|-----------|------|----------|----------|------|-------|---------|----------|
| **AuthService** | - | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ |
| **ProductService** | UserId validation | - | Category lookup | Product lookup | Product lookup | ❌ | ❌ |
| **CategoryService** | UserId validation | Category link | - | ❌ | ❌ | ❌ | ❌ |
| **CartService** | UserId, Session | Product lookup | ❌ | - | Cart data | ❌ | ❌ |
| **OrderService** | UserId | Inventory decrement | Product data | Cart lookup | - | Payment create | Shipment create |
| **PaymentService** | ❌ | ❌ | ❌ | ❌ | Order lookup | - | ❌ |
| **ShippingService** | UserId | ❌ | ❌ | ❌ | Order lookup | ❌ | - |

---

## 3. Cross-Module Sequences

### 3.1 Complete Checkout Flow

```mermaid
sequenceDiagram
    participant User
    participant CartAPI
    participant OrderAPI
    participant AuthService
    participant ProductService
    participant CartService
    participant OrderService
    participant PaymentService
    participant ShippingService
    participant Database
    
    User->>CartAPI: POST /cart/checkout
    CartAPI->>CartService: GetCartWithItems(cartId)
    CartService->>Database: Get cart items
    Database-->>CartService: items
    
    CartService->>ProductService: ValidateStock(items)
    ProductService->>Database: Check inventory
    Database-->>ProductService: stock available
    
    ProductService-->>CartService: validated
    
    CartService-->CartAPI: cart data
    CartAPI->>OrderService: PlaceOrder(cartId, addressId)
    OrderService->>AuthService: ValidateUser(userId)
    AuthService-->>OrderService: user valid
    
    OrderService->>ProductService: DecrementStock(items)
    ProductService->>Database: UPDATE inventory
    Database-->>ProductService: success
    
    OrderService->>CartService: ClearCart(cartId)
    CartService->>Database: DELETE items
    Database-->>CartService: cleared
    
    OrderService->>OrderService: CreateOrder()
    OrderService->>Database: INSERT order
    Database-->>OrderService: order created
    
    OrderService->>OrderService: CreateOrderItems()
    OrderService->>Database: INSERT items
    Database-->>OrderService: items created
    
    OrderService->>PaymentService: CreatePayment(orderId)
    PaymentService->>Database: INSERT payment
    Database-->>PaymentService: payment created
    
    OrderService->>ShippingService: CreateShipment(orderId, addressId)
    ShippingService->>Database: INSERT shipment
    Database-->>ShippingService: shipment created
    
    OrderService-->>CartAPI: OrderResult
    CartAPI-->>User: 201 Created
```

### 3.2 Product Browse Flow

```mermaid
sequenceDiagram
    participant User
    participant ProductAPI
    participant CategoryAPI
    participant ProductService
    participant CategoryService
    participant Database
    
    User->>ProductAPI: GET /products?category=laptops
    ProductAPI->>ProductService: GetByCategory(categoryId)
    ProductService->>CategoryService: GetCategoryBySlug(laptops)
    CategoryService->>Database: Get category
    Database-->>CategoryService: category
    
    CategoryService-->>ProductService: categoryId
    ProductService->>Database: Get products by category
    Database-->>ProductService: products
    
    ProductService-->>ProductAPI: products
    ProductAPI-->>User: 200 OK
```

### 3.3 User Registration to First Order

```mermaid
sequenceDiagram
    participant User
    participant AuthAPI
    participant ProductAPI
    participant CartAPI
    participant OrderAPI
    participant AuthService
    participant ProductService
    participant CartService
    participant OrderService
    
    rect rgb(240, 248, 255)
        Note over User,AuthService: Step 1: Register
        User->>AuthAPI: POST /auth/register
        AuthAPI->>AuthService: Register(email, password)
        AuthService->>AuthService: Hash password
        AuthService->>AuthService: Generate JWT
        AuthService-->>AuthAPI: token
        AuthAPI-->>User: user + token
    end
    
    rect rgb(255, 250, 240)
        Note over User,ProductService: Step 2: Browse Products
        User->>ProductAPI: GET /products
        ProductAPI->>ProductService: GetAll()
        ProductService-->>ProductAPI: products
        ProductAPI-->>User: product list
    end
    
    rect rgb(240, 255, 250)
        Note over User,CartService: Step 3: Add to Cart
        User->>CartAPI: POST /cart/items
        CartAPI->>CartService: AddItem(productId, qty)
        CartService->>ProductService: ValidateStock()
        ProductService-->>CartService: valid
        CartService-->>CartAPI: cart item
        CartAPI-->>User: cart updated
    end
    
    rect rgb(255, 240, 250)
        Note over User,OrderService: Step 4: Checkout
        User->>OrderAPI: POST /orders
        OrderAPI->>OrderService: PlaceOrder()
        OrderService->>AuthService: Validate user
        OrderService->>CartService: Get items
        OrderService->>ProductService: Decrement stock
        OrderService->>OrderService: Create order
        OrderService-->>OrderAPI: order created
        OrderAPI-->>User: order confirmation
    end
```

---

## 4. Shared Components

### 4.1 Cross-Cutting Concerns

```mermaid
classDiagram
    class IUnitOfWork {
        <<interface>>
        +BeginTransaction() Task~ITransaction~
        +CommitAsync() Task
        +RollbackAsync() Task
    }
    
    class IDomainEventDispatcher {
        <<interface>>
        +Publish~TEvent~(event) Task
        +Subscribe~TEvent~(handler) Task
    }
    
    class ICurrentUserService {
        <<interface>>
        +UserId~int~
        +Role~string~
        +IsAuthenticated~bool~
    }
    
    class IValidationService {
        <<interface>>
        +Validate~T~(request) Task~ValidationResult~
    }
    
    class IDateTimeService {
        <<interface>>
        +UtcNow~DateTime~
        +Now~DateTime~
    }
```

### 4.2 Shared DTOs

```mermaid
classDiagram
    class UserDto {
        +int Id
        +string Username
        +string Email
        +string Role
    }
    
    class ProductDto {
        +int Id
        +string Name
        +string Slug
        +decimal Price
        +int Stock
    }
    
    class AddressDto {
        +int Id
        +string FullName
        +string Street
        +string City
        +string ZipCode
    }
    
    class ErrorDto {
        +string Code
        +string Message
        +string~? Details
    }
    
    class PagedResult~T~ {
        +List~T~ Items
        +int TotalCount
        +int Page
        +int PageSize
    }
```

---

## 5. Data Flow Architecture

### 5.1 Read Paths (Queries)

```mermaid
flowchart LR
    subgraph "Products Read"
        P1[Browse Products] --> P2[Product Service]
        P2 --> P3[Product Repository]
        P3 --> P4[(Products Table)]
    end
    
    subgraph "Category Read"
        C1[Browse Categories] --> C2[Category Service]
        C2 --> C3[Category Repository]
        C3 --> C4[(Categories Table)]
    end
    
    subgraph "Cart Read"
        CR1[View Cart] --> CR2[Cart Service]
        CR2 --> CR3[Cart Repository]
        CR3 --> CR4[(Carts + Items Tables)]
    end
    
    subgraph "Order Read"
        O1[View Orders] --> O2[Order Service]
        O2 --> O3[Order Repository]
        O3 --> O4[(Orders + Items Tables)]
    end
```

### 5.2 Write Paths (Commands)

```mermaid
flowchart LR
    subgraph "Auth Write"
        A1[Register/Login] --> A2[Auth Service]
        A2 --> A3[User Repository]
        A3 --> A4[(Users Table)]
    end
    
    subgraph "Product Write"
        P1[Manage Product] --> P2[Product Service]
        P2 --> P3[Product Repository]
        P3 --> P4[(Products Table)]
    end
    
    subgraph "Cart Write"
        C1[Modify Cart] --> C2[Cart Service]
        C2 --> C3[Cart Repository]
        C3 --> C4[(Carts + Items Tables)]
    end
    
    subgraph "Order Write"
        O1[Place Order] --> O2[Order Service]
        O2 --> O3[Order Repository]
        O2 --> O4[Inventory Service]
        O2 --> O5[Payment Service]
        O2 --> O6[Shipment Service]
        O3 --> O7[(Orders + Items Tables)]
        O4 --> O8[(Inventories Table)]
        O5 --> O9[(Payments Table)]
        O6 --> O10[(Shipments Table)]
    end
```

### 5.3 Event-Driven Paths

```mermaid
flowchart TB
    subgraph "Domain Events"
        E1[Order Placed] --> E2[Payment Initiated]
        E2 --> E3[Inventory Decremented]
        E3 --> E4[Shipment Created]
        E4 --> E5[Confirmation Sent]
    end
    
    subgraph "Integration Events"
        IE1[Payment Completed] --> IE2[Order Status Updated]
        IE2 --> IE3[Shipping Notified]
        IE3 --> IE4[Customer Notified]
    end
```

---

## 6. Event Communication

### 6.1 Domain Events

```csharp
// Published by OrderService when order is placed
public record OrderPlacedEvent : INotification {
    public int OrderId { get; init; }
    public int UserId { get; init; }
    public decimal TotalAmount { get; init; }
}

// Published by PaymentService when payment completes
public record PaymentCompletedEvent : INotification {
    public int PaymentId { get; init; }
    public int OrderId { get; init; }
    public bool Success { get; init; }
}

// Published by ShippingService when shipped
public record ShipmentShippedEvent : INotification {
    public int ShipmentId { get; init; }
    public int OrderId { get; init; }
    public string TrackingNumber { get; init; }
}
```

### 6.2 Event Handlers

```csharp
// Handle OrderPlacedEvent -> Create Payment
public class OrderPlacedPaymentHandler : INotificationHandler<OrderPlacedEvent> {
    public async Task Handle(OrderPlacedEvent notification, CancellationToken ct) {
        // Create pending payment record
    }
}

// Handle OrderPlacedEvent -> Decrement Inventory
public class OrderPlacedInventoryHandler : INotificationHandler<OrderPlacedEvent> {
    public async Task Handle(OrderPlacedEvent notification, CancellationToken ct) {
        // Decrement stock for each item
    }
}

// Handle OrderPlacedEvent -> Create Shipment
public class OrderPlacedShipmentHandler : INotificationHandler<OrderPlacedEvent> {
    public async Task Handle(OrderPlacedEvent notification, CancellationToken ct) {
        // Create shipment record
    }
}

// Handle PaymentCompletedEvent -> Update Order Status
public class PaymentCompletedOrderHandler : INotificationHandler<PaymentCompletedEvent> {
    public async Task Handle(PaymentCompletedEvent notification, CancellationToken ct) {
        // Update order to Confirmed status
    }
}
```

### 6.3 Event Flow Diagram

```mermaid
sequenceDiagram
    participant OrderService
    participant Mediator
    participant PaymentHandler
    participant InventoryHandler
    participant ShipmentHandler
    participant NotificationHandler
    
    OrderService->>Mediator: Publish(OrderPlacedEvent)
    
    Mediator->>PaymentHandler: Handle(event)
    PaymentHandler-->>Mediator: Complete
    
    Mediator->>InventoryHandler: Handle(event)
    InventoryHandler-->>Mediator: Complete
    
    Mediator->>ShipmentHandler: Handle(event)
    ShipmentHandler-->>Mediator: Complete
    
    Mediator->>NotificationHandler: Handle(event)
    NotificationHandler-->>Mediator: Complete
```

---

## 7. Integration Points

### 7.1 External Services

| Service | Integration Type | Purpose |
|---------|----------------|---------|
| **Stripe/PayPal** | API Gateway | Payment processing |
| **UPS/FedEx/DHL** | Carrier API | Shipping labels + tracking |
| **SendGrid/SNS** | Notification API | Email/SMS notifications |
| **S3/Cloudflare** | Storage CDN | Image serving |

### 7.2 Service Adapters

```mermaid
classDiagram
    class IPaymentGateway {
        <<interface>>
        +Charge(amount, currency, paymentMethod) Task~ChargeResult~
        +Refund(chargeId, amount) Task~RefundResult~
    }
    
    class StripeGateway {
        +Charge(amount, currency, paymentMethod) Task~ChargeResult~
        +Refund(chargeId, amount) Task~RefundResult~
    }
    
    class PayPalGateway {
        +Charge(amount, currency, paymentMethod) Task~ChargeResult~
        +Refund(chargeId, amount) Task~RefundResult~
    }
    
    class ICarrierApi {
        <<interface>>
        +CreateLabel(shipment) Task~LabelResult~
        +GetTracking(trackingNumber) Task~TrackingResult~
    }
    
    class UPSCarrier {
        +CreateLabel(shipment) Task~LabelResult~
        +GetTracking(trackingNumber) Task~TrackingResult~
    }
    
    class FedExCarrier {
        +CreateLabel(shipment) Task~LabelResult~
        +GetTracking(trackingNumber) Task~TrackingResult~
    }
    
    IPaymentGateway <|.. StripeGateway
    IPaymentGateway <|.. PayPalGateway
    ICarrierApi <|.. UPSCarrier
    ICarrierApi <|.. FedExCarrier
```

### 7.3 Database Integration

```mermaid
erDiagram
    USERS ||--o{ ADDRESSES : has
    USERS ||--o{ CARTS : has
    USERS ||--o{ ORDERS : places
    
    PRODUCTS ||--o{ PRODUCT_CATEGORIES : belongs_to
    PRODUCTS ||--o{ CART_ITEMS : in
    PRODUCTS ||--o{ ORDER_ITEMS : in
    PRODUCTS ||--o{ INVENTORIES : has
    
    CATEGORIES ||--o{ PRODUCT_CATEGORIES : groups
    
    CARTS ||--o{ CART_ITEMS : contains
    
    ORDERS ||--o{ ORDER_ITEMS : contains
    ORDERS ||--o{ PAYMENTS : has
    ORDERS ||--o{ SHIPMENTS : ships
    
    ADDRESSES ||--o{ SHIPMENTS : ships_to
```

---

## Appendix: Module Communication Summary

### User Journeys

| Step | Module | Operation |
|------|--------|-----------|
| 1 | Auth | Register/Login |
| 2 | Products | Browse/Search |
| 3 | Category | Browse categories |
| 4 | Products | View product detail |
| 5 | Cart | Add to cart |
| 6 | Cart | Update quantity |
| 7 | Cart | View cart |
| 8 | Order | Place order |
| 9 | Order | Inventory validation |
| 10 | Order | Create order |
| 11 | Order | Clear cart |
| 12 | Payment | Process payment |
| 13 | Shipping | Create shipment |
| 14 | Shipping | Track shipment |

### Error Flows

| Error | Module | Response |
|-------|--------|----------|
| Invalid credentials | Auth | 401 Unauthorized |
| Product not found | Products | 404 Not Found |
| Out of stock | Products | 400 Bad Request |
| Empty cart | Cart | 400 Bad Request |
| Cart item stale | Cart | 409 Conflict |
| Insufficient inventory | Order | 400 Bad Request |
| Payment failed | Payment | 400 Bad Request |
| Shipping failed | Shipping | 400 Bad Request |

---

*See [LLD_INDEX.md](./LLD_INDEX.md) for individual module details*
*See [DB_DESIGN.md](../Database/DB_DESIGN.md) for database schema*