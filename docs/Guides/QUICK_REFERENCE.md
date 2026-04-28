# Quick Reference

This document provides essential commands, patterns, and cheat sheet for quick lookup.

---

## Essential Commands

### Build & Run

```bash
# Build the solution
dotnet build

# Run the application
dotnet run --project src/Host

# Clean build artifacts
dotnet clean

# Publish for production
dotnet publish -c Release
```

---

## CQRS Commands & Queries

### Products - Commands (WRITE)

```csharp
// Create
public record CreateProductCommand(
    string Name,
    decimal Price,
    string? Description,
    int Stock
) : IRequest<ProductResult>;

// Update
public record UpdateProductCommand(
    int Id,
    string Name,
    decimal Price,
    string? Description,
    int Stock
) : IRequest<ProductResult>;

// Delete
public record DeleteProductCommand(int Id) : IRequest<ProductResult>;
```

### Products - Queries (READ)

```csharp
// Get All (paginated)
public record GetAllProductsQuery(
    int Page,
    int PageSize
) : IRequest<ProductListResult>;

// Get By Id
public record GetProductByIdQuery(int Id) : IRequest<ProductResult>;

// Search
public record SearchProductsQuery(string Name) : IRequest<ProductListResult>;
```

### Auth - Commands

```csharp
// Register
public record RegisterCommand(
    string Username,
    string Email,
    string Password
) : IRequest<AuthResult>;

// Login
public record LoginCommand(
    string Username,
    string Password
) : IRequest<AuthResult>;
```

---

## Results

### Product Results

```csharp
public record ProductResult(
    bool Success,
    ProductDto? Product,
    string? Error
)
{
    public static ProductResult Ok(ProductDto product) => new(true, product, null);
    public static ProductResult NotFound(string error) => new(false, null, error);
}

public record ProductListResult(
    bool Success,
    List<ProductDto> Products,
    int Page,
    int PageSize,
    int TotalCount,
    string? Error
);

public record ProductDto(
    int Id,
    string Name,
    decimal Price,
    string? Description,
    int Stock,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
```

### Auth Results

```csharp
public record AuthResult(
    bool Success,
    AuthDto? Auth,
    string? Error
)
{
    public static AuthResult Ok(AuthDto auth) => new(true, auth, null);
    public static AuthResult Bad(string error) => new(false, null, error);
}

public record AuthDto(
    int UserId,
    string Username,
    string Email,
    string Role,
    string Token
);
```

---

## Controllers (using MediatR)

### Product Controller

```csharp
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetAllProductsQuery(page, pageSize));

        if (!result.Success)
            return BadRequest(result.Error);

        return Ok(new { result.Products, result.Page, result.PageSize, result.TotalCount });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetById), new { id = result.Product?.Id }, result.Product);
    }
}
```

### Auth Controller

```csharp
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(new { result.Error });

        return Created("", result.Auth);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
            return Unauthorized(new { result.Error });

        return Ok(result.Auth);
    }
}
```

---

## API Endpoints Quick Reference

### Authentication

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/api/auth/register` | No | Register |
| POST | `/api/auth/login` | No | Login |

### Products

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/product` | Yes | List (paginated) |
| GET | `/api/product/{id}` | Yes | Get by ID |
| POST | `/api/product` | Yes | Create |
| PUT | `/api/product/{id}` | Yes | Update |
| DELETE | `/api/product/{id}` | Yes | Delete |
| GET | `/api/product/search?name=X` | Yes | Search |

---

## HTTP Status Codes

| Code | Meaning |
|------|---------|
| 200 | OK |
| 201 | Created |
| 204 | No Content |
| 400 | Bad Request |
| 401 | Unauthorized |
| 403 | Forbidden |
| 404 | Not Found |
| 500 | Server Error |

---

## Service Lifetime

```csharp
// AddSingleton - created once, shared across all requests
builder.Services.AddSingleton(jwtSettings);

// AddScoped - created once per HTTP request
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// AddTransient - created every time it's requested
builder.Services.AddTransient<IEmailService, EmailService>();
```

---

## Middleware Registration Order

```csharp
app.UseExceptionHandlingMiddleware();  // 1. Catch errors
app.UseRequestLoggingMiddleware(); // 2. Log requests
app.UseSwagger();               // 3. OpenAPI
app.UseSwaggerUI();            // 4. OpenAPI UI
app.UseAuthentication();        // 5. JWT auth
app.UseAuthorization();        // 6. Authorization
app.MapControllers();          // 7. Map routes
```

---

## JWT Claims

```csharp
var claims = new[]
{
    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    new Claim(ClaimTypes.Name, user.Username),
    new Claim(ClaimTypes.Email, user.Email),
    new Claim(ClaimTypes.Role, user.Role),
    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
};
```

---

## BCrypt Usage

```csharp
// Hash password (registration)
var hash = BCrypt.Net.BCrypt.HashPassword(password);

// Verify password (login)
var isValid = BCrypt.Net.BCrypt.Verify(password, hash);
```

---

## Pagination

```csharp
// Repository
public async Task<List<Product>> GetAllAsync(int page, int pageSize)
{
    return await _context.Products
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
}
```

---

## Token Validation

```csharp
options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = "LearningApi",
    ValidAudience = "LearningApiClients",
    IssuerSigningKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(secretKey)),
    ClockSkew = TimeSpan.Zero  // Important!
};
```

---

## Route Attributes

```csharp
[ApiController]              // API conventions
[Route("api/[controller]")] // /api/product
[Authorize]               // Requires auth
[Authorize(Roles = "Admin")] // Role-based

[HttpGet]                    // GET
[HttpPost]                  // CREATE
[HttpPut("{id}")]         // UPDATE with ID in URL
[HttpDelete("{id}")]       // DELETE
```

---

## Database Query Examples

```csharp
// Get all
context.Products.ToListAsync();

// Get by ID
context.Products.FindAsync(id);

// Search
context.Products.Where(p => p.Name.Contains(search)).ToListAsync();

// Pagination
context.Products.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
```

---

## Swagger/OpenAPI

```bash
# Access in browser
http://localhost:5000/swagger

# JSON schema
http://localhost:5000/swagger/v1/swagger.json
```

---

## Next Steps

- Browse other doc files in this directory
- Run the application with `dotnet run`
- Test with http files in `src/Host/http/`