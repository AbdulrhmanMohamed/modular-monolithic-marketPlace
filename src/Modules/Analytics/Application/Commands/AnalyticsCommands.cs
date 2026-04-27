namespace Modules.Analytics.Application.Commands;

using MediatR;
using Modules.Analytics.Application.Results;

public record TrackEventCommand(
    string EventType,
    int UserId,
    int? ProductId,
    string? Metadata
) : IRequest<AnalyticsResult>;