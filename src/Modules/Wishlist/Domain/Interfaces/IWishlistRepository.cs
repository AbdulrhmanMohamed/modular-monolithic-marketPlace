namespace Modules.Wishlist.Domain.Interfaces;

using Modules.Wishlist.Domain.Entities;

public interface IWishlistRepository
{
    Task<List<Wishlist>> GetByUserIdAsync(int userId);
    Task<Wishlist?> GetByIdAsync(int id);
    Task<Wishlist> CreateAsync(Wishlist wishlist);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int userId, int productId);
}