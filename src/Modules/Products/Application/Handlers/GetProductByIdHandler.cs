namespace Modules.Products.Application.Handlers;

using MediatR;
using Modules.Products.Application.Queries;
using Modules.Products.Application.Results;
using Modules.Products.Domain.Interfaces;

public class GetProductByIdHandler(IProductRepository _repository) : IRequestHandler<GetProductByIdQuery, ProductResult>
{
    public async Task<ProductResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.Id);

        if (product is null)
            return ProductResult.NotFound("Product not found");

        var dto = new ProductDto(
            product.Id,
            product.Name,
            product.Price,
            product.Description,
            product.Stock,
            product.CreatedAt,
            product.UpdatedAt
        );

        return ProductResult.Ok(dto);
    }
}