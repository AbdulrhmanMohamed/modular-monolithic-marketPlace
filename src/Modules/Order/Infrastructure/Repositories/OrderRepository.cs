namespace Modules.Order.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using Modules.Order.Domain.Entities;
using Modules.Order.Domain.Interfaces;
using Modules.Order.Infrastructure.Data;

public class OrderRepository : IOrderRepository
{
    private readonly OrderDbContext _context;

    public OrderRepository(OrderDbContext context)
    {
        _context = context;
    }

    private DbSet<Order> Orders => _context.Orders;
    private DbSet<OrderItem> OrderItems => _context.OrderItems;

    public async Task<Order?> GetById(int id)
    {
        return await Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Order?> GetByNumber(string orderNumber)
    {
        return await Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
    }

    public async Task<List<Order>> GetByUserId(int userId)
    {
        return await Orders
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task<Order> Create(Order order)
    {
        Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<Order> Update(Order order)
    {
        Orders.Update(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<OrderItem> AddItem(OrderItem item)
    {
        OrderItems.Add(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<bool> UpdateStatus(int orderId, OrderStatus status)
    {
        var order = await Orders.FindAsync(orderId);
        if (order == null) return false;

        order.Status = status;
        if (status == OrderStatus.Confirmed)
            order.PlacedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Payment> CreatePayment(Payment payment)
    {
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task<Shipment> CreateShipment(Shipment shipment)
    {
        _context.Shipments.Add(shipment);
        await _context.SaveChangesAsync();
        return shipment;
    }
}