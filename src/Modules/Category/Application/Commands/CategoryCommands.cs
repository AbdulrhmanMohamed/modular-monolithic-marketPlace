namespace Modules.Category.Application.Commands;

using MediatR;
using Modules.Category.Application.Results;

public record CreateCategoryCommand(
    string Name,
    string? Description,
    string? ImageUrl,
    int? ParentId
) : IRequest<CategoryResult>;

public record UpdateCategoryCommand(
    int Id,
    string Name,
    string? Description,
    string? ImageUrl,
    int? ParentId
) : IRequest<CategoryResult>;

public record DeleteCategoryCommand(
    int Id
) : IRequest<CategoryResult>;

public record MoveCategoryCommand(
    int Id,
    int? NewParentId
) : IRequest<CategoryResult>;