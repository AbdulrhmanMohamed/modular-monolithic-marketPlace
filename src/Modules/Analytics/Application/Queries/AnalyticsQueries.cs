namespace Modules.Analytics.Application.Queries;

using MediatR;
using Modules.Analytics.Application.Results;

public record GetDashboardQuery : IRequest<DashboardResult>;