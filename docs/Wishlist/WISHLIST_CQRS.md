# Wishlist CQRS Implementation

---

## Commands

```csharp
public record AddToWishlistCommand(int UserId, int ProductId) : IRequest<WishlistResult>;

public record RemoveFromWishlistCommand(int WishlistId) : IRequest<bool>;
```