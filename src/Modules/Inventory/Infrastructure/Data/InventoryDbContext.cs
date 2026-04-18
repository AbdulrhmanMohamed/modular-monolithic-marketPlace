namespace Modules.Inventory.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using Modules.Inventory.Domain.Entities;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }

    public DbSet<Inventory> Inventories => Set<Inventory>();
}