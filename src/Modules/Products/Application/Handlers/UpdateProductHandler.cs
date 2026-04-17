namespace Modules.Products.Application.Handlers;

using MediatR;
using Modules.Products.Application.Commands;
using Modules.Products.Application.Results;
using Modules.Products.Domain.Interfaces;

public class UpdateProductHandler(IProductRepository _repository) : IRequestHandler<UpdateProductCommand, ProductResult>
{
    public async Task<ProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(request.Id);
        if (existing is null)
            return ProductResult.NotFound("Product not found");

        existing.Name = request.Name;
        existing.Price = request.Price;
        existing.Description = request.Description;
        existing.Stock = request.Stock;
        existing.UpdatedAt = DateTime.UtcNow;

        var updated = await _repository.UpdateAsync(existing);

        if (updated is null)
            return ProductResult.NotFound("Failed to update");

        var dto = new ProductDto(
            updated.Id,
            updated.Name,
            updated.Price,
            updated.Description,
            updated.Stock,
            updated.CreatedAt,
            updated.UpdatedAt
        );

        return ProductResult.Ok(dto);
    }
}