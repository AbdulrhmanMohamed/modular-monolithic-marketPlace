namespace Modules.Inventory.Application.Handlers;

using MediatR;
using Modules.Inventory.Application.Commands;
using Modules.Inventory.Application.Results;
using Modules.Inventory.Domain.Interfaces;

public class DecrementStockHandler : IRequestHandler<DecrementStockCommand, InventoryResult>
{
    private readonly IInventoryRepository _repository;

    public DecrementStockHandler(IInventoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<InventoryResult> Handle(DecrementStockCommand request, CancellationToken cancellationToken)
    {
        var success = await _repository.DecrementStockAsync(request.ProductId, request.Quantity);

        if (!success)
            return InventoryResult.Bad("Insufficient stock");

        var inventory = await _repository.GetByProductIdAsync(request.ProductId);

        var dto = new InventoryDto(
            inventory!.Id,
            inventory.ProductId,
            inventory.Quantity,
            inventory.ReservedQuantity
        );

        return InventoryResult.Ok(dto);
    }
}