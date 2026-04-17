namespace Modules.Products.Application.Handlers;

using MediatR;
using Modules.Products.Application.Queries;
using Modules.Products.Application.Results;
using Modules.Products.Domain.Interfaces;

public class GetAllProductsHandler(IProductRepository _repository) : IRequestHandler<GetAllProductsQuery, ProductListResult>
{
    public async Task<ProductListResult> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var page = request.Page < 1 ? 1 : request.Page;
        var pageSize = request.PageSize < 1 ? 10 : Math.Min(request.PageSize, 100);

        var products = await _repository.GetAllAsync(page, pageSize);

        var dtos = products.Select(p => new ProductDto(
            p.Id,
            p.Name,
            p.Price,
            p.Description,
            p.Stock,
            p.CreatedAt,
            p.UpdatedAt
        )).ToList();

        return ProductListResult.Ok(dtos, page, pageSize, dtos.Count);
    }
}