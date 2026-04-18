namespace Modules.Category.Application.Handlers;

using MediatR;
using Modules.Category.Application.Commands;
using Modules.Category.Application.Results;
using Modules.Category.Domain.Interfaces;

public class UpdateCategoryHandler(ICategoryRepository _categoryRepo) : IRequestHandler<UpdateCategoryCommand, CategoryResult>
{
    public async Task<CategoryResult> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepo.GetById(request.Id);
        if (category == null)
            return new() { Success = false, Error = "Category not found" };

        if (request.Name != category.Name && await _categoryRepo.ExistsByName(request.Name, request.ParentId))
            return new() { Success = false, Error = "Category with this name already exists" };

        category.Name = request.Name;
        category.Description = request.Description;
        category.ImageUrl = request.ImageUrl;
        category.ParentId = request.ParentId;

        var updated = await _categoryRepo.Update(category);

        return new()
        {
            Success = true,
            Category = MapToDto(updated)
        };
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