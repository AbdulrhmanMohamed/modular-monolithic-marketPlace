namespace Modules.Products.Application.Handlers;

using MediatR;
using Modules.Products.Application.Queries;
using Modules.Products.Application.Results;
using Modules.Products.Domain.Interfaces;

public class SearchProductsHandler(IProductRepository _repository) : IRequestHandler<SearchProductsQuery, ProductListResult>
{
    public async Task<ProductListResult> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return ProductListResult.Bad("Search term is required");

        var products = await _repository.SearchAsync(request.Name);

        var dtos = products.Select(p => new ProductDto(
            p.Id,
            p.Name,
            p.Price,
            p.Description,
            p.Stock,
            p.CreatedAt,
            p.UpdatedAt
        )).ToList();

        return ProductListResult.Ok(dtos, 1, dtos.Count, dtos.Count);
    }
}