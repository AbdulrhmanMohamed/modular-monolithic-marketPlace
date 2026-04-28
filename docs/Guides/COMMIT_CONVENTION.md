# Commit Convention Guide

---

## 1. Codebase Overview

### Project Structure
```
src/
├── Host/              # Main API entry point
├── Modules/           # 17 independent modules
│   ├── Auth/         # User authentication (JWT, login/register)
│   ├── Products/    # Product catalog (CRUD, search)
│   ├── Category/    # Category tree
│   ├── Cart/        # Shopping cart
│   ├── Order/       # Order placement
│   ├── Payment/     # Payment processing
│   ├── Shipping/    # Shipment tracking
│   ├── Reviews/     # Product reviews
│   ├── Inventory/  # Stock management
│   ├── Address/     # User addresses
│   ├── Wishlist/    # User wishlists
│   ├── Discounts/  # Coupon codes
│   ├── Search/     # Product search
│   ├── Notifications/ # User notifications
│   ├── Analytics/  # Event tracking
│   └── Media/      # File uploads
└── Shared/          # Base abstractions (BaseEntity)
```

### Architecture
- **Clean Architecture** (Per-Module structure)
- **CQRS + MediatR** pattern
- **EF Core InMemory** database

---

## 2. Module Structure

Each module follows Clean Architecture:
```
Module/
├── Domain/           # Entities, Interfaces
├── Application/    # Commands, Queries, Handlers, Results, Validators
├── Infrastructure/ # DbContext, Repositories
└── Api/           # Controllers
```

### Module Dependencies
- Products ← Cart, Order, Search, Wishlist (depend on Products)
- Auth, Category, Payment, Shipping, Reviews, Inventory, Address, Discounts, Notifications, Analytics, Media (standalone)

---

## 3. Commit Types

| Type | Description | When to Use |
|------|-------------|------------|
| `feat` | New module | Creating an entire new module |
| `ext` | Extend module | Adding to existing module |
| `fix` | Bug fix | Fixing errors |
| `refactor` | Code improvement | Restructuring without behavior change |
| `config` | Configuration | csproj packages |
| `docs` | Documentation | .md files |
| `chore` | Build/tooling | Build fixes |

### Usage Examples

#### `feat` - New module
```
feat(auth): add authentication module with JWT
feat(payment): add payment module with process and refund
```

#### `ext` - Extend existing module
```
ext(products): add search handler for product lookup
ext(cart): add clear cart functionality
```

#### `fix` - Bug fix
```
fix(auth): fix password hash verification
fix(cart): resolve quantity update issue
```

#### `refactor` - Code improvement
```
refactor(auth): simplify token generation
```

#### `config` - Configuration
```
config(payment): add MediatR package reference
```

#### `docs` - Documentation
```
docs(cart): update LLD with state diagram
```

#### `chore` - Build/tooling
```
chore: fix EF Core API compatibility
```

---

## 4. Commit Structure

### Format
```
<type>(<module>): <description>
```

### Rules
1. **Lowercase** type
2. **Lowercase** module name in parentheses
3. **Imperative mood** (add, not added)
4. **Max 72 characters** for subject line

### Valid Examples
```
feat(auth): add authentication module with JWT
ext(products): add product search handler
fix(cart): correct quantity update logic
config(payment): add MediatR package
docs(cart): update state diagram
chore: fix EF Core 10 API issue
```

### Invalid Examples
```
feat: add auth module                 # Missing module
feat(Auth): Add auth module      # Capital letter
feat(auth): Adding auth module   # Not imperative
```

---

## 5. Commit Grouping Rules

### Rule 1: Group by Module
All files for ONE module = ONE commit:
```
✓ GOOD
feat(payment): add complete payment module

✗ BAD (split)
feat(payment): add entities
feat(payment): add repository
```

### Rule 2: Group Related Modules
If modules are interdependent:
```
✓ GOOD - related
feat(order): add order module with cart integration
ext(cart): add clear after order placed
```

### Rule 3: Separate Unrelated
Different features = separate commits:
```
✓ GOOD
feat(address): add address module
feat(wishlist): add wishlist module
```

---

## 6. Summary Table

| Scenario | Example |
|----------|---------|
| New module | `feat(payment): add payment module` |
| Add to module | `ext(cart): add clear cart` |
| Bug fix | `fix(products): null check` |
| Code improvement | `refactor(auth): simplify` |
| Package changes | `config(shipping): add MediatR` |
| Documentation | `docs(order): update LLD` |
| Build fix | `chore: EF Core fix` |

---

## 7. Initial Commit History (Simulated Development)

The initial 21 commits represent a simulated 2-week development timeline (April 16-28, 2026) and follow a descriptive format different from the convention above.

### Commit Format (Initial History)
```
<commit-hash> - <description>
```

Examples:
- `a1b2c3d` - Initial project setup
- `e4f5g6h` - Auth module implementation
- `i7j8k9l` - Products module implementation

### Branch Strategy
- **`main`** - Primary branch with 21 simulated commits (April 16-28, 2026)
- All development happens on `main` branch
- No `master` branch used

### Security Practices
- No secrets in version control (JWT SecretKey is empty in `appsettings.json`)
- `.gitignore` prevents `appsettings.*.json` environment files from being committed
- Use environment variables or User Secrets for sensitive configuration

---

*Last updated: 2026-05-03*