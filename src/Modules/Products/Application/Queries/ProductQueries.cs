namespace Modules.Products.Application.Queries;

using MediatR;
using Modules.Products.Application.Results;

public record GetAllProductsQuery(
    int Page,
    int PageSize
) : IRequest<ProductListResult>;

public record GetProductByIdQuery(
    int Id
) : IRequest<ProductResult>;

public record SearchProductsQuery(
    string Name
) : IRequest<ProductListResult>;