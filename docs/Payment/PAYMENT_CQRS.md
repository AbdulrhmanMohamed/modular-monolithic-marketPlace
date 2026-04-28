# Payment CQRS Implementation

---

## Commands

```csharp
public record ProcessPaymentCommand(int PaymentId) : IRequest<PaymentResult>;
public record RefundPaymentCommand(int PaymentId) : IRequest<bool>;
```