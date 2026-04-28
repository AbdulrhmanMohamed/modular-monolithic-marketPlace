# Products Component - Low Level Design

---

## Table of Contents

1. [Use Cases](#1-use-cases)
2. [Class Diagram](#2-class-diagram)
3. [Sequence Diagrams](#3-sequence-diagrams)
4. [Class Responsibilities](#4-class-responsibilities)
5. [Design Patterns](#5-design-patterns)
6. [SOLID Compliance](#6-solid-compliance)

---

## 1. Use Cases

### 1.1 Create Product
```
Actors: Admin
Flow: Enter details → Validate → Create → Return
```

### 1.2 Update Product
```
Actors: Admin
Flow: Select product → Edit → Update → Return
```

### 1.3 Delete Product
```
Actors: Admin
Flow: Soft delete → Return success
```

### 1.4 Get Products
```
Actors: Customer, Admin
Flow: Fetch paginated → Return list
```

### 1.5 Search Products
```
Actors: Customer
Flow: Search query → Return results
```

---

## 2. Class Diagram

```mermaid
classDiagram
    class IProductService {
        <<interface>>
        +Create(command) Task~ProductResult~
        +Update(command) Task~ProductResult~
        +Delete(id) Task~bool~
        +GetById(id) Task~ProductResult~
        +GetAll() Task~List~ProductResult~~
    }
    
    class ProductService {
        -IProductRepository _repo
    }
    
    class Product {
        +int Id
        +string Name
        +string Slug
        +decimal Price
        +bool IsActive
    }
    
    IProductService <|.. ProductService
    ProductService --> IProductRepository
```

---

## 3. Sequence: Create Product

```mermaid
sequenceDiagram
    participant Admin
    participant ProductController
    participant ProductService
    participant ProductRepository
    
    Admin->>ProductController: POST /products
    ProductController->>ProductService: Create(command)
    ProductService->>ProductRepository: Create(product)
    ProductRepository-->>ProductService: created
    ProductService-->>ProductController: ProductResult
    ProductController-->>Admin: 201 Created
```

---

## 4. Responsibilities

| Method | Description |
|--------|-------------|
| Create | Add new product |
| Update | Edit product |
| Delete | Soft delete |
| GetAll | List products |
| GetById | Single product |
| Search | Full-text search |

---

## 5. Design Patterns ✅

- CQRS (Commands/Queries)
- Repository
- Service Layer

---

## 6. SOLID ✅

---

## 7. State Diagram

```mermaid
stateDiagram-v2
    [*] --> Active
    Active --> Inactive: Delete
    Inactive --> Active: Restore
```

---

*Last updated: 2026-04-28*