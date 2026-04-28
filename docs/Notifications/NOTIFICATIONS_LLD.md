# Notifications Component - Low Level Design

---

## Use Cases

1. Send Notification
2. Get User Notifications
3. Mark as Read

---

## Class Diagram

```mermaid
classDiagram
    class INotificationRepository {
        <<interface>>
        +CreateAsync(notification) Task~Notification~
        +GetByUserIdAsync(userId) Task~List~Notification~~
        +MarkAsReadAsync(id) Task~bool~
    }
    
    class NotificationRepository {
        -NotificationsDbContext _context
    }
    
    class Notification {
        +int Id
        +int UserId
        +string Title
        +string Message
        +string Type
        +bool IsRead
        +DateTime CreatedAt
    }
    
    INotificationRepository <|.. NotificationRepository
    NotificationRepository --> Notification
```

---

## Sequence: Send Notification

```mermaid
sequenceDiagram
    participant Service
    participant Mediator
    participant SendNotificationHandler
    participant NotificationRepository
    
    Service->>Mediator: Send(SendNotificationCommand)
    Mediator->>SendNotificationHandler: Handle
    SendNotificationHandler->>NotificationRepository: CreateAsync(notification)
    NotificationRepository-->>SendNotificationHandler: notification
    SendNotificationHandler-->>Mediator: NotificationResult
```

---

## Sequence: Get User Notifications

```mermaid
sequenceDiagram
    participant User
    participant NotificationsController
    participant Mediator
    participant GetUserNotificationsHandler
    participant NotificationRepository
    
    User->>NotificationsController: GET /notifications/user/1
    NotificationsController->>Mediator: Send(GetUserNotificationsQuery)
    Mediator->>GetUserNotificationsHandler: Handle
    GetUserNotificationsHandler->>NotificationRepository: GetByUserIdAsync(userId)
    NotificationRepository-->>GetUserNotificationsHandler: notifications
    GetUserNotificationsHandler-->>Mediator: NotificationListResult
    Mediator-->>NotificationsController: NotificationListResult
    NotificationsController-->>User: 200 OK + Notifications
```

---

## Design Patterns ✅

- CQRS (Commands/Queries)
- Repository
- Observer (event-driven notifications)
- Strategy (email/SMS/push)

---

## SOLID ✅

| Principle | Status | Description |
|-----------|--------|-------------|
| **S** - Single Responsibility | ✅ | Each handler has one job |
| **O** - Open/Closed | ✅ | Can extend with new notification types |
| **L** - Liskov Substitution | ✅ | Repository interchangeable |
| **I** - Interface Segregation | ✅ | Focused interfaces |
| **D** - Dependency Inversion | ✅ | Depends on abstractions |

---

*Last updated: 2026-04-30*
