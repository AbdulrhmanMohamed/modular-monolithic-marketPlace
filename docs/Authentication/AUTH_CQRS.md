# Auth CQRS Implementation

---

## Commands

```csharp
public record RegisterCommand(string Username, string Email, string Password) : IRequest<AuthResult>;

public record LoginCommand(string Username, string Password) : IRequest<AuthResult>;

public record RefreshTokenCommand(string RefreshToken) : IRequest<AuthResult>;
```

## Queries

No direct queries - Auth uses commands that return AuthResult with token data.

---

## Flow

```mermaid
sequenceDiagram
    participant Client
    participant API
    participant Mediator
    participant Handler
    participant Repository
    participant TokenService
    
    Client->>API: POST /auth/register
    API->>Mediator: Send(RegisterCommand)
    Mediator->>Handler: Handle
    Handler->>Repository: ExistsAsync(username)
    Repository-->>Handler: false
    Handler->>Handler: Hash password
    Handler->>TokenService: GenerateRefreshToken()
    Handler->>Repository: CreateAsync(user)
    Repository-->>Handler: user
    Handler->>TokenService: GenerateToken(user)
    Handler-->>API: AuthResult
    API-->>Client: 201 Created + Token
    
    Client->>API: POST /auth/login
    API->>Mediator: Send(LoginCommand)
    Mediator->>Handler: Handle
    Handler->>Repository: GetByUsernameAsync(username)
    Repository-->>Handler: user
    Handler->>Handler: Verify password
    Handler->>TokenService: GenerateRefreshToken()
    Handler->>Repository: UpdateAsync(user)
    Handler->>TokenService: GenerateToken(user)
    Handler-->>API: AuthResult
    API-->>Client: 200 OK + Token
```
