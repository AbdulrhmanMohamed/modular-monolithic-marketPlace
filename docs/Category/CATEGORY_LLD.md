# Category Component - Low Level Design

---

## Use Cases

1. Create Category
2. Update Category
3. Delete Category
4. Get Categories (tree)
5. Move Category

---

## Class Diagram

```mermaid
classDiagram
    class ICategoryService {
        +Create(command) Task~CategoryResult~
        +GetAll() Task~List~CategoryResult~~
    }
    
    class Category {
        +int Id
        +int~? ParentId
        +string Name
        +string Slug
    }
    
    class CategoryService {
        -ICategoryRepository _repo
    }
```

---

## Sequence: Create Category

```mermaid
sequenceDiagram
    participant Admin
    participant CategoryController
    participant CategoryService
    
    Admin->>CategoryController: POST /categories
    CategoryController->>CategoryService: Create(command)
    CategoryService->>CategoryService: Create(category)
    CategoryService-->>CategoryController: CategoryResult
    CategoryController-->>Admin: 201 Created
```

---

## State Diagram

```mermaid
stateDiagram-v2
    [*] --> Active
    Active --> Inactive: Delete
    Inactive --> Active: Restore
```

---

## Design Patterns

- Tree/Composite
- CQRS

---

## SOLID ✅