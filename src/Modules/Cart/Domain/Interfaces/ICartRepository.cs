namespace Modules.Cart.Domain.Interfaces;

using Modules.Cart.Domain.Entities;

public interface ICartRepository
{
    Task<Cart?> GetByUserId(int userId);
    Task<Cart?> GetBySessionId(string sessionId);
    Task<Cart> Create(Cart cart);
    Task<CartItem> AddItem(CartItem item);
    Task<CartItem> UpdateItem(CartItem item);
    Task<bool> RemoveItem(int itemId);
    Task<bool> ClearCart(int cartId);
    Task<Cart?> GetCartWithItems(int userId);
}

public interface IProductRepository
{
    Task<Modules.Products.Domain.Entities.Product?> GetById(int id);
    Task<bool> CheckStock(int productId, int quantity);
}