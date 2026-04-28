# Reviews Component - Low Level Design

---

## Use Cases

1. Create Review (verified purchase)
2. Get Product Reviews
3. Update Review
4. Delete Review
5. Approve Review (admin)

---

## Class Diagram

```mermaid
classDiagram
    class IReviewService {
        +Create(command) Task~ReviewResult~
        +GetByProduct(productId) Task~List~ReviewResult~~
        +Approve(id) Task~bool~
    }
    
    class Review {
        +int Id
        +int ProductId
        +int Rating
        +string Title
        +string Comment
        +bool IsApproved
    }
```

---

## Sequence

```mermaid
sequenceDiagram
    participant User
    participant ReviewController
    participant ReviewService
    
    User->>ReviewController: POST /reviews
    ReviewController->>ReviewService: Create(command)
    ReviewService-->>ReviewController: ReviewResult
    ReviewController-->>User: 201 Created
```

---

## State Diagram

```mermaid
stateDiagram-v2
    [*] --> Pending
    Pending --> Approved
    Pending --> Rejected
```

---

## SOLID ✅