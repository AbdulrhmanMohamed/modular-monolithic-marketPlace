# Category CQRS Implementation

---

## Commands

```csharp
public record CreateCategoryCommand(string Name) : IRequest<CategoryResult>;
```

## Queries

```csharp
public record GetAllCategoriesQuery() : IRequest<List<CategoryResult>>;
```