namespace Modules.Discounts.Application.Results;

public record DiscountResult(
    bool Success,
    DiscountDto? Discount,
    decimal? DiscountAmount,
    string? Error
)
{
    public static DiscountResult Ok(DiscountDto discount, decimal? discountAmount = null) 
        => new(true, discount, discountAmount, null);
    public static DiscountResult Bad(string error) => new(false, null, null, error);
};

public record DiscountListResult(
    bool Success,
    List<DiscountDto> Discounts,
    string? Error
)
{
    public static DiscountListResult Ok(List<DiscountDto> discounts) => new(true, discounts, null);
    public static DiscountListResult Bad(string error) => new(false, new List<DiscountDto>(), error);
};

public record DiscountDto(
    int Id,
    string Code,
    string Description,
    decimal DiscountPercent,
    decimal? MinOrderAmount,
    DateTime ValidFrom,
    DateTime ValidUntil,
    bool IsActive
);