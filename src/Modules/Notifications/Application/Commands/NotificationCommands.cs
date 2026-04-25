namespace Modules.Notifications.Application.Commands;

using MediatR;
using Modules.Notifications.Application.Results;

public record SendNotificationCommand(
    int UserId,
    string Title,
    string Message,
    string Type
) : IRequest<NotificationResult>;

public record MarkAsReadCommand(
    int NotificationId
) : IRequest<bool>;