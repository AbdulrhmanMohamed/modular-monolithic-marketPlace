namespace Modules.Products.Application.Handlers;

using MediatR;
using Modules.Products.Application.Commands;
using Modules.Products.Application.Results;
using Modules.Products.Domain.Entities;
using Modules.Products.Domain.Interfaces;

public class CreateProductHandler(IProductRepository _repository) : IRequestHandler<CreateProductCommand, ProductResult>
{
    public async Task<ProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = request.Name,
            Price = request.Price,
            Description = request.Description,
            Stock = request.Stock,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _repository.CreateAsync(product);

        var dto = new ProductDto(
            created.Id,
            created.Name,
            created.Price,
            created.Description,
            created.Stock,
            created.CreatedAt,
            created.UpdatedAt
        );

        return ProductResult.Ok(dto);
    }
}