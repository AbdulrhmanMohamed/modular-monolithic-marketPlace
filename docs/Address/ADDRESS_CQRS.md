# Address CQRS Implementation

---

## Commands

```csharp
public record CreateAddressCommand(AddressDto dto) : IRequest<AddressResult>;
```

## Queries

```csharp
public record GetUserAddressesQuery(int UserId) : IRequest<List<AddressResult>>;
```