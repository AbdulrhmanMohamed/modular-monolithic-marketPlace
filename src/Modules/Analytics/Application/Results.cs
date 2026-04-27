namespace Modules.Analytics.Application.Results;

public record AnalyticsResult(
    bool Success,
    string? Error
)
{
    public static AnalyticsResult Ok() => new(true, null);
    public static AnalyticsResult Bad(string error) => new(false, error);
};

public record DashboardResult(
    bool Success,
    DashboardDto? Data,
    string? Error
)
{
    public static DashboardResult Ok(DashboardDto data) => new(true, data, null);
    public static DashboardResult Bad(string error) => new(false, null, error);
};

public record DashboardDto(
    int TotalOrders,
    int TotalProducts,
    int TotalUsers,
    decimal TotalRevenue
);