namespace Modules.Shipping.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using Modules.Shipping.Domain.Entities;
using Modules.Shipping.Domain.Interfaces;
using Modules.Shipping.Infrastructure.Data;

public class ShipmentRepository : IShipmentRepository
{
    private readonly ShippingDbContext _context;

    public ShipmentRepository(ShippingDbContext context)
    {
        _context = context;
    }

    public async Task<List<Shipment>> GetByOrderIdAsync(int orderId)
    {
        return await _context.Shipments
            .Where(s => s.OrderId == orderId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<Shipment?> GetByIdAsync(int id)
    {
        return await _context.Shipments.FindAsync(id);
    }

    public async Task<Shipment> CreateAsync(Shipment shipment)
    {
        _context.Shipments.Add(shipment);
        await _context.SaveChangesAsync();
        return shipment;
    }

    public async Task<Shipment?> UpdateAsync(Shipment shipment)
    {
        _context.Shipments.Update(shipment);
        await _context.SaveChangesAsync();
        return shipment;
    }
}