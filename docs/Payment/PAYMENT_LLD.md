# Payment Component - Low Level Design

---

## Use Cases

1. Create Payment
2. Process Payment
3. Refund Payment
4. Get Payment

---

## Class Diagram

```mermaid
classDiagram
    class IPaymentService {
        +Create(command) Task~PaymentResult~
        +Process(command) Task~PaymentResult~
        +Refund(id) Task~bool~
    }
    
    class Payment {
        +int Id
        +int OrderId
        +string Status
        +decimal Amount
    }
    
    class PaymentGateway {
        <<interface>>
        +Charge(payment) Task~bool~
        +Refund(payment) Task~bool~
    }
```

---

## Sequence: Process Payment

```mermaid
sequenceDiagram
    participant User
    participant PaymentController
    participant PaymentService
    participant PaymentGateway
    
    User->>PaymentController: POST /payments/{id}/process
    PaymentController->>PaymentService: Process(command)
    PaymentService->>PaymentGateway: Charge(payment)
    PaymentGateway-->>PaymentService: success
    PaymentService-->>PaymentController: PaymentResult
    PaymentController-->>User: 200 OK
```

---

## State Diagram

```mermaid
stateDiagram-v2
    [*] --> Pending
    Pending --> Processing
    Processing --> Completed
    Processing --> Failed
    Completed --> Refunded
```

---

## Design Patterns

- Strategy (gateways)
- Adapter

---

## SOLID ✅