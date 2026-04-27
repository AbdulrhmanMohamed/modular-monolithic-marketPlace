namespace Modules.Wishlist.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using Modules.Wishlist.Domain.Entities;
using Modules.Wishlist.Domain.Interfaces;
using Modules.Wishlist.Infrastructure.Data;

public class WishlistRepository : IWishlistRepository
{
    private readonly WishlistDbContext _context;

    public WishlistRepository(WishlistDbContext context)
    {
        _context = context;
    }

    public async Task<List<Wishlist>> GetByUserIdAsync(int userId)
    {
        return await _context.Wishlists
            .Where(w => w.UserId == userId)
            .ToListAsync();
    }

    public async Task<Wishlist?> GetByIdAsync(int id)
    {
        return await _context.Wishlists.FindAsync(id);
    }

    public async Task<Wishlist> CreateAsync(Wishlist wishlist)
    {
        _context.Wishlists.Add(wishlist);
        await _context.SaveChangesAsync();
        return wishlist;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var wishlist = await _context.Wishlists.FindAsync(id);
        if (wishlist is null) return false;
        _context.Wishlists.Remove(wishlist);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int userId, int productId)
    {
        return await _context.Wishlists
            .AnyAsync(w => w.UserId == userId && w.ProductId == productId);
    }
}