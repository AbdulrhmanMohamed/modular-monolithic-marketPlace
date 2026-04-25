namespace Modules.Search.Application.Results;

using ProductEntity = Modules.Products.Domain.Entities.Product;

public record SearchResult(
    bool Success,
    List<ProductEntity> Products,
    string? Error
)
{
    public static SearchResult Ok(List<ProductEntity> products) => new(true, products, null);
    public static SearchResult Bad(string error) => new(false, new List<ProductEntity>(), error);
};

public record SearchHistoryResult(
    bool Success,
    SearchHistoryDto? History,
    string? Error
)
{
    public static SearchHistoryResult Ok(SearchHistoryDto history) => new(true, history, null);
    public static SearchHistoryResult Bad(string error) => new(false, null, error);
};

public record SearchHistoryListResult(
    bool Success,
    List<SearchHistoryDto> Histories,
    string? Error
)
{
    public static SearchHistoryListResult Ok(List<SearchHistoryDto> histories) => new(true, histories, null);
    public static SearchHistoryListResult Bad(string error) => new(false, new List<SearchHistoryDto>(), error);
};

public record SearchHistoryDto(
    int Id,
    int UserId,
    string Query,
    DateTime CreatedAt
);