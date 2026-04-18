namespace Modules.Category.Application.Queries;

using MediatR;
using Modules.Category.Application.Results;

public record GetCategoryByIdQuery(int Id) : IRequest<CategoryResult>;

public record GetAllCategoriesQuery() : IRequest<CategoryResult>;

public record GetCategoriesByParentQuery(int? ParentId) : IRequest<CategoryResult>;