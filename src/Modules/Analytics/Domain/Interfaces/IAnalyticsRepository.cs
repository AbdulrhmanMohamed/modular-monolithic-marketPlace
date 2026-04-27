namespace Modules.Analytics.Domain.Interfaces;

using Modules.Analytics.Application.Results;
using Modules.Analytics.Domain.Entities;

public interface IAnalyticsRepository
{
    Task<AnalyticsEvent> CreateAsync(AnalyticsEvent analyticsEvent);
    Task<DashboardResult> GetDashboardAsync();
}