# Analytics Component - Low Level Design

---

## Use Cases

1. Track Event
2. Get Dashboard

---

## Class Diagram

```mermaid
classDiagram
    class IAnalyticsRepository {
        <<interface>>
        +CreateEventAsync(event) Task~AnalyticsEvent~
        +GetDashboardAsync() Task~DashboardDto~
        +GetEventsAsync(filter) Task~List~AnalyticsEvent~~
    }
    
    class AnalyticsRepository {
        -AnalyticsDbContext _context
    }
    
    class AnalyticsEvent {
        +int Id
        +string EventType
        +int UserId
        +string ProductId
        +string Metadata
    }
    
    class DashboardDto {
        +int TotalOrders
        +int TotalProducts
        +int TotalUsers
        +decimal TotalRevenue
    }
    
    IAnalyticsRepository <|.. AnalyticsRepository
    AnalyticsRepository --> AnalyticsEvent
```

---

## Sequence: Track Event

```mermaid
sequenceDiagram
    participant Service
    participant Mediator
    participant TrackEventHandler
    participant AnalyticsRepository
    
    Service->>Mediator: Send(TrackEventCommand)
    Mediator->>TrackEventHandler: Handle
    TrackEventHandler->>AnalyticsRepository: CreateEventAsync(event)
    AnalyticsRepository-->>TrackEventHandler: saved
    TrackEventHandler-->>Mediator: AnalyticsResult
```

---

## Sequence: Get Dashboard

```mermaid
sequenceDiagram
    participant Admin
    participant AnalyticsController
    participant Mediator
    participant GetDashboardHandler
    participant AnalyticsRepository
    
    Admin->>AnalyticsController: GET /analytics/dashboard
    AnalyticsController->>Mediator: Send(GetDashboardQuery)
    Mediator->>GetDashboardHandler: Handle
    GetDashboardHandler->>AnalyticsRepository: GetDashboardAsync()
    AnalyticsRepository-->>GetDashboardHandler: dashboardDto
    GetDashboardHandler-->>Mediator: DashboardResult
    Mediator-->>AnalyticsController: DashboardResult
    AnalyticsController-->>Admin: 200 OK + Dashboard
```

---

## Design Patterns ✅

- CQRS (Commands/Queries)
- Repository
- Observer (event tracking)

---

## SOLID ✅

| Principle | Status | Description |
|-----------|--------|-------------|
| **S** - Single Responsibility | ✅ | Each handler has one job |
| **O** - Open/Closed | ✅ | Can extend with new event types |
| **L** - Liskov Substitution | ✅ | Repository interchangeable |
| **I** - Interface Segregation | ✅ | Focused interfaces |
| **D** - Dependency Inversion | ✅ | Depends on abstractions |

---

*Last updated: 2026-04-30*
