namespace Modules.Category.Application.Handlers;

using MediatR;
using Modules.Category.Application.Commands;
using Modules.Category.Application.Results;
using Modules.Category.Domain.Interfaces;

public class CreateCategoryHandler(ICategoryRepository _categoryRepo) : IRequestHandler<CreateCategoryCommand, CategoryResult>
{
    public async Task<CategoryResult> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return new() { Success = false, Error = "Name is required" };

        if (await _categoryRepo.ExistsByName(request.Name, request.ParentId))
            return new() { Success = false, Error = "Category with this name already exists" };

        var slug = GenerateSlug(request.Name);

        if (await _categoryRepo.ExistsBySlug(slug))
            return new() { Success = false, Error = "Category slug already exists" };

        var category = new Domain.Entities.Category
        {
            Name = request.Name,
            Slug = slug,
            Description = request.Description,
            ImageUrl = request.ImageUrl,
            ParentId = request.ParentId,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _categoryRepo.Create(category);

        return new()
        {
            Success = true,
            Category = MapToDto(created)
        };
    }

    private static string GenerateSlug(string name)
    {
        return name.ToLower()
            .Replace(" ", "-")
            .Replace("_", "-")
            .Trim();
    }

    private static CategoryDto MapToDto(Domain.Entities.Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            ParentId = category.ParentId,
            Name = category.Name,
            Slug = category.Slug,
            Description = category.Description,
            ImageUrl = category.ImageUrl,
            IsActive = category.IsActive,
            SortOrder = category.SortOrder,
            CreatedAt = category.CreatedAt
        };
    }
}