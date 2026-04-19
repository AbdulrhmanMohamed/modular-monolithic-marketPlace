namespace Modules.Cart.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using Modules.Cart.Domain.Entities;
using Modules.Cart.Domain.Interfaces;
using Modules.Cart.Infrastructure.Data;
using Modules.Products.Infrastructure.Data;

public class CartRepository : ICartRepository
{
    private readonly CartDbContext _context;

    public CartRepository(CartDbContext context)
    {
        _context = context;
    }

    private DbSet<Cart> Carts => _context.Carts;
    private DbSet<CartItem> CartItems => _context.CartItems;

    public async Task<Cart?> GetByUserId(int userId)
    {
        return await Carts.FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task<Cart?> GetBySessionId(string sessionId)
    {
        return await Carts.FirstOrDefaultAsync(c => c.SessionId == sessionId);
    }

    public async Task<Cart> Create(Cart cart)
    {
        Carts.Add(cart);
        await _context.SaveChangesAsync();
        return cart;
    }

    public async Task<CartItem> AddItem(CartItem item)
    {
        var existing = await CartItems
            .FirstOrDefaultAsync(i => i.CartId == item.CartId && i.ProductId == item.ProductId);

        if (existing != null)
        {
            existing.Quantity += item.Quantity;
            existing.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return existing;
        }

        CartItems.Add(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<CartItem> UpdateItem(CartItem item)
    {
        CartItems.Update(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<bool> RemoveItem(int itemId)
    {
        var item = await CartItems.FindAsync(itemId);
        if (item == null) return false;

        CartItems.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ClearCart(int cartId)
    {
        var items = await CartItems.Where(i => i.CartId == cartId).ToListAsync();
        if (!items.Any()) return false;

        CartItems.RemoveRange(items);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Cart?> GetCartWithItems(int userId)
    {
        return await Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }
}

public class CartProductRepository : IProductRepository
{
    private readonly ProductsDbContext _context;

    public CartProductRepository(ProductsDbContext context)
    {
        _context = context;
    }

    public async Task<Products.Domain.Entities.Product?> GetById(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<bool> CheckStock(int productId, int quantity)
    {
        var product = await _context.Products.FindAsync(productId);
        return product != null && product.Stock >= quantity;
    }
}