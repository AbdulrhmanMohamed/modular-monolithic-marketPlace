namespace Modules.Notifications.Application.Results;

public record NotificationResult(
    bool Success,
    NotificationDto? Notification,
    string? Error
)
{
    public static NotificationResult Ok(NotificationDto notification) => new(true, notification, null);
    public static NotificationResult Bad(string error) => new(false, null, error);
};

public record NotificationListResult(
    bool Success,
    List<NotificationDto> Notifications,
    string? Error
)
{
    public static NotificationListResult Ok(List<NotificationDto> notifications) => new(true, notifications, null);
    public static NotificationListResult Bad(string error) => new(false, new List<NotificationDto>(), error);
};

public record NotificationDto(
    int Id,
    int UserId,
    string Title,
    string Message,
    string Type,
    bool IsRead,
    DateTime CreatedAt
);