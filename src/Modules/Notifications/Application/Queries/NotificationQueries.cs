namespace Modules.Notifications.Application.Queries;

using MediatR;
using Modules.Notifications.Application.Results;

public record GetUserNotificationsQuery(
    int UserId
) : IRequest<NotificationListResult>;