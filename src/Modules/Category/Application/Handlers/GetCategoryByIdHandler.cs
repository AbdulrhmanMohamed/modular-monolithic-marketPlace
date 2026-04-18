namespace Modules.Category.Application.Handlers;

using MediatR;
using Modules.Category.Application.Queries;
using Modules.Category.Application.Results;
using Modules.Category.Domain.Interfaces;

public class GetCategoryByIdHandler(ICategoryRepository _categoryRepo) : IRequestHandler<GetCategoryByIdQuery, CategoryResult>
{
    public async Task<CategoryResult> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepo.GetById(request.Id);
        if (category == null)
            return new() { Success = false, Error = "Category not found" };

        return new()
        {
            Success = true,
            Category = MapToDto(category)
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