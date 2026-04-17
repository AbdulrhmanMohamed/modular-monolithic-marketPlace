namespace Modules.Products.Application.Commands;

using MediatR;
using Modules.Products.Application.Results;

public record CreateProductCommand(
    string Name,
    decimal Price,
    string? Description,
    int Stock
) : IRequest<ProductResult>;

public record UpdateProductCommand(
    int Id,
    string Name,
    decimal Price,
    string? Description,
    int Stock
) : IRequest<ProductResult>;

public record DeleteProductCommand(
    int Id
) : IRequest<ProductResult>;