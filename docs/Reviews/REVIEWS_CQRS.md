# Reviews CQRS Implementation

---

## Commands

```csharp
public record CreateReviewCommand(int ProductId, int Rating, string Title, string Comment) : IRequest<ReviewResult>;
```

## Queries

```csharp
public record GetProductReviewsQuery(int ProductId) : IRequest<List<ReviewResult>>;
```