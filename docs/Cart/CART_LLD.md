# Cart Component - Low Level Design

---

## Table of Contents

1. [Use Cases](#1-use-cases)
2. [Class Diagram](#2-class-diagram)
3. [Sequence Diagrams](#3-sequence-diagrams)
4. [State Diagram](#4-state-diagram)
5. [Design Patterns](#5-design-patterns)
6. [SOLID Compliance](#6-solid-compliance)

---

## 1. Use Cases

### 1.1 Add to Cart
### 1.2 Remove from Cart
### 1.3 Update Quantity
### 1.4 Get Cart
### 1.5 Clear Cart

---

## 2. Class Diagram

```mermaid
classDiagram
    class ICartService {
        <<interface>>
        +AddItem(command) Task~CartResult~
        +RemoveItem(id) Task~bool~
        +UpdateQuantity(command) Task~CartResult~
        +GetCart(query) Task~CartResult~
    }
    
    class CartService {
        -ICartRepository _repo
    }
    
    class Cart {
        +int Id
        +int UserId
    }
    
    class CartItem {
        +int Id
        +int CartId
        +int ProductId
        +int Quantity
    }
    
    ICartService <|.. CartService
```

---

## 3. Sequence: Add to Cart

```mermaid
sequenceDiagram
    participant User
    participant CartController
    participant CartService
    participant CartRepository
    
    User->>CartController: POST /cart/items
    CartController->>CartService: AddItem(command)
    CartService->>CartRepository: AddItem(item)
    CartRepository-->>CartService: created
    CartService-->>CartController: CartResult
    CartController-->>User: 200 OK
```

---

## 4. State Diagram

```mermaid
stateDiagram-v2
    [*] --> Empty
    Empty --> Active: Add item
    Active --> Empty: Clear
    Active --> Active: Add/Remove
```

---

## 5. Design Patterns ✅

- CQRS
- Repository
- Unit of Work

---

## 6. SOLID ✅