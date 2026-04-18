namespace Modules.Category.Application.Handlers;

using MediatR;
using Modules.Category.Application.Queries;
using Modules.Category.Application.Results;
using Modules.Category.Domain.Interfaces;

public class GetAllCategoriesHandler(ICategoryRepository _categoryRepo) : IRequestHandler<GetAllCategoriesQuery, CategoryResult>
{
    public async Task<CategoryResult> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepo.GetAll();

        var categoryDtos = categories.Select(MapToDto).ToList();

        return new()
        {
            Success = true,
            Categories = categoryDtos
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

public class GetCategoriesByParentHandler(ICategoryRepository _categoryRepo) : IRequestHandler<GetCategoriesByParentQuery, CategoryResult>
{
    public async Task<CategoryResult> Handle(GetCategoriesByParentQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepo.GetByParentId(request.ParentId);

        var categoryDtos = categories.Select(MapToDto).ToList();

        return new()
        {
            Success = true,
            Categories = categoryDtos
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