# Dependency Flow Justification

## The Question

Why does **Host reference all modules** instead of modules referencing Host?

## Answer: Clean Architecture Dependency Rule

### The Golden Rule

> **Dependencies always point INWARD** - from outer layers to inner layers

```
┌─────────────────────────────────────────┐
│                    UI LAYER                     │
│              Controllers / HTTP Endpoints                  │
└──────────────────────┬──────────────────────────────────┘
                       │ Send via MediatR
┌──────────────────────▼──────────────────────────────────┐
│                    APPLICATION LAYER                     │
│   Commands │ Queries │ Handlers │ Validators │ Results  │
└──────────────────────┬──────────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────────┐
│                      DOMAIN LAYER                       │
│         Entities │ Interfaces │ Business Rules              │
└──────────────────────┬──────────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────────┐
│                   INFRASTRUCTURE LAYER                   │
│              DbContext │ Repositories │ External                     │
└─────────────────────────────────────────────────────┘
```

### Why Host References Modules (Correct)

```
┌─────────────────────────────────────────┐
│           Host.exe (Composition Root)            │
│   - Registers all module services            │
│   - Maps controllers                        │
│   - Configures MediatR                    │
└──────────┬──────────────────────────────────┘
           │ References
    ┌──────▼──────────┐   ┌──────▼──────────┐
    │ Products.dll    │   │ Auth.dll      │
    │ (Module)       │   │ (Module)       │
    └──────┬────────┘   └──────┬────────┘
           │                    │
           ▼                    ▼
    ┌──────▼────────┐   ┌──────▼────────┐
    │ Shared.dll     │   │ Shared.dll    │
    │ (BaseEntity)   │   │ (BaseEntity)  │
    └────────────────┘   └────────────────┘
```

1. **Host is the Composition Root** - the outermost layer that wires everything together
2. **Host needs to know about modules** to:
   - Register their services: `builder.Services.AddMediatR(typeof(ProductsModule).Assembly);`
   - Expose their controllers
   - Configure their DbContexts

### Why Modules Do NOT Reference Host (Correct)

If modules referenced Host, you'd have **circular dependencies**:

```
❌ WRONG:
Auth.csproj → Host.csproj → Auth.csproj (circular!)
```

**Modules are independent:**
- Can be extracted to microservices later
- Don't depend on web framework (ASP.NET)
- Can be tested in isolation

### Module Independence Proof

```csharp
// Products.csproj (NO reference to Host)
// Just references:
// - Shared.csproj (for BaseEntity)
// - MediatR (for IRequest)
// - EF Core (for DbContext)

// Can be tested without Host:
var handler = new CreateProductHandler(mockRepository);
var result = await handler.Handle(command);
```

### What Host Does (Composition Root)

```csharp
// Program.cs (Host)
var builder = WebApplication.CreateBuilder(args);

// Register ALL modules (this is why Host references them)
builder.Services.AddMediatR(typeof(Products.Program), typeof(Auth.Program));
builder.Services.AddDbContext<ProductsDbContext>(...);
builder.Services.AddDbContext<AuthDbContext>(...);

// Modules' controllers are automatically discovered
builder.Services.AddControllers();
```

### Microservices Migration Path

**Current (Monolith):**
```
Host.exe
├── Products.dll (referenced by Host)
├── Auth.dll (referenced by Host)
└── Shared.dll
```

**After Extraction:**
```
Host.exe (API Gateway)
    │
    ├── HTTP → products-service (extracted Products.dll)
    └── HTTP → auth-service (extracted Auth.dll)
```

The extraction works because:
- ✅ Modules don't reference Host
- ✅ Modules are self-contained
- ✅ Communication via MediatR (can swap to HTTP later)

## Summary

| Question | Answer |
|----------|--------|
| Why Host → Modules? | Host is composition root that wires everything |
| Why not Modules → Host? | Would create circular dependencies |
| Can modules be tested alone? | ✅ Yes, they don't need Host |
| Can modules become microservices? | ✅ Yes, they're self-contained |

## Conclusion

**The dependency direction is CORRECT:**
- Host → Modules (composition)
- Modules → Shared (common abstractions)
- Modules → Nothing else (independent)
