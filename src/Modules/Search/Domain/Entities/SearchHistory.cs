namespace Modules.Search.Domain.Entities;

using Shared.Abstractions;

public class SearchHistory : BaseEntity
{
    public int UserId { get; set; }
    public string Query { get; set; } = string.Empty;
}