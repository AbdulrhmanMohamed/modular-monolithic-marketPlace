# Complete Request Flow

This document traces the complete flow of HTTP requests through the application with CQRS + MediatR.

---

## Architecture Diagram

```mermaid
flowchart TD
    subgraph Client["Client (Browser/Mobile)"]
        HTTP["HTTP Request"]
    end

    subgraph Kestrel["Kestrel Web Server"]
        Receive[Receive HTTP]
    end

    subgraph Middleware["Middleware Pipeline"]
        EXC[Exception Handling]
        LOG[Request Logging]
        SW[Swagger]
        AUTH[Authentication]
        AUTHZ[Authorization]
        ROUTE[Routing]
    end

    subgraph Controller["Controllers"]
        PC[ProductController]
        AC[AuthController]
    end

    subgraph MediatR["MediatR"]
        M[Mediator Routes]
    end

    subgraph Handler["Handlers"]
        PH[ProductHandlers]
        AH[AuthHandlers]
    end

    subgraph Repository["Infrastructure Layer"]
        PR[ProductRepository]
        UR[UserRepository]
    end

    subgraph Database["Database"]
        DB[(In-Memory DB)]
    end

    HTTP --> Kestrel
    Kestrel --> Middleware
    Middleware --> EXC --> LOG --> SW --> AUTH --> AUTHZ --> ROUTE
    ROUTE -->|POST /api/product| PC
    ROUTE -->|POST /api/auth| AC
    PC --> MediatR
    AC --> MediatR
    MediatR --> Handler
    Handler --> Repository
    Repository --> DB
```

---

## Request Flow: Create Product

```mermaid
sequenceDiagram
    participant Client
    participant Middleware
    participant Controller
    participant MediatR
    participant Handler
    participant Repository
    participant Database

    Client->>Middleware: POST /api/product<br/>Authorization: Bearer token

    Note over Middleware: 1. Extract JWT<br/>2. Validate signature<br/>3. Check expiration

    Middleware->>Controller: User is authenticated<br/>CreateProductCommand

    Controller->>MediatR: Send(CreateProductCommand)

    Note over MediatR: Route to RegisterHandler

    MediatR->>Handler: Handle(command)

    Handler->>Repository: CreateAsync(product)

    Note over Repository: _context.Products.Add()<br/>SaveChangesAsync()

    Repository->>Database: INSERT Products ...

    Database-->>Repository: Success

    Repository-->>Handler: Product with Id

    Handler-->>MediatR: ProductResult.Ok(dto)

    MediatR-->>Controller: ProductResult

    Controller-->>Client: 201 Created<br/>{Product}
```

---

## Request Flow: Login

```mermaid
sequenceDiagram
    participant Client
    participant Middleware
    participant Controller
    participant MediatR
    participant Handler
    participant UserRepository
    participant TokenService

    Client->>Middleware: POST /api/auth/login<br/>{username, password}

    Middleware->>Controller: [FromBody] LoginCommand

    Controller->>MediatR: Send(LoginCommand)

    MediatR->>Handler: Route to LoginHandler

    Handler->>UserRepository: GetByUsernameAsync(username)

    UserRepository-->>Handler: User

    Note over Handler: Verify password with BCrypt

    Handler->>TokenService: GenerateToken(user)

    Note over TokenService: Create JWT claims<br/>Sign with secret key

    TokenService-->>Handler: JWT token string

    Handler-->>MediatR: AuthResult.Ok(dto)

    MediatR-->>Controller: AuthResult

    Controller-->>Client: 200 OK<br/>{userId, username, token}
```

---

## Request Flow: Get Products (Paginated)

```mermaid
sequenceDiagram
    participant Client
    participant Controller
    participant MediatR
    participant Handler
    participant Repository

    Client->>Controller: GET /api/product?page=1&pageSize=10

    Note over Controller: Default: page=1, pageSize=10<br/>Validates: page >= 1, 1 <= pageSize <= 100

    Controller->>MediatR: Send(GetAllProductsQuery)

    MediatR->>Handler: Route to GetAllProductsHandler

    Handler->>Repository: GetAllAsync(page, pageSize)

    Note over Repository: Skip((1-1) * 10) = 0<br/>Take(10)

    Repository-->>Handler: List<Product>

    Handler-->>MediatR: ProductListResult.Ok(dtos, ...)

    MediatR-->>Controller: ProductListResult

    Controller-->>Client: 200 OK<br/>{items, page, pageSize, totalCount}
```

---

## Request Flow: Access Protected Endpoint

```mermaid
flowchart LR
    A[Client] -->|1. No token| B[401 Unauthorized]
    A -->|2. Invalid token| C[401 Unauthorized]
    A -->|3. Expired token| D[401 Unauthorized]
    A -->|4. Valid token| E[200 OK]
```

### Complete Auth Flow

```mermaid
flowchart TD
    A[Request] --> B{Has Authorization header?}

    B -->|No| C[401 Unauthorized]
    B -->|Yes| D{Starts with "Bearer"?}

    D -->|No| E[401 Unauthorized]
    D -->|Yes| F[Extract JWT token]

    F --> G[Validate token]
    G --> H{Signature valid?}

    H -->|No| I[401 Unauthorized]
    H -->|Yes| J{Token expired?}

    J -->|Yes| K[401 Unauthorized]
    J -->|No| L{Create User.Identity}

    L --> M[Continue to MediatR]
    M --> N[Handler processes request]
```

---

## Error Flow

```mermaid
flowchart TD
    A[Request] --> B[FluentValidation]

    B -->|Pass| C[MediatR routes to Handler]
    B -->|Fail| D[400 Bad Request<br/>Return errors]

    C --> E[Handler processes]

    E -->|Success| F[200 OK]
    E -->|Not Found| G[404 Not Found]

    E --> H[Exception]

    H --> I[Exception Handling Middleware]

    I --> J[500 Internal Server Error<br/>Log error]
```

---

## Middleware Pipeline

```mermaid
flowchart LR
    subgraph Request["Incoming Request"]
        REQ[HTTP Request]
    end

    subgraph Pipeline1["Middleware Pipeline"]
        EXC[Exception Handling<br/>Catches all errors]
        LOG[Request Logging<br/>Logs request/response]
        SW1[Swagger<br/>Serve OpenAPI]
        AUTH[Authentication<br/>JWT]
        AUTHz[Authorization<br/>[Authorize]]
        ROUTE[Routing<br/>Match route]
    end

    subgraph MediatRRoute["CQRS Pattern"]
        M[MediatR]
        H[Handler]
    end

    subgraph Response["Response"]
        RES[HTTP Response]
    end

    REQ --> EXC --> LOG --> SW1 --> AUTH --> AUTHz --> ROUTE
    ROUTE --> M --> H --> RES
```

### Order Matters

The middleware order in `Program.cs`:

```csharp
app.UseExceptionHandlingMiddleware();  // 1. Catch errors
app.UseRequestLoggingMiddleware();    // 2. Log requests
app.UseSwagger();                   // 3. OpenAPI
app.UseSwaggerUI();                 // 4. OpenAPI UI
app.UseAuthentication();            // 5. JWT auth
app.UseAuthorization();           // 6. Authorization
app.MapControllers();             // 7. Map routes
```

**Why this order?**

1. Exception handling first: catches all errors
2. Logging: for all requests
3. Swagger: available in development
4. Authentication: before authorization
5. Authorization: after auth
6. Map: last

---

## CQRS Flow Summary

### Command Flow (WRITE)

```mermaid
flowchart LR
    CMD[Controller] -->|Send Command| M[MediatR]
    M -->|Route| H[Handler]
    H -->|Business Logic| R[Repository]
    R -->|Create/Update/Delete| DB[(Database)]
    DB -->|Result| H -->|ProductResult| M -->|Success| CMD
```

### Query Flow (READ)

```mermaid
flowchart LR
    QRY[Controller] -->|Send Query| M[MediatR]
    M -->|Route| H[Handler]
    H -->|Read Data| R[Repository]
    R -->|Query| DB[(Database)]
    DB -->|Data| H -->|ProductListResult| M -->|Success| QRY
```

---

## Request-Response Summary

| Operation | Flow | Status Codes |
|-----------|------|-------------|
| Register | POST /api/auth/register -> MediatR -> Handler -> Repository -> DB | 201, 400 |
| Login | POST /api/auth/login -> MediatR -> Handler -> Token -> JWT | 200, 401 |
| Get All | GET /api/product -> MediatR -> Handler -> Repository -> DB | 200 |
| Get One | GET /api/product/{id} -> MediatR -> Handler -> Repository -> DB | 200, 404 |
| Create | POST /api/product -> MediatR -> Handler -> Repository -> DB | 201, 400 |
| Update | PUT /api/product/{id} -> MediatR -> Handler -> Repository -> DB | 200, 404 |
| Delete | DELETE /api/product/{id} -> MediatR -> Handler -> Repository -> DB | 204, 404 |

---

## Next Steps

- See [CODE_WALKTHROUGH.md](./CODE_WALKTHROUGH.md) for detailed code explanations
- See [QUICK_REFERENCE.md](./QUICK_REFERENCE.md) for commands
- See [CQRS_MEDIATOR.md](./CQRS_MEDIATOR.md) for CQRS pattern details