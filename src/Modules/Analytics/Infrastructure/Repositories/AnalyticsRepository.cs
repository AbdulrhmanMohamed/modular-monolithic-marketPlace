using Modules.Analytics.Application.Results;
using Modules.Analytics.Domain.Entities;
using Modules.Analytics.Domain.Interfaces;

namespace Modules.Analytics.Infrastructure.Repositories;

public class AnalyticsRepository : IAnalyticsRepository
{
    private readonly List<AnalyticsEvent> _events = new();

    public Task<AnalyticsEvent> CreateAsync(AnalyticsEvent analyticsEvent)
    {
        _events.Add(analyticsEvent);
        return Task.FromResult(analyticsEvent);
    }

    public Task<DashboardResult> GetDashboardAsync()
    {
        var dashboard = new DashboardDto(
            TotalOrders: _events.Count(e => e.EventType == "order_placed"),
            TotalProducts: _events.Count(e => e.EventType == "product_viewed"),
            TotalUsers: _events.Select(e => e.UserId).Distinct().Count(),
            TotalRevenue: 0
        );

        return Task.FromResult(new DashboardResult(true, dashboard, null));
    }
}