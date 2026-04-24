namespace Modules.Reviews.Domain.Entities;

using Shared.Abstractions;

public class Review : BaseEntity
{
    public int ProductId { get; set; }
    public int UserId { get; set; }
    public int Rating { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Comment { get; set; }
    public bool IsApproved { get; set; } = false;
}