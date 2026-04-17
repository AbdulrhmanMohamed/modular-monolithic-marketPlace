namespace Modules.Products.Application.Handlers;

using MediatR;
using Modules.Products.Application.Commands;
using Modules.Products.Application.Results;
using Modules.Products.Domain.Interfaces;

public class DeleteProductHandler(IProductRepository _repository) : IRequestHandler<DeleteProductCommand, ProductResult>
{
    public async Task<ProductResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var deleted = await _repository.DeleteAsync(request.Id);

        if (!deleted)
            return ProductResult.NotFound("Product not found");

        return ProductResult.Ok(new ProductDto(0, "", 0, null, 0, DateTime.UtcNow, DateTime.UtcNow));
    }
}