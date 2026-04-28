# Authentication & JWT Flow

This document explains how JWT authentication works end-to-end.

---

## What is JWT?

**JSON Web Token (JWT)** is a compact, URL-safe token format for securely transmitting claims between parties.

```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwibmFtZSI6ImpvaG4iLCJlbWFpbCI6ImpvaG5AZXhhbXBsZS5jb20iLCJpYXQiOjE3MTY4MDAwMDB9. signature
│    │                                                      │
│    │                                                      └─ Signature (verifies sender)
│    └─ Payload (the data)
└─ Header (token type, algorithm)
```

### JWT Structure

Three parts separated by dots:
1. **Header**: Token type and algorithm
2. **Payload**: The actual data (claims)
3. **Signature**: Cryptographic signature

---

## JWT Payload (Claims)

```json
{
  "sub": "1",
  "name": "john",
  "email": "john@example.com",
  "role": "User",
  "jti": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
  "exp": 1716800000,
  "iss": "LearningApi",
  "aud": "LearningApiClients",
  "iat": 1716796800
}
```

### Standard Claims

| Claim | Meaning | Purpose |
|-------|---------|---------|
| `sub` | Subject | User ID |
| `name` | Name | Display name |
| `email` | Email | User email |
| `role` | Role | Authorization |
| `jti` | JWT ID | Unique identifier (for revocation) |
| `exp` | Expiration | When token expires |
| `iat` | Issued At | When token created |
| `iss` | Issuer | Who created token |
| `aud` | Audience | Who token is for |

---

## Complete Authentication Flow

### 1. Registration (CQRS Flow)

```
CLIENT                              SERVER
   │                                    │
   │  POST /api/auth/register            │
   │  { username, password, email }      │
   ├─────────────────────────────────►  │
   │                                    │  MediatR routes to RegisterHandler
   │                                    │  Check if username exists
   │                                    │  Hash password with BCrypt
   │                                    │  Create User entity
   │                                    │  Save to database
   │                                    │  Generate JWT token
   │                                    │
   │  201 Created                       │
   │  { userId, username, token }  ◄───┤
   │
   ▼
```

**Register Handler**:
```csharp
// Handlers/RegisterHandler.cs
public class RegisterHandler : IRequestHandler<RegisterCommand, AuthResult>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public async Task<AuthResult> Handle(RegisterCommand request, CancellationToken ct)
    {
        if (await _userRepository.ExistsAsync(request.Username))
            return AuthResult.Bad("Username is already taken");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            Password = passwordHash,
            Role = "User",
            CreatedAt = DateTime.UtcNow
        };

        var created = await _userRepository.CreateAsync(user);
        var token = _tokenService.GenerateToken(created);

        return AuthResult.Ok(new AuthDto(
            created.Id,
            created.Username,
            created.Email,
            created.Role,
            token
        ));
    }
}
```

### 2. Login (CQRS Flow)

```
CLIENT                              SERVER
   │                                    │
   │  POST /api/auth/login               │
   │  { username, password }        │
   ├─────────────────────────────────►  │
   │                                    │  MediatR routes to LoginHandler
   │                                    │  Find user by username
   │                                    │  Verify password with BCrypt
   │                                    │  Generate JWT token
   │                                    │
   │  200 OK                        │
   │  { userId, username, token } ◄───┤
   │
   ▼
```

**Login Handler**:
```csharp
// Handlers/LoginHandler.cs
public class LoginHandler : IRequestHandler<LoginCommand, AuthResult>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public async Task<AuthResult> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);

        if (user is null)
            return AuthResult.Bad("Invalid username or password");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            return AuthResult.Bad("Invalid username or password");

        var token = _tokenService.GenerateToken(user);

        return AuthResult.Ok(new AuthDto(
            user.Id,
            user.Username,
            user.Email,
            user.Role,
            token
        ));
    }
}
```

### 3. Token Generation (TokenService)

```csharp
// Services/TokenService.cs
public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _key;

    public TokenService(byte[] secretKey)
    {
        _key = new SymmetricSecurityKey(secretKey);
    }

    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "LearningApi",
            audience: "LearningApiClients",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),  // 30 minutes!
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
```

### 4. Accessing Protected Resources

```
CLIENT                              SERVER
   │                                    │
   │  GET /api/product                   │
   │  Authorization: Bearer eyJ...    │
   ├─────────────────────────────────►  │
   │                                    │  Extract token from header
   │                                    │  Validate signature
   │                                    │  Check expiration
   │                                    │  Create User.Identity
   │                                    │
   │  200 OK                          │
   │  [{ products }]              ◄───┤
   │
   ▼
```

---

## JWT Configuration

### Program.cs Setup

```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
            ClockSkew = TimeSpan.Zero
        };
    });
```

### Validation Parameters Explained

| Parameter | Purpose |
|-----------|---------|
| `ValidateIssuer` | Verify token wasn't forged |
| `ValidateAudience` | Verify token is for this app |
| `ValidateLifetime` | Reject expired tokens |
| `ValidateIssuerSigningKey` | Verify cryptographic signature |
| `ClockSkew` | Grace period for expiration |

### ClockSkew = TimeSpan.Zero

This is **critical** for security:

- **Default**: 5 minute grace period (expired tokens still work!)
- **TimeSpan.Zero**: No grace period

```csharp
// WRONG - allows expired tokens for 5 more minutes
ClockSkew = TimeSpan.FromMinutes(5)

// CORRECT - rejects immediately when expired
ClockSkew = TimeSpan.Zero
```

---

## Password Security with BCrypt

### Why BCrypt?

| Feature | Benefit |
|---------|---------|
| **Salted** | Each hash is unique |
| **Work Factor** | Configurable cost (default 12) |
| **Slow** | Resistant to brute force |
| **Proven** | Widely used |

### BCrypt Code

```csharp
// Hash password (registration)
PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);

// Verify password (login)
BCrypt.Net.BCrypt.Verify(inputPassword, storedHash);
```

### Work Factor

```csharp
// Default work factor (12) - takes ~0.3 seconds
BCrypt.HashPassword(password);

// Higher security (14) - takes ~1 second
BCrypt.HashPassword(password, workFactor: 14);
```

---

## Authorization

### Using [Authorize] Attribute

```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]  // Requires authentication
public class ProductController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll() => Ok();
}
```

### Role-Based Authorization

```csharp
[Authorize(Roles = "Admin")]
public IActionResult AdminOnly() => Ok();

// Or in code
if (User.IsInRole("Admin"))
{
    // Do admin stuff
}
```

---

## Security Best Practices Implemented

### Implemented

1. ✅ **Short Token Expiry** (30 minutes, not 7 days)
2. ✅ **ClockSkew = TimeSpan.Zero** (no grace period)
3. ✅ **JTI Claim** (unique ID for revocation)
4. ✅ **HmacSha256** Algorithm (not deprecated `none`)
5. ✅ **BCrypt** for password hashing

### Recommended for Production

1. ⬜ **HTTPS** enforcement
2. ⬜ **Refresh Tokens** for long sessions
3. ⬜ **Rate Limiting** for brute force prevention
4. ⬜ **Key Vault** for secrets
5. ⬜ **Audit Logging**

---

## Token Storage (Frontend)

### Recommended: httpOnly Cookies

```csharp
// Server sets cookie (more secure than localStorage)
Response.Cookies.Append("token", token, new CookieOptions
{
    HttpOnly = true,  // JavaScript can't access
    Secure = true,   // HTTPS only
    SameSite = SameSiteMode.Strict
});
```

### NOT Recommended: localStorage

```javascript
// Vulnerable to XSS attacks
localStorage.setItem('token', response.token);
```

---

## Token Revocation (Future)

To revoke tokens, maintain a blocklist:

```csharp
// Using JTI claim
var revokedTokens = new HashSet<string>();

// Check on each request
if (revokedTokens.Contains(tokenId))
    return Unauthorized();
```

---

## Debugging JWT

### Decode JWT at jwt.io

Paste token at https://jwt.io to see:
- Header
- Payload
- Signature status

### Add Claims to Response

```csharp
// Debug: include claims in response
var debugInfo = new
{
    UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
    Username = User.FindFirst(ClaimTypes.Name)?.Value,
    Role = User.FindFirst(ClaimTypes.Role)?.Value
};
```

---

## Next Steps

- See [ENDPOINTS.md](./ENDPOINTS.md) for all endpoints
- See [REQUEST_FLOW.md](./REQUEST_FLOW.md) for complete request flow
- See [CODE_WALKTHROUGH.md](./CODE_WALKTHROUGH.md) for code