namespace Modules.Shipping.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using Modules.Shipping.Domain.Entities;

public class ShippingDbContext : DbContext
{
    public ShippingDbContext(DbContextOptions<ShippingDbContext> options) : base(options) { }

    public DbSet<Shipment> Shipments => Set<Shipment>();
}