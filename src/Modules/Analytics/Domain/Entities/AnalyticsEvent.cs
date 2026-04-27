namespace Modules.Analytics.Domain.Entities;

using Shared.Abstractions;

public class AnalyticsEvent : BaseEntity
{
    public string EventType { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string? ProductId { get; set; }
    public string? Metadata { get; set; }
}