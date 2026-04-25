namespace Modules.Notifications.Application.Handlers;

using MediatR;
using Modules.Notifications.Application.Commands;
using Modules.Notifications.Application.Queries;
using Modules.Notifications.Application.Results;
using Modules.Notifications.Domain.Entities;
using Modules.Notifications.Domain.Interfaces;

public class SendNotificationHandler : IRequestHandler<SendNotificationCommand, NotificationResult>
{
    private readonly INotificationRepository _repository;

    public SendNotificationHandler(INotificationRepository repository)
    {
        _repository = repository;
    }

    public async Task<NotificationResult> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = new Notification
        {
            UserId = request.UserId,
            Title = request.Title,
            Message = request.Message,
            Type = request.Type
        };

        var created = await _repository.CreateAsync(notification);

        var dto = new NotificationDto(
            created.Id,
            created.UserId,
            created.Title,
            created.Message,
            created.Type,
            created.IsRead,
            created.CreatedAt
        );

        return NotificationResult.Ok(dto);
    }
}

public class MarkAsReadHandler : IRequestHandler<MarkAsReadCommand, bool>
{
    private readonly INotificationRepository _repository;

    public MarkAsReadHandler(INotificationRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
    {
        return await _repository.MarkAsReadAsync(request.NotificationId);
    }
}

public class GetUserNotificationsHandler : IRequestHandler<GetUserNotificationsQuery, NotificationListResult>
{
    private readonly INotificationRepository _repository;

    public GetUserNotificationsHandler(INotificationRepository repository)
    {
        _repository = repository;
    }

    public async Task<NotificationListResult> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
    {
        var notifications = await _repository.GetByUserIdAsync(request.UserId);

        var dtos = notifications.Select(n => new NotificationDto(
            n.Id,
            n.UserId,
            n.Title,
            n.Message,
            n.Type,
            n.IsRead,
            n.CreatedAt
        )).ToList();

        return NotificationListResult.Ok(dtos);
    }
}