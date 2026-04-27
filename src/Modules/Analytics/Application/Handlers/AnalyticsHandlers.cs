namespace Modules.Analytics.Application.Handlers;

using MediatR;
using Modules.Analytics.Application.Commands;
using Modules.Analytics.Application.Queries;
using Modules.Analytics.Application.Results;
using Modules.Analytics.Domain.Entities;
using Modules.Analytics.Domain.Interfaces;

public class TrackEventHandler : IRequestHandler<TrackEventCommand, AnalyticsResult>
{
    private readonly IAnalyticsRepository _repository;

    public TrackEventHandler(IAnalyticsRepository repository)
    {
        _repository = repository;
    }

    public async Task<AnalyticsResult> Handle(TrackEventCommand request, CancellationToken cancellationToken)
    {
        var analyticsEvent = new AnalyticsEvent
        {
            EventType = request.EventType,
            UserId = request.UserId,
            ProductId = request.ProductId?.ToString(),
            Metadata = request.Metadata
        };

        await _repository.CreateAsync(analyticsEvent);

        return AnalyticsResult.Ok();
    }
}

public class GetDashboardHandler : IRequestHandler<GetDashboardQuery, DashboardResult>
{
    private readonly IAnalyticsRepository _repository;

    public GetDashboardHandler(IAnalyticsRepository repository)
    {
        _repository = repository;
    }

    public async Task<DashboardResult> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetDashboardAsync();
    }
}