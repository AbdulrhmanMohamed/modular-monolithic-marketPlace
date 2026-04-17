namespace Modules.Products.Application.Results;

public record ProductResult(
    bool Success,
    ProductDto? Product,
    string? Error
)
{
    public static ProductResult Ok(ProductDto product) => new(true, product, null);
    public static ProductResult NotFound(string error) => new(false, null, error);
    public static ProductResult Bad(string error) => new(false, null, error);
}

public record ProductListResult(
    bool Success,
    List<ProductDto> Products,
    int Page,
    int PageSize,
    int TotalCount,
    string? Error
)
{
    public static ProductListResult Ok(List<ProductDto> products, int page, int pageSize, int total)
        => new(true, products, page, pageSize, total, null);
    public static ProductListResult Bad(string error)
        => new(false, new List<ProductDto>(), 0, 0, 0, error);
}

public record ProductDto(
    int Id,
    string Name,
    decimal Price,
    string? Description,
    int Stock,
    DateTime CreatedAt,
    DateTime UpdatedAt
);