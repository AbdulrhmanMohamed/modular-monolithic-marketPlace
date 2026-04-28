# Auth Component - Low Level Design

---

## Use Cases

1. Register User
2. Login User
3. Refresh Token

---

## Class Diagram

```mermaid
classDiagram
    class IUserRepository {
        <<interface>>
        +GetByUsernameAsync(username) Task~User?~
        +GetByRefreshTokenAsync(refreshToken) Task~User?~
        +CreateAsync(user) Task~User~
        +UpdateAsync(user) Task~User~
        +ExistsAsync(username) Task~bool~
    }
    
    class UserRepository {
        -AuthDbContext _context
    }
    
    class User {
        +int Id
        +string Username
        +string Password
        +string Email
        +string Role
        +string RefreshToken
        +DateTime RefreshTokenExpiry
    }
    
    class ITokenService {
        <<interface>>
        +GenerateToken(user) string
        +GenerateRefreshToken() string
    }
    
    class TokenService {
        -SymmetricSecurityKey _key
    }
    
    class IAuthService {
        <<interface>>
        +Register(command) Task~AuthResult~
        +Login(command) Task~AuthResult~
    }
    
    IUserRepository <|.. UserRepository
    ITokenService <|.. TokenService
    UserRepository --> User
```

---

## Sequence: Register

```mermaid
sequenceDiagram
    participant User
    participant AuthController
    participant Mediator
    participant RegisterHandler
    participant UserRepository
    participant TokenService
    
    User->>AuthController: POST /auth/register
    AuthController->>Mediator: Send(RegisterCommand)
    Mediator->>RegisterHandler: Handle
    RegisterHandler->>UserRepository: ExistsAsync(username)
    UserRepository-->>RegisterHandler: false
    RegisterHandler->>RegisterHandler: Hash password
    RegisterHandler->>TokenService: GenerateRefreshToken()
    RegisterHandler->>UserRepository: CreateAsync(user)
    UserRepository-->>RegisterHandler: user
    RegisterHandler->>TokenService: GenerateToken(user)
    RegisterHandler-->>Mediator: AuthResult
    Mediator-->>AuthController: AuthResult
    AuthController-->>User: 201 Created + Token
```

---

## Sequence: Login

```mermaid
sequenceDiagram
    participant User
    participant AuthController
    participant Mediator
    participant LoginHandler
    participant UserRepository
    participant TokenService
    
    User->>AuthController: POST /auth/login
    AuthController->>Mediator: Send(LoginCommand)
    Mediator->>LoginHandler: Handle
    LoginHandler->>UserRepository: GetByUsernameAsync(username)
    UserRepository-->>LoginHandler: user
    LoginHandler->>LoginHandler: Verify password
    LoginHandler->>TokenService: GenerateRefreshToken()
    LoginHandler->>UserRepository: UpdateAsync(user)
    LoginHandler->>TokenService: GenerateToken(user)
    LoginHandler-->>Mediator: AuthResult
    Mediator-->>AuthController: AuthResult
    AuthController-->>User: 200 OK + Token
```

---

## Design Patterns ✅

- CQRS (Commands/Queries)
- Repository
- Service Layer
- Factory (Token generation)

---

## SOLID ✅

| Principle | Status | Description |
|-----------|--------|-------------|
| **S** - Single Responsibility | ✅ | Each class has one reason to change |
| **O** - Open/Closed | ✅ | Can extend with new auth providers |
| **L** - Liskov Substitution | ✅ | Repositories and services are interchangeable |
| **I** - Interface Segregation | ✅ | Focused interfaces |
| **D** - Dependency Inversion | ✅ | Depends on abstractions |

---

## Security Notes

- Passwords hashed using BCrypt
- JWT tokens for authentication
- Refresh tokens with 7-day expiry
- Tokens stored in database linked to user

---

*Last updated: 2026-04-30*
