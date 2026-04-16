

using Host.Configuration;
using Host.Middleware;


var builder = WebApplication.CreateBuilder(args);

var jwtSettings = new JwtSettings();
builder.Configuration.GetSection(JwtSettings.SectionName).Bind(jwtSettings);
builder.Services.AddSingleton(jwtSettings);

builder.Services.AddDbContext<ProductsDbContext>(options =>
    options.UseInMemoryDatabase("ProductsDb"));

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseInMemoryDatabase("AuthDb"));

builder.Services.AddDbContext<CategoryDbContext>(options =>
    options.UseInMemoryDatabase("CategoryDb"));

builder.Services.AddDbContext<CartDbContext>(options =>
    options.UseInMemoryDatabase("CartDb"));

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseInMemoryDatabase("OrderDb"));

builder.Services.AddDbContext<PaymentDbContext>(options =>
    options.UseInMemoryDatabase("PaymentDb"));

builder.Services.AddDbContext<ShippingDbContext>(options =>
    options.UseInMemoryDatabase("ShippingDb"));

builder.Services.AddDbContext<ReviewsDbContext>(options =>
    options.UseInMemoryDatabase("ReviewsDb"));

builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseInMemoryDatabase("InventoryDb"));

builder.Services.AddDbContext<AddressDbContext>(options =>
    options.UseInMemoryDatabase("AddressDb"));

builder.Services.AddDbContext<WishlistDbContext>(options =>
    options.UseInMemoryDatabase("WishlistDb"));

builder.Services.AddDbContext<DiscountsDbContext>(options =>
    options.UseInMemoryDatabase("DiscountsDb"));

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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// Register repositories
builder.Services.AddScoped<Modules.Products.Domain.Interfaces.IProductRepository, Modules.Products.Infrastructure.Repositories.ProductRepository>();
builder.Services.AddScoped<Modules.Auth.Domain.Interfaces.IUserRepository, Modules.Auth.Infrastructure.Repositories.UserRepository>();
builder.Services.AddScoped<Modules.Category.Domain.Interfaces.ICategoryRepository, Modules.Category.Infrastructure.Repositories.CategoryRepository>();
builder.Services.AddScoped<Modules.Cart.Domain.Interfaces.ICartRepository, Modules.Cart.Infrastructure.Repositories.CartRepository>();
builder.Services.AddScoped<Modules.Order.Domain.Interfaces.IOrderRepository, Modules.Order.Infrastructure.Repositories.OrderRepository>();
builder.Services.AddScoped<Modules.Payment.Domain.Interfaces.IPaymentRepository, Modules.Payment.Infrastructure.Repositories.PaymentRepository>();
builder.Services.AddScoped<Modules.Shipping.Domain.Interfaces.IShipmentRepository, Modules.Shipping.Infrastructure.Repositories.ShipmentRepository>();
builder.Services.AddScoped<Modules.Reviews.Domain.Interfaces.IReviewRepository, Modules.Reviews.Infrastructure.Repositories.ReviewRepository>();
builder.Services.AddScoped<Modules.Inventory.Domain.Interfaces.IInventoryRepository, Modules.Inventory.Infrastructure.Repositories.InventoryRepository>();
builder.Services.AddScoped<Modules.Address.Domain.Interfaces.IAddressRepository, Modules.Address.Infrastructure.Repositories.AddressRepository>();
builder.Services.AddScoped<Modules.Wishlist.Domain.Interfaces.IWishlistRepository, Modules.Wishlist.Infrastructure.Repositories.WishlistRepository>();
builder.Services.AddScoped<Modules.Discounts.Domain.Interfaces.IDiscountRepository, Modules.Discounts.Infrastructure.Repositories.DiscountRepository>();

// Register services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<ITokenService>(sp =>
    new TokenService(jwtSettings));

// Register Payment Gateways (Strategy Pattern)
builder.Services.AddScoped<Modules.Payment.Infrastructure.Gateways.StripeGateway>();
builder.Services.AddScoped<Modules.Payment.Infrastructure.Gateways.PayPalGateway>();
builder.Services.AddScoped<Modules.Payment.Application.Services.PaymentGatewaySelector>(sp =>
    new Modules.Payment.Application.Services.PaymentGatewaySelector(
        sp.GetRequiredService<Modules.Payment.Infrastructure.Gateways.StripeGateway>(),
        sp.GetRequiredService<Modules.Payment.Infrastructure.Gateways.PayPalGateway>()
    ));

// Add FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(typeof(Modules.Products.Application.Commands.CreateProductCommand).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(Modules.Category.Application.Commands.CreateCategoryCommand).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(Modules.Cart.Application.Commands.AddToCartCommand).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(Modules.Order.Application.Commands.PlaceOrderCommand).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(Modules.Payment.Application.Commands.ProcessPaymentCommand).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(Modules.Discounts.Application.Commands.CreateDiscountCommand).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(Modules.Auth.Application.Commands.RegisterCommand).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(Modules.Search.Application.Commands.SaveSearchHistoryCommand).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(Modules.Notifications.Application.Commands.SendNotificationCommand).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(Modules.Analytics.Application.Commands.TrackEventCommand).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(Modules.Media.Application.Commands.UploadMediaCommand).Assembly);

// Add MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly));
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Modules.Auth.Application.Commands.RegisterCommand).Assembly));
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Modules.Category.Application.Commands.CreateCategoryCommand).Assembly));
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Modules.Cart.Application.Commands.AddToCartCommand).Assembly));
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Modules.Order.Application.Commands.PlaceOrderCommand).Assembly));
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Modules.Payment.Application.Commands.ProcessPaymentCommand).Assembly));
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Modules.Shipping.Application.Commands.CreateShipmentCommand).Assembly));
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Modules.Reviews.Application.Commands.CreateReviewCommand).Assembly));
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Modules.Inventory.Application.Commands.DecrementStockCommand).Assembly));
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Modules.Address.Application.Commands.CreateAddressCommand).Assembly));
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Modules.Wishlist.Application.Commands.AddToWishlistCommand).Assembly));
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Modules.Discounts.Application.Commands.CreateDiscountCommand).Assembly));
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Modules.Search.Application.Commands.SaveSearchHistoryCommand).Assembly));
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Modules.Notifications.Application.Commands.SendNotificationCommand).Assembly));
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Modules.Analytics.Application.Commands.TrackEventCommand).Assembly));
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Modules.Media.Application.Commands.UploadMediaCommand).Assembly));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

app.UseExceptionHandlingMiddleware();
app.UseRequestLoggingMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Learning API - CQRS with MediatR");

app.Run();