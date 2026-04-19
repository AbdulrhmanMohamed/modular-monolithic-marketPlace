namespace Modules.Cart.Application.Results;

public class CartResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public CartDto? Cart { get; set; }
    public List<CartItemDto>? Items { get; set; }
    public int ItemCount { get; set; }
    public decimal Subtotal { get; set; }
}

public class CartDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime ExpiresAt { get; set; }
}

public class CartItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}