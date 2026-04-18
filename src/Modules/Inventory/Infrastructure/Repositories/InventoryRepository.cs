namespace Modules.Inventory.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using Modules.Inventory.Domain.Entities;
using Modules.Inventory.Domain.Interfaces;
using Modules.Inventory.Infrastructure.Data;

public class InventoryRepository : IInventoryRepository
{
    private readonly InventoryDbContext _context;

    public InventoryRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<Inventory?> GetByProductIdAsync(int productId)
    {
        return await _context.Inventories.FirstOrDefaultAsync(i => i.ProductId == productId);
    }

    public async Task<List<Inventory>> GetAllAsync()
    {
        return await _context.Inventories.ToListAsync();
    }

    public async Task<Inventory> CreateAsync(Inventory inventory)
    {
        _context.Inventories.Add(inventory);
        await _context.SaveChangesAsync();
        return inventory;
    }

    public async Task<Inventory?> UpdateAsync(Inventory inventory)
    {
        _context.Inventories.Update(inventory);
        await _context.SaveChangesAsync();
        return inventory;
    }

    public async Task<bool> DecrementStockAsync(int productId, int quantity)
    {
        var inventory = await GetByProductIdAsync(productId);
        if (inventory is null) return false;
        if (inventory.Quantity - inventory.ReservedQuantity < quantity) return false;
        
        inventory.Quantity -= quantity;
        _context.Inventories.Update(inventory);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IncrementStockAsync(int productId, int quantity)
    {
        var inventory = await GetByProductIdAsync(productId);
        if (inventory is null) return false;
        
        inventory.Quantity += quantity;
        _context.Inventories.Update(inventory);
        await _context.SaveChangesAsync();
        return true;
    }
}