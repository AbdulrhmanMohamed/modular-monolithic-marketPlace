namespace Modules.Shipping.Domain.Interfaces;

using Modules.Shipping.Domain.Entities;

public interface IShipmentRepository
{
    Task<List<Shipment>> GetByOrderIdAsync(int orderId);
    Task<Shipment?> GetByIdAsync(int id);
    Task<Shipment> CreateAsync(Shipment shipment);
    Task<Shipment?> UpdateAsync(Shipment shipment);
}