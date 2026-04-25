namespace Modules.Notifications.Domain.Interfaces;

using Modules.Notifications.Domain.Entities;

public interface INotificationRepository
{
    Task<List<Notification>> GetByUserIdAsync(int userId);
    Task<Notification> CreateAsync(Notification notification);
    Task<bool> MarkAsReadAsync(int id);
}