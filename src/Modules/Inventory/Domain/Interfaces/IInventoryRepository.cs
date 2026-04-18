namespace Modules.Inventory.Domain.Interfaces;

using Modules.Inventory.Domain.Entities;

public interface IInventoryRepository
{
    Task<Inventory?> GetByProductIdAsync(int productId);
    Task<List<Inventory>> GetAllAsync();
    Task<Inventory> CreateAsync(Inventory inventory);
    Task<Inventory?> UpdateAsync(Inventory inventory);
    Task<bool> DecrementStockAsync(int productId, int quantity);
    Task<bool> IncrementStockAsync(int productId, int quantity);
}