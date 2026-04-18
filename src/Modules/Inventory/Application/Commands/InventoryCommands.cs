namespace Modules.Inventory.Application.Commands;

using MediatR;
using Modules.Inventory.Application.Results;

public record DecrementStockCommand(
    int ProductId,
    int Quantity
) : IRequest<InventoryResult>;

public record IncrementStockCommand(
    int ProductId,
    int Quantity
) : IRequest<InventoryResult>;

public record ReserveStockCommand(
    int ProductId,
    int Quantity
) : IRequest<InventoryResult>;

public record ReleaseStockCommand(
    int ProductId,
    int Quantity
) : IRequest<InventoryResult>;