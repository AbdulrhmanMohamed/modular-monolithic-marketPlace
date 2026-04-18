namespace Modules.Inventory.Application.Queries;

using MediatR;
using Modules.Inventory.Application.Results;

public record GetStockQuery(
    int ProductId
) : IRequest<InventoryResult>;

public record GetAllInventoryQuery : IRequest<InventoryListResult>;