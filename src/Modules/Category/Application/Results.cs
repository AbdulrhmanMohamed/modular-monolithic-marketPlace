namespace Modules.Category.Application.Results;

public class CategoryResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public CategoryDto? Category { get; set; }
    public List<CategoryDto>? Categories { get; set; }
}

public class CategoryDto
{
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public int SortOrder { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<CategoryDto>? Children { get; set; }
}