namespace Modules.Inventory.Domain.Entities;

using Shared.Abstractions;

public class Inventory : BaseEntity
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public int ReservedQuantity { get; set; }
}