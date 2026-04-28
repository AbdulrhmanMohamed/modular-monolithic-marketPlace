# Search Component - Low Level Design

---

## Use Cases

1. Search Products
2. Save Search History
3. Get Search History

---

## Class Diagram

```mermaid
classDiagram
    class ISearchRepository {
        <<interface>>
        +SearchProductsAsync(query, page, pageSize) Task~List~Product~~
        +CreateSearchHistoryAsync(history) Task~SearchHistory~
        +GetSearchHistoryAsync(userId) Task~List~SearchHistory~~
    }
    
    class SearchRepository {
        -SearchDbContext _context
    }
    
    class SearchHistory {
        +int Id
        +int UserId
        +string Query
        +DateTime CreatedAt
    }
    
    ISearchRepository <|.. SearchRepository
    SearchRepository --> SearchHistory
```

---

## Sequence: Search Products

```mermaid
sequenceDiagram
    participant User
    participant SearchController
    participant Mediator
    participant SearchProductsHandler
    participant SearchRepository
    
    User->>SearchController: GET /search?q=laptop
    SearchController->>Mediator: Send(SearchProductsQuery)
    Mediator->>SearchProductsHandler: Handle
    SearchProductsHandler->>SearchRepository: SearchProductsAsync(query, page, pageSize)
    SearchRepository-->>SearchProductsHandler: products
    SearchProductsHandler-->>Mediator: SearchResult
    Mediator-->>SearchController: SearchResult
    SearchController-->>User: 200 OK + Products
```

---

## Sequence: Save Search History

```mermaid
sequenceDiagram
    participant User
    participant SearchController
    participant Mediator
    participant SaveSearchHistoryHandler
    participant SearchRepository
    
    User->>SearchController: GET /search?q=laptop
    SearchController->>Mediator: Send(SearchProductsQuery)
    Note over SearchController: After search results
    SearchController->>Mediator: Send(SaveSearchHistoryCommand)
    Mediator->>SaveSearchHistoryHandler: Handle
    SaveSearchHistoryHandler->>SearchRepository: CreateSearchHistoryAsync(history)
    SearchRepository-->>SaveSearchHistoryHandler: history
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
| **O** - Open/Closed | ✅ | Can extend with new search filters |
| **L** - Liskov Substitution | ✅ | Repository interchangeable |
| **I** - Interface Segregation | ✅ | Focused interfaces |
| **D** - Dependency Inversion | ✅ | Depends on abstractions |

---

*Last updated: 2026-04-30*
