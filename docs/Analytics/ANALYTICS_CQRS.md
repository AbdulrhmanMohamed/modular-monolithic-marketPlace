# Analytics CQRS Implementation

---

## Commands

```csharp
public record TrackEventCommand(string EventType, int UserId, int? ProductId, string? Metadata) : IRequest<AnalyticsResult>;
```

## Queries

```csharp
public record GetDashboardQuery() : IRequest<DashboardResult>;
```