namespace Modules.Inventory.Application.Results;

public record InventoryResult(
    bool Success,
    InventoryDto? Inventory,
    string? Error
)
{
    public static InventoryResult Ok(InventoryDto inventory) => new(true, inventory, null);
    public static InventoryResult Bad(string error) => new(false, null, error);
};

public record InventoryListResult(
    bool Success,
    List<InventoryDto> Inventories,
    string? Error
)
{
    public static InventoryListResult Ok(List<InventoryDto> inventories) => new(true, inventories, null);
    public static InventoryListResult Bad(string error) => new(false, new List<InventoryDto>(), error);
};

public record InventoryDto(
    int Id,
    int ProductId,
    int Quantity,
    int ReservedQuantity
)
{
    public int AvailableQuantity => Quantity - ReservedQuantity;
};