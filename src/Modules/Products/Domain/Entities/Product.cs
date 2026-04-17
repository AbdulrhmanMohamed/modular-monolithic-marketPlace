namespace Modules.Products.Domain.Entities;

using Shared.Abstractions;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public int Stock { get; set; }
}