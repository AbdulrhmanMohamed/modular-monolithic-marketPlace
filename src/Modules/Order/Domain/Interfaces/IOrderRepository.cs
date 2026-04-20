namespace Modules.Order.Domain.Interfaces;

using Modules.Order.Domain.Entities;

public interface IOrderRepository
{
    Task<Order?> GetById(int id);
    Task<Order?> GetByNumber(string orderNumber);
    Task<List<Order>> GetByUserId(int userId);
    Task<Order> Create(Order order);
    Task<Order> Update(Order order);
    Task<OrderItem> AddItem(OrderItem item);
    Task<bool> UpdateStatus(int orderId, OrderStatus status);
    }

public interface IOrderService
{
    Task<Order> PlaceOrder(int userId, int cartId, int? addressId);
    Task<bool> CancelOrder(int orderId);
    Task<bool> UpdateStatus(int orderId, OrderStatus status);
}