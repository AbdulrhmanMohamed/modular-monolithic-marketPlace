namespace Modules.Inventory.Application.Handlers;

using MediatR;
using Modules.Inventory.Application.Queries;
using Modules.Inventory.Application.Results;
using Modules.Inventory.Domain.Interfaces;

public class GetStockHandler : IRequestHandler<GetStockQuery, InventoryResult>
{
    private readonly IInventoryRepository _repository;

    public GetStockHandler(IInventoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<InventoryResult> Handle(GetStockQuery request, CancellationToken cancellationToken)
    {
        var inventory = await _repository.GetByProductIdAsync(request.ProductId);

        if (inventory is null)
            return InventoryResult.Bad("Inventory not found");

        var dto = new InventoryDto(
            inventory.Id,
            inventory.ProductId,
            inventory.Quantity,
            inventory.ReservedQuantity
        );

        return InventoryResult.Ok(dto);
    }
}