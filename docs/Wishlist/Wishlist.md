# Wishlist Module - Low Level Design

---

## Table of Contents

1. [Use Cases](#1-use-cases)
2. [Class Diagram](#2-class-diagram)
3. [Sequence Diagrams](#3-sequence-diagrams)
4. [Design Patterns](#4-design-patterns)
5. [SOLID Compliance](#5-solid-compliance)

---

## 1. Use Cases

### 1.1 Add to Wishlist

```
Actors: Customer
Flow: Select product → Add to wishlist → Return success
```

### 1.2 Remove from Wishlist

```
Actors: Customer
Flow: Select item → Remove → Return success
```

### 1.3 Get Wishlist

```
Actors: Customer
Flow: Fetch all wishlist items → Return with product details
```

### 1.4 Move to Cart

```
Actors: Customer
Flow: Select wishlist item → Add to cart → Optionally remove from wishlist
```

### 1.5 Share Wishlist

```
Actors: Customer
Flow: Generate share link → Return public URL
```

---

## 2. Class Diagram

```mermaid
classDiagram
    class IWishlistService {
        <<interface>>
        +AddItem(command) Task~WishlistResult~
        +RemoveItem(id) Task~bool~
        +GetByUser(userId) Task~List~WishlistResult~~
        +MoveToCart(id) Task~CartResult~
        +Share(userId) Task~string~
    }
    
    class WishlistService {
        -IWishlistRepository _repo
        -ICartService _cartService
    }
    
    class IWishlistRepository {
        <<interface>>
        +GetByUserId(userId) Task~List~WishlistItem~~
        +Add(item) Task~WishlistItem~
        +Remove(id) Task~bool~
    }
    
    class WishlistItem {
        +int Id
        +int UserId
        +int ProductId
        +int Priority
        +DateTime CreatedAt
    }
    
    class WishlistResult {
        +int Id
        +int ProductId
        +string ProductName
        +decimal Price
        +bool InStock
    }
    
    IWishlistService <|.. WishlistService
    WishlistService --> IWishlistRepository
    WishlistService --> ICartService
```

---

## 3. Sequence: Add to Wishlist

```mermaid
sequenceDiagram
    participant User
    participant WishlistController
    participant WishlistService
    participant WishlistRepository
    
    User->>WishlistController: POST /wishlist (productId)
    WishlistController->>WishlistService: AddItem(command)
    
    WishlistService->>WishlistRepository: GetByUserAndProduct(userId, productId)
    alt Already exists
        WishlistRepository-->>WishlistService: existing
        WishlistService-->>WishlistController: Already in wishlist
    else
        WishlistRepository-->>WishlistService: null
        
        WishlistService->>WishlistRepository: Add(item)
        WishlistRepository-->>WishlistService: created
        
        WishlistService-->>WishlistController: WishlistResult
        WishlistController-->>User: 201 Created
    end
```

---

## 4. Design Patterns

### 4.1 Cart Wishlist Integration

```csharp
public async Task<CartResult> MoveToCart(int wishlistItemId) {
    var item = await _repo.GetById(wishlistItemId);
    var cartResult = await _cartService.AddItem(new AddToCartCommand {
        ProductId = item.ProductId,
        Quantity = 1
    });
    return cartResult;
}
```

---

## 5. SOLID Compliance ✅